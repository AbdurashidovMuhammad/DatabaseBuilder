﻿using Application.Services;
using Core.DTOs;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnController : ControllerBase
    {
        private readonly ColumnService _columnService;

        public ColumnController(ColumnService columnService)
        {
            _columnService = columnService;
        }

        [HttpPost("create")]
        public IActionResult CreateColumn([FromBody] CreateColumnDto dto)
        {
            try
            {
                _columnService.CreateColumn(dto.TableId, dto.Name, dto.DataType, dto.IsNullable, dto.IsUnique);
                return Ok("Column muvaffaqiyatli yaratildi.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("insert/{tableId}")]
        public IActionResult InsertRow(Guid tableId, [FromBody] Dictionary<string, object> columnValues)
        {
            _columnService.InsertRow(tableId, columnValues);
            return Ok();
        }

        [HttpGet("table/{tableId}")]
        public IActionResult GetTableColumns(Guid tableId)
        {
            var columns = _columnService.GetTableColumns(tableId);
            return Ok(columns);
        }
    }
}
