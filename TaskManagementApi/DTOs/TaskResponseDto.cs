using TaskManagementApi.enums;

namespace TaskManagementApi.DTOs
{
   
        public class TaskResponseDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public TaskStatusEnum Status { get; set; }
            public TaskPriority Priority { get; set; }
        
    }
}
