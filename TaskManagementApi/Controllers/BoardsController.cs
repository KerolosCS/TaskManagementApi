using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs;
using TaskManagementApi.models;

namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BoardsController (AppDbContext context): ControllerBase
    {
        [HttpPost]
        public IActionResult CreateBoard([FromBody] BoardsCreateDto boarddto)
        {


            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var board = new Board
            {
                Name = boarddto.Name,
                OwnerId = userId,
                CreatedAt = DateTime.UtcNow
            };

            context.Boards.Add(board);
            context.SaveChanges();
            var result = new BoardsResponseDto
            {
                Id = board.Id,
                Name = board.Name,
                CreatedAt = board.CreatedAt
            };
            return Ok(result);
        }

        [HttpGet]

        public IActionResult GetBoards()
        { 
        
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var boards = context.Boards.Where(b => b.OwnerId == userId).ToList();


            var boardsResponse = boards.Select(b => new BoardsResponseDto
            {
                Id = b.Id,
                Name = b.Name,
                CreatedAt = b.CreatedAt
            }).ToList();
            return Ok(boardsResponse);

        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteBoard([FromRoute] int id) { 
        
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var board = context.Boards.FirstOrDefault(b => b.Id == id && b.OwnerId == userId);
            if (board == null)
            {
                return NotFound("Board not found or you don't have permission to delete it.");
            }
            context.Boards.Remove(board);
            context.SaveChanges();
            return Ok("Board deleted successfully");

        }
        [HttpGet("{id:int}")]
        public IActionResult GetBoardDetails([FromRoute]int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var board = context.Boards
                .Where(b => b.Id == id && b.OwnerId == userId)
                .Select(b => new BoardsDetailsDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Tasks = b.Tasks.Select(t => new TaskDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Status = t.Status,
                        Priority = t.Priority
                    }).ToList()
                })
                .FirstOrDefault();

            if (board == null)
                return NotFound();

            return Ok(board);
        }

    }
}
