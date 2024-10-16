﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Persistence.Domain.StoredProcedures;

namespace Todo.Persistence.Domain
{
    // Entity Framework Heart
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(
            DbContextOptions<TodoDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoDbContext).Assembly);  // reflection

            modelBuilder.Entity<sp_FetchTodoResult>().HasNoKey().ToView("dbo.sp_FetchTodo");
        }


        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<AuditEntity>())
            {
                if(entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = System.DateTime.UtcNow;
                    entry.Entity.CreatedBy = "System";
                }
            }
            return base.SaveChanges();
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<TodoList> Todos { get; set; }
        public DbSet<TodoDetails> TodoDetails { get; set; }
        public DbSet<SampleTable> SampleTables { get; set; } 
        public DbSet<sp_FetchTodoResult> FetchTodoResult { get; set; }
    }
}