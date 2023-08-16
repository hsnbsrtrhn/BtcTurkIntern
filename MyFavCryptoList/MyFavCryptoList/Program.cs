using Microsoft.EntityFrameworkCore;
using CryptoListApi.BackgroundHostedServices;
using CryptoListApi.Entitiys;
using CryptoListApi.Services;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<CryptoDbContext>(opt =>
    opt.UseInMemoryDatabase("CryptoList"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<TimedHostedService>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<TickerServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseAuthorization();

app.MapControllers();

app.Run();
