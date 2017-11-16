using Microsoft.EntityFrameworkCore;

namespace SimpleToDo.Model.Entities
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions options) : base(options) { }

        public DbSet<List> List { get; set; }
        public DbSet<Task> Task { get; set; }
    }
}