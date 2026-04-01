using TaskManagementApi.models;

namespace TaskManagementApi.Data.Repositories.Interfaces
{
    public interface IBoardRepository
    {
        Task<IEnumerable<Board>> GetUserBoardsAsync(int userId);
        Task<Board?> GetBoardByIdAsync(int id, int userId);
        Task AddBoardAsync(Board board);
        void UpdateBoard(Board board);
        void DeleteBoard(Board board);
        Task<bool> SaveChangesAsync();
    }
}
