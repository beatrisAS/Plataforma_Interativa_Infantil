using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

public class HomeController : Controller {
    public IActionResult Index() => View();
    public IActionResult CriancaDashboard() => View();
    public IActionResult PaiDashboard() => View();
    public IActionResult ProfessorDashboard() => View();
}
