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
    public class CommentsController(ICommentRepository commentRepo, ITaskRepository taskRepo) : ControllerBase
    {
        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentCreateDto commentdto)
        {

            var task = await taskRepo.GetTaskByIdAsync(commentdto.TaskId, GetUserId());
            if (task == null)
            {
                return NotFound("Task not found.");
            }
            var comment = new Comment
            {
                UserId = GetUserId(),
                TaskId = commentdto.TaskId,
                Content = commentdto.Content,
                CreatedAt = DateTime.UtcNow
            };
            await commentRepo.AddCommentAsync(comment);
            await commentRepo.SaveChangesAsync();

            
            await commentRepo.LoadUserAsync(comment);
            return Ok(new CommentResponseDto
            {
                Id = comment.Id,

                UserName = comment.User.Name,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            });
        }


        [HttpGet("task/{taskId:int}")]

        public async Task<IActionResult> GetComments(int taskId)
        {
            var comments = await commentRepo.GetCommentsByTaskIdAsync(taskId);

            var response = comments.Select(c => new CommentResponseDto
                {

                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UserName = c.User.Name,
                });


            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment(int id)
        {


            var comment = await commentRepo.GetCommentByIdAsync(id, GetUserId());
            if (comment == null) return NotFound();

            commentRepo.DeleteComment(comment);
            await commentRepo.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateComment(int id, CommentUpdateDto commentdto)
        {
            var comment = await commentRepo.GetCommentByIdAsync(id, GetUserId());
            if (comment == null) return NotFound();
            comment.Content = commentdto.Content;
            await commentRepo.SaveChangesAsync();

            await commentRepo.LoadUserAsync(comment);
            return Ok(new CommentResponseDto
            {
                Id = comment.Id,
                UserName = comment.User.Name,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            });
        }
    }
}