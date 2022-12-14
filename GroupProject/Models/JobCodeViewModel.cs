using System.Diagnostics.CodeAnalysis;
using GroupProject.DataAccess;

namespace GroupProject.Models
{
    public class JobCodeViewModel
    {
        public JobCode JobCode { get; set; }

        public List<JobCode>? JobCodes { get; set; }
    }
}
