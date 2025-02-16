# Sprint02Tasks - Task Management Application

## Description
Sprint02Tasks is a robust task management application built with C# that implements clean architecture and design patterns. The application provides a comprehensive task management system with features like task status tracking, priority management, and categorization.

## Architecture & Design Patterns

### Repository Pattern
- `ITaskRepository` interface defines the contract for task operations
- Implementation in `TaskRepository` class provides data access logic
- Separates business logic from data access concerns
- Enables easy switching between different data storage implementations

### Unit of Work Pattern
- `IUnitOfWork` interface manages transactions and data persistence
- Ensures data consistency across operations
- Provides transaction management with Begin/Commit/Rollback capabilities
- Centralizes data access through repository management

### DTO Pattern
- `TaskDTO` handles data transfer between layers
- Separates domain models from data transfer objects
- Provides clean data contracts for external communication
- Reduces coupling between layers

## Features
- Create new tasks with descriptions
- Set and update task status (Pending, In Progress, Completed)
- Assign priority levels (Alta, Media, Baja)
- Add and manage task categories
- Filter tasks by status
- Search tasks by description
- Task validation (minimum description length)
- Due date management
- Persistent storage using JSON
- Transaction management
- Thread-safe operations

## Project Structure
```
Sprint02Tasks/
├── DTOs/
│   └── TaskDTO.cs              # Data Transfer Objects
├── Infrastructure/
│   ├── TaskRepository.cs       # Repository implementation
│   └── UnitOfWork.cs          # Unit of Work implementation
├── Interfaces/
│   ├── ITaskRepository.cs     # Repository interface
│   └── IUnitOfWork.cs        # Unit of Work interface
├── Program.cs                 # Application entry point
├── TaskItem.cs               # Domain model
└── Tests/                    # Unit tests directory
    ├── TaskItemTests.cs      # Tests for TaskItem
    └── TaskRepositoryTests.cs # Tests for TaskRepository
```

## Requirements
- .NET 6.0 or higher
- Visual Studio 2022 or compatible IDE
- NuGet package manager

## Installation
1. Clone the repository
2. Open the solution in Visual Studio
3. Restore NuGet packages
4. Build the solution

## Usage Example
```csharp
// Using Unit of Work pattern
using (IUnitOfWork unitOfWork = new UnitOfWork())
{
    try
    {
        // Begin transaction
        unitOfWork.BeginTransaction();

        // Create a new task using DTO
        var taskDto = new TaskDTO
        {
            Description = "Complete project documentation",
            Status = TaskStatus.Pending,
            Priority = Priority.Alta,
            DueDate = DateTime.Now.AddDays(7)
        };

        // Add task through repository
        unitOfWork.TaskRepository.Add(taskDto);

        // Commit transaction and save changes
        unitOfWork.CommitTransaction();
        unitOfWork.SaveChanges();
    }
    catch (Exception)
    {
        // Rollback on error
        unitOfWork.RollbackTransaction();
        throw;
    }
}
```

## Design Patterns Benefits
1. **Repository Pattern**
   - Centralizes data access logic
   - Enables unit testing with mock repositories
   - Provides consistent data access interface

2. **Unit of Work Pattern**
   - Maintains data consistency
   - Manages transactions
   - Groups related operations

3. **DTO Pattern**
   - Reduces over-posting vulnerabilities
   - Separates domain logic from data transfer
   - Provides clear contracts for data exchange

## Testing
The project includes comprehensive unit tests using MSTest framework:
- TaskItemTests: Validates task model behavior
- TaskRepositoryTests: Ensures correct data operations

To run tests:
1. Open Test Explorer in Visual Studio
2. Click "Run All Tests"

## Data Persistence
Tasks are stored in a JSON file (`tasks.json`) with the following structure:
```json
[
  {
    "id": 1,
    "description": "Task description",
    "status": "Pending",
    "priority": "Alta",
    "categories": ["Category1", "Category2"],
    "creationDate": "2024-02-16T14:30:00Z",
    "lastModifiedDate": "2024-02-16T14:30:00Z",
    "dueDate": "2024-02-23T14:30:00Z"
  }
]
```
## Mejoras Implementadas

### 1. Optimizaciones de Rendimiento
- **Caché de Tareas**
  - Implementación de ICacheService para tareas frecuentes
  - Invalidación automática de caché
  - Gestión eficiente de memoria

- **Paginación**
  - PaginationParams para grandes conjuntos de datos
  - Límites configurables por página
  - Ordenamiento optimizado

- **Consultas Optimizadas**
  - Métodos asíncronos en ITaskRepository
  - Búsquedas indexadas
  - Carga eficiente de datos

### 2. Seguridad
- **Validación de Datos**
  - InputSanitizer para prevención de inyección
  - Validación robusta de entradas
  - Sanitización de salida

- **Logging y Auditoría**
  - ISecurityLogger para operaciones críticas
  - Registro de modificaciones
  - Monitoreo de seguridad

### 3. Documentación
- Comentarios XML detallados
- Ejemplos de uso
- Manejo de excepciones
- Casos especiales

## Estructura

### Interfaces
- `ITaskService`: Gestión de tareas
- `ITaskRepository`: Persistencia y caché
- `ICacheService`: Manejo de caché
- `ISecurityLogger`: Logging de seguridad
- `ITaskValidator`: Validación de datos

### Modelos
- `TaskDTO`: Transferencia de datos
- `PaginationParams`: Configuración de paginación
- `TaskItem`: Entidad principal

### Seguridad
- `InputSanitizer`: Sanitización de datos
- Validación de entrada/salida
- Prevención de XSS

## Uso

### Ejemplo con Caché y Paginación
```csharp
// Obtener tareas con paginación
var paginationParams = new PaginationParams(1, 10);
var tasks = await repository.GetAllPagedAsync(paginationParams);

// Usar caché
var cachedTask = await cacheService.GetAsync<TaskDTO>(taskId);
```

### Ejemplo de Seguridad
```csharp
// Sanitizar entrada
var description = InputSanitizer.SanitizeTaskDescription(input);

// Logging de seguridad
await securityLogger.LogOperationAsync(
    "UpdateTask",
    $"Task {taskId} updated",
    userId
);
```

## Configuración
- .NET 6.0+
- SQL Server
- Redis (opcional)

## Pruebas
- Unitarias
- Integración
- Rendimiento
- Seguridad

## Validation Rules
- Task description must be between 10 and 100 characters
- Task status must be a valid TaskStatus enum value
- Priority must be a valid Priority enum value
- Task ID must be unique
- Creation date and last modified date are automatically managed

## Error Handling
The application includes robust error handling for:
- Invalid task parameters
- File I/O operations
- Data validation
- Concurrent access to task data
- Transaction management

## Best Practices
- SOLID principles implementation
- Clean Code architecture
- Design patterns usage
- Comprehensive unit testing
- Exception handling
- Data validation
- Thread-safe operations
- Transaction management

## Contributing
1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License
This project is licensed under the MIT License - see the LICENSE file for details.
