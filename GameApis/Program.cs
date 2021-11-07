using GameApis.Domain.Exceptions;
using GameApis.Models;
using GameApis.OperationFilters;
using GameApis.SecretHitler.Api;
using GameApis.Shared.CancellationTokens;
using GameApis.Shared.MongoAggregateStorage;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddAggregateMongoStorage("mongodb://localhost:27017");
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpContextCancellationTokenPassing();
builder.Services.AddSwaggerGen(opts => opts.OperationFilter<AddDomainResponseOperationFilter>());
builder.Services.AddSecretHitlerApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (DomainException ex)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new DomainExceptionResponse(ex));
    }
});

app.MapSecretHitlerEndpoints();

await app.RunAsync();