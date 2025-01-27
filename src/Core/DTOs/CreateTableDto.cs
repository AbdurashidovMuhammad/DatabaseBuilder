namespace Core.DTOs
{
    public class CreateTableDto
    {
        public Guid SchemaId { get; set; }
        public required string Name { get; set; }
    }
}
