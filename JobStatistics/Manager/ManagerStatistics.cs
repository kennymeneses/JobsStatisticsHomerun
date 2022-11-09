using JobStatistics.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace JobStatistics.Manager
{
    public class ManagerStatistics : IManagerStatistics
    {
        IConfiguration configuration;
        private readonly string TableConnectionString;
        private readonly string tableNameIM;
        private readonly string tableNameJR;
        private CloudStorageAccount storageAccount;
        private CloudTableClient tableClient;
        private CloudTable tableJobResult;
        private CloudTable tableSourceJobs;

        public ManagerStatistics(IConfiguration _configuration)
        {
            configuration = _configuration;

            TableConnectionString = configuration["AzureTableStorage:ConnectionString"];
            tableNameIM = "incomingmessage";
            tableNameJR = "jobResult";

            storageAccount = CloudStorageAccount.Parse(TableConnectionString);
            tableClient = storageAccount.CreateCloudTableClient();
            tableSourceJobs = tableClient.GetTableReference(tableNameIM);
            tableJobResult = tableClient.GetTableReference(tableNameJR);
        }

        public async Task<int> GetFailedJobsNumber()
        {
            var condition = TableQuery.GenerateFilterCondition(Constants.COLUMN_RESULT, QueryComparisons.Equal, Constants.FAIL);
            var query = new TableQuery<JobResultMessage>().Where(condition);
            var result = await tableJobResult.ExecuteQuerySegmentedAsync(query, null);

            return result.Count();
        }

        public async Task<int> GetSucceededJobsNumber()
        {
            var condition = TableQuery.GenerateFilterCondition(Constants.COLUMN_RESULT, QueryComparisons.Equal, Constants.PASS);
            var query = new TableQuery<JobResultMessage>().Where(condition);
            var result = await tableJobResult.ExecuteQuerySegmentedAsync(query, null);

            return result.Count();
        }

        public async Task<List<InnerJobTablesResults>> GetOriginalInfoJobs()
        {
            var condition = TableQuery.GenerateFilterCondition(Constants.COLUMN_RESULT, QueryComparisons.Equal, Constants.FAIL);
            var query = new TableQuery<JobResultMessage>().Where(condition);
            var failJobResults = await tableJobResult.ExecuteQuerySegmentedAsync(query, null);
            //var condition2 = TableQuery.GenerateFilterCondition("ColumnB", QueryComparisons.Equal, "Anotherthing");

            //var finalFilter = TableQuery.CombineFilters(condition, TableOperators.And, condition2);

            //var query = new TableQuery<JobResultMessage>().Where(condition).Where(finalFilter);

            var queryJobSource = new TableQuery<IncomingMessage>();
            var originalJobsInfo = await tableSourceJobs.ExecuteQuerySegmentedAsync(queryJobSource, null);

            var joinedList = (from failJobs in failJobResults
                              join jobsInfo in originalJobsInfo on failJobs.RowKey equals jobsInfo.RowKey
                              select new InnerJobTablesResults { rowKey = jobsInfo.RowKey, 
                                                                 earliestStart = (DateTime)jobsInfo.earliestStart,
                                                                 latestStart = jobsInfo.latestStart,
                                                                startedAt = failJobs.startedAt });

            return joinedList.ToList();
        }

        public int GetJobWithBiggestDelayTime(List<InnerJobTablesResults> failJobs)
        {
            int diff = 0;
            foreach (InnerJobTablesResults failJob in failJobs)
            {
                if((failJob.startedAt.Minute - failJob.earliestStart.Minute) >= diff)
                {
                    diff = failJob.startedAt.Minute - failJob.earliestStart.Minute;
                }
            }

            return diff;
        }

        public async Task<int> GetJobsPassed()
        {
            var condition = TableQuery.GenerateFilterCondition(Constants.COLUMN_RESULT, QueryComparisons.Equal, Constants.PASS);
            var query = new TableQuery<JobResultMessage>().Where(condition);
            var passedJobResults = await tableJobResult.ExecuteQuerySegmentedAsync(query, null);

            return passedJobResults.Count();
        }

        public async Task<int> GetJobsFailed()
        {
            var condition = TableQuery.GenerateFilterCondition(Constants.COLUMN_RESULT, QueryComparisons.Equal, Constants.FAIL);
            var query = new TableQuery<JobResultMessage>().Where(condition);
            var passedJobResults = await tableJobResult.ExecuteQuerySegmentedAsync(query, null);

            return passedJobResults.Count();
        }        
    }
}
