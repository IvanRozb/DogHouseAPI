using Microsoft.Extensions.Configuration;

namespace Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("api/ping")]
[ApiController]
public class PingController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public PingController(IConfiguration configuration) => _configuration = configuration;
    
    [HttpGet]
    public IActionResult Ping()
    {
        var version = _configuration.GetSection("ApiVersion").Value;

        if (string.IsNullOrEmpty(version))
        {
            return BadRequest("API version not configured.");
        }

        if (!version.StartsWith("Dogshouseservice.Version"))
        {
            return BadRequest("Invalid API version format.");
        }

        return Ok(version);
    }
}
