using Core.DTOs;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SchemaService
    {
        private readonly SchemaRepository _schemaRepository;

        public SchemaService(SchemaRepository schemaRepository)
        {
            _schemaRepository = schemaRepository;
        }

        public void CreateSchema(Guid databaseId, string schemaName)
        {
            _schemaRepository.CreateSchema(databaseId, schemaName);
        }

        public List<SchemaDto> GetDatabaseSchemas(Guid databaseId)
        {
            var schemas = _schemaRepository.GetDatabaseSchemas(databaseId);
            return schemas.Select(s => new SchemaDto { Id = s.Id, Name = s.Name }).ToList();
        }
    }
}
