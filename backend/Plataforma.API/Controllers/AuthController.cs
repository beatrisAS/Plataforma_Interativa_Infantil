using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plataforma.API.Data;
using Plataforma.API.DTOs;
using Plataforma.API.Models;
using Plataforma.API.Services;
using System.Security.Cryptography;
using System.Text;

namespace Plataforma.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IJwtService _jwt;
        public AuthController(AppDbContext db, IJwtService jwt) { _db = db; _jwt = jwt; }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest req)
        {
            if (await _db.Users.AnyAsync(u => u.Email == req.Email))
                return Conflict("Email já cadastrado");

            var user = new User
            {
                Name = req.Name,
                Email = req.Email,
                PasswordHash = Hash(req.Password),
                Role = "aluno"
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var token = _jwt.GenerateToken(user);
            return Ok(new AuthResponse(token, user.Name, user.Email, user.Role));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest req)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
            if (user is null || user.PasswordHash != Hash(req.Password))
                return Unauthorized("Credenciais inválidas");

            var token = _jwt.GenerateToken(user);
            return Ok(new AuthResponse(token, user.Name, user.Email, user.Role));
        }

        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }
    }
}
