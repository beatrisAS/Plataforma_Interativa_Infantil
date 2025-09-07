using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plataforma.API.Data;
using Plataforma.API.DTOs;

namespace Plataforma.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UsersController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<UserResponse>> GetAll()
            => await _db.Users
                .OrderByDescending(u => u.Id)
                .Select(u => new UserResponse(u.Id, u.Name, u.Email, u.Role, u.CreatedAt))
                .ToListAsync();
    }
}
