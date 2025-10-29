using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BairroAlerta.Api.Services; 
using BairroAlerta.Api.Hubs;
using BairroAlerta.Api.Data;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IAlertasService, AlertasService>();

// --- logging (útil em dev) ---
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// --- DbContext: InMemory (rápido para dev). 
// Se preferir SQL Server, troque UseInMemoryDatabase por UseSqlServer e adicione connection string.
builder.Services.AddDbContext<AlertaContext>(options =>
    options.UseInMemoryDatabase("BairroAlertaDev"));

// serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// lifecycle logging
app.Lifetime.ApplicationStarted.Register(() => Console.WriteLine("=== ApplicationStarted ==="));
app.Lifetime.ApplicationStopping.Register(() => Console.WriteLine("=== ApplicationStopping ==="));
app.Lifetime.ApplicationStopped.Register(() => Console.WriteLine("=== ApplicationStopped ==="));

// Swagger em Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BairroAlerta API V1");
        c.RoutePrefix = string.Empty; // abre swagger na raiz: https://localhost:7158/
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

// servir arquivos estáticos (wwwroot)
app.UseStaticFiles();

app.MapControllers();

// mapeia o hub (use AlertaHub, que você já tem)
app.MapHub<AlertaHub>("/alertaHub");

try
{
    Console.WriteLine("Starting app.Run() ...");
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("EXCEPTION during app.Run():");
    Console.WriteLine(ex);
    throw;
}
