using Core.Common;

namespace Core.Entities
{
    public class Database : BaseEntity, IAuditEntity
    {
        public string Name { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
