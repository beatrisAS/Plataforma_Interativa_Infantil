using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using backend.ViewModels; // Seu ProfileEditViewModel está aqui
using Microsoft.EntityFrameworkCore;
using backend.Utils; // Para PasswordHasher
using System.Security.Claims; // Para pegar o ID do usuário logado
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Proteger as ações
using Microsoft.AspNetCore.Authentication; // Para HttpContext.SignOutAsync
using Microsoft.AspNetCore.Authentication.Cookies; // Para CookieAuthenticationDefaults
using System.Linq; // Para SelectMany e FirstOrDefault

namespace backend.Controllers
{
    [Authorize] // Garante que apenas usuários logados acessem as ações de perfil
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        // Injeta o DbContext
        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // Ação chamada pelo formulário no ProfileModal para salvar alterações
        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra CSRF
        public async Task<IActionResult> Edit(ProfileEditViewModel model)
        {
            // Valida os dados recebidos do formulário
            if (!ModelState.IsValid)
            {
                // Pega a primeira mensagem de erro de validação
                var firstError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
                TempData["ProfileError"] = firstError?.ErrorMessage ?? "Dados inválidos. Verifique os campos.";

                // Redireciona de volta para o dashboard do usuário atual
                return RedirectToUserDashboard();
            }

            // --- Pega o ID do usuário logado ---
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out int usuarioId))
            {
                // Isso não deve acontecer se [Authorize] estiver funcionando
                return Unauthorized("Usuário não encontrado ou não autenticado.");
            }

            // --- Encontra o usuário no banco de dados ---
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
            {
                // Se o usuário não existe mais no banco por algum motivo
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Desloga
                return RedirectToAction("Login", "Account"); // Manda para login
            }

            // --- Atualiza as propriedades do usuário ---
            usuario.Nome = model.Nome;

            // Verifica se o email foi alterado e se já existe em outra conta
            if (usuario.Email != model.Email)
            {
                var emailExists = await _context.Usuarios.AnyAsync(u => u.Email == model.Email && u.Id != usuarioId);
                if (emailExists)
                {
                    TempData["ProfileError"] = "Este email já está em uso por outra conta.";
                    return RedirectToUserDashboard();
                }
                usuario.Email = model.Email;
            }

            // --- Atualiza a senha (se uma nova foi fornecida) ---
            if (!string.IsNullOrEmpty(model.NovaSenha))
            {
                // Validação de senha pode ser adicionada aqui (comprimento, complexidade)
                usuario.Senha = PasswordHasher.Hash(model.NovaSenha);
            }

            // --- Salva as alterações no banco ---
            try
            {
                await _context.SaveChangesAsync();
                TempData["ProfileSuccess"] = "Perfil atualizado com sucesso!";
            }
            catch (DbUpdateException )
            {
                // Idealmente, logar o erro 'ex' para diagnóstico
                TempData["ProfileError"] = "Erro ao salvar as alterações. Tente novamente.";
            }

            // --- Redireciona de volta para o dashboard do usuário ---
            return RedirectToUserDashboard();
        }

        // Ação chamada pelo formulário de confirmação de exclusão no ProfileModal
        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra CSRF
        public async Task<IActionResult> Delete() // Nenhum parâmetro, usa o usuário logado
        {
             // --- Pega o ID do usuário logado ---
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out int usuarioId))
            {
                return Unauthorized("Usuário não encontrado.");
            }

            // --- Encontra o usuário no banco ---
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
            {
                // Usuário já deletado ou ID inválido, apenas redireciona para Home
                return RedirectToAction("Index", "Home");
            }

            // --- Deleta Dados Relacionados (IMPORTANTE!) ---
            try
            {
                if (usuario.Perfil.ToLower() == "crianca")
                {
                    var criancaProfile = await _context.Criancas.FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
                    if (criancaProfile != null)
                    {
                         // Deleta respostas da criança (se não configurado cascade)
                         var respostas = await _context.RespostasAtividades.Where(r => r.CriancaId == criancaProfile.Id).ToListAsync();
                         if (respostas.Any()) _context.RespostasAtividades.RemoveRange(respostas);
                         
                         // **********************************************************
                         // ▼▼▼ CORREÇÃO ADICIONADA AQUI ▼▼▼
                         // Deleta vínculos professor-aluno (Esta é a correção do erro)
                         var vinculosProfessor = await _context.ProfessorAlunos
                             .Where(pa => pa.CriancaId == criancaProfile.Id) // (Assumindo que o nome da propriedade é CriancaId)
                             .ToListAsync();
                         
                         if (vinculosProfessor.Any()) _context.ProfessorAlunos.RemoveRange(vinculosProfessor);
                         // ▲▲▲ FIM DA CORREÇÃO ▲▲▲
                         // **********************************************************

                        _context.Criancas.Remove(criancaProfile);
                    }
                }
                else if (usuario.Perfil.ToLower() == "pai")
                {
                    // Desvincula crianças ligadas a este pai
                    var criancasVinculadas = await _context.Criancas.Where(c => c.IdResponsavel == usuarioId).ToListAsync();
                    foreach (var crianca in criancasVinculadas)
                    {
                        crianca.IdResponsavel = null; // Remove o vínculo
                    }
                }
                else if (usuario.Perfil.ToLower() == "professor")
                {
                     // Remove vínculos professor-aluno (se não configurado cascade)
                     var vinculos = await _context.ProfessorAlunos.Where(pa => pa.ProfessorId == usuarioId).ToListAsync();
                     if (vinculos.Any()) _context.ProfessorAlunos.RemoveRange(vinculos);

                     // (Adicione here a remoção de atividades criadas pelo professor, se necessário)
                }

                // --- Deleta o Usuário ---
                _context.Usuarios.Remove(usuario);

                // --- Salva todas as alterações ---
                await _context.SaveChangesAsync();

                // --- Desloga o usuário ---
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                TempData["AccountDeleted"] = "Sua conta foi excluída com sucesso.";
                return RedirectToAction("Index", "Home"); // Redireciona para a página inicial
            }
            catch (DbUpdateException )
            {
                // Idealmente, logar o erro 'ex'
                TempData["ProfileError"] = "Erro ao excluir a conta. Verifique dependências.";
                // Redireciona de volta ao dashboard se a exclusão falhar
                return RedirectToUserDashboard();
            }
        }

        // Método auxiliar para redirecionar para o dashboard correto
        private IActionResult RedirectToUserDashboard()
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role)?.ToLower() ?? "home";
            string dashboardController = userRole switch {
                "pai" => "Pai",
                "professor" => "Professor",
                "crianca" => "Crianca",
                _ => "Home" // Caso padrão (inclui 'home' e qualquer outro inesperado)
            };
            return RedirectToAction("Index", dashboardController);
        }
    }
}