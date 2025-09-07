using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plataforma.API.Data;
using Plataforma.API.Models;
using System.Security.Claims;

namespace Plataforma.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ActivitiesController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<Activity>> GetAll()
            => await _db.Activities.OrderByDescending(a => a.CreatedAt).ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Activity>> Get(int id)
            => await _db.Activities.FindAsync(id) is { } a ? Ok(a) : NotFound();

        [HttpPost]
        [Authorize(Roles = "admin,professor")]
        public async Task<ActionResult<Activity>> Create(Activity a)
        {
            // if authenticated, set CreatedByUserId from token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out var id)) a.CreatedByUserId = id;

            _db.Activities.Add(a);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = a.Id }, a);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "admin,professor")]
        public async Task<IActionResult> Update(int id, Activity input)
        {
            var a = await _db.Activities.FindAsync(id);
            if (a is null) return NotFound();
            a.Title = input.Title; a.Description = input.Description; a.Type = input.Type; a.Url = input.Url;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var a = await _db.Activities.FindAsync(id);
            if (a is null) return NotFound();
            _db.Activities.Remove(a);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
