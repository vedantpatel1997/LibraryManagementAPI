using AutoMapper;
using LibraryManagement.API.Container.Service;
using LibraryManagement.API.Helper;
using LibraryManagement.API.Modal;
using LibraryManagement.API.Repos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Text;

namespace LibraryManagement.API.Container.Implimentation
{
    public class UsersService : IUsersService
    {
        private readonly LibraryManagementContext _dbContext;
        private readonly IMapper _mapper;

        public UsersService(LibraryManagementContext dbContext, IMapper mapper)
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
                // Check for duplicate values
                var duplicateValue = await GetDuplicateValue(user);
                if (!string.IsNullOrEmpty(duplicateValue))
                {
                    response.ResponseCode = 400; // Bad Request
                    response.ErrorMessage = duplicateValue;
                }
                else
                {
                    User data = _mapper.Map<UserModal, User>(user);
                    await _dbContext.Users.AddAsync(data);
                    await _dbContext.SaveChangesAsync();

                    response.Data = _mapper.Map<User, UserModal>(data);
                    response.IsSuccess = true;
                    response.ResponseCode = 201; // Created
                }
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
                    // Check for duplicate values
                    var duplicateValue = await GetDuplicateValue(user);
                    if (!string.IsNullOrEmpty(duplicateValue))
                    {
                        response.ResponseCode = 400; // Bad Request
                        response.ErrorMessage = duplicateValue;
                    }
                    else
                    {
                        // Saving the data
                        existingUser.FirstName = user.FirstName;
                        existingUser.LastName = user.LastName;
                        existingUser.Username = user.Username;
                        existingUser.Email = user.Email;
                        existingUser.Dob = user.Dob;
                        existingUser.Gender = user.Gender;
                        existingUser.Phone = user.Phone;

                        await _dbContext.SaveChangesAsync();
                        response.IsSuccess = true;
                        response.ResponseCode = 200;
                    }
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

        private async Task<string> GetDuplicateValue(UserModal user)
        {
            if (await _dbContext.Users.AnyAsync(u =>
                (u.UserId != user.UserId) && // Exclude the current user being updated
                (u.Username == user.Username)))
            {
                return "Username is already registered with us.";
            }
            if (await _dbContext.Users.AnyAsync(u =>
                (u.UserId != user.UserId) && // Exclude the current user being updated
                (u.Email == user.Email)))
            {
                return "Email already exists.";
            }
            if (await _dbContext.Users.AnyAsync(u =>
                (u.UserId != user.UserId) && // Exclude the current user being updated
                (u.Phone == user.Phone)))
            {
                return "Phone number is already registered with us.";
            }
            return null;
        }

        public async Task<APIResponse<AddressModal>> CreateAddress(AddressModal addressData, int userId)
        {
            APIResponse<AddressModal> response = new APIResponse<AddressModal>();
            try
            {
                // Check if the user already has an address
                var existingUser = await _dbContext.Users.FindAsync(userId);
                if (existingUser == null)
                {
                    response.ResponseCode = 404; // Not Found
                    response.ErrorMessage = "User not found.";
                    return response;
                }

                if (existingUser.AddressId != null)
                {
                    response.ResponseCode = 400; // Bad Request
                    response.ErrorMessage = "User already has an address registered.";
                    return response;
                }

                Address data = _mapper.Map<AddressModal, Address>(addressData);
                await _dbContext.Addresses.AddAsync(data);
                await _dbContext.SaveChangesAsync();
                int newAddressId = data.AddressId;

                // Update the user's AddressId
                existingUser.AddressId = newAddressId;
                await _dbContext.SaveChangesAsync();

                response.Data = _mapper.Map<Address, AddressModal>(data);
                response.IsSuccess = true;
                response.ResponseCode = 201; // Created
            }
            catch (DbUpdateException ex)
            {
                response.ResponseCode = 400; // Bad Request
                response.ErrorMessage = "Failed to create the address. Please check the data provided.";
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500; // Internal Server Error
                response.ErrorMessage = ex.Message;
            }
            return response;
        }


        public async Task<APIResponse<AddressModal>> GetAddressByUserId(int userId)
        {
            APIResponse<AddressModal> response = new APIResponse<AddressModal>();
            try
            {
                var user = await _dbContext.Users.FindAsync(userId);

                if (user == null)
                {
                    response.ResponseCode = 404; // Not Found
                    response.ErrorMessage = "User not found";
                }
                else if (user.AddressId == null)
                {
                    response.ResponseCode = 404; // Not Found
                    response.ErrorMessage = "Address data not found for this user";
                }
                else
                {
                    var addressData = await _dbContext.Addresses.FindAsync(user.AddressId);
                    if (addressData != null)
                    {
                        response.Data = _mapper.Map<Address, AddressModal>(addressData);
                        response.IsSuccess = true;
                        response.ResponseCode = 200;
                    }
                    else
                    {
                        response.ResponseCode = 404; // Not Found
                        response.ErrorMessage = "Address data not found";
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.ResponseCode = 500; // Internal Server Error
            }
            return response;
        }
        public async Task<APIResponse<AddressModal>> UpdateAddress(AddressModal addressData, int userId)
        {
            APIResponse<AddressModal> response = new APIResponse<AddressModal>();
            try
            {
                // Find the address to be updated
                var existingAddress = await _dbContext.Addresses.FindAsync(addressData.AddressId);
                if (existingAddress == null)
                {
                    response.ResponseCode = 404; // Not Found
                    response.ErrorMessage = "Address not found.";
                    return response;
                }

                // Update the address data
                _mapper.Map(addressData, existingAddress);
                await _dbContext.SaveChangesAsync();

                response.Data = _mapper.Map<Address, AddressModal>(existingAddress);
                response.IsSuccess = true;
                response.ResponseCode = 200; // OK
            }
            catch (DbUpdateException ex)
            {
                response.ResponseCode = 400; // Bad Request
                response.ErrorMessage = "Failed to update the address. Please check the data provided.";
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500; // Internal Server Error
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> UpdatePassword(UpdatePassword password)
        {
            APIResponse response = new APIResponse();
            try
            {
                // Find the user whose password you want to update
                var user = await _dbContext.Users.FindAsync(password.userId); // Replace userId with the appropriate method to find the user based on your application's logic

                if (user == null)
                {
                    response.ResponseCode = 404; // Not Found
                    response.ErrorMessage = "User not found.";
                    return response;
                }

                // Check if the old password matches the current password
                if (user.Password != password.oldPassword)
                {
                    response.ResponseCode = 400; // Bad Request
                    response.ErrorMessage = "Invalid credentials";
                    return response;
                }

                user.Password = password.newPassword;

                // Save changes to the database
                await _dbContext.SaveChangesAsync();

                response.IsSuccess = true;
                response.ResponseCode = 200; // OK
            }
            catch (DbUpdateException ex)
            {
                response.ResponseCode = 400; // Bad Request
                response.ErrorMessage = "Failed to update the password.";
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500; // Internal Server Error
                response.ErrorMessage = ex.Message;
            }
            return response;
        }



    }



}
