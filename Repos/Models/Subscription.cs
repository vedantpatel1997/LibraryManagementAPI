using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Repos.Models;

[Keyless]
[Table("Subscription")]
public partial class Subscription
{
    public int? SubscriptionId { get; set; }

    [StringLength(50)]
    public string? Name { get; set; }

    public int? Days { get; set; }

    [StringLength(10)]
    public string? Price { get; set; }

    [Column("Access Level")]
    public int? AccessLevel { get; set; }
}
