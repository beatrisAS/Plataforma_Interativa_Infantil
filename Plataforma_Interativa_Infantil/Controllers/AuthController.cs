using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
    private readonly AppDbContext _db;

    public AuthController(AppDbContext db) {
        _db = db;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Usuario model) {
        if (await _db.Usuarios.AnyAsync(u => u.Email == model.Email))
            return BadRequest(new { error = "Email já cadastrado" });

        _db.Usuarios.Add(model);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Registrado com sucesso" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req) {
        var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == req.Email);
        if (user == null) return Unauthorized();

    
        if (req.Senha != user.Senha) return Unauthorized();

        return Ok(new { message = "Login bem-sucedido", role = user.Perfil });
    }

    public class LoginRequest {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
