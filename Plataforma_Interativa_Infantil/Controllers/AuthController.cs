using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using backend.Utils;
using backend.Services;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
    private readonly AppDbContext _db;
    private readonly JwtService _jwt;
    public AuthController(AppDbContext db, IConfiguration cfg) {
        _db = db;
        _jwt = new JwtService(cfg["Jwt:Key"] ?? "supersecretkey");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Usuario model) {
        if (await _db.Usuarios.AnyAsync(u => u.Email == model.Email)) return BadRequest(new { error = "Email já cadastrado" });
        // hash password
        model.SenhaHash = PasswordHasher.Hash(model.SenhaHash);
        _db.Usuarios.Add(model);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Registrado com sucesso" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req) {
        var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == req.Email);
        if (user == null) return Unauthorized();
        if (!PasswordHasher.Verify(req.Senha, user.SenhaHash)) return Unauthorized();
        var token = _jwt.GenerateToken(user.Id, user.Email, user.Perfil);
        return Ok(new { token, role = user.Perfil });
    }

    public class LoginRequest { public string Email { get; set; } = string.Empty; public string Senha { get; set; } = string.Empty; }
}
