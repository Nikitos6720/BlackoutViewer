using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackoutViewer.Models;

public class Address
{
    [Key]
    public int Id { get; set; }

    public int? GroupId { get; set; } = null;

    [Required(ErrorMessage = "Cannot create address without address text")]
    [MaxLength(100, ErrorMessage = "Max length must be 100 symbols")]
    [RegularExpression(@"^[a-zа-яA-ZА-Я0-9/,.-\s]+$", ErrorMessage = "")]
    public required string Title { get; set; }

    [ForeignKey(nameof(GroupId))]
    public virtual Group? Group { get; set; }
}