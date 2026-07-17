using Application.Interfaces.Services;
using Application.Services;
using Core.Interfaces.Repository;
using Data.Context;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GestorTarefasAPI",
        Version = "v1",
        Description = "API RESTful para gestão de tarefas — .NET, DDD, EF Core InMemory"
    });
});

builder.Services.AddDbContext<TarefaDbContext>(options =>
    options.UseInMemoryDatabase("GestorTarefasDb"));

builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddScoped<ITarefaService, TarefaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseStaticFiles();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "GestorTarefasAPI v1");
        options.InjectStylesheet("/swagger-dark.css");
    });

    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();