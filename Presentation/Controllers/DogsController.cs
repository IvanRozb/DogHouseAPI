using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers;

[ApiController]
public class DogsController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public DogsController(IServiceManager serviceManager) => _serviceManager = serviceManager;
    
    [HttpGet("dogs")]
    public async Task<IActionResult> GetDogsAsync(
        [FromQuery] string? attribute,
        [FromQuery] string? order,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken = default)
    {
        var dogs = await _serviceManager.DogsService.GetAllAsync(attribute, order, pageNumber, pageSize, cancellationToken);
        
        return Ok(dogs);
    }
    
    [HttpPost("dog")]
    public async Task<IActionResult> CreateDogAsync([FromBody] Dog dog, CancellationToken cancellationToken = default)
    {
        var response = await _serviceManager.DogsService.CreateAsync(dog, cancellationToken);
        
        return CreatedAtAction("CreateDog", new { Name = response.Name}, response);
    }
}