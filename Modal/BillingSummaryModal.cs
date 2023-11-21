namespace LibraryManagement.API.Modal
{
    public class BillingSummaryModal
    {
      
        public int BillingId { get; set; }
        public int UserId { get; set; }
       
        public int BookQuantity { get; set; }

        public bool? Delivery { get; set; }

        public int AddressId { get; set; }
        public bool? Pickup { get; set; }

        public decimal Tax { get; set; }

        public decimal TotalAmount { get; set; }

    }
}
