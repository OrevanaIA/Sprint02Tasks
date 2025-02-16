using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Sprint02Tasks.DTOs;
using Sprint02Tasks.Interfaces;

namespace Sprint02Tasks.Infrastructure
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _filePath;
        private List<TaskItem> _tasks;
        private readonly object _lock = new object();

        public TaskRepository(string filePath = "tasks.json")
        {
            _filePath = filePath;
            LoadTasks();
        }

        private void LoadTasks()
        {
            lock (_lock)
            {
                if (System.IO.File.Exists(_filePath))
                {
                    string json = System.IO.File.ReadAllText(_filePath);
                    _tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
                }
                else
                {
                    _tasks = new List<TaskItem>();
                }
            }
        }

        public void SaveChanges()
        {
            lock (_lock)
            {
                string json = JsonSerializer.Serialize(_tasks, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(_filePath, json);
            }
        }

        public async Task SaveChangesAsync()
        {
            string json = JsonSerializer.Serialize(_tasks, new JsonSerializerOptions { WriteIndented = true });
            await System.IO.File.WriteAllTextAsync(_filePath, json);
        }

        public TaskDTO GetById(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            return task != null ? TaskDTO.FromEntity(task) : null;
        }

        public IEnumerable<TaskDTO> GetAll()
        {
            return _tasks.Select(TaskDTO.FromEntity);
        }

        public IEnumerable<TaskDTO> GetByStatus(TaskStatus status)
        {
            return _tasks.Where(t => t.Status == status).Select(TaskDTO.FromEntity);
        }

        public IEnumerable<TaskDTO> Search(string searchTerm)
        {
            return _tasks
                .Where(t => t.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Select(TaskDTO.FromEntity);
        }

        public void Add(TaskDTO taskDto)
        {
            if (taskDto == null) throw new ArgumentNullException(nameof(taskDto));
            
            var task = taskDto.ToEntity();
            if (_tasks.Any(t => t.Id == task.Id))
            {
                throw new InvalidOperationException($"Task with ID {task.Id} already exists");
            }
            
            _tasks.Add(task);
        }

        public void Update(TaskDTO taskDto)
        {
            if (taskDto == null) throw new ArgumentNullException(nameof(taskDto));

            var existingIndex = _tasks.FindIndex(t => t.Id == taskDto.Id);
            if (existingIndex == -1)
            {
                throw new InvalidOperationException($"Task with ID {taskDto.Id} not found");
            }

            _tasks[existingIndex] = taskDto.ToEntity();
        }

        public void Delete(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _tasks.Remove(task);
            }
        }

        public IEnumerable<TaskDTO> GetByPriority(Priority priority)
        {
            return _tasks.Where(t => t.Priority == priority).Select(TaskDTO.FromEntity);
        }

        public void AddCategory(int taskId, string category)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                throw new InvalidOperationException($"Task with ID {taskId} not found");
            }

            if (!task.Categories.Contains(category))
            {
                task.Categories.Add(category);
            }
        }

        public void UpdateStatus(int taskId, TaskStatus status)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                throw new InvalidOperationException($"Task with ID {taskId} not found");
            }

            task.Status = status;
            task.LastModifiedDate = DateTime.Now;
        }

        public void UpdatePriority(int taskId, Priority priority)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                throw new InvalidOperationException($"Task with ID {taskId} not found");
            }

            task.Priority = priority;
            task.LastModifiedDate = DateTime.Now;
        }
    }
}
