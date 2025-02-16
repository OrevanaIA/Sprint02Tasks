﻿using System;
using Sprint02Tasks.DTOs;
using Sprint02Tasks.Infrastructure;
using Sprint02Tasks.Interfaces;

namespace Sprint02Tasks
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\nTask Manager");
                    Console.WriteLine("1. Add Task");
                    Console.WriteLine("2. List Tasks");
                    Console.WriteLine("3. Update Task Status");
                    Console.WriteLine("4. Delete Task");
                    Console.WriteLine("5. Add Category to Task");
                    Console.WriteLine("6. Update Task Priority");
                    Console.WriteLine("7. Search Tasks");
                    Console.WriteLine("8. Exit");
                    Console.Write("Select an option: ");

                    string option = Console.ReadLine();

                    try
                    {
                        unitOfWork.BeginTransaction();

                        switch (option)
                        {
                            case "1":
                                AddTask(unitOfWork);
                                break;
                            case "2":
                                ListTasks(unitOfWork);
                                break;
                            case "3":
                                UpdateTaskStatus(unitOfWork);
                                break;
                            case "4":
                                DeleteTask(unitOfWork);
                                break;
                            case "5":
                                AddCategoryToTask(unitOfWork);
                                break;
                            case "6":
                                UpdateTaskPriority(unitOfWork);
                                break;
                            case "7":
                                SearchTasks(unitOfWork);
                                break;
                            case "8":
                                exit = true;
                                break;
                            default:
                                Console.WriteLine("Invalid option");
                                break;
                        }

                        unitOfWork.CommitTransaction();
                        unitOfWork.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        unitOfWork.RollbackTransaction();
                    }
                }
            }
        }

        static void AddTask(IUnitOfWork unitOfWork)
        {
            Console.Write("Enter task description: ");
            string description = Console.ReadLine();

            Console.WriteLine("Select status:");
            Console.WriteLine("1. Pending");
            Console.WriteLine("2. In Progress");
            Console.WriteLine("3. Completed");
            string statusInput = Console.ReadLine();
            TaskStatus status = statusInput switch
            {
                "1" => TaskStatus.Pending,
                "2" => TaskStatus.InProgress,
                "3" => TaskStatus.Completed,
                _ => TaskStatus.Pending
            };

            Console.WriteLine("Select priority:");
            Console.WriteLine("1. Alta");
            Console.WriteLine("2. Media");
            Console.WriteLine("3. Baja");
            string priorityInput = Console.ReadLine();
            Priority priority = priorityInput switch
            {
                "1" => Priority.Alta,
                "2" => Priority.Media,
                "3" => Priority.Baja,
                _ => Priority.Media
            };

            var taskDto = new TaskDTO
            {
                Id = DateTime.Now.Ticks.GetHashCode(),
                Description = description,
                Status = status,
                Priority = priority,
                CreationDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            };

            unitOfWork.TaskRepository.Add(taskDto);
            Console.WriteLine("Task added successfully");
        }

        static void ListTasks(IUnitOfWork unitOfWork)
        {
            var tasks = unitOfWork.TaskRepository.GetAll();
            foreach (var task in tasks)
            {
                Console.WriteLine($"\nID: {task.Id}");
                Console.WriteLine($"Description: {task.Description}");
                Console.WriteLine($"Status: {task.Status}");
                Console.WriteLine($"Priority: {task.Priority}");
                Console.WriteLine($"Created: {task.CreationDate}");
                if (task.Categories.Count > 0)
                {
                    Console.WriteLine($"Categories: {string.Join(", ", task.Categories)}");
                }
                Console.WriteLine("------------------------");
            }
        }

        static void UpdateTaskStatus(IUnitOfWork unitOfWork)
        {
            Console.Write("Enter task ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Select new status:");
                Console.WriteLine("1. Pending");
                Console.WriteLine("2. In Progress");
                Console.WriteLine("3. Completed");
                string statusInput = Console.ReadLine();
                TaskStatus status = statusInput switch
                {
                    "1" => TaskStatus.Pending,
                    "2" => TaskStatus.InProgress,
                    "3" => TaskStatus.Completed,
                    _ => TaskStatus.Pending
                };

                unitOfWork.TaskRepository.UpdateStatus(id, status);
                Console.WriteLine("Task status updated successfully");
            }
            else
            {
                Console.WriteLine("Invalid task ID");
            }
        }

        static void DeleteTask(IUnitOfWork unitOfWork)
        {
            Console.Write("Enter task ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                unitOfWork.TaskRepository.Delete(id);
                Console.WriteLine("Task deleted successfully");
            }
            else
            {
                Console.WriteLine("Invalid task ID");
            }
        }

        static void AddCategoryToTask(IUnitOfWork unitOfWork)
        {
            Console.Write("Enter task ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.Write("Enter category name: ");
                string category = Console.ReadLine();
                unitOfWork.TaskRepository.AddCategory(id, category);
                Console.WriteLine("Category added successfully");
            }
            else
            {
                Console.WriteLine("Invalid task ID");
            }
        }

        static void UpdateTaskPriority(IUnitOfWork unitOfWork)
        {
            Console.Write("Enter task ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Select new priority:");
                Console.WriteLine("1. Alta");
                Console.WriteLine("2. Media");
                Console.WriteLine("3. Baja");
                string priorityInput = Console.ReadLine();
                Priority priority = priorityInput switch
                {
                    "1" => Priority.Alta,
                    "2" => Priority.Media,
                    "3" => Priority.Baja,
                    _ => Priority.Media
                };

                unitOfWork.TaskRepository.UpdatePriority(id, priority);
                Console.WriteLine("Task priority updated successfully");
            }
            else
            {
                Console.WriteLine("Invalid task ID");
            }
        }

        static void SearchTasks(IUnitOfWork unitOfWork)
        {
            Console.Write("Enter search term: ");
            string searchTerm = Console.ReadLine();
            var tasks = unitOfWork.TaskRepository.Search(searchTerm);
            foreach (var task in tasks)
            {
                Console.WriteLine($"\nID: {task.Id}");
                Console.WriteLine($"Description: {task.Description}");
                Console.WriteLine($"Status: {task.Status}");
                Console.WriteLine($"Priority: {task.Priority}");
                Console.WriteLine("------------------------");
            }
        }
    }
}
