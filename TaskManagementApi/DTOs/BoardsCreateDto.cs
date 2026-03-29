using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs
{
    public class BoardsCreateDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(500)]
        public string Name { get; set; }
    }
}
