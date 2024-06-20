using Microsoft.EntityFrameworkCore;
using revenue_api.Context;
using revenue_api.Middlewares;
using revenue_api.Models;
using revenue_api.Repositories;
using revenue_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IRevenueService, RevenueService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<RevenueDbContext>(opt =>
{
    string? connString = builder.Configuration.GetConnectionString("DefaultConnection");
    opt.UseSqlServer(connString);
});


var app = builder.Build();


//Seed data

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.InitializeClients(services);
    
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();



app.Run();

