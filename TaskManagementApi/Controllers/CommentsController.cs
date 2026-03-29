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
    public class CommentsController(AppDbContext context) : ControllerBase
    {

        [HttpPost]
        public IActionResult AddComment(CommentCreateDto commentdto)
        {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var task = context.Tasks.FirstOrDefault(t => t.Id == commentdto.TaskId);
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (task == null)
            {
                return NotFound("Task not found.");
            }
            var comment = new Comment
            {
                UserId = userId,
                TaskId = commentdto.TaskId,
                Content = commentdto.Content,
                CreatedAt = DateTime.UtcNow
            };
            context.Comments.Add(comment);
            context.SaveChanges();
            context.Entry(comment).Reference(c => c.User).Load();
            return Ok(new CommentResponseDto
            {
                Id = comment.Id,

                UserName = comment.User.Name,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            });
        }


        [HttpGet("task/{taskId:int}")]

        public IActionResult GetComments(int taskId)
        {
            var comments = context.Comments
                .Where(c => c.TaskId == taskId).Select(c => new CommentResponseDto
                {

                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UserName = c.User.Name,
                }).ToList();


            return Ok(comments);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteComment(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var comment = context.Comments
                .FirstOrDefault(c => c.Id == id && c.UserId == userId);

            if (comment == null)
                return NotFound();

            context.Comments.Remove(comment);
            context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateComment(int id, CommentUpdateDto commentdto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var comment = context.Comments
                .FirstOrDefault(c => c.Id == id && c.UserId == userId);
            if (comment == null)
                return NotFound();
            comment.Content = commentdto.Content;
            context.SaveChanges();
            context.Entry(comment).Reference(c => c.User).Load();
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