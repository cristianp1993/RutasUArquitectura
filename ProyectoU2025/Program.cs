using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using ProyectoU2025.Db;
using ProyectoU2025.Repositories;
using ProyectoU2025.Repositories.Interfaces;
using ProyectoU2025.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDapperContext, DapperContext>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISessionManager, SessionManager>();
builder.Services.AddScoped<ISalonRepository, SalonRepository>();
//builder.Services.AddHttpClient<DeepSeekService>();
builder.Services.AddScoped<SalonService>();


// Configuración de sesión 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(200);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
}); 

// Configuración de autenticación
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        options.CallbackPath = "/Profile";

        options.Scope.Add("profile");
        options.Scope.Add("email");

        options.ClaimActions.MapJsonKey("picture", "picture");
        options.ClaimActions.MapJsonKey("locale", "locale");
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseSession(); 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();