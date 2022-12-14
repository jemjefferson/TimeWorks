using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using GroupProject.DataAccess;

namespace GroupProject.Models
{
	public class PasswordChangeViewModel
	{
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
