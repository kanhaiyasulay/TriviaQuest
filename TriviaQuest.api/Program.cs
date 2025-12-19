using Microsoft.EntityFrameworkCore;
using TriviaQuest.api.Dtos;
using TriviaQuest.api.Data;
using TriviaQuest.api.Models;
using TriviaQuest.api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext (SQLite file in app root)
builder.Services.AddDbContext<TriviaDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default") 
                  ?? "Data Source=trivia.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// -------------------- Minimal API endpoints --------------------
app.MapTriviaEndpoints();


app.Run();
