using ChatApp.Application.DTOs;
using ChatApp.Core.Entities;
using ChatApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ChatDbContext _db;
        public UserController(ChatDbContext db)
        {
            _db = db;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest("Usuário já existe");
            var user = new User { Id = Guid.NewGuid(), Username = dto.Username, Email = dto.Email, CreatedAt = DateTime.UtcNow, LastSeen = DateTime.UtcNow };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Ok(new { user.Id, user.Username });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
                return Unauthorized("Usuário não encontrado");
            user.LastSeen = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(new { user.Id, user.Username });
        }
    }
}
