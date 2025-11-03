using TimeSpan = System.TimeSpan;
using System.Text;
using Infrastructure.Data;
using Infrastructure.Interface.Auth;
using Infrastructure.Interface.Branche;
using Infrastructure.Interface.Car;
using Infrastructure.Interface.Customer;
using Infrastructure.Interface.Rental;
using Infrastructure.Service.Auth;
using Infrastructure.Service.Branche;
using Infrastructure.Service.Car;
using Infrastructure.Service.Customer;
using Infrastructure.Service.Rental;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sample web API",
        Version = "v1",
        Description = "Sample API Services.",
        Contact = new OpenApiContact
        {
            Name = "Maga",
            Email = "Muhammad@gmail.com"
        },
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste token in this format: Bearer {token}.",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt =>
    {
        opt.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["access_token"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
        var key =Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
        opt.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IBrancheCrudService, BrancheCrudService>();
builder.Services.AddScoped<IBrancheGetService, BrancheGetService>();
builder.Services.AddScoped<ICarCrudService, CarCrudService>();
builder.Services.AddScoped<ICarGetService, CarGetService>();
builder.Services.AddScoped<ICustomerGetService, CustomerGetService>();
builder.Services.AddScoped<ICustomerCrudService, CustomerCrudService>();
builder.Services.AddScoped<IRentalCrudService, RentalCrudService>();
builder.Services.AddScoped<IRentalGetService, RentalGetService>();
builder.Services.AddScoped<IAuthService, AuthService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();