using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Users;
using Services.Interfaces;
using Utils.Constants;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _users;

        public AuthController(IUserService users)
        {
            _users = users;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginRequestDto request)
        {
            return Ok(await _users.LoginAsync(request));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create(UserLoginRequestDto request)
        {
            return Ok(await _users.LoginAsync(request));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CreateUpdateUserDto request)
        {
            return Ok(await _users.UpdateAsync(id, request));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Ok(await _users.DeleteAsync(id));
        }

        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
        {
            return Ok(await _users.ChangePassword(request));
        }

        [HttpGet]
        [Authorize]
        [Route("user")]
        public async Task<IActionResult> CurrentUser()
        {
            return Ok(await _users.GetAsync());
        }
    }
}
