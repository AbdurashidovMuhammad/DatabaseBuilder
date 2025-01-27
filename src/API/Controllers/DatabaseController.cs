using Application.Services;
using Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public DatabaseController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost("create")]
        public IActionResult CreateDatabase([FromBody] CreateDatabaseDto dto)
        {
            try
            {
                _databaseService.CreateDatabase(dto.UserId, dto.Name);
                return Ok("Database muvaffaqiyatli yaratildi.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetUserDatabases(Guid userId)
        {
            var databases = _databaseService.GetUserDatabases(userId);
            return Ok(databases);
        }
    }
}
