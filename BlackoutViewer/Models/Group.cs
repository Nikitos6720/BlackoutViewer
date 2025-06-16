using System.ComponentModel.DataAnnotations;

namespace BlackoutViewer.Models;

public class Group
{
    [Key]
    public int Id { get; set; }

    [Required]
    [RegularExpression(@"^[a-zа-яA-ZА-Я-\s]+$")]
    public required string Name { get; set; }

    [MaxLength(200, ErrorMessage = "Group description's text length must be no more that 200 symbols")]
    [RegularExpression(@"^[a-zа-яA-ZА-Я-\s]+$")]
    public string Description { get; set; } = String.Empty;

    public virtual ICollection<Address>? Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Schedule>? Schedules { get; set; } = new List<Schedule>();
}