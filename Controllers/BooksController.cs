using LibraryManagement.API.Container.Service;
using LibraryManagement.API.Modal;
using LibraryManagement.API.Repos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksService _booksSvc;

        public BooksController(IBooksService booksSvc)
        {
            this._booksSvc = booksSvc;
        }
        [AllowAnonymous]
        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            var data = await _booksSvc.GetAll();
            return Ok(data);
        }

        [HttpGet("GetBookById")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var data = await _booksSvc.GetById(id);
            return Ok(data);
        }

        [HttpPost("GetBooksByIds")]
        public async Task<IActionResult> GetBooksByIds(int[] id)
        {
            var data = await _booksSvc.GetBooksByIds(id);
            return Ok(data);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> create([FromBody] BookUpdateModal book)
        {
            var data = await _booksSvc.Create(book);
            return Ok(data);
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update(BookUpdateModal book, int id)
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

        [HttpGet("GetCartItemsByUserId")]
        public async Task<IActionResult> GetCartItemsByUserId(int userId)
        {
            var data = await _booksSvc.GetCartItemsByUserId(userId);
            return Ok(data);
        }


        

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(CartModal cart)
        {
            var data = await _booksSvc.AddToCart(cart.BookId, cart.UserId);
            return Ok(data);
        }

        [HttpPost("RemoveFromCart")]
        public async Task<IActionResult> RemoveFromCart(CartModal cart)
        {
            var data = await _booksSvc.RemoveFromCart(cart.BookId, cart.UserId);
            return Ok(data);
        }
    }
}

