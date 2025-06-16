using System.ComponentModel.DataAnnotations;

namespace BlackoutViewer.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        public required int GroupId { get; set; }

        [Required]
        public required DayEnum Day { get; set; }

        [Required]
        public required TimeOnly StartTime { get; set; }

        [Required]
        public required TimeOnly EndTime { get; set; }

        public virtual Group? Group { get; set; }
    }
}