using System;
using System.Collections.Generic;
using Sprint02Tasks.DTOs;
using Sprint02Tasks.Interfaces;

namespace Sprint02Tasks.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskValidator _taskValidator;

        public TaskService(IUnitOfWork unitOfWork, ITaskValidator taskValidator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _taskValidator = taskValidator ?? throw new ArgumentNullException(nameof(taskValidator));
        }

        public TaskDTO CreateTask(TaskDTO taskDto)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _taskValidator.ValidateTask(taskDto);

                _unitOfWork.TaskRepository.Add(taskDto);
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();

                return taskDto;
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public TaskDTO GetTask(int id)
        {
            return _unitOfWork.TaskRepository.GetById(id);
        }

        public IEnumerable<TaskDTO> GetAllTasks()
        {
            return _unitOfWork.TaskRepository.GetAll();
        }

        public IEnumerable<TaskDTO> GetTasksByStatus(TaskStatus status)
        {
            return _unitOfWork.TaskRepository.GetByStatus(status);
        }

        public IEnumerable<TaskDTO> GetTasksByPriority(Priority priority)
        {
            return _unitOfWork.TaskRepository.GetByPriority(priority);
        }

        public IEnumerable<TaskDTO> SearchTasks(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentException("Search term cannot be empty", nameof(searchTerm));
            }

            return _unitOfWork.TaskRepository.Search(searchTerm);
        }

        public void UpdateTask(TaskDTO taskDto)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _taskValidator.ValidateTask(taskDto);

                _unitOfWork.TaskRepository.Update(taskDto);
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public void DeleteTask(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _unitOfWork.TaskRepository.Delete(id);
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public void AddCategoryToTask(int taskId, string category)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                if (string.IsNullOrWhiteSpace(category))
                {
                    throw new ArgumentException("Category cannot be empty", nameof(category));
                }

                _unitOfWork.TaskRepository.AddCategory(taskId, category);
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public void UpdateTaskStatus(int taskId, TaskStatus status)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _unitOfWork.TaskRepository.UpdateStatus(taskId, status);
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public void UpdateTaskPriority(int taskId, Priority priority)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _unitOfWork.TaskRepository.UpdatePriority(taskId, priority);
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
