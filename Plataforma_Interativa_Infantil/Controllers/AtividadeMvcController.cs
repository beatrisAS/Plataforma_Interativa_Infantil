using Microsoft.AspNetCore.Mvc;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace backend.Controllers
{
    public class AtividadeMvcController : Controller
    {
        private readonly AppDbContext _db;
        public AtividadeMvcController(AppDbContext db) { _db = db; }


       public async Task<IActionResult> Detalhe(int id, int questaoIndex = 0)
{
    var atividade = await _db.Atividades
        .Include(a => a.Questoes)
        .ThenInclude(q => q.Alternativas)
        .FirstOrDefaultAsync(a => a.Id == id);

    if (atividade == null) return NotFound();

    ViewBag.QuestaoIndex = questaoIndex; // envia o índice da questão atual
    return View("Atividade", atividade);
}


        public async Task<IActionResult> Proxima(int id)
        {
            var atividadeAtual = await _db.Atividades.FirstOrDefaultAsync(a => a.Id == id);
            if (atividadeAtual == null) return NotFound();

            var proxima = await _db.Atividades
                .Where(a => a.Id > atividadeAtual.Id)
.OrderBy(a => a.Id)

                .FirstOrDefaultAsync();

            if (proxima == null)
                return View("Fim"); // 👈 você cria uma view de conclusão

            return RedirectToAction("Detalhe", new { id = proxima.Id });
        }
        
        public async Task<IActionResult> ProximaQuestao(int atividadeId, int questaoId)
{
    var atividade = await _db.Atividades
        .Include(a => a.Questoes.OrderBy(q => q.Id))
        .ThenInclude(q => q.Alternativas)
        .FirstOrDefaultAsync(a => a.Id == atividadeId);

    if (atividade == null) return NotFound();

    var questaoAtual = atividade.Questoes.FirstOrDefault(q => q.Id == questaoId);
    if (questaoAtual == null) return NotFound();

    var proxima = atividade.Questoes
        .Where(q => q.Id > questaoAtual.Id)
        .OrderBy(q => q.Id)
        .FirstOrDefault();

    if (proxima == null)
    {
        // Última questão → vai para tela de conclusão
        return RedirectToAction("FimAtividade", new { id = atividadeId });
    }

    return View("Questao", proxima); // 👈 nova view só para mostrar a questão
}
        public IActionResult FimAtividade(int id)
        {
            // Aqui você pode mostrar um resumo da atividade, pontuação, etc.
            ViewBag.AtividadeId = id;
            return View("Fim"); // 👈 você cria uma view de conclusão
        }

        public async Task<IActionResult> FimAtividade(int id, int criancaId)
{
    // Busca a criança
    var crianca = await _db.Criancas.FindAsync(criancaId);
    if (crianca != null)
    {
        crianca.Estrelas += 1;  // adiciona a estrelinha
        await _db.SaveChangesAsync();
    }

    ViewBag.AtividadeId = id;
    ViewBag.CriancaId = criancaId;
    return View();
}
public async Task<IActionResult> Questao(int atividadeId, int ordem = 0)
{
    var atividade = await _db.Atividades
        .Include(a => a.Questoes.OrderBy(q => q.Ordem))
        .ThenInclude(q => q.Alternativas)
        .FirstOrDefaultAsync(a => a.Id == atividadeId);

    if (atividade == null) return NotFound();

    var questao = atividade.Questoes.Skip(ordem).FirstOrDefault();
    if (questao == null)
    {
        // acabou a atividade → tela final
        return RedirectToAction("FimAtividade", new { id = atividadeId });
    }

    ViewBag.AtividadeTitulo = atividade.Titulo;
    ViewBag.Ordem = ordem;
    ViewBag.AtividadeId = atividadeId;
    return View("Questao", questao);
}



    }
}
