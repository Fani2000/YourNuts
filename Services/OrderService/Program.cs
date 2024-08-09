using Microsoft.EntityFrameworkCore;
using YourNuts.Domain;
using YourNuts.KafkaFlowIntegration.Options;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<YourNutsDbContext>(options =>
{
    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddOptions<KafkaOptions>().Bind(Configuration.GetSection(KafkaOptions.SectionName));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
