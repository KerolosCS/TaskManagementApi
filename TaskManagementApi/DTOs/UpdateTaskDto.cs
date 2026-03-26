using TaskManagementApi.enums;

namespace TaskManagementApi.DTOs
{
    public class UpdateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatusEnum Status { get; set; } // ToDo, InProgress, Done
        public TaskPriority Priority { get; set; }
    }
}
