using TaskManagementApi.Data.Repositories.Interfaces;
using TaskManagementApi.models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TaskManagementApi.Data.Repositories
{
    public class TaskRepository(AppDbContext context) : ITaskRepository
    {
        public async Task<TaskItem?> GetTaskByIdAsync(int taskId, int userId)
        {
            return await context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.CreatedBy == userId);
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByBoardIdAsync(int boardId)
        {
            return await context.Tasks
                .Where(t => t.BoardId == boardId)
                .ToListAsync();
        }

        public async Task AddTaskAsync(TaskItem task) => await context.Tasks.AddAsync(task);

        public void UpdateTask(TaskItem task) => context.Tasks.Update(task);

        public void DeleteTask(TaskItem task) => context.Tasks.Remove(task);

        public async Task<bool> SaveChangesAsync() => await context.SaveChangesAsync() > 0;
    }
}