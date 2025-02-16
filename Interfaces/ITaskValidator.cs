using Sprint02Tasks.DTOs;

namespace Sprint02Tasks.Interfaces
{
    public interface ITaskValidator
    {
        void ValidateTask(TaskDTO task);
        void ValidateDescription(string description);
        void ValidateStatus(TaskStatus status);
        void ValidatePriority(Priority priority);
        void ValidateCategory(string category);
    }
}
