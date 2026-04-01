using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementApi.Data;
using TaskManagementApi.Data.Repositories.Interfaces;
using TaskManagementApi.DTOs;
using TaskManagementApi.enums;
using TaskManagementApi.models;
using Microsoft.EntityFrameworkCore;
namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController(ITaskRepository taskRepo, IBoardRepository boardRepo) : ControllerBase
    {
        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto taskCreateDto)
        {
          
         var board  = await boardRepo.GetBoardByIdAsync(taskCreateDto.BoardId,GetUserId());
            if (board == null)
                return NotFound("Board not found");
            var task = new TaskItem
            {
                Title = taskCreateDto.Title,
                Description = taskCreateDto.Description,
                Priority = taskCreateDto.Priority,
                BoardId = taskCreateDto.BoardId,
                Status = TaskStatusEnum.ToDo,
                CreatedBy = GetUserId(),
                CreatedAt = DateTime.UtcNow
            };
           
           await taskRepo.AddTaskAsync(task);
           await taskRepo.SaveChangesAsync();
            return Ok(new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Status = task.Status,
                Priority = task.Priority
            });
        }


        [HttpGet("board/{boardId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTasksByBoard(int boardId)
        {
          var board  = await  boardRepo.GetBoardByIdAsync(boardId,GetUserId());
            if (board == null)
                return NotFound("Board not found");
            var tasks =  await taskRepo.GetTasksByBoardIdAsync(boardId);

            var response = tasks.Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Status = t.Status,
                Priority = t.Priority
            });
            return Ok(response);
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto taskUpdateDto)
        {
            var task = await taskRepo.GetTaskByIdAsync(id, GetUserId());
            if (task == null)
                return NotFound("Task not found");
            task.Title = taskUpdateDto.Title;
            task.Description = taskUpdateDto.Description;
            task.Priority = taskUpdateDto.Priority;
            task.Status = taskUpdateDto.Status;
            taskRepo.UpdateTask(task);
            await taskRepo.SaveChangesAsync();
            return Ok(new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Status = task.Status,
                Priority = task.Priority
            });

        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await taskRepo.GetTaskByIdAsync(id, GetUserId());
            if (task == null) return NotFound("Task not found");

            taskRepo.DeleteTask(task);
            await taskRepo.SaveChangesAsync();

            return NoContent();
        }

    }
}