using CompanyManagement.Core.Abstractions.Services;
using CompanyManagement.Domain.Models;
using CompanyManagement.WebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.WebApi.Controllers;

[ApiController]
[Route("position")]
public class PositionController : ControllerBase
{
    private readonly IPositionService _positionService;

    public PositionController(IPositionService positionService)
    {
        _positionService = positionService;
    }
    
    [HttpGet("getpositions")]
    public async Task<IEnumerable<Position>> GetPositions()
    {
        return await _positionService.GetPositions();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPositionById(int id)
    {
        var position = await _positionService.GetPositionById(id);
        if (position is null)
            return NotFound();
        return Ok(position);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPositionByTitle([FromQuery] string title)
    {
        var position = await _positionService.GetPositionByTitle(title);
        if (position is null)
            return NotFound();
        return Ok(position);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePosition([FromBody] CreateOrUpdateTitle titleRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var title = titleRequest.Title;
        var position = await _positionService.GetPositionByTitle(title);
        if (position != null)
            return BadRequest($@"Position ""{title}"" already exists");
        var result = await _positionService.CreatePosition(title);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePosition(int id, [FromBody] CreateOrUpdateTitle titleRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var position = await _positionService.GetPositionById(id);
        if (position is null)
            return NotFound();
        var title = titleRequest.Title;
        position.Title = title;
        await _positionService.UpdatePosition(position);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePosition(int id)
    {
        var position = await _positionService.GetPositionById(id);
        if (position is null)
            return NotFound();
        await _positionService.DeletePosition(position);
        return Ok();
    }
}