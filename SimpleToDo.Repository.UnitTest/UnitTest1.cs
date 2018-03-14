using Microsoft.EntityFrameworkCore;
using SimpleToDo.Model.Entities;
using SimpleToDo.Repository.Implementations;
using System;
using Xunit;

namespace SimpleToDo.Repository.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange            
            var toDoListRepository = CreateToDoListRepository();
            var newToDoList = new List
            {
                Name = "Unit Test"
            };
            
            //Act
            toDoListRepository.CreateToDoList(newToDoList);

            //Assert
            Assert.True(newToDoList.ListId != 0);
        }

        private ToDoListRepository CreateToDoListRepository()
        {
            var dbOptions = new DbContextOptionsBuilder<ToDoDbContext>()
               .UseInMemoryDatabase(databaseName: "ToDoDb")
               .Options;

           var context = new ToDoDbContext(dbOptions);

            return new ToDoListRepository(context);
        }

        private void DisposeContext(ToDoDbContext context)
        {
            context.Dispose();
        }
    }
}
