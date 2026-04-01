using TaskManagementApi.models;

namespace TaskManagementApi.Data.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment?> GetCommentByIdAsync(int id, int userId);
        Task<IEnumerable<Comment>> GetCommentsByTaskIdAsync(int taskId);
        Task AddCommentAsync(Comment comment);
        void DeleteComment(Comment comment);
        Task LoadUserAsync(Comment comment); 
        Task<bool> SaveChangesAsync();
    }
}
