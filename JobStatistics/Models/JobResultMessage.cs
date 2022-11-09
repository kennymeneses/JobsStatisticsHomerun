using Microsoft.WindowsAzure.Storage.Table;

namespace JobStatistics.Models
{
    public class JobResultMessage : TableEntity
    {
        public string processName { get; set; }
        public string result { get; set; }
        public DateTime startedAt { get; set; }
        public int processNumber { get; set; }
        public string JobId { get; set; }

    }

    public class IncomingMessage : TableEntity
    {
        public string name { get; set; }
        public int process { get; set; }
        public DateTime? earliestStart { get; set; }
        public DateTime latestStart { get; set; }
        public string arguments { get; set; }

    }
}
