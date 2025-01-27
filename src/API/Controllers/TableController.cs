using Application.Services;
using Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly TableService _tableService;

        public TableController(TableService tableService)
        {
            _tableService = tableService;
        }

        [HttpPost("create")]
        public IActionResult CreateTable([FromBody] CreateTableDto dto)
        {
            try
            {
                _tableService.CreateTable(dto.SchemaId, dto.Name);
                return Ok("Table muvaffaqiyatli yaratildi.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("schema/{schemaId}")]
        public IActionResult GetSchemaTables(Guid schemaId)
        {
            var tables = _tableService.GetSchemaTables(schemaId);
            return Ok(tables);
        }


    }
}
