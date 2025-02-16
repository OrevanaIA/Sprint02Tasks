using System;
using System.Collections.Generic;
using Sprint02Tasks.DTOs;

namespace Sprint02Tasks.Interfaces
{
    /// <summary>
    /// Interfaz que define los servicios principales para la gestión de tareas en el sistema.
    /// Proporciona operaciones CRUD y funcionalidades adicionales para manipular tareas.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Service Layer, actuando como intermediario
    /// entre la capa de presentación y la capa de acceso a datos.
    /// </remarks>
    /// <example>
    /// Ejemplo de uso básico:
    /// <code>
    /// ITaskService taskService = new TaskService();
    /// var newTask = new TaskDTO { Title = "Nueva tarea", Description = "Descripción" };
    /// var createdTask = taskService.CreateTask(newTask);
    /// </code>
    /// </example>
    public interface ITaskService
    {
        /// <summary>
        /// Crea una nueva tarea en el sistema.
        /// </summary>
        /// <param name="taskDto">DTO con la información de la tarea a crear</param>
        /// <returns>DTO de la tarea creada, incluyendo el ID asignado</returns>
        /// <exception cref="ArgumentNullException">Si taskDto es null</exception>
        /// <exception cref="ValidationException">Si los datos de la tarea no son válidos</exception>
        /// <example>
        /// <code>
        /// var newTask = new TaskDTO 
        /// { 
        ///     Title = "Completar informe",
        ///     Description = "Realizar informe mensual de ventas",
        ///     DueDate = DateTime.Now.AddDays(7)
        /// };
        /// var createdTask = taskService.CreateTask(newTask);
        /// </code>
        /// </example>
        TaskDTO CreateTask(TaskDTO taskDto);

        /// <summary>
        /// Obtiene una tarea específica por su ID.
        /// </summary>
        /// <param name="id">ID de la tarea a obtener</param>
        /// <returns>DTO de la tarea encontrada</returns>
        /// <exception cref="ArgumentException">Si el ID es menor o igual a 0</exception>
        /// <exception cref="NotFoundException">Si no se encuentra la tarea con el ID especificado</exception>
        TaskDTO GetTask(int id);

        /// <summary>
        /// Obtiene todas las tareas existentes en el sistema.
        /// </summary>
        /// <returns>Colección de DTOs de todas las tareas</returns>
        IEnumerable<TaskDTO> GetAllTasks();

        /// <summary>
        /// Obtiene las tareas filtradas por su estado.
        /// </summary>
        /// <param name="status">Estado de las tareas a buscar</param>
        /// <returns>Colección de DTOs de las tareas que coinciden con el estado</returns>
        IEnumerable<TaskDTO> GetTasksByStatus(TaskStatus status);

        /// <summary>
        /// Obtiene las tareas filtradas por su prioridad.
        /// </summary>
        /// <param name="priority">Prioridad de las tareas a buscar</param>
        /// <returns>Colección de DTOs de las tareas que coinciden con la prioridad</returns>
        IEnumerable<TaskDTO> GetTasksByPriority(Priority priority);

        /// <summary>
        /// Busca tareas que contengan el término especificado en su título o descripción.
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Colección de DTOs de las tareas que coinciden con la búsqueda</returns>
        /// <exception cref="ArgumentNullException">Si searchTerm es null</exception>
        IEnumerable<TaskDTO> SearchTasks(string searchTerm);

        /// <summary>
        /// Actualiza una tarea existente.
        /// </summary>
        /// <param name="taskDto">DTO con la información actualizada de la tarea</param>
        /// <exception cref="ArgumentNullException">Si taskDto es null</exception>
        /// <exception cref="NotFoundException">Si no se encuentra la tarea a actualizar</exception>
        /// <exception cref="ValidationException">Si los datos actualizados no son válidos</exception>
        void UpdateTask(TaskDTO taskDto);

        /// <summary>
        /// Elimina una tarea por su ID.
        /// </summary>
        /// <param name="id">ID de la tarea a eliminar</param>
        /// <exception cref="ArgumentException">Si el ID es menor o igual a 0</exception>
        /// <exception cref="NotFoundException">Si no se encuentra la tarea a eliminar</exception>
        void DeleteTask(int id);

        /// <summary>
        /// Añade una categoría a una tarea existente.
        /// </summary>
        /// <param name="taskId">ID de la tarea</param>
        /// <param name="category">Categoría a añadir</param>
        /// <exception cref="ArgumentException">Si el ID es menor o igual a 0 o la categoría es null o vacía</exception>
        /// <exception cref="NotFoundException">Si no se encuentra la tarea</exception>
        void AddCategoryToTask(int taskId, string category);

        /// <summary>
        /// Actualiza el estado de una tarea.
        /// </summary>
        /// <param name="taskId">ID de la tarea</param>
        /// <param name="status">Nuevo estado de la tarea</param>
        /// <exception cref="ArgumentException">Si el ID es menor o igual a 0</exception>
        /// <exception cref="NotFoundException">Si no se encuentra la tarea</exception>
        void UpdateTaskStatus(int taskId, TaskStatus status);

        /// <summary>
        /// Actualiza la prioridad de una tarea.
        /// </summary>
        /// <param name="taskId">ID de la tarea</param>
        /// <param name="priority">Nueva prioridad de la tarea</param>
        /// <exception cref="ArgumentException">Si el ID es menor o igual a 0</exception>
        /// <exception cref="NotFoundException">Si no se encuentra la tarea</exception>
        void UpdateTaskPriority(int taskId, Priority priority);
    }
}
