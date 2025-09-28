using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    public class HomeController : Controller
    {
        // A página inicial (/) agora apenas mostra a tela de login/registro.
        public IActionResult Index()
        {
            return View();
        }
    }
}

