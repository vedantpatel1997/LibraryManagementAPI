using AutoMapper;
using LibraryManagement.API.Container.Service;
using LibraryManagement.API.Helper;
using LibraryManagement.API.Modal;
using LibraryManagement.API.Repos;
using LibraryManagement.API.Repos.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Container.Implimentation
{
    public class BooksService : IBooksService
    {
        private readonly Repos.Models.LibraryManagementContext _dbContext;
        private readonly IMapper _mapper;

        public BooksService(LibraryManagementContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<APIResponse<List<BookModal>>> GetAll()
        {
            var response = new APIResponse<List<BookModal>>();
            try
            {                var data = await _dbContext.Books.ToListAsync();
                response.Data = _mapper.Map<List<Book>, List<BookModal>>(data);
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

        public async Task<APIResponse<BookModal>> Create(BookModal book)
        {
            var response = new APIResponse<BookModal>();
            try
            {
                var data = _mapper.Map<BookModal, Book>(book);
                await _dbContext.AddAsync(data);
                await _dbContext.SaveChangesAsync();

                response.Data = _mapper.Map<Book, BookModal>(data);
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

        public async Task<APIResponse<BookModal>> GetById(int bookId)
        {
            var response = new APIResponse<BookModal>();
            try
            {
                var data = await _dbContext.Books.FirstOrDefaultAsync(i => i.BookId == bookId);
                if (data != null)
                {
                    response.Data = _mapper.Map<Book, BookModal>(data);
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

        public async Task<APIResponse> Remove(int bookId)
        {
            var response = new APIResponse();

            try
            {
                // Check if the book exists
                var book = await _dbContext.Books.SingleOrDefaultAsync(b => b.BookId == bookId);

                if (book == null)
                {
                    response.ResponseCode = 404; // Not Found
                    response.ErrorMessage = "Data not found";
                    return response;
                }

                // Check if the book is issued by any user
                var isBookIssued = await _dbContext.BookIssues.AnyAsync(bi => bi.BookId == bookId);

                if (isBookIssued)
                {
                    // Book is issued, generate an error response
                    response.ResponseCode = 403; // Forbidden
                    response.ErrorMessage = "This book is issued by a user and cannot be deleted.";
                }
                else
                {
                    // Book is not issued, proceed with deletion
                    _dbContext.Remove(book);
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



        public async Task<APIResponse> Update(BookModal book, int bookId)
        {
            var response = new APIResponse();
            try
            {
                var existingBook = await _dbContext.Books.FirstOrDefaultAsync(i => i.BookId == bookId);
                if (existingBook != null)
                {
                    _mapper.Map(book, existingBook);
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
