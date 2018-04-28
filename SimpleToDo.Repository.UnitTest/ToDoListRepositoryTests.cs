using Microsoft.EntityFrameworkCore;
using SimpleToDo.Model.Entities;
using SimpleToDo.Repository.Implementations;
using Xunit;

namespace SimpleToDo.Repository.UnitTest
{
    public class ToDoListRepositoryTests
    {
        [Fact]
        public void CreateToDoList_WithValidObject_NewListIdIsNotEqualsToZero()
        {
            //Arrange                        
            var newToDoList = new List
            {
                Name = "Unit Test"
            };

            var sut = CreateSUT();

            //Act
            sut.CreateToDoList(newToDoList);

            //Assert
            Assert.True(newToDoList.ListId != 0);
        }

        private ToDoListRepository CreateSUT()
        {
            var dbOptions = new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(databaseName: "ToDoDb")
                .Options;

            var context = new ToDoDbContext(dbOptions);

            return new ToDoListRepository(context);
        }
    }
}
