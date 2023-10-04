using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Repos.Models;

[PrimaryKey("IssueId", "BookId", "UserId")]
[Table("BookIssue")]
public partial class BookIssue
{
    [Key]
    public int IssueId { get; set; }

    [Key]
    public int BookId { get; set; }

    [Key]
    public int UserId { get; set; }

    [Column(TypeName = "date")]
    public DateTime IssueDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? ReturnDate { get; set; }

    public bool Returned { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("BookIssues")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("BookIssues")]
    public virtual User User { get; set; } = null!;
}
