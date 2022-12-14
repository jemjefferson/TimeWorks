using System.ComponentModel.DataAnnotations;

namespace TimeWorks.DataAccess
{
    public class Break
    {
        public int Id { get; set; }

        public DateTime StartTime { get;set; }  

        public DateTime? EndTime { get; set; }

        public bool Paid { get; set; }

        public Hour Hour { get; set; }

        public int HourId { get; set; }

        [MaxLength(250)]
        public string? Comment { get; set; }
    }
}
