using FitHub.Platform.Common;
using FitHub.Platform.Workout.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

DependencyInjection.ConfigureDependencies(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Problem Details
app.UseStatusCodePages();
app.UseExceptionHandler(GlobalExceptionHandler.Configure);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
