namespace ENROLLMENTSYSTEMBACKEND.Models
{
    public class CompletionRate
    {
        
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public decimal Rate { get; set; } 
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
