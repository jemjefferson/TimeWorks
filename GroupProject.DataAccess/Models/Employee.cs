using System.ComponentModel.DataAnnotations;

namespace GroupProject.DataAccess
{
    public class Employee
    {
        [Display(Name = "Employee Id")]
        public int Id { get; set; }

        [MaxLength(25)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(25)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [MaxLength(75)]
        public string City { get; set; }

        [StringLength(2, MinimumLength = 2)]
        public string State { get; set; }

        [MaxLength(10)]
        public string Zip { get; set; }

        public int Manager { get; set; }

        public List<EmployeeJobCode>? EmployeeJobCodes { get; set; }

        public Login Login { get; set; }

        public List<Hour>? Hours { get; set; }
    }
}
