using Core.Common;

namespace Core.Entities
{
    public class User : BaseEntity, IAuditEntity
    {
        public required string FullName { get; set; }

        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
