using System.Collections.Generic;
using AutoFixture;
using SimpleToDo.Model.Entities;

namespace SimpleToDo.Web.IntegrationTest.Factory
{
    public static class ToDoListFactory
    {
        private static readonly AutoFixture.Fixture Fixture = new AutoFixture.Fixture();

        public static IEnumerable<ToDoList> Create(int count = 1)
            => Fixture
                .Build<ToDoList>()
                .With(x => x.Id, 0)
                .With(x => x.Tasks, new List<Task>())
                .CreateMany<ToDoList>(count);
    }
}