namespace TaskManagementApi.DTOs
{
    public class BoardsDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<TaskDto> Tasks { get; set; }
    }
}
