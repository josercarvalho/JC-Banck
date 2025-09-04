
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

var connectionString = Environment.GetEnvironmentVariable("ConnectionString") ?? "Host=localhost;Port=5432;Database=bankmore;Username=user;Password=password";
builder.Services.AddSingleton<DatabaseBootstrap>(new DatabaseBootstrap(connectionString));
builder.Services.AddSingleton<ITransferenciaRepository>(new TransferenciaRepository(connectionString));
builder.Services.AddSingleton<IContaCorrenteRepository>(new ContaCorrenteRepository(connectionString));
builder.Services.AddSingleton<IMovimentoRepository>(new MovimentoRepository(connectionString));
builder.Services.AddSingleton<IIdempotenciaRepository>(new IdempotenciaRepository(connectionString));
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? "123as4d56asd45ads465a4s5d6";
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
