using System;
using System.Collections.Generic;
using Sprint02Tasks.DTOs;

namespace Sprint02Tasks.Interfaces
{
    public interface ITaskRepository
    {
        TaskDTO GetById(int id);
        IEnumerable<TaskDTO> GetAll();
        IEnumerable<TaskDTO> GetByStatus(TaskStatus status);
        IEnumerable<TaskDTO> Search(string searchTerm);
        void Add(TaskDTO task);
        void Update(TaskDTO task);
        void Delete(int id);
        IEnumerable<TaskDTO> GetByPriority(Priority priority);
        void AddCategory(int taskId, string category);
        void UpdateStatus(int taskId, TaskStatus status);
        void UpdatePriority(int taskId, Priority priority);
    }
}
