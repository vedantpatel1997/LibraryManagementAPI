using LibraryManagement.API.Helper;
using LibraryManagement.API.Modal;

namespace LibraryManagement.API.Container.Service
{
    public interface IBooksUsersTransactions
    {
        Task<APIResponse<List<BookModal>>> GetBooksByUserId(int userId);
        Task<APIResponse<List<UserModal>>> GetUsersByBookId(int bookId);
        Task<APIResponse> IssueBook(IssueSubmitDTO issueSubmitDTO);
        Task<APIResponse> IssueBooks(List<IssueSubmitDTO> issueSubmitDTO);
        Task<APIResponse> SubmitBook(IssueSubmitDTO issueSubmitDTO);
    }
}
