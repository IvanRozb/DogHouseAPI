using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers;

[ApiController]
[Route("dogs")]
public class DogsController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public DogsController(IServiceManager serviceManager) => _serviceManager = serviceManager;
    
    [HttpGet]
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
    
    [HttpPost]
    public async Task<IActionResult> CreateDogAsync([FromBody] Dog dog, CancellationToken cancellationToken = default)
    {
        if (dog == null)
        {
            throw new BadRequestException("Invalid dog data");
        }

        await _serviceManager.DogsService.CreateAsync(dog, cancellationToken);
        return CreatedAtAction("CreateDog", dog);
    }
}