using Microsoft.EntityFrameworkCore;
using TiptapWebApi.Database;
using TiptapWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDocumentService, DocumentService>();

builder.Services.AddControllers();

builder.Services.AddDbContext<DocumentDbContext>(options =>
{
    var dbSettings = builder.Configuration.GetSection("Db").Get<DbSettings>()!;
    var connectionString = $"Host={dbSettings.Host};Port={dbSettings.Port};Username={dbSettings.Username};Password={dbSettings.Password};Database={dbSettings.Database}";
    options.UseNpgsql(connectionString);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
