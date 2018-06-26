using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SimpleToDo.Model.Entities;

public class Fixture
{
    static Fixture()
    {
        Configuration = GetConfiguration();
        CreateDatabase();
    }

    private static IConfiguration GetConfiguration()
        => new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    private static void CreateDatabase()
    {
        var options = new DbContextOptionsBuilder<ToDoDbContext>()
            .UseSqlServer(Configuration["SimpleToDo"])
            //.UseInMemoryDatabase(databaseName: "SimpleToDoList")
            .Options;

        new ToDoDbContext(options).Database.Migrate();
    }

    protected static IConfiguration Configuration { get; }
}