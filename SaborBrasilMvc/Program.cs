using Microsoft.EntityFrameworkCore;
using SaborBrasildbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o DbContext
builder.Services.AddDbContext<SaborBrasilContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("SaborBrasilConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("SaborBrasilConnection"))));

// Adiciona MVC com Views (necessário para TempData)
builder.Services.AddControllersWithViews();

// Adiciona suporte a sessões
builder.Services.AddSession();

var app = builder.Build();

// Teste de Conexão com o Banco de Dados
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SaborBrasilContext>();

    try
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            Console.WriteLine("Conexão com o banco de dados bem-sucedida!");
        }
        else
        {
            Console.WriteLine("Falha na conexão com o banco de dados.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao tentar conectar: {ex.Message}");
    }
}

// Configuração do pipeline de requisições
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
