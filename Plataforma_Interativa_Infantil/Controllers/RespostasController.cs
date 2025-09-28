using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RespostasController(AppDbContext db) : ControllerBase
    {
        public class RespostaAtividadeDto
        {
            public int CriancaId { get; set; }
            public int AtividadeId { get; set; }
            public int Desempenho { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RespostaAtividadeDto dto)
        {
            if (dto == null) return BadRequest();

            var crianca = await db.Criancas.FindAsync(dto.CriancaId);
            if (crianca == null)
                return BadRequest($"Criança {dto.CriancaId} não existe.");

            var atividade = await db.Atividades.FindAsync(dto.AtividadeId);
            if (atividade == null)
                return BadRequest($"Atividade {dto.AtividadeId} não existe.");

            var resposta = new RespostaAtividade
            {
                CriancaId = dto.CriancaId,
                AtividadeId = dto.AtividadeId,
                Desempenho = dto.Desempenho,
                DataRealizacao = DateTime.UtcNow
            };

            db.RespostasAtividades.Add(resposta);
            await db.SaveChangesAsync();

            return CreatedAtAction(null, new { id = resposta.Id }, resposta);
        }
    }
}
