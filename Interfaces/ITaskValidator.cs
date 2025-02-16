using Sprint02Tasks.DTOs;

namespace Sprint02Tasks.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de validación para las tareas y sus componentes.
    /// Implementa el patrón Validator para centralizar y estandarizar las reglas de validación.
    /// </summary>
    /// <remarks>
    /// Esta interfaz es fundamental para mantener la integridad de los datos y
    /// asegurar que todas las tareas cumplan con las reglas de negocio establecidas.
    /// </remarks>
    /// <example>
    /// Ejemplo de uso básico:
    /// <code>
    /// ITaskValidator validator = new TaskValidator();
    /// var task = new TaskDTO { Description = "Nueva tarea" };
    /// validator.ValidateTask(task); // Valida toda la tarea
    /// validator.ValidateDescription("Descripción específica"); // Valida solo la descripción
    /// </code>
    /// </example>
    public interface ITaskValidator
    {
        /// <summary>
        /// Valida una tarea completa, verificando todos sus campos y reglas de negocio.
        /// </summary>
        /// <param name="task">DTO de la tarea a validar</param>
        /// <exception cref="ArgumentNullException">Si task es null</exception>
        /// <exception cref="ValidationException">Si algún campo de la tarea no cumple con las reglas de validación</exception>
        /// <remarks>
        /// Realiza una validación completa de todos los campos de la tarea:
        /// - Descripción no vacía y dentro del límite de caracteres
        /// - Estado válido
        /// - Prioridad válida
        /// - Fecha límite válida (si está establecida)
        /// - Categorías válidas
        /// </remarks>
        void ValidateTask(TaskDTO task);

        /// <summary>
        /// Valida la descripción de una tarea.
        /// </summary>
        /// <param name="description">Descripción a validar</param>
        /// <exception cref="ValidationException">
        /// Si la descripción es null, vacía, solo espacios en blanco,
        /// o excede el límite de caracteres permitido
        /// </exception>
        /// <remarks>
        /// La descripción debe:
        /// - No ser null o vacía
        /// - Tener al menos 3 caracteres
        /// - No exceder 500 caracteres
        /// - No contener solo espacios en blanco
        /// </remarks>
        void ValidateDescription(string description);

        /// <summary>
        /// Valida el estado de una tarea.
        /// </summary>
        /// <param name="status">Estado a validar</param>
        /// <exception cref="ValidationException">Si el estado no es un valor válido del enum TaskStatus</exception>
        /// <remarks>
        /// Verifica que el estado sea uno de los valores definidos en el enum TaskStatus:
        /// - Pending
        /// - InProgress
        /// - Completed
        /// - Cancelled
        /// </remarks>
        void ValidateStatus(TaskStatus status);

        /// <summary>
        /// Valida la prioridad de una tarea.
        /// </summary>
        /// <param name="priority">Prioridad a validar</param>
        /// <exception cref="ValidationException">Si la prioridad no es un valor válido del enum Priority</exception>
        /// <remarks>
        /// Verifica que la prioridad sea uno de los valores definidos en el enum Priority:
        /// - Low
        /// - Medium
        /// - High
        /// </remarks>
        void ValidatePriority(Priority priority);

        /// <summary>
        /// Valida una categoría de tarea.
        /// </summary>
        /// <param name="category">Categoría a validar</param>
        /// <exception cref="ValidationException">
        /// Si la categoría es null, vacía, solo espacios en blanco,
        /// o no cumple con el formato requerido
        /// </exception>
        /// <remarks>
        /// La categoría debe:
        /// - No ser null o vacía
        /// - Tener entre 2 y 50 caracteres
        /// - Contener solo letras, números, espacios y guiones
        /// - No comenzar ni terminar con espacios
        /// </remarks>
        void ValidateCategory(string category);
    }
}
