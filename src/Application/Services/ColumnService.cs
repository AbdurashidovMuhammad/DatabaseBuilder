using Core.DTOs;
using DataAccess;

namespace Application.Services
{
    public class ColumnService
    {
        private readonly ColumnRepository _columnRepository;

        public ColumnService(ColumnRepository columnRepository)
        {
            _columnRepository = columnRepository;
        }

        public void CreateColumn(Guid tableId, string name, string dataType, bool isNullable, bool isUnique)
        {
            _columnRepository.AddColumn(tableId, name, dataType, isNullable, isUnique);
        }

        public void InsertRow(Guid tableId, Dictionary<string, object> columnValues)
        {
            _columnRepository.InsertRow(tableId, columnValues);
        }

        public List<ColumnDto> GetTableColumns(Guid tableId)
        {
            var columns = _columnRepository.GetTableColumns(tableId);
            return columns.Select(c => new ColumnDto
            {
                Id = c.Id,
                Name = c.Name,
                DataType = c.DataType,
                IsNullable = c.IsNullable,
                IsUnique = c.IsUnique
            }).ToList();
        }
    }
}
