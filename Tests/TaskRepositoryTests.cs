using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Sprint02Tasks.Tests
{
    [TestClass]
    public class TaskRepositoryTests
    {
        private string testJsonPath;
        private TaskRepository repository;

        [TestInitialize]
        public void Setup()
        {
            testJsonPath = Path.Combine(Path.GetTempPath(), "test_tasks.json");
            if (File.Exists(testJsonPath))
            {
                File.Delete(testJsonPath);
            }
            repository = new TaskRepository();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(testJsonPath))
            {
                File.Delete(testJsonPath);
            }
        }

        [TestMethod]
        public void AddTask_ValidParameters_TaskAddedSuccessfully()
        {
            // Arrange
            string description = "Test task description";
            TaskStatus status = TaskStatus.Pending;
            Priority priority = Priority.Alta;
            DateTime dueDate = DateTime.Now.AddDays(7);
            var categories = new List<string> { "Test Category" };

            // Act
            repository.AddTask(description, status, priority, dueDate, categories);
            var tasks = repository.ListTasks();
            var addedTask = tasks.FirstOrDefault();

            // Assert
            Assert.IsNotNull(addedTask);
            Assert.AreEqual(1, tasks.Count);
            Assert.AreEqual(description, addedTask.Description);
            Assert.AreEqual(status, addedTask.Status);
            Assert.AreEqual(priority, addedTask.Priority);
            Assert.AreEqual(dueDate.Date, addedTask.DueDate?.Date);
            CollectionAssert.AreEqual(categories, addedTask.Categories);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTask_ShortDescription_ThrowsArgumentException()
        {
            // Arrange & Act
            repository.AddTask("Short", TaskStatus.Pending);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTask_InvalidStatus_ThrowsArgumentException()
        {
            // Arrange & Act
            repository.AddTask("Valid task description", (TaskStatus)999);
        }

        [TestMethod]
        public void UpdateTaskStatus_ValidStatus_UpdatesSuccessfully()
        {
            // Arrange
            repository.AddTask("Test task description", TaskStatus.Pending);
            var task = repository.ListTasks().First();

            // Act
            repository.UpdateTaskStatus(task.Id, TaskStatus.Completed);
            var updatedTask = repository.FindTask(task.Id);

            // Assert
            Assert.AreEqual(TaskStatus.Completed, updatedTask.Status);
        }

        [TestMethod]
        public void UpdateTaskPriority_ValidPriority_UpdatesSuccessfully()
        {
            // Arrange
            repository.AddTask("Test task description", TaskStatus.Pending);
            var task = repository.ListTasks().First();

            // Act
            repository.UpdateTaskPriority(task.Id, Priority.Alta);
            var updatedTask = repository.FindTask(task.Id);

            // Assert
            Assert.AreEqual(Priority.Alta, updatedTask.Priority);
        }

        [TestMethod]
        public void AddTaskCategory_ValidCategory_AddsSuccessfully()
        {
            // Arrange
            repository.AddTask("Test task description", TaskStatus.Pending);
            var task = repository.ListTasks().First();
            string category = "Test Category";

            // Act
            repository.AddTaskCategory(task.Id, category);
            var updatedTask = repository.FindTask(task.Id);

            // Assert
            CollectionAssert.Contains(updatedTask.Categories.ToList(), category);
        }

        [TestMethod]
        public void ListTasks_WithFilters_ReturnsFilteredTasks()
        {
            // Arrange
            repository.AddTask("Task 1", TaskStatus.Pending, Priority.Alta);
            repository.AddTask("Task 2", TaskStatus.Completed, Priority.Baja);
            repository.AddTask("Task 3", TaskStatus.Pending, Priority.Media);

            // Act
            var filteredTasks = repository.ListTasks(
                statusFilter: TaskStatus.Pending,
                searchTerm: "Task",
                sortBy: "priority",
                ascending: true
            );

            // Assert
            Assert.AreEqual(2, filteredTasks.Count);
            Assert.IsTrue(filteredTasks.All(t => t.Status == TaskStatus.Pending));
        }

        [TestMethod]
        public void DeleteTask_ExistingTask_DeletesSuccessfully()
        {
            // Arrange
            repository.AddTask("Test task description", TaskStatus.Pending);
            var task = repository.ListTasks().First();

            // Act
            bool result = repository.DeleteTask(task.Id, false);
            var remainingTasks = repository.ListTasks();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, remainingTasks.Count);
        }

        [TestMethod]
        public void DeleteTask_NonExistingTask_ReturnsFalse()
        {
            // Arrange
            int nonExistingId = 999;

            // Act
            bool result = repository.DeleteTask(nonExistingId, false);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FindTask_ExistingTask_ReturnsTask()
        {
            // Arrange
            repository.AddTask("Test task description", TaskStatus.Pending);
            var task = repository.ListTasks().First();

            // Act
            var foundTask = repository.FindTask(task.Id);

            // Assert
            Assert.IsNotNull(foundTask);
            Assert.AreEqual(task.Id, foundTask.Id);
        }

        [TestMethod]
        public void FindTask_NonExistingTask_ReturnsNull()
        {
            // Arrange
            int nonExistingId = 999;

            // Act
            var foundTask = repository.FindTask(nonExistingId);

            // Assert
            Assert.IsNull(foundTask);
        }
    }
}
