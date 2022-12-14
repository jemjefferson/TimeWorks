using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TimeWorks.DataAccess
{
    public class Hour
    {
        public int Id { get; set; } 

        public DateTime TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        public Employee? Employee { get; set; }

        public int EmployeeId { get; set; }

        public DateTime? TimeEntered { get; set; }

        [MaxLength(250)]
        public string? Comment { get; set; }

        public EmployeeJobCode? EmployeeJobCode { get; set; }

        public int EmployeeJobCodeId { get; set; }

        public PayPeriod? PayPeriod { get; set; }

        public int? PayPeriodId { get; set; }

        public int? ApprovedBy { get; set; }

        public bool? EmployeeApproved { get; set; }

        public double PayRate { get; set; }

        public List<Break>? Breaks { get; set; } = new List<Break>();
    }
}
