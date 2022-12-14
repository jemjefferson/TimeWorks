using GroupProject.DataAccess;

namespace GroupProject.Models
{
    public class EmployeeViewModel
    {
        public Employee Employee { get; set; }

        public List<Employee>? Employees { get; set; }
        
        public List<JobCode>? JobCodes { get; set; }
    }
}