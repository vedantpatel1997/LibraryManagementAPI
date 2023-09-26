using LibraryManagement.API.Container.Service;
using LibraryManagement.API.Modal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersSvc;

        public UsersController(IUsersService usersSvc)
        {
            this._usersSvc = usersSvc;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var data = await _usersSvc.GetAll();
            return Ok(data);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _usersSvc.GetById(id);
            return Ok(data);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] UserModal user)
        {
            var data = await _usersSvc.Create(user);
            return Ok(data);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(UserModal user, int id)
        {
            var data = await _usersSvc.Update(user, id);
            return Ok(data);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _usersSvc.Remove(id);
            return Ok(data);
        }
    }
}
