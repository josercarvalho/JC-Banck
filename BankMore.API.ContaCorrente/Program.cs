
using BankMore.Core.Commands.Validators;
using BankMore.Core.Interfaces;
using BankMore.Infra.Database;
using BankMore.Infra.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

//builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.Load("BankMore.Core")));
builder.Services.AddValidatorsFromAssemblyContaining<CreateContaCorrenteCommandValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(BankMore.Core.Handlers.CreateContaCorrenteCommandHandler).GetTypeInfo().Assembly);

var connectionString = Environment.GetEnvironmentVariable("ConnectionString") ?? "Data Source=bankmore.db";
builder.Services.AddSingleton<DatabaseBootstrap>(new DatabaseBootstrap(connectionString));
builder.Services.AddSingleton<IContaCorrenteRepository>(new ContaCorrenteRepository(connectionString));
builder.Services.AddSingleton<IMovimentoRepository>(new MovimentoRepository(connectionString));
builder.Services.AddSingleton<IIdempotenciaRepository>(new IdempotenciaRepository(connectionString));

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
