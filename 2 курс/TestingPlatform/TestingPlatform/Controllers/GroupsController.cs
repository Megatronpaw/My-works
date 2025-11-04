using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Responses.Group;

namespace practice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController(IGroupRepository groupRepository, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllGroups()
    {
        var groups = await groupRepository.GetAllAsync();

        return Ok(mapper.Map<IEnumerable<GroupResponse>>(groups));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGroupById(int id)
    {
        var group = await groupRepository.GetByIdAsync(id);

        return Ok(mapper.Map<GroupResponse>(group));
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest group)
    {
        var id = await groupRepository.CreateAsync(mapper.Map<GroupDto>(group));

        return StatusCode(StatusCodes.Status201Created, new { Id = id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateGroup([FromBody] UpdateGroupRequest group)
    {
        await groupRepository.UpdateAsync(mapper.Map<GroupDto>(group));

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        await groupRepository.DeleteAsync(id);

        return NoContent();
    }
}

