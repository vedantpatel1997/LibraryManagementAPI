﻿using LibraryManagement.API.Helper;
using LibraryManagement.API.Modal;
using LibraryManagement.API.Repos.Models;

namespace LibraryManagement.API.Container.Service
{
    public interface IBooksUsersTransactions
    {
        Task<APIResponse<List<IssueDTO>>> GetBooksByUserId(int userId);
        Task<APIResponse<List<IssueDTO>>> GetUsersByBookId(int bookId);
        Task<APIResponse<List<SubmitBooksInfo>>> GetBooksHistoryByUserId(int bookId);
        Task<APIResponse> IssueBook(IssueDTO issueDTO);
        Task<APIResponse> IssueBooks(List<IssueDTO> issueDTO);
        Task<APIResponse> SubmitBook(SubmitDTO SubmitDTO);
    }
}
