using Microsoft.AspNetCore.Mvc;

namespace DentalTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<object> GetHealth()
    {
        return Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"
        });
    }

    [HttpGet("ready")]
    public ActionResult<object> GetReadiness()
    {
        return Ok(new
        {
            Status = "Ready",
            Timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("live")]
    public ActionResult<object> GetLiveness()
    {
        return Ok(new
        {
            Status = "Alive",
            Timestamp = DateTime.UtcNow
        });
    }
}