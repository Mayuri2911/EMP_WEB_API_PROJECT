using BAL.Services.EmpRegister;
using BAL.Services.Login;
using DAL.Repository.EmpRegister;
using DAL.Repository.Login;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Set the application URL
builder.WebHost.UseUrls("https://192.168.1.102:5000");

// Register services
builder.Services.AddScoped<IEmpRegisterRepositroy, EmpRegisterRepositroy>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IAesRepository, AesRepository>();

builder.Services.AddScoped<IEmpRegisterService, EmpRegisterService>();
builder.Services.AddScoped<ILoginService, LoginService>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Add controllers
builder.Services.AddControllers();

// Configure JWT Authentication
//var key = System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    options.RequireHttpsMetadata = false;
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(key),
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ClockSkew = TimeSpan.Zero // Disable clock skew to expire JWT tokens immediately when they expire
//    };
//});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero, // Optional: allows zero skew for token expiration
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add Swagger/OpenAPI configuration
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Emp_Management_System",
        Version = "v1",
        Description = "Emp_Management_System",
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// Configure middleware and HTTP request pipeline
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply CORS policy
app.UseCors("AllowOrigin");

// Add authentication middleware before authorization
app.UseAuthentication(); // Ensure authentication is applied before authorization
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
