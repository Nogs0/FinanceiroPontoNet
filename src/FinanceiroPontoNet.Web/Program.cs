using System.Reflection;
using FinanceiroPontoNet.Application;
using FinanceiroPontoNet.Infrastructure;
using FinanceiroPontoNet.Infrastructure.Persistence;
using FinanceiroPontoNet.Web.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices().AddInfrastructureServices(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Version = "v1",
            Title = "API FinanceiroPontoNet - Questor",
            Description = "API criada para o processo seletivo para a Questor",
        }
    );

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
var isRunningInContainer = Environment.GetEnvironmentVariable("IS_RUNNING_IN_CONTAINER");
Console.WriteLine("RuningInContainer: ", isRunningInContainer);
if (bool.TryParse(isRunningInContainer, out var isContainer) && isContainer)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var dbContext = services.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();

            Console.WriteLine("Migrações do EF Core aplicadas com sucesso.");
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Ocorreu um erro ao aplicar as migrações do EF Core.");
        }
    }
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger/index.html", permanent: true))
    .ExcludeFromDescription();

app.Run();
