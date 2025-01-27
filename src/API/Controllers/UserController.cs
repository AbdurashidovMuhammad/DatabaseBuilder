using Application.Services;
using Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            try
            {
                _userService.RegisterUser(dto);
                return Ok("Foydalanuvchi muvaffaqiyatli ro‘yxatdan o‘tdi.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            if (_userService.LoginUser(dto, out var user))
            {
                return Ok(new { Message = "Tizimga muvaffaqiyatli kirdingiz.", UserId = user.Id });
            }
            return Unauthorized("Noto‘g‘ri email yoki parol.");
        }
    }
}
