using GroupProject.DataAccess;

namespace GroupProject.Models
{
    public class EmployeeJobCodeViewModel
    {
        public EmployeeJobCode? EmployeeJobCode { get; set; }

        public int EmployeeId { get; set; }

        public JobCode? JobCode { get; set; }

        public List<JobCode>? JobCodes { get; set; }
    }
}
