# Sprint02Tasks - Task Management Application

## Description
Sprint02Tasks is a robust task management application built with C# that allows users to create, manage, and track tasks efficiently. The application provides a user-friendly interface for task management with features like task status tracking, priority management, and categorization.

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

## Project Structure
```
Sprint02Tasks/
├── Program.cs              # Application entry point
├── TaskItem.cs            # Task model definition
├── TaskRepository.cs      # Data persistence and task operations
├── tasks.json            # Task storage file
└── Tests/                # Unit tests directory
    ├── TaskItemTests.cs     # Tests for TaskItem class
    └── TaskRepositoryTests.cs # Tests for TaskRepository class
```

## Classes

### TaskItem
Represents a single task with properties:
- Id: Unique identifier
- Description: Task description (10-100 characters)
- Status: Current task status
- Priority: Task priority level
- Categories: List of task categories
- CreationDate: When the task was created
- LastModifiedDate: Last modification timestamp
- DueDate: Optional due date

### TaskRepository
Handles task data management:
- AddTask: Creates new tasks
- UpdateTaskStatus: Changes task status
- UpdateTaskPriority: Modifies task priority
- AddTaskCategory: Adds categories to tasks
- DeleteTask: Removes tasks
- FindTask: Retrieves specific tasks
- ListTasks: Returns filtered task lists

## Requirements
- .NET 6.0 or higher
- Visual Studio 2022 or compatible IDE
- NuGet package manager

## Installation
1. Clone the repository
2. Open the solution in Visual Studio
3. Restore NuGet packages
4. Build the solution

## Usage
```csharp
// Create a new task repository
var repository = new TaskRepository();

// Add a new task
repository.AddTask(
    description: "Complete project documentation",
    status: TaskStatus.Pending,
    priority: Priority.Alta,
    dueDate: DateTime.Now.AddDays(7),
    categories: new List<string> { "Documentation", "Important" }
);

// Update task status
repository.UpdateTaskStatus(taskId: 1, status: TaskStatus.Completed);

// List all pending tasks
var pendingTasks = repository.ListTasks(statusFilter: TaskStatus.Pending);
```

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

## Best Practices
- SOLID principles implementation
- Clean Code architecture
- Comprehensive unit testing
- Exception handling
- Data validation
- Thread-safe operations

## Contributing
1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License
This project is licensed under the MIT License - see the LICENSE file for details.
