using Microsoft.AspNetCore.Mvc;
using WebApplication4.Application.Reports;

namespace WebApplication4.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class ReportsController : ControllerBase
        {
            private readonly IReportService _reportService;

            public ReportsController(IReportService reportService)
            {
                _reportService = reportService;
            }

            [HttpGet("group-leaders")]
            public async Task<IActionResult> GetGroupLeaders([FromQuery] GroupLeadersAndLaggardsFilter filter)
            {
                var result = await _reportService.GetGroupLeadersAndLaggardsAsync(filter);
                return Ok(result);
            }

            [HttpGet("student-test-results")]
            public async Task<IActionResult> GetStudentTestResults([FromQuery] StudentTestResultsFilter filter)
            {
                var result = await _reportService.GetStudentTestResultsAsync(filter);
                return Ok(result);
            }
    }

}
