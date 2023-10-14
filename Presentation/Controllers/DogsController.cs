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
        if (attribute is not null)
        {
            var availableAttributes = new List<string> { "name", "color", "tail_length", "weight" };
            if (!availableAttributes.Contains(attribute))
            {
                throw new BadRequestException("attribute must be name, color, tail_length, or weight");
            }
        }
        
        if (order is not null)
        {
            var availableOrders = new List<string> { "asc", "desc"};
            if (!availableOrders.Contains(order))
            {
                throw new BadRequestException("order must be asc or desc");
            }
        }
        
        if (pageNumber < 1)
        {
            throw new BadRequestException("pageNumber must be higher then 0");
        }
        
        if (pageSize < 1)
        {
            throw new BadRequestException("pageSize must be higher then 0");
        }
        
        var dogs = await _serviceManager.DogsService.GetAllAsync(attribute, order, pageNumber, pageSize, cancellationToken);
        return Ok(dogs);
    }
    
    [HttpPost("dog")]
    public async Task<IActionResult> CreateDogAsync([FromBody] Dog dog, CancellationToken cancellationToken = default)
    {
        await _serviceManager.DogsService.CreateAsync(dog, cancellationToken);
        return CreatedAtAction("CreateDog", dog);
    }
}