namespace practice.Config
{
    public class DataAccessOptions
    {
        public const string SectionName = "DataAccess";
        public string Provider { get; set; } = "Dapper"; // по умолчанию
    }
}