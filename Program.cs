﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Sprint02Tasks.DTOs;
using Sprint02Tasks.Infrastructure;
using Sprint02Tasks.Interfaces;
using Sprint02Tasks.Services;

namespace Sprint02Tasks
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            ConfigureServices();
            
            using (var scope = _serviceProvider.CreateScope())
            {
                var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
                RunApplication(taskService);
            }

            DisposeServices();
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register services
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITaskValidator, TaskValidator>();

            _serviceProvider = services.BuildServiceProvider();
        }

        private static void RunApplication(ITaskService taskService)
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
                    switch (option)
                    {
                        case "1":
                            AddTask(taskService);
                            break;
                        case "2":
                            ListTasks(taskService);
                            break;
                        case "3":
                            UpdateTaskStatus(taskService);
                            break;
                        case "4":
                            DeleteTask(taskService);
                            break;
                        case "5":
                            AddCategoryToTask(taskService);
                            break;
                        case "6":
                            UpdateTaskPriority(taskService);
                            break;
                        case "7":
                            SearchTasks(taskService);
                            break;
                        case "8":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private static void AddTask(ITaskService taskService)
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

            taskService.CreateTask(taskDto);
            Console.WriteLine("Task added successfully");
        }

        private static void ListTasks(ITaskService taskService)
        {
            var tasks = taskService.GetAllTasks();
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

        private static void UpdateTaskStatus(ITaskService taskService)
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

                taskService.UpdateTaskStatus(id, status);
                Console.WriteLine("Task status updated successfully");
            }
            else
            {
                Console.WriteLine("Invalid task ID");
            }
        }

        private static void DeleteTask(ITaskService taskService)
        {
            Console.Write("Enter task ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                taskService.DeleteTask(id);
                Console.WriteLine("Task deleted successfully");
            }
            else
            {
                Console.WriteLine("Invalid task ID");
            }
        }

        private static void AddCategoryToTask(ITaskService taskService)
        {
            Console.Write("Enter task ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.Write("Enter category name: ");
                string category = Console.ReadLine();
                taskService.AddCategoryToTask(id, category);
                Console.WriteLine("Category added successfully");
            }
            else
            {
                Console.WriteLine("Invalid task ID");
            }
        }

        private static void UpdateTaskPriority(ITaskService taskService)
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

                taskService.UpdateTaskPriority(id, priority);
                Console.WriteLine("Task priority updated successfully");
            }
            else
            {
                Console.WriteLine("Invalid task ID");
            }
        }

        private static void SearchTasks(ITaskService taskService)
        {
            Console.Write("Enter search term: ");
            string searchTerm = Console.ReadLine();
            var tasks = taskService.SearchTasks(searchTerm);
            foreach (var task in tasks)
            {
                Console.WriteLine($"\nID: {task.Id}");
                Console.WriteLine($"Description: {task.Description}");
                Console.WriteLine($"Status: {task.Status}");
                Console.WriteLine($"Priority: {task.Priority}");
                Console.WriteLine("------------------------");
            }
        }

        private static void DisposeServices()
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
