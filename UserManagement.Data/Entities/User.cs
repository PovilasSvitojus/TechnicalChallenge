using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 3)]
    [DataType(DataType.Text)]
    public string Forename { get; set; } = default!;
    [Required]
    [StringLength(100, MinimumLength = 3)]
    [DataType(DataType.Text)]
    public string Surname { get; set; } = default!;
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = default!;
    [Required]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }
    public bool IsActive { get; set; }
}
