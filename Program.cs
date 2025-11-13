using Microsoft.EntityFrameworkCore;
using sistemaVeterinario.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SistemaVeterinarioContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllersWithViews();

/**
 * Configurar las Variable Sessions.
 * - Se debe hacer ANTES del 'builder.Build()'
 */
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15); // tiempo de expiracion de la session.
    options.Cookie.HttpOnly = true; // 'cookie' solo accesible por el servidor (no puede acceder por JS).
    options.Cookie.Name = "SistemaVeterinario.Session"; // nombre de la 'cookie' de session.
    options.Cookie.IsEssential = true; // permite utilizar 'cookies' aunque el usuario no las acepte...
});

var app = builder.Build();

// Habilitar el uso de sessions en la aplicacion.
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    // Hacemos que la pagina de inico sea Login.
    pattern: "{controller=Home}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
