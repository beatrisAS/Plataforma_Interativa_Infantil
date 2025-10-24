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
using backend.Utils; 
using System; 
using System.Linq; 

namespace backend.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

    
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                 var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                 return RedirectToDashboard(userRole ?? "pai");
            }
            return View();
        }

     
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Senha))
            {
                TempData["Error"] = "Email e Senha são obrigatórios.";
                return RedirectToAction("Login"); 
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (usuario == null || !PasswordHasher.Verify(model.Senha, usuario.Senha))
            {
                TempData["Error"] = "Email ou senha inválidos.";
                return RedirectToAction("Login"); 
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
                var firstError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
                TempData["ErrorRegister"] = firstError?.ErrorMessage ?? "Por favor, preencha todos os campos corretamente.";
                return RedirectToAction("Login"); 
            }

            var existingUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                TempData["ErrorRegister"] = "Este email já está cadastrado.";
                return RedirectToAction("Login");
            }
            
  
            if (model.Perfil.ToLower() == "crianca")
            {
              
                if (model.DataNascimento == null || string.IsNullOrEmpty(model.Genero))
                {
                    TempData["ErrorRegister"] = "Para perfil 'Criança', Data de Nascimento e Gênero são obrigatórios.";
                    return RedirectToAction("Login");
                }

                
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        
                        var novoUsuarioCrianca = new Usuario
                        {
                            Nome = model.Nome,
                            Email = model.Email,
                            Senha = PasswordHasher.Hash(model.Senha),
                            Perfil = "crianca"
                        };
                        _context.Usuarios.Add(novoUsuarioCrianca);
                        await _context.SaveChangesAsync(); 

                  
                        var novaCrianca = new Crianca
                        {
                            Nome = model.Nome,
                            DataNascimento = model.DataNascimento.Value,
                            Genero = model.Genero,
                            Estrelas = 0,
                            IdResponsavel = null, 
                            UsuarioId = novoUsuarioCrianca.Id,
                            CodigoDeVinculo = GerarCodigoUnico() 
                        };
                        _context.Criancas.Add(novaCrianca);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                   
                        await FazerLoginAposRegistro(novoUsuarioCrianca);
                        return RedirectToDashboard(novoUsuarioCrianca.Perfil);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        TempData["ErrorRegister"] = "Erro ao criar perfil criança: " + ex.Message;
                        return RedirectToAction("Login");
                    }
                }
            }
            else
            {
            
                var novoUsuario = new Usuario
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    Senha = PasswordHasher.Hash(model.Senha),
                    Perfil = model.Perfil
                };

                _context.Usuarios.Add(novoUsuario);
                await _context.SaveChangesAsync();
                
                TempData["SuccessRegister"] = "Registro realizado com sucesso! Faça o login.";
                return RedirectToAction("Login");
            }
        }
        
 
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        private string GerarCodigoUnico()
        {
   
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async Task FazerLoginAposRegistro(Usuario usuario)
        {
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
        }
    }
}