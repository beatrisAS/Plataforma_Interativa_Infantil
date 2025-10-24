using backend.Data;
using backend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.ViewComponents
{
    public class ProfileModalViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public ProfileModalViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new ProfileEditViewModel();

            // Usamos UserClaimsPrincipal em um ViewComponent
            if (UserClaimsPrincipal != null && UserClaimsPrincipal.Identity != null && UserClaimsPrincipal.Identity.IsAuthenticated)
            {
                // Pega o ID do usuário logado
                var userIdValue = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

                if (int.TryParse(userIdValue, out int usuarioId))
                {
                    // Busca o usuário no banco
                    var usuario = await _context.Usuarios
                                                .AsNoTracking() // Bom para operações de leitura
                                                .FirstOrDefaultAsync(u => u.Id == usuarioId);
                    
                    if (usuario != null)
                    {
                        // Preenche o modelo com os dados do banco
                        model.Nome = usuario.Nome;
                        model.Email = usuario.Email;
                    }
                }
            }
            
            // Retorna a view 'Default.cshtml' com o modelo (preenchido ou vazio)
            return View(model);
        }
    }
}