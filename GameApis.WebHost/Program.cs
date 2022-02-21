using GameApis.MongoDb;
using GameApis.Shared;
using GameApis.TicTacToe.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGameApisMongoDbPersistence(builder.Configuration.GetConnectionString("MongoDb"));
builder.Services.AddGameApisServices();

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