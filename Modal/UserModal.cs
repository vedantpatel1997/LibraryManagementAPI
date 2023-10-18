using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Modal
{
    public class UserModal
    {
        public int UserId { get; set; }

        public string Salutation { get; set; } 

        public string FirstName { get; set; } 

        public string? MiddleName { get; set; }

        public string LastName { get; set; } 

        public int Age { get; set; }

        public DateTime Dob { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; } 

        public string Phone { get; set; } 
       
        public string? Role { get; set; }

        public string? Username { get; set; }
    }
}
