using System.Collections.Generic;
using AutoFixture;
using SimpleToDo.Model.Entities;

public static class ToDoListFactory
{
    private static AutoFixture.Fixture fixture = new AutoFixture.Fixture();

    public static IEnumerable<ToDoList> Create(int count = 1)
        => fixture
            .Build<ToDoList>()
            .With(x => x.Id, 0)
            .With(x => x.Tasks, new List<Task>())
            .CreateMany<ToDoList>(count);
}