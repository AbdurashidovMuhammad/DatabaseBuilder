namespace Core.DTOs
{
    public class CreateDatabaseDto
    {
        public Guid UserId { get; set; }
        public required string Name { get; set; }
    }
}
