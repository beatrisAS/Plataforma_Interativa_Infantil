using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("Default"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default"))));

builder.Services.AddScoped<ActivityService>();
builder.Services.AddScoped<AchievementService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// --- CORREÇÃO GERAL DA AUTENTICAÇÃO ---
// Simplificado para usar apenas a autenticação por Cookies, que é o padrão para MVC.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Se o usuário não estiver logado, ele será enviado para a página inicial.
        options.LoginPath = "/";
        options.ExpireTimeSpan = TimeSpan.FromHours(8); // O login expira em 8 horas
    });
// --- FIM DA CORREÇÃO ---

builder.Services.AddAuthorization(options => {
    options.AddPolicy("ProfessorOnly", policy => policy.RequireRole("professor"));
    options.AddPolicy("PaiOrProfessor", policy => policy.RequireRole("pai", "professor"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
} else {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

// A ordem é importante: Autenticação primeiro, depois Autorização.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

