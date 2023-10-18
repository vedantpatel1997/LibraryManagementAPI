using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Repos.Models;

[Index("Username", Name = "UQ__Users__536C85E418573FD7", IsUnique = true)]
[Index("Phone", Name = "UQ__Users__5C7E359E3136F3C5", IsUnique = true)]
[Index("Email", Name = "UQ__Users__A9D105346A9A4187", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(50)]
    public string Salutation { get; set; } = null!;

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string? MiddleName { get; set; }

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    public int? Age { get; set; }

    [Column(TypeName = "date")]
    public DateTime? Dob { get; set; }

    [StringLength(50)]
    public string? Gender { get; set; }

    [StringLength(50)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string Phone { get; set; } = null!;

    [StringLength(50)]
    public string Password { get; set; } = null!;

    [StringLength(10)]
    public string? Role { get; set; }

    [StringLength(20)]
    public string? Username { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<BookIssue> BookIssues { get; set; } = new List<BookIssue>();

    [InverseProperty("User")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
