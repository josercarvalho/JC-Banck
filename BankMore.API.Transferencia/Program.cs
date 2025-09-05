
using BankMore.Core.Commands.Validators;
using BankMore.Core.Interfaces;
using BankMore.Infra.Database;
using BankMore.Infra.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<CreateTransferenciaCommandValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", info: new OpenApiInfo
    {
        Title = "BanckMore.API",
        Version = "v1.API.Transferencia",
        Description = "API for managing bank transfers",
        Contact = new OpenApiContact
        {
            Name = "José Ribeiro Carvalho",
            Email = "josercarvalho@gmail.com",
            Url = new Uri("https://github.com/josercarvalho")
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddMediatR(typeof(BankMore.Core.Handlers.CreateTransferenciaCommandHandler).GetTypeInfo().Assembly);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' não está configurada.");
}

builder.Services.AddSingleton(new DbConnectionFactory(connectionString));
builder.Services.AddSingleton<DatabaseBootstrap>();
builder.Services.AddSingleton<ITransferenciaRepository, TransferenciaRepository>();
builder.Services.AddSingleton<IContaCorrenteRepository, ContaCorrenteRepository>();
builder.Services.AddSingleton<IMovimentoRepository, MovimentoRepository>();
builder.Services.AddSingleton<IIdempotenciaRepository, IdempotenciaRepository>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

var jwtKey = builder.Configuration["JwtSettings:Secret"] ?? "WW91clN1cGVyU2VjcmV0S2V5Rm9ySnd0VG9rZW5HZW5lcmF0aW9uVGhhdFNob3VsZEJlQXRMZWFzdDMyQnl0ZXNMb25n";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Services.GetService<DatabaseBootstrap>().Setup();

app.Run();
