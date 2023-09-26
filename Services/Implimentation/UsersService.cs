using AutoMapper;
using LibraryManagement.API.Container.Service;
using LibraryManagement.API.Helper;
using LibraryManagement.API.Modal;
using LibraryManagement.API.Repos.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Container.Implimentation
{
    public class UsersService : IUsersService
    {
        private readonly LibraryProjectContext _dbContext;
        private readonly IMapper _mapper;

        public UsersService(LibraryProjectContext dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<APIResponse<List<UserModal>>> GetAll()
        {
            APIResponse<List<UserModal>> response = new APIResponse<List<UserModal>>();
            try
            {
                var data = await _dbContext.Users.ToListAsync();
                response.Data = _mapper.Map<List<User>, List<UserModal>>(data);
                response.IsSuccess = true;
                response.ResponseCode = 200;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.ResponseCode = 500; // Internal Server Error
            }
            return response;
        }

        public async Task<APIResponse<UserModal>> Create(UserModal user)
        {
            APIResponse<UserModal> response = new APIResponse<UserModal>();
            try
            {
                User data = _mapper.Map<UserModal, User>(user);
                await _dbContext.AddAsync(data);
                await _dbContext.SaveChangesAsync();

                response.Data = _mapper.Map<User, UserModal>(data);
                response.IsSuccess = true;
                response.ResponseCode = 201; // Created
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.ResponseCode = 500; // Internal Server Error
            }
            return response;
        }

        public async Task<APIResponse<UserModal>> GetById(int userId)
        {
            APIResponse<UserModal> response = new APIResponse<UserModal>();
            try
            {
                var data = await _dbContext.Users.FirstOrDefaultAsync(i => i.UserId == userId);
                if (data != null)
                {
                    response.Data = _mapper.Map<User, UserModal>(data);
                    response.IsSuccess = true;
                    response.ResponseCode = 200;
                }
                else
                {
                    response.ResponseCode = 404; // Not Found
                    response.ErrorMessage = "Data not found";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.ResponseCode = 500; // Internal Server Error
            }
            return response;
        }

        public async Task<APIResponse> Remove(int userId)
        {
            var response = new APIResponse();

            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null)
                {
                    response.ResponseCode = 404; // Not Found
                    response.ErrorMessage = "Data not found";
                    return response;
                }

                // Check if the user has any pending books to submit
                var hasPendingBooks = await _dbContext.BookIssues
                    .AnyAsync(bi => bi.UserId == userId);

                if (hasPendingBooks)
                {
                    // User has pending books, generate an error response
                    response.ResponseCode = 403; // Forbidden
                    response.ErrorMessage = "This user has some books to submit. Cannot delete.";
                }
                else
                {
                    // User has no pending books, proceed with deletion
                    _dbContext.Remove(user);
                    await _dbContext.SaveChangesAsync();
                    response.IsSuccess = true;
                    response.ResponseCode = 204; // No Content (successful delete)
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.ResponseCode = 500; // Internal Server Error
            }

            return response;
        }



        public async Task<APIResponse> Update(UserModal user, int userId)
        {
            APIResponse response = new APIResponse();
            try
            {
                var existingUser = await _dbContext.Users.FirstOrDefaultAsync(i => i.UserId == userId);
                if (existingUser != null)
                {
                    _mapper.Map(user, existingUser);
                    await _dbContext.SaveChangesAsync();
                    response.IsSuccess = true;
                    response.ResponseCode = 200;
                }
                else
                {
                    response.ResponseCode = 404; // Not Found
                    response.ErrorMessage = "Data not found";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.ResponseCode = 500; // Internal Server Error
            }
            return response;
        }
    }
}
