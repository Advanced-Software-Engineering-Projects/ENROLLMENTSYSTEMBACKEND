using Microsoft.AspNetCore.Mvc;

namespace ENROLLMENTSYSTEMBACKEND.Microservices.FormsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FormsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "Forms Service is running!" });
    }

    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok(new { status = "healthy" });
    }
} 