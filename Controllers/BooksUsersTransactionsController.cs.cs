using LibraryManagement.API.Container.Service;
using LibraryManagement.API.Modal;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksUsersTransactionsController : ControllerBase
    {
        private readonly IBooksUsersTransactions _bUTransactionSvs;

        public BooksUsersTransactionsController(IBooksUsersTransactions BUTransactionSvs)
        {
            _bUTransactionSvs = BUTransactionSvs;
        }
        [HttpGet("GetBooksByUserID")]
        public async Task<IActionResult> GetBooksByUserID(int userId)
        {
            var data = await this._bUTransactionSvs.GetBooksByUserId(userId);
            return Ok(data);
        }
        [HttpGet("GetUsersByBookID")]
        public async Task<IActionResult> GetUsersByBookID(int bookId)
        {
            var data = await this._bUTransactionSvs.GetUsersByBookId(bookId);
            return Ok(data);
        } 
        [HttpPost("IssueBook")]
        public async Task<IActionResult> IssueBook(IssueSubmitDTO issueSubmitDTO)
        {
            var data = await this._bUTransactionSvs.IssueBook(issueSubmitDTO);
            return Ok(data);
        }
        [HttpPost("SubmitBook")]
        public async Task<IActionResult> SubmitBook(IssueSubmitDTO issueSubmitDTO)
        {
            var data = await this._bUTransactionSvs.SubmitBook(issueSubmitDTO);
            return Ok(data);
        }
    }
}
