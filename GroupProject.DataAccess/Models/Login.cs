using System.ComponentModel.DataAnnotations;

namespace GroupProject.DataAccess
{
    public class Login
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "User name must be less than 50 characters")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        public Employee? Employee { get; set; }

        public int EmployeeID { get; set; }

        public bool Admin { get; set; }

        [Display(Name = "Password")]
        public string PasswordHash { get; set; }
    }
}
