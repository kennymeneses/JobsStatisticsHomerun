using JobStatistics.Manager;
using JobStatistics.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobStatistics.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class JobStatistics : ControllerBase
    {
        IManagerStatistics manager;

        public JobStatistics(IManagerStatistics _managerStatistics)
        {
            manager = _managerStatistics;
        }

        [HttpGet]
        [Route("GetReport")]
        public async Task<IActionResult> GetSomething()
        {
            var statisticsResponse = new StatisticsResponse();
            var listFailMessages = await manager.GetOriginalInfoJobs();

            statisticsResponse.JobsPassed = await manager.GetJobsPassed();
            statisticsResponse.JobsFailed = await manager.GetJobsFailed();
            statisticsResponse.JobsStartedOnTime = await manager.GetJobsPassed();
            statisticsResponse.JobsDelayedOnTime = await manager.GetJobsFailed();
            statisticsResponse.BiggestDelayJobTimeInMinutes = manager.GetJobWithBiggestDelayTime(listFailMessages);

            return Ok(statisticsResponse);
        }
    }
}
