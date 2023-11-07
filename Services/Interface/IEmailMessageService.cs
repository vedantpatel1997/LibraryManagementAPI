namespace LibraryManagement.API.Services.Interface
{
    public interface IEmailMessageService
    {
        Task SendMessage(string email, string subject, string bodyHtml);

    }
}
