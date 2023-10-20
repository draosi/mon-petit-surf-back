using Microsoft.EntityFrameworkCore;
using MonPetiSurf.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddDbContext<MonPetitSurfContext>(options => options.UseMySql(
    builder.Configuration.GetConnectionString("Default"),
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql")
));
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
