using Microsoft.EntityFrameworkCore;
using TiptapWebApi.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<DocumentDbContext>(options =>
{
    var dbSettings = builder.Configuration.GetSection("Db").Get<DbSettings>()!;
    var connectionString = $"Host={dbSettings.Host};Port={dbSettings.Port};Username={dbSettings.Username};Password={dbSettings.Password};Database={dbSettings.Database}";
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
