using TaskManagementApi.enums;

namespace TaskManagementApi.models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public TaskStatusEnum Status { get; set; } 
        public TaskPriority Priority { get; set; } 

        public int BoardId { get; set; }
        public Board Board { get; set; }

        public int CreatedBy { get; set; }
        public User Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
