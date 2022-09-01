using Microsoft.EntityFrameworkCore;
using UsersServer.Controllers.Dependencies;
using UsersServer.Domain.Auth.Dependencies;
using UsersServer.Domain.Auth.Repositories;
using UsersServer.Domain.Users.Repositories;
using UsersServer.Infrastructure.AppDatabase;
using UsersServer.Operations.PasswordCrypto;
using UsersServer.Operations.UsersRegistration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("App:InMemory"));

// domain
builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<AuthProfilesRepository>();
builder.Services.AddScoped<SessionTokensRepository>();

// operations
builder.Services.AddScoped<IUsersRegistrationService, UserRegistrationService>();
builder.Services.AddScoped<IPasswordCryptoService, PasswordCryptoService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment() == false)
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

PrepAppDB.PrepPopulation(app);

app.Run();
