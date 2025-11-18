using Application.Mappers;
using Application.Services;
using Application.Services.Interfaces;
using Application.Validators;
using Domain.Abstractions;
using FluentValidation;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString,
    b => b.MigrationsAssembly("Infrastructure"))
);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});
// Add services to the container.

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IHealthInsuranceRepository, HealthInsuranceRepository>();

builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IHealthInsuranceService, HealthInsuranceService>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddValidatorsFromAssembly(typeof(CreatePatientDtoValidator).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "There has been a problem while applying the migrations");
    }
}

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
