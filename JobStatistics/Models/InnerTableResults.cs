namespace JobStatistics.Models
{
    public class InnerJobTablesResults
    {
        public string rowKey { get; set; }
        public DateTime earliestStart { get; set; }
        public DateTime latestStart { get; set; }
        public DateTime startedAt { get; set; }
    }
}
