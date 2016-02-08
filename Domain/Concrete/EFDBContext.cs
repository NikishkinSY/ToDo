using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // настройка полей с помощью Fluent API
            modelBuilder.Entity<User>()
                .HasMany<Task>(s => s.CreateTasks)
                .WithRequired(s => s.CreateUser)
                .WillCascadeOnDelete(true);

            //modelBuilder.Entity<User>()
            //    .HasMany<Task>(s => s.ResponsableTasks)
            //    .WithRequired(s => s.ResponsableUser)
            //    .WillCascadeOnDelete(false);
        }
    }
}
