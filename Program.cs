using Microsoft.EntityFrameworkCore;
using SertaCup_site.Data;

var builder = WebApplication.CreateBuilder(args);

// L� vari�veis de ambiente (necess�rio para Render)
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/myip", async (HttpContext context) =>
{
    using var httpClient = new HttpClient();
    var ip = await httpClient.GetStringAsync("https://api.ipify.org");
    await context.Response.WriteAsync($"Render Public IP: {ip}");
});

app.Run();
