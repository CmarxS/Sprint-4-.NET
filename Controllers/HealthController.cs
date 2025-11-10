using Microsoft.AspNetCore.Mvc;

namespace MottoMap.Controllers
{
    /// <summary>
    /// Controller para Health Checks
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Tags("?? Health")]
    public class HealthController : ControllerBase
    {
    /// <summary>
        /// Endpoint básico de health check
     /// </summary>
        /// <returns>Status da API</returns>
     /// <response code="200">API está funcionando</response>
     [HttpGet]
   [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetHealth()
        {
            return Ok(new
            {
        status = "Healthy",
           timestamp = DateTime.UtcNow,
        service = "MottoMap API",
     version = "1.0"
 });
        }

        /// <summary>
        /// Health check detalhado com informações do sistema
   /// </summary>
     /// <returns>Status detalhado da API</returns>
        /// <response code="200">Informações detalhadas do sistema</response>
     [HttpGet("detailed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
   public IActionResult GetDetailedHealth()
      {
            return Ok(new
      {
       status = "Healthy",
          timestamp = DateTime.UtcNow,
        service = "MottoMap API",
 version = "1.0",
      uptime = TimeSpan.FromMilliseconds(Environment.TickCount64),
    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
         machineName = Environment.MachineName,
         processorCount = Environment.ProcessorCount,
           osVersion = Environment.OSVersion.ToString()
         });
 }
    }
}
