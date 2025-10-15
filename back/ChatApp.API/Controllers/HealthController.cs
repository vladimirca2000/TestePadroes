using Microsoft.AspNetCore.Mvc;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        private readonly ChatDbContext _dbContext;

        public HealthController(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("alive")]
        public IActionResult Alive()
        {
            return Ok("Estou vivo");
        }

        [HttpGet("db")]
        public async Task<IActionResult> DatabaseAlive()
        {
            try
            {
                // Consulta simples para garantir que o banco está acessível
                await _dbContext.Database.ExecuteSqlRawAsync("SELECT 1");
                return Ok("Banco vivo");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Banco indisponível: {ex.Message}");
            }
        }
    }
}
