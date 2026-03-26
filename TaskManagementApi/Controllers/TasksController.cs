using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs;
using TaskManagementApi.enums;
using TaskManagementApi.models;

namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController(AppDbContext context) : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskCreateDto taskCreateDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var board = context.Boards.FirstOrDefault(b => b.Id == taskCreateDto.BoardId && b.OwnerId == userId);
            if (board == null)
                return NotFound("Board not found");
            var task = new TaskItem
            {

                Title = taskCreateDto.Title,
                Description = taskCreateDto.Description,
                Priority = taskCreateDto.Priority,
                BoardId = taskCreateDto.BoardId,
                Status = TaskStatusEnum.ToDo
                ,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,

            };
            context.Tasks.Add(task);
            context.SaveChanges();
            return Ok(new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Status = task.Status,
                Priority = task.Priority
            });
        }


        [HttpGet("board/{boardId:int}")]

        public IActionResult GetTasksByBoard(int boardId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var board = context.Boards.FirstOrDefault(b => b.Id == boardId && b.OwnerId == userId);
            if (board == null)
                return NotFound("Board not found");
            var tasks = context.Tasks.Where(t => t.BoardId == boardId).Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Status = t.Status,
                Priority = t.Priority
            }).ToList();
            return Ok(tasks);
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateTask(int id, [FromBody] UpdateTaskDto taskUpdateDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var task = context.Tasks.FirstOrDefault(t => t.Id == id && t.CreatedBy == userId);
            if (task == null)
                return NotFound("Task not found");
            task.Title = taskUpdateDto.Title;
            task.Description = taskUpdateDto.Description;
            task.Priority = taskUpdateDto.Priority;
            task.Status = taskUpdateDto.Status;
            context.SaveChanges();
            return Ok(new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Status = task.Status,
                Priority = task.Priority
            });



        }

           [HttpDelete("{id:int}")]
           public IActionResult DeleteTask(int id) {
               var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
               var task = context.Tasks.FirstOrDefault(t => t.Id == id && t.CreatedBy == userId);
               if (task == null)
                   return NotFound("Task not found");
               context.Tasks.Remove(task);
               context.SaveChanges();
               return NoContent();
        }
    }
}