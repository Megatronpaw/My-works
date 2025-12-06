using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Infrastructure.Repositories;
using TestingPlatform.Requests.Test;
using TestingPlatform.Responses.Test;

namespace TestingPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestsController : ControllerBase
{
    private readonly ITestRepository _testRepository;
    private readonly IMapper _mapper;

    public TestsController(ITestRepository testRepository, IMapper mapper)
    {
        _testRepository = testRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTests(
        [FromQuery] bool? isPublic = null,
        [FromQuery] List<int> groupIds = null,
        [FromQuery] List<int> studentIds = null)
    {
        var tests = await _testRepository.GetAllAsync(isPublic, groupIds, studentIds);
        return Ok(_mapper.Map<IEnumerable<TestResponse>>(tests));
    }

    [HttpGet("for-student/{studentId:int}")]
    public async Task<IActionResult> GetAllTestsForStudent(int studentId)
    {
        var tests = await _testRepository.GetAllForStudent(studentId);
        return Ok(_mapper.Map<IEnumerable<TestResponse>>(tests));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTestById(int id)
    {
        try
        {
            var test = await _testRepository.GetByIdAsync(id);
            return Ok(_mapper.Map<TestResponse>(test));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("recent/{count:int?}")]
    public async Task<IActionResult> GetTopRecentTests(int? count = 5)
    {
        var tests = await _testRepository.GetTopRecentAsync(count ?? 5);
        return Ok(_mapper.Map<IEnumerable<TestResponse>>(tests));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTest([FromBody] CreateTestRequest testRequest)
    {
        try
        {
            var testId = await _testRepository.CreateAsync(_mapper.Map<TestDto>(testRequest));
            return StatusCode(StatusCodes.Status201Created, new { Id = testId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTest(int id, [FromBody] UpdateTestRequest testRequest)
    {
        try
        {
            if (id != testRequest.Id)
                return BadRequest("ID в пути и в теле запроса не совпадают");

            await _testRepository.UpdateAsync(_mapper.Map<TestDto>(testRequest));
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTest(int id)
    {
        try
        {
            await _testRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    #region Statistics Endpoints

    [HttpGet("stats/count-by-type")]
    public async Task<IActionResult> GetTestCountByType()
    {
        var stats = await _testRepository.GetTestCountByTypeAsync();
        return Ok(stats);
    }

    [HttpGet("stats/course-stats")]
    public async Task<IActionResult> GetCourseStats()
    {
        var stats = await _testRepository.GetCourseStatsAsync();
        return Ok(stats);
    }

    [HttpGet("stats/direction-averages")]
    public async Task<IActionResult> GetDirectionAverages()
    {
        var stats = await _testRepository.GetDirectionAveragesAsync();
        return Ok(stats);
    }

    [HttpGet("stats/timeline-by-public")]
    public async Task<IActionResult> GetTestTimelineByPublic()
    {
        var stats = await _testRepository.GetTestTimelineByPublicAsync();
        return Ok(stats);
    }

    [HttpGet("stats/top-groups/{top:int?}")]
    public async Task<IActionResult> GetTopGroupsByTestCount(int? top = 10)
    {
        var stats = await _testRepository.GetTopGroupsByTestCountAsync(top ?? 10);
        return Ok(stats);
    }

    [HttpGet("manage")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(typeof(IEnumerable<TestResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTestsForManager([FromQuery] bool? isPublic, [FromQuery] List<int> groupIds, [FromQuery] List<int> studentIds)
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var tests = await _testRepository.GetAllAsync(isPublic, groupIds, studentIds);

        return Ok(_mapper.Map<IEnumerable<TestResponse>>(tests));
    }
    #endregion
}