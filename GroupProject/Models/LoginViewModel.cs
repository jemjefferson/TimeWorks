using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using GroupProject.DataAccess;

namespace GroupProject.Models
{
	public class LoginViewModel
	{
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
