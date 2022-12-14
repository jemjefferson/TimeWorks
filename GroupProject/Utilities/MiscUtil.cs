using GroupProject.DataAccess;

namespace GroupProject.Utilities
{
    public static class MiscUtil
    {
        public static double GetTotalMinutes(Hour hour)
        {
            if (hour.TimeOut == null)
            {
                return 0;
            }
            double totalMinutes = ((DateTime)hour.TimeOut - hour.TimeIn).TotalMinutes;
            foreach(Break b in hour.Breaks.Where(x => !x.Paid))
            {
                if (b.EndTime == null)
                {
                    continue;
                }
                totalMinutes -= ((DateTime)b.EndTime - b.StartTime).TotalMinutes;
            }

            return totalMinutes;
        }

        public static string GetFormattedHours(double totalMinutes)
        {
            if (totalMinutes < 0)
            {
                return "Clocked In";
            }
            TimeSpan total = TimeSpan.FromMinutes(totalMinutes);
            string minutes = total.Minutes.ToString();
            if (minutes.Length == 1)
            {
                minutes = "0" + minutes;
            }
            return $"{total.Hours}:{minutes}";
        }
    }
}
