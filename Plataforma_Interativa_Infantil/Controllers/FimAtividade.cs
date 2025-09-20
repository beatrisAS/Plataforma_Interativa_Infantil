using Microsoft.AspNetCore.Mvc;

public class FimAtividadeController : Controller
{
    public IActionResult FimAtividade(int id)
    {
        ViewBag.AtividadeId = id;
        return View();
    }
}
