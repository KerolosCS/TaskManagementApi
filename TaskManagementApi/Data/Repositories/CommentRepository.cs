using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data.Repositories.Interfaces;
using TaskManagementApi.models;

namespace TaskManagementApi.Data.Repositories
{
    public class CommentRepository(AppDbContext context) : ICommentRepository
    {
        public async Task<Comment?> GetCommentByIdAsync(int id, int userId)
        {
            return await context.Comments
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByTaskIdAsync(int taskId)
        {
            return await context.Comments
                .Include(c => c.User) 
                .Where(c => c.TaskId == taskId)
                .ToListAsync();
        }

        public async Task AddCommentAsync(Comment comment) => await context.Comments.AddAsync(comment);

        public void DeleteComment(Comment comment) => context.Comments.Remove(comment);

        public async Task LoadUserAsync(Comment comment)
        {
            await context.Entry(comment).Reference(c => c.User).LoadAsync();
        }

        public async Task<bool> SaveChangesAsync() => await context.SaveChangesAsync() > 0;
    }
}
