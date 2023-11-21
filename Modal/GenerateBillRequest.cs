namespace LibraryManagement.API.Modal
{
    public class GenerateBillRequest
    {
        public BillingSummaryModal BillingSummary { get; set; }
        public List<BillingBooksInfoModal> BillingBooksInfo { get; set; }
    }
}
