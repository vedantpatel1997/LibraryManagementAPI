using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Repos.Models;

[Keyless]
[Table("BookAccess")]
public partial class BookAccess
{
    [Column("AccessID")]
    public int? AccessId { get; set; }

    [StringLength(50)]
    public string? Access { get; set; }
}
