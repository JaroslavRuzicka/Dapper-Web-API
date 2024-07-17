using Dapper;
using System.Data.SqlClient;
using WebApplicationDapperTest.Context;
using WebApplicationDapperTest.Contracts;
using WebApplicationDapperTest.Repository;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// This will eventually go into seperate File
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();

