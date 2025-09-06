using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TaskManager.Api.Hubs;
using TaskManager.Api.MiddLewares;
using TaskManager.Api.Notifications;
using TaskManager.Application.Interfaces.Factories;
using TaskManager.Application.Interfaces.Respositories.Security;
using TaskManager.Application.Interfaces.Respositories.Task;
using TaskManager.Application.Interfaces.Services;
using TaskManager.Application.Interfaces.Services.Security;
using TaskManager.Application.Services.Security;
using TaskManager.Application.Services.Task;
using TaskManager.Persistence.Context;
using TaskManager.Persistence.Repositories;
using TaskManager.Persistence.Repositories.Security;
using TaskManager.Security.Interfaces.Security;
using TaskManager.Security.Services.Security;

namespace TaskManager.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<TaskManagerContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TaskManagerConn")));

            builder.Services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddScoped<ITareaRepository, TareaRepository>();
            builder.Services.AddTransient<ITareaService, TareaService>();
            builder.Services.AddScoped<ITareaFactory, TareaFactory>();
            builder.Services.AddSignalR();
            builder.Services.AddTransient<ITareaNotifier, SignalRTareaNotifier>();



            // Configurations JWT
            var jwtKey = builder.Configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT Key not configured.");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var jwtIssuer = builder.Configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer not configured.");
            var jwtAudience = builder.Configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("JWT Audience not configured.");


            if (jwtKey.Length < 32)
            {
                throw new InvalidOperationException("JWT Key must be at least 32 characters long.");
            }

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
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = securityKey,
                    ClockSkew = TimeSpan.FromMinutes(5),

                };

            });

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TaskManager API",
                    Version = "v1"
                });


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
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

            var app = builder.Build();

            // Database migration
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<TaskManagerContext>();
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database migration failed: {ex.Message}");
                    throw;
                }
            }


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionMiddLeware>();
            app.MapControllers();

            app.MapHub<NotificationHub>("/receiveNotification");

            app.Run();
        }
    }
}