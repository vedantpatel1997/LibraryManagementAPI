using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Repos.Models;

[Table("BookIssue")]
public partial class BookIssue
{
    [Key]
    public int IssueId { get; set; }

    public int BookId { get; set; }

    public int UserId { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("BookIssues")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("BookIssues")]
    public virtual User User { get; set; } = null!;
}
