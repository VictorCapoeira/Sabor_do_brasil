using Microsoft.EntityFrameworkCore;
using SaborBrasilContext;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL("server=localhost;database=saborDeMinas;user=root;password=1234"));
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseStaticFiles();
app.MapGet("/", () => Results.Redirect("/index.cshtml"));

app.Run();
