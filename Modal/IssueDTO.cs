using LibraryManagement.API.Repos.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.API.Modal
{
    public class IssueDTO
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public int Days { get; set; }
        public DateTime? IssueDate { get; set; }
        public Book? Book { get; set; }
        public User? User { get; set; }
    }


}
