using Core.Common;

namespace Core.Entities
{
    public class Scheme : BaseEntity, IAuditEntity
    {
        public string Name { get; set; }

        public Database Database { get; set; }
        public Guid DatabaseId { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
