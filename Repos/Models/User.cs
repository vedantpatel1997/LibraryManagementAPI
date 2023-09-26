using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Repos.Models;

public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(50)]
    public string Salutation { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public int Age { get; set; }

    [Column(TypeName = "date")]
    public DateTime Dob { get; set; }

    [StringLength(50)]
    public string Gender { get; set; } = null!;

    [StringLength(50)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string Phone { get; set; } = null!;

    [StringLength(10)]
    public string? Password { get; set; }

    [StringLength(10)]
    public string? Role { get; set; }

    [StringLength(50)]
    public string? Username { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<BookIssue> BookIssues { get; set; } = new List<BookIssue>();
}
