using DataAccess.Interface;
using DataAccess.Repository;
using NLog.Web;
using Operation.Interface;
using Operation.Operation;
using UrlShortener.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUrlShortenerRepository, UrlShortenerRepository>();
builder.Services.AddScoped<IUrlShortenerOperation, UrlShortenerOperation>();
builder.Services.AddScoped<IUrlGenerationOperation, UrlGenerationOperation>();


builder.Host.ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.AddGlobalExceptionHandling();

app.Run();
