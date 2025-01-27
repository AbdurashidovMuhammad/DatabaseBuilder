namespace Core.DTOs
{
    public class InsertRowDto
    {
        public string DatabaseName { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> ColumnValues { get; set; } = new();
    }
}
