namespace ENROLLMENTSYSTEMBACKEND.DTOs
{
    public class FormAutoFillDataDto
    {
        public string StudentId { get; set; }
        public string FormType { get; set; }
        public Dictionary<string, string> Fields { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; }
        public DateTime LastUpdated { get; set; }
        public string DataSource { get; set; }
    }
} 