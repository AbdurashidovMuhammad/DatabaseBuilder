using Core.Common;

namespace Core.Entities
{
    public class Table : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SchemeId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
