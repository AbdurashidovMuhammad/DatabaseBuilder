using Core.DTOs;
using Core.Entities;
using DataAccess;

namespace Application.Services
{
    public class TableService
    {
        private readonly TableRepository _tableRepository;

        public TableService(TableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }

        public void CreateTable(Guid schemaId, string tableName)
        {


            _tableRepository.CreateTable(schemaId, tableName);
        }

        public List<TableDto> GetSchemaTables(Guid schemaId)
        {
            var tables = _tableRepository.GetSchemaTables(schemaId);
            return tables.Select(t => new TableDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToList();
        }
    }
}
