namespace TaskManagementApi.DTOs
{
    public class CommentCreateDto
    {
        public string Content { get; set; }
        public int TaskId { get; set; }
    }
}
