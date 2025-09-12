using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

public class AccountController : Controller {
    private readonly AppDbContext _db;

    public AccountController(AppDbContext db) {
        _db = db;
    }

    // LOGIN
    [HttpPost]
    public async Task<IActionResult> Login(string Email, string Senha, string Perfil) {
        var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == Email && u.Senha == Senha);
        if (user == null) {
            TempData["Error"] = "E-mail ou senha inválidos!";
            return RedirectToAction("Index", "Home");
        }

        return user.Perfil switch {
            "crianca" => RedirectToAction("CriancaDashboard", "Home"),
            "pai" => RedirectToAction("PaiDashboard", "Home"),
            "professor" => RedirectToAction("ProfessorDashboard", "Home"),
            _ => RedirectToAction("Index", "Home")
        };
    }

    // REGISTER
    [HttpPost]
    public async Task<IActionResult> Register(string Nome, string Email, string Senha, string Perfil) {
        if (await _db.Usuarios.AnyAsync(u => u.Email == Email)) {
            TempData["ErrorRegister"] = "E-mail já cadastrado!";
            return RedirectToAction("Index", "Home");
        }

        var novo = new Usuario {
            Nome = Nome,
            Email = Email,
            Senha = Senha,
            Perfil = Perfil
        };

        _db.Usuarios.Add(novo);
        await _db.SaveChangesAsync();

        TempData["SuccessRegister"] = "Cadastro realizado! Faça login.";
        return RedirectToAction("Index", "Home");
    }
}
