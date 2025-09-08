using Microsoft.AspNetCore.Mvc;

namespace Plataforma.API.Controllers
{
    public class ActivityPageController : Controller
    {
        public IActionResult Index()
        {
            return View("Activities"); // Renderiza sua view
        }
    }
}
