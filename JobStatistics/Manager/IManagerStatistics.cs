using JobStatistics.Models;

namespace JobStatistics.Manager
{
    public interface IManagerStatistics
    {
        Task<int> GetFailedJobsNumber();
        Task<int> GetSucceededJobsNumber();
        Task<List<InnerJobTablesResults>> GetOriginalInfoJobs();
        int GetJobWithBiggestDelayTime(List<InnerJobTablesResults> failJobs);
        Task<int> GetJobsPassed();
        Task<int> GetJobsFailed();
    }
}
