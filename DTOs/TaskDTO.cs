using System;
using System.Collections.Generic;

namespace Sprint02Tasks.DTOs
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public Priority Priority { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public List<string> Categories { get; set; } = new List<string>();

        public static TaskDTO FromEntity(TaskItem entity)
        {
            return new TaskDTO
            {
                Id = entity.Id,
                Description = entity.Description,
                Status = entity.Status,
                Priority = entity.Priority,
                CreationDate = entity.CreationDate,
                LastModifiedDate = entity.LastModifiedDate,
                DueDate = entity.DueDate,
                Categories = new List<string>(entity.Categories)
            };
        }

        public TaskItem ToEntity()
        {
            var task = new TaskItem(Id, Description, Status)
            {
                Priority = Priority,
                DueDate = DueDate
            };
            task.Categories.AddRange(Categories);
            return task;
        }
    }
}
