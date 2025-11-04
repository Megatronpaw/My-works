using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
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
    public async Task<IActionResult> GetAllTests()
    {
        var tests = await _testRepository.GetAllAsync();
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

    [HttpPost]
    public async Task<IActionResult> CreateTest([FromBody] CreateTestRequest testRequest)
    {
        var testId = await _testRepository.CreateAsync(_mapper.Map<TestDto>(testRequest));
        return StatusCode(StatusCodes.Status201Created, new { Id = testId });
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
}