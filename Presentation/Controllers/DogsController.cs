using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers;

[ApiController]
[Route("api/dogs")]
public class DogsController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public DogsController(IServiceManager serviceManager) => _serviceManager = serviceManager;
    
    [HttpGet]
    public async Task<IActionResult> GetDogsAsync(
        [FromQuery] string attribute,
        [FromQuery] string order,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var dogs = await _serviceManager.DogsService.GetAllAsync(attribute, order, pageNumber, pageSize, cancellationToken);
            return Ok(dogs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateDogAsync([FromBody] Dog dog, CancellationToken cancellationToken = default)
    {
        if (dog == null)
        {
            return BadRequest("Invalid dog data");
        }

        try
        {
            await _serviceManager.DogsService.CreateAsync(dog, cancellationToken);
            return CreatedAtRoute("GetDog", new { id = dog.Id }, dog);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}