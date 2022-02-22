using GameApis.MongoDb;
using GameApis.Shared;
using GameApis.TicTacToe.Endpoints;
using GameApis.TicTacToe.GameState;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGameApisMongoDbPersistence(builder.Configuration.GetConnectionString("MongoDb"));
builder.Services.AddGameApisServices(builder => builder.RegisterGameContext<TicTacToeContext>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseTicTacToeEndpoints();

app.Run();