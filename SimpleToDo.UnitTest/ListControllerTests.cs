using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using SimpleToDo.Model.Entities;
using SimpleToDo.Service.Contracts;
using SimpleToDo.Web.Controllers;
using Xunit;

namespace SimpleToDo.Web.UnitTest
{
    public class ListControllerTests
    {
        [Fact]
        public void Index_ReturnViewResult()
        {
            //Arrange
            var listServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => listServiceFake.GetToDoLists())
                .Returns(new List<List>());

            var sut = CreateSut(listServiceFake);

            //Act
            var result = sut.Index().Result;

            //Assert
            result
                .Should()
                .BeViewResult();
        }

        private ListController CreateSut(IToDoListService listServiceFake)
        {
            return new ListController(listServiceFake);
        }

        [Fact]
        public void Index_ReturnTypeIsListOfToDoList()
        {
            //Arrange
            var listServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => listServiceFake.GetToDoLists())
                .Returns(new List<List>());

            var sut = CreateSut(listServiceFake);

            //Act
            var result = sut.Index().Result;

            //Assert
            result
                .Should()
                .BeViewResult()
                .Model
                .Should()
                .BeOfType<List<List>>();
        }

        [Fact]
        public void Index_ReturnDefaultViewName()
        {
            //Arrange
            var listServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => listServiceFake.GetToDoLists())
                .Returns(new List<List>());

            var sut = CreateSut(listServiceFake);

            //Act
            var result = sut.Index().Result;

            //Assert
            result
                .Should()
                .BeViewResult()
                .WithDefaultViewName();
        }

        //[Fact]
        //public void Index_ReturnViewWithNameIndex()
        //{
        //    //Arrange
        //    var listServiceFake = A.Fake<IToDoListService>();
        //    A.CallTo(() => listServiceFake.GetToDoLists())
        //        .Returns(new List<List>());

        //    var sut = CreateSut(listServiceFake);

        //    //Act
        //    var result = sut.Index().Result;

        //    //Assert
        //    result
        //        .Should()
        //        .BeViewResult()
        //        .WithViewName("Index");
        //}

        [Fact]
        public void Index_ReturnOneToDoList()
        {
            //Arrange
            var listServiceFake = A.Fake<IToDoListService>();
            var toDoLists = new List<List>
            {
                CreateToDoListDefault()
            };

            A.CallTo(() => listServiceFake.GetToDoLists())
                .Returns(toDoLists);

            var sut = CreateSut(listServiceFake);

            //Act
            var result = sut.Index().Result;

            //Assert
            result
                .Should()
                .BeViewResult()
                .ModelAs<List<List>>()
                .Should()
                .HaveCount(1);
        }

        private List CreateToDoListDefault()
        {
            return new List
            {
                ListId = 1,
                Name = "ToDo"
            };
        }

        [Fact]
        public void Edit_ReceiveNullId_ReturnNotFoundStatusCode()
        {
            //Arrange
            var notFoundStatusCode = 404;
            var listServiceFake = A.Fake<IToDoListService>();
            var sut = CreateSut(listServiceFake);

            //Act
            var result = sut.Details(null).Result;

            //Assert
            result
                .As<NotFoundResult>()
                .StatusCode
                .Should()
                .Be(notFoundStatusCode);
        }

        [Fact]
        public void Edit_ToDoListNotFound_ReturnNotFoundStatusCode()
        {
            //Arrange
            var toDoListId = 1;
            var notFoundStatusCode = 404;
            var listServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => listServiceFake.GetToDoListById(A<int>.Ignored))
                .Returns(System.Threading.Tasks.Task.FromResult((List)null));

            var sut = CreateSut(listServiceFake);

            //Act
            var result = sut.Edit(toDoListId).Result;

            //Assert
            result
                .As<NotFoundResult>()
                .StatusCode
                .Should()
                .Be(notFoundStatusCode);
        }

        [Fact]
        public void Edit_ToDoListExists_ReturnTypeIsToDoList()
        {
            //Arrange
            var toDoListId = 1;
            var toDoList = CreateToDoListDefault();
            var listServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => listServiceFake.GetToDoListById(A<int>.Ignored))
                .Returns(toDoList);

            var sut = CreateSut(listServiceFake);

            //Act
            var result = sut.Edit(toDoListId).Result;

            //Assert
            result
                .Should()
                .BeViewResult()
                .Model
                .Should()
                .BeOfType<List>();
        }

        [Fact]
        public void Edit_ToDoListExists_ReturnToDoList()
        {
            //Arrange
            var toDoListId = 1;
            var toDoList = CreateToDoListDefault();
            var listServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => listServiceFake.GetToDoListById(A<int>.Ignored))
                .Returns(toDoList);

            var sut = CreateSut(listServiceFake);

            //Act
            var result = sut.Edit(toDoListId).Result;

            //Assert
            result
                .Should()
                .BeViewResult()
                .Model
                .Should()
                .Be(toDoList);
        }
    }
}
