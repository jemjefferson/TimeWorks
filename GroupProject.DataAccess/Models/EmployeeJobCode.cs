using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GroupProject.DataAccess
{
    public class EmployeeJobCode
    {
        public int Id { get; set; }

        public Employee? Employee { get; set; }

        public int EmployeeId { get; set; }

        public JobCode? JobCode { get; set; }

        [Display(Name = "Job Code")]
        public int JobCodeId { get; set; }

        public double PayRate { get; set; }

        public bool Active { get; set; }

        public List<Hour>? Hours { get; set; }
    }
}
