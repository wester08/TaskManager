using Microsoft.EntityFrameworkCore;
using TaskManager.Api.MiddLewares;
using TaskManager.Application.Interfaces.Factories;
using TaskManager.Application.Interfaces.Respositories.Task;
using TaskManager.Application.Interfaces.Services;
using TaskManager.Application.Services.Task;
using TaskManager.Persistence.Context;
using TaskManager.Persistence.Repositories;

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

            builder.Services.AddScoped<ITareaRepository, TareaRepository>();
            builder.Services.AddTransient<ITareaService, TareaService>();
            builder.Services.AddScoped<ITareaFactory, TareaFactory>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TaskManagerContext>();
                dbContext.Database.Migrate();
            }

            // Configure the HTTP request pipeline.


            

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddLeware>();

            app.MapControllers();

            app.Run();
        }
    }
}


