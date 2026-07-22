using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Extensions;
using TaskFlow.Api.Middleware;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Services;
using TaskFlow.Infrastructure.Persistence;
using TaskFlow.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddDbContext<TaskFlowDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
