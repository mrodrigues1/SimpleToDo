using Microsoft.EntityFrameworkCore;
using SimpleToDo.Model.Entities;
using System;
using Xunit;

namespace SimpleToDo.Repository.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var dbOptions = new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(databaseName: "ToDoDb")
                .Options;

            using (var context = new ToDoDbContext(dbOptions))
            {
                

            }


        }
    }
}
