using System.ComponentModel.DataAnnotations;
using TaskManagementApi.enums;

namespace TaskManagementApi.DTOs
{
    public class TaskCreateDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        public TaskPriority Priority { get; set; }
        [Required]
        public int BoardId { get; set; }
    }
}
