﻿using System;
using System.Collections.Generic;

namespace Sprint02Tasks;

public class Program
{
    private static readonly string ErrorLogPath = "error.log";

    private static void LogError(string message, Exception? ex = null)
    {
        var logMessage = $"{DateTime.Now}: {message}";
        if (ex != null)
        {
            logMessage += $" - {ex.Message}";
        }
        File.AppendAllText(ErrorLogPath, logMessage + "\n");
    }

    public static void Main(string[] args)
    {
        var taskRepository = new TaskRepository();

        while (true)
        {
            try
            {
                Console.WriteLine("\nTask Manager Menu:");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. List Tasks");
                Console.WriteLine("3. Find Task");
                Console.WriteLine("4. Delete Task");
                Console.WriteLine("5. Edit Task");
                Console.WriteLine("6. Filter Tasks");
                Console.WriteLine("7. Exit");

                Console.Write("\nEnter your choice: ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        AddTask(taskRepository);
                        break;

                    case "2":
                        ListTasks(taskRepository);
                        break;

                    case "3":
                        FindTask(taskRepository);
                        break;

                    case "4":
                        DeleteTask(taskRepository);
                        break;

                    case "5":
                        EditTask(taskRepository);
                        break;

                    case "6":
                        FilterTasks(taskRepository);
                        break;

                    case "7":
                        Console.WriteLine("Exiting Task Manager.");
                        return;

                    default:
                        Console.WriteLine("Error: Invalid choice. Please enter a number between 1 and 7.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred. Please try again.");
                LogError("Unexpected error in main menu", ex);
            }
        }
    }

    private static void AddTask(TaskRepository repository)
    {
        try
        {
            // Validate description
            string description;
            do
            {
                Console.Write("Enter task description (10-100 characters): ");
                description = Console.ReadLine() ?? "";
                
                if (string.IsNullOrWhiteSpace(description))
                {
                    Console.WriteLine("Error: Description cannot be empty.");
                }
                else if (description.Length < 10)
                {
                    Console.WriteLine("Error: Description must be at least 10 characters long.");
                }
                else if (description.Length > 100)
                {
                    Console.WriteLine("Error: Description cannot exceed 100 characters.");
                }
            } while (string.IsNullOrWhiteSpace(description) || description.Length < 10 || description.Length > 100);

            // Get priority
            Console.WriteLine("\nSelect Priority:");
            Console.WriteLine("1. Alta");
            Console.WriteLine("2. Media");
            Console.WriteLine("3. Baja");
            Console.Write("Enter choice (1-3): ");
            Priority priority;
            while (true)
            {
                string input = Console.ReadLine() ?? "";
                priority = input switch
                {
                    "1" => Priority.Alta,
                    "2" => Priority.Media,
                    "3" => Priority.Baja,
                    _ => Priority.Media
                };
                if (input != "1" && input != "2" && input != "3")
                {
                    Console.WriteLine("Error: Invalid priority. Please enter 1, 2, or 3.");
                    continue;
                }
                break;
            }

            // Get due date
            Console.Write("\nAdd due date? (y/n): ");
            DateTime? dueDate = null;
            if ((Console.ReadLine() ?? "").ToLower() == "y")
            {
                bool validDate = false;
                do
                {
                    Console.Write("Enter due date (dd/MM/yyyy): ");
                    string dateInput = Console.ReadLine() ?? "";
                    
                    if (DateTime.TryParse(dateInput, out DateTime date))
                    {
                        if (date < DateTime.Now)
                        {
                            Console.WriteLine("Error: Due date cannot be in the past.");
                        }
                        else
                        {
                            dueDate = date;
                            validDate = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid date format. Please use dd/MM/yyyy format.");
                    }
                } while (!validDate);
            }

            // Get categories
            List<string> categories = new();
            while (true)
            {
                Console.Write("\nAdd a category? (y/n): ");
                if ((Console.ReadLine() ?? "").ToLower() != "y") break;

                Console.Write("Enter category name: ");
                string category = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(category))
                {
                    Console.WriteLine("Error: Category name cannot be empty.");
                    continue;
                }
                categories.Add(category);
            }

            repository.AddTask(description, TaskStatus.Pending, priority, dueDate, categories);
            Console.WriteLine("Task added successfully.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error adding task: {ex.Message}");
            LogError("Error adding task", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An unexpected error occurred while adding the task.");
            LogError("Unexpected error adding task", ex);
        }
    }

    private static void ListTasks(TaskRepository repository)
    {
        try
        {
            var tasks = repository.ListTasks(null, null, null, true, null, 100, 1);
            if (!tasks.Any())
            {
                Console.WriteLine("No tasks found.");
                return;
            }

            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while listing tasks.");
            LogError("Error listing tasks", ex);
        }
    }

    private static void FindTask(TaskRepository repository)
    {
        try
        {
            Console.Write("Enter task ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Error: Invalid task ID. Please enter a number.");
                return;
            }

            var task = repository.FindTask(id);
            if (task != null)
            {
                Console.WriteLine(task);
            }
            else
            {
                Console.WriteLine($"Error: Task with ID {id} not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while finding the task.");
            LogError("Error finding task", ex);
        }
    }

    private static void DeleteTask(TaskRepository repository)
    {
        try
        {
            Console.Write("Enter task ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Error: Invalid task ID. Please enter a number.");
                return;
            }

            Console.Write($"Are you sure you want to delete task {id}? (y/n): ");
            if ((Console.ReadLine() ?? "").ToLower() != "y")
            {
                Console.WriteLine("Delete operation cancelled.");
                return;
            }

            if (repository.DeleteTask(id, false))
            {
                Console.WriteLine("Task deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Error: Task with ID {id} not found or could not be deleted.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while deleting the task.");
            LogError("Error deleting task", ex);
        }
    }

    private static void EditTask(TaskRepository repository)
    {
        try
        {
            Console.Write("Enter task ID to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Error: Invalid task ID. Please enter a number.");
                return;
            }

            var task = repository.FindTask(id);
            if (task == null)
            {
                Console.WriteLine($"Error: Task with ID {id} not found.");
                return;
            }

            Console.WriteLine("\nCurrent task details:");
            Console.WriteLine(task);

            // Update description
            Console.Write("\nEnter new description (10-100 characters, or press Enter to keep current): ");
            string description = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(description))
            {
                if (description.Length < 10 || description.Length > 100)
                {
                    Console.WriteLine("Error: Description must be between 10 and 100 characters. Description not updated.");
                }
                else
                {
                    task.Description = description;
                }
            }

            // Update status
            Console.WriteLine("\nSelect new status:");
            Console.WriteLine("1. Pending");
            Console.WriteLine("2. In Progress");
            Console.WriteLine("3. Completed");
            Console.WriteLine("4. Cancelled");
            Console.Write("Enter choice (1-4) or press Enter to keep current: ");
            string statusChoice = Console.ReadLine() ?? "";
            task.Status = statusChoice switch
            {
                "1" => TaskStatus.Pending,
                "2" => TaskStatus.InProgress,
                "3" => TaskStatus.Completed,
                "4" => TaskStatus.Cancelled,
                _ => task.Status
            };

            // Update priority
            Console.WriteLine("\nSelect new priority:");
            Console.WriteLine("1. Alta");
            Console.WriteLine("2. Media");
            Console.WriteLine("3. Baja");
            Console.Write("Enter choice (1-3) or press Enter to keep current: ");
            string priorityChoice = Console.ReadLine() ?? "";
            task.Priority = priorityChoice switch
            {
                "1" => Priority.Alta,
                "2" => Priority.Media,
                "3" => Priority.Baja,
                _ => task.Priority
            };

            // Update due date
            Console.Write("\nUpdate due date? (y/n): ");
            if ((Console.ReadLine() ?? "").ToLower() == "y")
            {
                Console.Write("Enter new due date (dd/MM/yyyy) or 'clear' to remove: ");
                string dueDateInput = Console.ReadLine() ?? "";
                if (dueDateInput.ToLower() == "clear")
                {
                    task.DueDate = null;
                }
                else if (DateTime.TryParse(dueDateInput, out DateTime date))
                {
                    if (date < DateTime.Now)
                    {
                        Console.WriteLine("Error: Due date cannot be in the past. Due date not updated.");
                    }
                    else
                    {
                        task.DueDate = date;
                    }
                }
                else
                {
                    Console.WriteLine("Error: Invalid date format. Due date not updated.");
                }
            }

            // Update categories
            Console.Write("\nUpdate categories? (y/n): ");
            if ((Console.ReadLine() ?? "").ToLower() == "y")
            {
                task.Categories.Clear();
                while (true)
                {
                    Console.Write("Add a category? (y/n): ");
                    if ((Console.ReadLine() ?? "").ToLower() != "y") break;

                    Console.Write("Enter category name: ");
                    string category = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        Console.WriteLine("Error: Category name cannot be empty.");
                        continue;
                    }
                    task.Categories.Add(category);
                }
            }

            try
            {
                repository.UpdateTaskStatus(task.Id, task.Status);
                repository.UpdateTaskPriority(task.Id, task.Priority);
                repository.UpdateTaskDueDate(task.Id, task.DueDate);
                
                if (task.Categories.Any())
                {
                    foreach (var category in task.Categories)
                    {
                        repository.AddTaskCategory(task.Id, category);
                    }
                }
                Console.WriteLine("Task updated successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error updating task: {ex.Message}");
                LogError($"Error updating task {task.Id}", ex);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An unexpected error occurred while editing the task.");
            LogError("Unexpected error editing task", ex);
        }
    }

    private static void FilterTasks(TaskRepository repository)
    {
        try
        {
            Console.WriteLine("\nFilter Options:");
            
            // Get status filter
            Console.WriteLine("\nSelect status filter:");
            Console.WriteLine("0. All");
            Console.WriteLine("1. Pending");
            Console.WriteLine("2. In Progress");
            Console.WriteLine("3. Completed");
            Console.WriteLine("4. Cancelled");
            Console.Write("Enter choice (0-4): ");
            string statusChoice = Console.ReadLine() ?? "";
            TaskStatus? statusFilter = statusChoice switch
            {
                "1" => TaskStatus.Pending,
                "2" => TaskStatus.InProgress,
                "3" => TaskStatus.Completed,
                "4" => TaskStatus.Cancelled,
                "0" => null,
                _ => null
            };
            if (statusChoice != "0" && !new[] { "1", "2", "3", "4" }.Contains(statusChoice))
            {
                Console.WriteLine("Warning: Invalid status choice. Showing all statuses.");
            }

            // Get search term
            Console.Write("\nEnter search term (or press Enter to skip): ");
            string searchTerm = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = null;
            }

            // Get completed tasks preference
            Console.Write("\nShow completed tasks? (y/n): ");
            bool includeCompleted = (Console.ReadLine() ?? "").ToLower() == "y";

            var tasks = repository.ListTasks(statusFilter, searchTerm, null, includeCompleted, null, 100, 1);
            
            if (!tasks.Any())
            {
                Console.WriteLine("No tasks found matching the filters.");
                return;
            }

            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while filtering tasks.");
            LogError("Error filtering tasks", ex);
        }
    }
}
