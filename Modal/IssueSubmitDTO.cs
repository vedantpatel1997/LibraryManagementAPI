using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.API.Modal
{
    public class IssueSubmitDTO
    {
        public int BookId { get; set; }
        public int UserId { get; set; }

        public DateTime IssueDate { get; set; }


        public DateTime? ReturnDate { get; set; }
        public int Days{ get; set; }

        public bool Returned { get; set; }
    }

  
}
