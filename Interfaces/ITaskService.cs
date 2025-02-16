using System;
using System.Collections.Generic;
using Sprint02Tasks.DTOs;

namespace Sprint02Tasks.Interfaces
{
    public interface ITaskService
    {
        TaskDTO CreateTask(TaskDTO taskDto);
        TaskDTO GetTask(int id);
        IEnumerable<TaskDTO> GetAllTasks();
        IEnumerable<TaskDTO> GetTasksByStatus(TaskStatus status);
        IEnumerable<TaskDTO> GetTasksByPriority(Priority priority);
        IEnumerable<TaskDTO> SearchTasks(string searchTerm);
        void UpdateTask(TaskDTO taskDto);
        void DeleteTask(int id);
        void AddCategoryToTask(int taskId, string category);
        void UpdateTaskStatus(int taskId, TaskStatus status);
        void UpdateTaskPriority(int taskId, Priority priority);
    }
}
