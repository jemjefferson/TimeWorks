using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using TimeWorks.DataAccess;

namespace TimeWorks.Models
{
	public class PasswordChangeViewModel
	{
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public Employee? Employee { get; set; }
    }
}
