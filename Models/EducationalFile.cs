namespace Fazilat.Models
{
    public class EducationalFile
    {
        public string UserId { get; set; }
        public string Grade { get; set; }
        public string LastAvg { get; set; }
        public string RegistrationFormFileName { get; set; }
        public string LastWorkbookFileName { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
