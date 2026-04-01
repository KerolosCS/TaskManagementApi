using TaskManagementApi.models;

namespace TaskManagementApi.Data.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetTaskByIdAsync(int taskId, int userId);
        Task<IEnumerable<TaskItem>> GetTasksByBoardIdAsync(int boardId);
        Task AddTaskAsync(TaskItem task);
        void UpdateTask(TaskItem task);
        void DeleteTask(TaskItem task);
        Task<bool> SaveChangesAsync();
    }
}
