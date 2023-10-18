﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Repos.Models;

[Keyless]
[Table("BookIssue")]
public partial class BookIssue
{
    public int IssueId { get; set; }

    public int BookId { get; set; }

    public int UserId { get; set; }

    [Column(TypeName = "date")]
    public DateTime IssueDate { get; set; }

    public int Days { get; set; }
}
