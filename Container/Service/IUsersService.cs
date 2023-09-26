using LibraryManagement.API.Helper;
using LibraryManagement.API.Modal;

namespace LibraryManagement.API.Container.Service
{
    public interface IUsersService
    {
        Task<APIResponse<List<UserModal>>> GetAll(); 
        Task<APIResponse<UserModal>> Create(UserModal user); 
        Task<APIResponse<UserModal>> GetById(int userId);
        Task<APIResponse> Remove(int userId);
        Task<APIResponse> Update(UserModal user, int id);
    }
}
