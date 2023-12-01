using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using MonPetitSurf;
using System.Text;
using MonPetitSurf.Models;
using MonPetitSurf.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var key = Encoding.UTF8.GetBytes(Secrets.JWT_SECRET);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddDbContext<MonPetitSurfContext>(options => options.UseMySql(
    builder.Configuration.GetConnectionString("Default"),
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql")
));
builder.Services.AddScoped<MonPetitSurfService>();
builder.Services.AddScoped<RegionsService>();
builder.Services.AddScoped<UsersService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Vérifie la validité du jwt
builder.Services.AddAuthentication(options =>
    { 
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    { 
        options.TokenValidationParameters = new TokenValidationParameters
        { 
            ValidateIssuer = true, 
            ValidIssuer = Secrets.Issuer,
            ValidateAudience = true, 
            ValidAudience = Secrets.Audience, 
            ValidateIssuerSigningKey = true, 
            IssuerSigningKey = new SymmetricSecurityKey(key), 
            ValidateLifetime = true, 
        }; 
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
