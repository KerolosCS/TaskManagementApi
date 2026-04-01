using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data.Repositories.Interfaces;
using TaskManagementApi.models;

namespace TaskManagementApi.Data.Repositories
{
    public class BoardRepository(AppDbContext context) : IBoardRepository
    {
        

        public async Task<IEnumerable<Board>> GetUserBoardsAsync(int userId)
        {
            return await context.Boards
                .Where(b => b.OwnerId == userId)
                .ToListAsync();
        }

        public async Task<Board?> GetBoardByIdAsync(int id, int userId)
        {
            return await context.Boards
                .Include(b => b.Tasks) 
                .FirstOrDefaultAsync(b => b.Id == id && b.OwnerId == userId);
        }

        public async Task AddBoardAsync(Board board) => await context.Boards.AddAsync(board);

        public void UpdateBoard(Board board) => context.Boards.Update(board);

        public void DeleteBoard(Board board) => context.Boards.Remove(board);

        public async Task<bool> SaveChangesAsync() => (await context.SaveChangesAsync() > 0);
    }
}
