using Domain.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("ping")]
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
            throw new BadRequestException("API version not configured.");
        }

        if (!version.StartsWith("Dogshouseservice.Version"))
        {
            throw new BadRequestException("Invalid API version format.");
        }

        return Ok(version);
    }
}
