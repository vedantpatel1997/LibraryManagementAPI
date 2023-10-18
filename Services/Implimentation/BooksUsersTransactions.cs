using LibraryManagement.API.Container.Service;
using LibraryManagement.API.Helper;
using LibraryManagement.API.Modal;
using LibraryManagement.API.Repos.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Container.Implimentation
{
    public class BooksUsersTransactions : IBooksUsersTransactions
    {
        private readonly LibraryManagementContext _dbContext;
        private readonly IBooksService _bookSvc;

        public BooksUsersTransactions(LibraryManagementContext dbContext, IBooksService bookService)
        {
            this._dbContext = dbContext;
            this._bookSvc = bookService;
        }
        public async Task<APIResponse<List<BookModal>>> GetBooksByUserId(int userId)
        {
            var response = new APIResponse<List<BookModal>>();

            try
            {
                var data = await this._dbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
                if (data == null)
                {
                    response.ResponseCode = 404;
                    response.ErrorMessage = "Data not found";
                    return response;
                }

                // Query the database to get books issued to the user with the specified userId
                var books = await _dbContext.Books
                    .Join(_dbContext.BookIssues, b => b.BookId, bi => bi.BookId, (b, bi) => new { b, bi })
                    .Where(join => join.bi.UserId == userId)
                    .Select(join => new BookModal
                    {
                        BookId = join.b.BookId,
                        Title = join.b.Title,
                        Author = join.b.Author,
                        TotalQuantity = join.b.TotalQuantity,
                        AvailableQuantity = join.b.AvailableQuantity,
                        IssuedQuantity = join.b.IssuedQuantity,
                        Price = join.b.Price
                    })
                    .ToListAsync();

                response.Data = books;
                response.IsSuccess = true;
                response.ResponseCode = 200; // OK
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.ResponseCode = 500; // Internal Server Error
            }

            return response;
        }

        public async Task<APIResponse<List<UserModal>>> GetUsersByBookId(int bookId)
        {
            var response = new APIResponse<List<UserModal>>();

            try
            {
                var data = await this._dbContext.Books.FirstOrDefaultAsync(x => x.BookId == bookId);
                if (data == null)
                {
                    response.ResponseCode = 404;
                    response.ErrorMessage = "Data not found";
                    return response;
                }

                // Query the database to get users who have issued the book with the specified bookId
                var users = await _dbContext.Users
                    .Join(_dbContext.BookIssues, u => u.UserId, bi => bi.UserId, (u, bi) => new { u, bi })
                    .Where(join => join.bi.BookId == bookId)
                    .Select(join => new UserModal
                    {
                        Salutation = join.u.Salutation,
                        UserId = join.u.UserId,
                        Name = join.u.FirstName,
                        Age = (int)join.u.Age,
                        Email = join.u.Email,
                        Phone = join.u.Phone,
                        Dob = (DateTime)join.u.Dob,
                        Gender = join.u.Gender
                    })
                    .ToListAsync();

                response.Data = users;
                response.IsSuccess = true;
                response.ResponseCode = 200; // OK
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.ResponseCode = 500; // Internal Server Error
            }

            return response;
        }


        public async Task<APIResponse> IssueBook(IssueDTO issueDTO)
        {
            var response = new APIResponse();

            try
            {
                // Retrieve the book
                var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.BookId == issueDTO.BookId);
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == issueDTO.UserId);

                if (book == null)
                {
                    response.ResponseCode = 404; // Not Found
                    response.ErrorMessage = "Invalid Book.";
                    return response;
                }
                else if (user == null)
                {
                    response.ResponseCode = 404; // Not Found
                    response.ErrorMessage = "Invalid User.";
                    return response;
                }

                // Check if the book is already issued
                var isBookIssued = await _dbContext.BookIssues.AnyAsync(bi => bi.BookId == issueDTO.BookId && bi.UserId == issueDTO.UserId);

                if (isBookIssued)
                {
                    response.ResponseCode = 400; // Bad Request
                    response.ErrorMessage = $"{user.Salutation}.{user.FirstName} has already issued this book.";
                    return response;
                }

                // Check available quantity
                if (book.AvailableQuantity == 0)
                {
                    response.ResponseCode = 400; // Bad Request
                    response.ErrorMessage = "No available copies of this book.";
                    return response;
                }

                // Calculate the return date
                var returnDate = issueDTO.IssueDate?.AddDays(issueDTO.Days);

                // Begin a transaction
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        // Issue the book
                        var availableQty = book.AvailableQuantity - 1;
                        var issueQty = book.IssuedQuantity + 1;
                        book.AvailableQuantity = availableQty;
                        book.IssuedQuantity = issueQty;
                        await _dbContext.SaveChangesAsync();

                        // Create a book issue record
                        var bookIssue = new BookIssue()
                        {
                            BookId = issueDTO.BookId,
                            UserId = issueDTO.UserId,
                            IssueDate = DateTime.Now,
                        };

                        await _dbContext.BookIssues.AddAsync(bookIssue);
                        await _dbContext.SaveChangesAsync();

                        // Commit the transaction
                        transaction.Commit();

                        response.IsSuccess = true;
                        response.ResponseCode = 200; // OK
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();

                        response.ErrorMessage = ex.Message;
                        response.ResponseCode = 500; // Internal Server Error
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


        public async Task<APIResponse> IssueBooks(List<IssueDTO> issueDTOs)
        {
            var response = new APIResponse();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var curIssueDTO in issueDTOs)
                    {
                        // Retrieve the book
                        var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.BookId == curIssueDTO.BookId);
                        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == curIssueDTO.UserId);

                        if (book == null)
                        {
                            response.ResponseCode = 404; // Not Found
                            response.ErrorMessage = "Invalid Book.";
                            return response;
                        }
                        else if (user == null)
                        {
                            response.ResponseCode = 404; // Not Found
                            response.ErrorMessage = "Invalid User.";
                            return response;
                        }

                        // Check if the book is already issued
                        var isBookIssued = await _dbContext.BookIssues.AnyAsync(bi => bi.BookId == curIssueDTO.BookId && bi.UserId == curIssueDTO.UserId);

                        if (isBookIssued)
                        {
                            response.ResponseCode = 400; // Bad Request
                            response.ErrorMessage = $"{user.Salutation}.{user.FirstName} has already issued one of these book.";
                            return response;
                        }

                        // Check available quantity
                        if (book.AvailableQuantity == 0)
                        {
                            response.ResponseCode = 400; // Bad Request
                            response.ErrorMessage = "No available copies of this book.";
                            return response;
                        }


                        try
                        {
                            // Issue the book
                            var availableQty = book.AvailableQuantity - 1;
                            var issueQty = book.IssuedQuantity + 1;
                            book.AvailableQuantity = availableQty;
                            book.IssuedQuantity = issueQty;
                            await _dbContext.SaveChangesAsync();

                            // Create a book issue record
                            var bookIssue = new BookIssue()
                            {
                                BookId = curIssueDTO.BookId,
                                UserId = curIssueDTO.UserId,
                                IssueDate = DateTime.Now,
                            };

                            await _bookSvc.RemoveFromCart(curIssueDTO.BookId, curIssueDTO.UserId);

                            await _dbContext.BookIssues.AddAsync(bookIssue);
                            await _dbContext.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction in case of an error
                            transaction.Rollback();

                            response.ErrorMessage = ex.Message;
                            response.ResponseCode = 500; // Internal Server Error
                            return response; // Return the response immediately upon error
                        }
                    }

                    // Commit the transaction after the loop
                    transaction.Commit();

                    response.IsSuccess = true;
                    response.ResponseCode = 200; // OK
                }
                catch (Exception ex)
                {
                    response.ErrorMessage = ex.Message;
                    response.ResponseCode = 500; // Internal Server Error
                }
            }

            return response;
        }


        public async Task<APIResponse> SubmitBook(SubmitDTO SubmitDTO)
        {
            var response = new APIResponse();

            try
            {
                // Retrieve the book
                var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.BookId == SubmitDTO.BookId);
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == SubmitDTO.UserId);

                if (book == null || user == null)
                {
                    response.ResponseCode = 404; // Not Found
                    response.ErrorMessage = "Invalid Data.";
                    return response;
                }


                // Check if the book is already issued
                var isBookIssued = await _dbContext.BookIssues.AnyAsync(bi => bi.BookId == SubmitDTO.BookId && bi.UserId == SubmitDTO.UserId);

                if (!isBookIssued)
                {
                    response.ResponseCode = 400; // Bad Request
                    response.ErrorMessage = $"{user.Salutation}.{user.FirstName} has not issued this book.";
                    return response;
                }

                // Check available quantity
                if (book.AvailableQuantity < 0)
                {
                    response.ResponseCode = 400; // Bad Request
                    response.ErrorMessage = "Inaccurate book count.";
                    return response;
                }

                // Begin a transaction
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        // Issue the book
                        var availableQty = book.AvailableQuantity + 1;
                        var issueQu = book.IssuedQuantity - 1;
                        book.AvailableQuantity = availableQty;
                        book.IssuedQuantity = issueQu;
                        await _dbContext.SaveChangesAsync();

                        // Getting a book issue record
                        var bookIssued = await _dbContext.BookIssues.FirstOrDefaultAsync(x => x.UserId == SubmitDTO.UserId && x.BookId == SubmitDTO.BookId);

                        _dbContext.BookIssues.Remove(bookIssued);
                        await _dbContext.SaveChangesAsync();

                        // Commit the transaction
                        transaction.Commit();

                        response.IsSuccess = true;
                        response.ResponseCode = 200; // OK
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();

                        response.ErrorMessage = ex.Message;
                        response.ResponseCode = 500; // Internal Server Error
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
    }
}
