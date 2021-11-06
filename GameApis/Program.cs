using GameApis.Domain.Exceptions;
using GameApis.SecretHitler.Api;
using GameApis.Shared.CancellationTokens;
using GameApis.Shared.MongoAggregateStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAggregateMongoStorage("mongodb://localhost:27017");
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpContextCancellationTokenPassing();
builder.Services.AddSwaggerGen();
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
        await context.Response.WriteAsJsonAsync(new { Error = ex.Message, Code = ex.Code.ToString() });
    }
});

app.MapSecretHitlerEndpoints();

await app.RunAsync();