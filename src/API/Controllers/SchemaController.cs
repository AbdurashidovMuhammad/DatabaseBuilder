using Application.Services;
using Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemaController : ControllerBase
    {
        private readonly SchemaService _schemaService;

        public SchemaController(SchemaService schemaService)
        {
            _schemaService = schemaService;
        }

        [HttpPost("create")]
        public IActionResult CreateSchema([FromBody] CreateSchemaDto dto)
        {
            try
            {
                _schemaService.CreateSchema(dto.DatabaseId, dto.Name);
                return Ok("Schema muvaffaqiyatli yaratildi.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("database/{databaseId}")]
        public IActionResult GetDatabaseSchemas(Guid databaseId)
        {
            var schemas = _schemaService.GetDatabaseSchemas(databaseId);
            return Ok(schemas);
        }
    }
}
