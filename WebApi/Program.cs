using Infrastructure.Data;
using Infrastructure.Interface.Branche;
using Infrastructure.Interface.Car;
using Infrastructure.Service.Branche;
using Infrastructure.Service.Car;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBrancheCrudService, BrancheCrudService>();
builder.Services.AddScoped<IBrancheGetService, BrancheGetService>();
builder.Services.AddScoped<ICarCrudService, CarCrudService>();
builder.Services.AddScoped<ICarGetService, CarGetService>();
builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();