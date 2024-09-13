using DAL.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public partial class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }

        DbSet<TaskEntity> Tasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TaskEntity>()
                .HasKey(t => t.Id);
        }
    }
}
