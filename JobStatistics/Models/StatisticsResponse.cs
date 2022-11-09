namespace JobStatistics.Models
{
    public class StatisticsResponse
    {
        public int JobsPassed { get; set; }
        public int JobsFailed { get; set; }
        public int JobsStartedOnTime { get; set; }
        public int JobsDelayedOnTime { get; set; }
        public int BiggestDelayJobTimeInMinutes { get; set; }        
    }
}
