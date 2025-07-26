
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Persistence.Context
{
    public class TaskManagerContext : DbContext

    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
           
        }
  
        public DbSet<Tarea> Tareas { get; set; }

    }

}



