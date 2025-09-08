using Microsoft.AspNetCore.Mvc;
using Plataforma.API.Data;
using Plataforma.API.Models;

namespace Plataforma.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ResponsesController(AppDbContext db) => _db = db;

        // GET api/responses
        [HttpGet]
        public IEnumerable<Response> GetAll() => _db.Responses.ToList();

        // GET api/responses/5
        [HttpGet("{id:int}")]
        public ActionResult<Response> Get(int id)
            => _db.Responses.Find(id) is { } r ? Ok(r) : NotFound();

        // POST api/responses
        [HttpPost]
        public async Task<ActionResult<Response>> Create(Response r)
        {
            _db.Responses.Add(r);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = r.Id }, r);
        }
    }
}
