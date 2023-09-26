using LibraryManagement.API.Helper;
using LibraryManagement.API.Modal;

namespace LibraryManagement.API.Container.Service
{
    public interface IBooksService
    {
        Task<APIResponse<List<BookModal>>> GetAll();
        Task<APIResponse<BookModal>> Create(BookModal book);
        Task<APIResponse<BookModal>> GetById(int BookId);
        Task<APIResponse> Remove(int BookId);
        Task<APIResponse> Update(BookModal book, int id);
    }


}
