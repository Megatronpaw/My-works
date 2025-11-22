using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Requests.Attempt;

namespace TestingPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class AttemptsController(IAttemptRepository attemptRepository, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Начать попытку
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAttempt(CreateAttemptRequest attempt)
    {
        var attemptDto = mapper.Map<AttemptDto>(attempt);

        var attemptId = await attemptRepository.CreateAsync(attemptDto);

        return StatusCode(StatusCodes.Status201Created, new { Id = attemptId });
    }

    /// <summary>
    /// Закончить попытку
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAttempt(UpdateAttemptRequest attempt)
    {
        var attemptDto = mapper.Map<AttemptDto>(attempt);

        await attemptRepository.UpdateAsync(attemptDto);

        return StatusCode(StatusCodes.Status200OK);
    }
}

