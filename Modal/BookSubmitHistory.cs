using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Modal
{
    public class BookSubmitHistory
    {

        public int Id { get; set; }

        public int BookId { get; set; }
        public string BookTitle{ get; set; }

        public int UserId { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public int Days { get; set; }
    }
}
