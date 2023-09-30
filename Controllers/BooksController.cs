using LibraryManagement.API.Container.Service;
using LibraryManagement.API.Modal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [EnableCors]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksService _booksSvc;

        public BooksController(IBooksService booksSvc)
        {
            this._booksSvc = booksSvc;
        }

        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            var data = await _booksSvc.GetAll();
            return Ok(data);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _booksSvc.GetById(id);
            return Ok(data);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> create([FromBody] BookModal book)
        {
            var data = await _booksSvc.Create(book);
            return Ok(data);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(BookModal book, int id)
        {
            var data = await _booksSvc.Update(book, id);
            return Ok(data);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _booksSvc.Remove(id);
            return Ok(data);
        }
    }
}

