namespace TimeWorks.DataAccess
{
    public class PayPeriod
    {
        public int Id { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public List<Hour> Hours { get; set; }
    }
}
