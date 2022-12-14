using System.ComponentModel.DataAnnotations;

namespace TimeWorks.DataAccess
{
    public class JobCode
    {
        public int Id { get; set; }

        [Display(Name = "Job Title")]
        [MaxLength(100)]
        public string JobTitle { get; set; }

        [Range(.01, 1000000)]
        [Display(Name = "Starting Pay")]
        public double StartingPay { get; set; }

        [MaxLength(50)]
        public string Department { get; set; }

        public List<EmployeeJobCode>? EmployeeJobCodes { get; set; }
    }
}
