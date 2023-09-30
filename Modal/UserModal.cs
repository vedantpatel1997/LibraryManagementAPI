using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Modal
{
    public class UserModal
    {
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
    }
}
