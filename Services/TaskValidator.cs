using System;
using Sprint02Tasks.DTOs;
using Sprint02Tasks.Interfaces;

namespace Sprint02Tasks.Services
{
    public class TaskValidator : ITaskValidator
    {
        private const int MinDescriptionLength = 10;
        private const int MaxDescriptionLength = 100;

        public void ValidateTask(TaskDTO task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            ValidateDescription(task.Description);
            ValidateStatus(task.Status);
            ValidatePriority(task.Priority);

            if (task.Categories != null)
            {
                foreach (var category in task.Categories)
                {
                    ValidateCategory(category);
                }
            }

            if (task.DueDate.HasValue && task.DueDate.Value < DateTime.Now.Date)
            {
                throw new ArgumentException("Due date cannot be in the past");
            }
        }

        public void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be empty", nameof(description));
            }

            if (description.Length < MinDescriptionLength || description.Length > MaxDescriptionLength)
            {
                throw new ArgumentException(
                    $"Description must be between {MinDescriptionLength} and {MaxDescriptionLength} characters",
                    nameof(description));
            }
        }

        public void ValidateStatus(TaskStatus status)
        {
            if (!Enum.IsDefined(typeof(TaskStatus), status))
            {
                throw new ArgumentException("Invalid task status", nameof(status));
            }
        }

        public void ValidatePriority(Priority priority)
        {
            if (!Enum.IsDefined(typeof(Priority), priority))
            {
                throw new ArgumentException("Invalid priority level", nameof(priority));
            }
        }

        public void ValidateCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("Category cannot be empty", nameof(category));
            }

            if (category.Length > 50)
            {
                throw new ArgumentException("Category name cannot exceed 50 characters", nameof(category));
            }
        }
    }
}
