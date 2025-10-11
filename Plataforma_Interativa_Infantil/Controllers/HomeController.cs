using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Se o usuário já estiver logado...
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                // ...redireciona para o dashboard correto.
                if (!string.IsNullOrEmpty(userRole))
                {
                    return RedirectToDashboard(userRole);
                }
            }
            // Se não estiver logado, mostra a tela de login (a própria Home).
            return View();
        }

        private IActionResult RedirectToDashboard(string perfil)
        {
            return perfil.ToLower() switch
            {
                "pai" => RedirectToAction("Index", "Pai"),
                "professor" => RedirectToAction("Index", "Professor"),
                "crianca" => RedirectToAction("Index", "Crianca"),
                _ => RedirectToAction("Index", "Home"),
            };
        }
    }
}