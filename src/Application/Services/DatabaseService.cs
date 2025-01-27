using Core.DTOs;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DatabaseService
    {
        private readonly DatabaseRepository _databaseRepository;

        public DatabaseService(DatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        public void CreateDatabase(Guid userId, string databaseName)
        {
            _databaseRepository.CreateDatabase(userId, databaseName);
        }

        public List<DatabaseDto> GetUserDatabases(Guid userId)
        {
            var databases = _databaseRepository.GetUserDatabases(userId);
            return databases.Select(db => new DatabaseDto { Id = db.Id, Name = db.Name }).ToList();
        }
    }
}
