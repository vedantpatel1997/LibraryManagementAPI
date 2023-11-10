using LibraryManagement.API.Container.Service;
using LibraryManagement.API.Modal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [EnableCors]
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
        public async Task<IActionResult> IssueBook(IssueDTO issueDTO)
        {
            var data = await this._bUTransactionSvs.IssueBook(issueDTO);
            return Ok(data);
        }
        [HttpPost("IssueBooks")]
        public async Task<IActionResult> IssueBooks(List<IssueDTO> issueDTOs)
        {
            var data = await this._bUTransactionSvs.IssueBooks(issueDTOs);
            return Ok(data);
        }
        [HttpPost("SubmitBook")]
        public async Task<IActionResult> SubmitBook(SubmitDTO SubmitDTO)
        {
            var data = await this._bUTransactionSvs.SubmitBook(SubmitDTO);
            return Ok(data);
        }

        [HttpGet("GetBooksHistoryByUserId")]
        public async Task<IActionResult> GetBooksHistoryByUserId(int userId)
        {
            var data = await _bUTransactionSvs.GetBooksHistoryByUserId(userId);
            return Ok(data);
        }

        [HttpGet("SendReminderForPendingBooks")]
        public async Task<IActionResult> SendReminderForPendingBooks(int userId)
        {
            var data = await _bUTransactionSvs.SendReminderForPendingBooks(userId);
            return Ok(data);
        }
    }
}
