using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagementApi.Data;
using TaskManagementApi.Data.Repositories.Interfaces;
using TaskManagementApi.DTOs;
using TaskManagementApi.models;

namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BoardsController (IBoardRepository boardRepository): ControllerBase
    {
        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateBoard([FromBody] BoardsCreateDto boarddto)
        {
               var board = new Board
            {
                Name = boarddto.Name,
                OwnerId = GetUserId(),
                CreatedAt = DateTime.UtcNow
            };

            await boardRepository.AddBoardAsync(board);
            await boardRepository.SaveChangesAsync();
            var result = new BoardsResponseDto
            {
                Id = board.Id,
                Name = board.Name,
                CreatedAt = board.CreatedAt
            };
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBoard(int id, [FromBody] BoardsUpdateDto boarddto)
        {
            var board = await boardRepository.GetBoardByIdAsync(id, GetUserId());
            if (board == null)
            {
                return NotFound("Board not found or you don't have permission to update it.");
            }
            boardRepository.UpdateBoard(board);
            await boardRepository.SaveChangesAsync();
            var result = new BoardsResponseDto
            {
                Id = board.Id,
                Name = board.Name,
                CreatedAt = board.CreatedAt
            };
            return Ok(result);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
     
        public async Task<IActionResult> GetBoards()
        { 
        
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var boards = await boardRepository.GetUserBoardsAsync(GetUserId());


            var boardsResponse = boards.Select(b => new BoardsResponseDto
            {
                Id = b.Id,
                Name = b.Name,
                CreatedAt = b.CreatedAt
            }).ToList();
            return Ok(boardsResponse);
        }

        [HttpDelete("{id:int}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBoard([FromRoute] int id) {


            var board = await boardRepository.GetBoardByIdAsync(id, GetUserId());
            if (board == null)
            {
                return NotFound("Board not found or you don't have permission to delete it.");
            }
            boardRepository.DeleteBoard(board);
            await boardRepository.SaveChangesAsync();
            return Ok("Board deleted successfully");

        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <IActionResult> GetBoardDetails([FromRoute]int id)
        {


            var board = await boardRepository.GetBoardByIdAsync(id, GetUserId());

            if (board == null)
                return NotFound();

            var boardDetailsDto = new BoardsDetailsDto
            {
                Id = board.Id,
                Name = board.Name,
                Tasks = board.Tasks.Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Status = t.Status,
                    Priority = t.Priority
                }).ToList()
            };

            return Ok(boardDetailsDto);
        }

    }
}
