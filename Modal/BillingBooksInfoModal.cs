namespace LibraryManagement.API.Modal
{
    public class BillingBooksInfoModal
    {
        public int? BillingBookInfoId { get; set; }

        public int BookId { get; set; }

        public int RentDays { get; set; }

        public decimal bookRentPrice { get; set; }

        public DateTime EstimatedReturnDate { get; set; }

        public int? BillingId { get; set; }
    }
}
