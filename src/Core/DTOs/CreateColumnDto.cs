using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CreateColumnDto
    {
        public Guid TableId { get; set; }
        public required string Name { get; set; }
        public required string DataType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsUnique { get; set; }
    }
}
