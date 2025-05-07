using Microsoft.EntityFrameworkCore;
using SaborBrasilContext;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL("server=localhost;database=saborDeMinas;user=root;password=1234"));

app.MapGet("/", () => "Hello World!");

app.Run();
