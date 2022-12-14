using System.Diagnostics.CodeAnalysis;
using TimeWorks.DataAccess;

namespace TimeWorks.Models
{
    public class JobCodeViewModel
    {
        public JobCode JobCode { get; set; }

        public List<JobCode>? JobCodes { get; set; }
    }
}
