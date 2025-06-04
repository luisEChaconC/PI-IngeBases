using System.ComponentModel.DataAnnotations;

namespace backend.Application.DTOs
{
    public class DayCommandDto
    {
        [Required(ErrorMessage = "Worked hours are required")]
        [Range(0, 24, ErrorMessage = "Worked hours must be between 0 and 24")]
        public int WorkedHours { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(300, ErrorMessage = "Description must be less than 300 characters")]
        public string Description { get; set; }
    }
}