using Core.Common;

namespace Core.Entities
{
    public class Column : BaseEntity, IAuditEntity
    {
        public string Name { get; set; }
        public string DataType { get; set; }

        public Table Table { get; set; }
        public Guid TableId { get; set; }

        public bool IsNullable { get; set; }
        public bool IsUnique { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
