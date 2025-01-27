namespace Core.DTOs
{
    public class CreateSchemaDto
    {
        public Guid DatabaseId { get; set; }
        public required string Name { get; set; }
    }
}
