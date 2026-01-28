using Microsoft.EntityFrameworkCore;
using Fase4_WebExterna.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// ⭐ DB
builder.Services.AddDbContext<LaserTagContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LaserTagBD")));

// ⭐ Session
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// ⭐ Authentication (COOKIES)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

// Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

// ⭐ MUY IMPORTANTE: authentication ANTES de authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();