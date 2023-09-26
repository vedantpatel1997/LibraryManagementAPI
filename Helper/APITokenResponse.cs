namespace LibraryManagement.API.Helper
{
    public class APITokenResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsSuccess { get; set; }
        public int ResponseCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
