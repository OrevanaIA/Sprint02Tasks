using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

public enum Priority
{
    Alta,
    Media,
    Baja
}

public enum TaskStatus
{
    Pending,
    InProgress,
    Completed,
    Cancelled
}

public class TaskItem
{
    
    public int Id { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(100, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 100 characters.")]
    public string Description { get; set; } = string.Empty;

    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    public DateTime CreationDate { get; set; } = DateTime.Now;
    
    public DateTime? DueDate { get; set; }
    
    public Priority Priority { get; set; } = Priority.Media;
    
    public DateTime LastModifiedDate { get; set; } = DateTime.Now;
    
    public List<string> Categories { get; set; } = new List<string>();

    public TaskItem(int id, string description, TaskStatus status)
    {
        Id = id;
        Description = description;
        Status = status;
        CreationDate = DateTime.Now;
        LastModifiedDate = DateTime.Now;
    }

    public override string ToString()
    {
        var dueDate = DueDate.HasValue ? $", Due: {DueDate.Value}" : "";
        var categories = Categories.Any() ? $", Categories: {string.Join(", ", Categories)}" : "";
        return $"Task ID: {Id}, Description: {Description}, Status: {Status}, Priority: {Priority}, Created: {CreationDate}, Last Modified: {LastModifiedDate}{dueDate}{categories}";
    }
}
