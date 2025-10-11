using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using backend.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;

namespace backend.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Senha))
            {
                TempData["Error"] = "Email e Senha são obrigatórios.";
                return RedirectToAction("Index", "Home");
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Senha == model.Senha);

            if (usuario == null)
            {
                TempData["Error"] = "Email ou senha inválidos.";
                return RedirectToAction("Index", "Home");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new(ClaimTypes.Name, usuario.Nome),
                new(ClaimTypes.Role, usuario.Perfil)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToDashboard(usuario.Perfil);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorRegister"] = "Por favor, preencha todos os campos corretamente.";
                return RedirectToAction("Index", "Home");
            }

            var existingUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                TempData["ErrorRegister"] = "Este email já está cadastrado.";
                return RedirectToAction("Index", "Home");
            }

            var novoUsuario = new Usuario
            {
                Nome = model.Nome,
                Email = model.Email,
                Senha = model.Senha,
                Perfil = model.Perfil
            };

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();
            
            TempData["SuccessRegister"] = "Registro realizado com sucesso! Faça o login.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
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

