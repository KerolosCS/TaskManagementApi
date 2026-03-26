using TaskManagementApi.enums;

namespace TaskManagementApi.DTOs
{
    public class TaskCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
        public int BoardId { get; set; }
    }
}
