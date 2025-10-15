using ChatApp.Infrastructure.Data;
using ChatApp.API.Hubs;
using Microsoft.EntityFrameworkCore;
using ChatApp.API.Extensions;
using ChatApp.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Serviços do projeto (Application, Infra, EF, Redis, SignalR, Swagger, CORS, Controllers)
builder.Services
    .AddAppServices()
    .AddEntityFramework(builder.Configuration)
    .AddRedis(builder.Configuration)
    .AddSignalRWithRedis(builder.Configuration)
    .AddSwaggerDocs()
    .AddAngularCors()
    .AddControllers();

var app = builder.Build();

// Migrações e Seed do banco
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
    await context.Database.MigrateAsync();
    context.EnsureSeeded();
}

// Swagger UI (apenas em desenvolvimento)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", context => {
        context.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });
}

app.UseCors("AllowAngular");
app.UseRouting();

app.MapControllers();
app.MapHub<ChatHub>("/chathub");

await app.RunAsync();
