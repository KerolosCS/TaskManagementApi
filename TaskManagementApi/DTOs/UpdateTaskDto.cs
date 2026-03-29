using System.ComponentModel.DataAnnotations;
using TaskManagementApi.enums;

namespace TaskManagementApi.DTOs
{
    public class UpdateTaskDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public TaskStatusEnum Status { get; set; } // ToDo, InProgress, Done
        [Required]
        public TaskPriority Priority { get; set; }
    }
}
