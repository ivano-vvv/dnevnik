using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using UsersServer.Domain.Auth.Dependencies;
using UsersServer.Domain.Auth.Repositories;
using UsersServer.Domain.Users.Repositories;
using UsersServer.Infrastructure.AppDatabase;
using UsersServer.Operations.PasswordCrypto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("App:InMemory"));

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standart Bearer Authentication: \"bearer {token}\"",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "Authorization"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetValue<string>("ACCESS_TOKEN_KEY")
            )),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddHttpContextAccessor();

// domain
builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<AuthProfilesRepository>();
builder.Services.AddScoped<SessionTokensRepository>();

// operations
builder.Services.AddScoped<IPasswordCryptoService, PasswordCryptoService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

PrepAppDB.PrepPopulation(app);

app.Run();
