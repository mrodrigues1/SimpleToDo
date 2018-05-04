using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
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
                .BeOfType<ViewResult>();
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
                .As<ViewResult>()
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
                .As<ViewResult>()
                .ViewName
                .Should()
                .BeNull();
        }

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
                .As<ViewResult>()
                .Model
                .As<List<List>>()
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
                .As<ViewResult>()
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
                .As<ViewResult>()
                .Model
                .Should()
                .Be(toDoList);
        }

        [Fact]
        public void Edit_ToDoListExists_ReturnEditViewName()
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
                .As<ViewResult>()
                .ViewName
                .Should()
                .Be("Edit");
        }

        [Fact]
        public void DeleteConfirm_ValidToDoListId_RedirectToIndexView()
        {
            //Arrange
            var toDoListId = 1;
            var toDoName = "ToDo List Unit Test";
            var listServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => listServiceFake.RemoveToDoList(A<int>.Ignored))
                .Returns(toDoName);

            var sut = CreateSut(listServiceFake);
            sut.TempData = new TempDataDictionary(A.Fake<HttpContext>(), A.Fake<ITempDataProvider>());

            //Act
            var result = sut.DeleteConfirmed(toDoListId).Result;

            //Assert
            result
                .Should()
                .BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public void EditPost_ConcurrencyInUpdateAndToDoListExists_ThrowsDbUpdateConcurrencyException()
        {
            //Arrange
            var toDoListId = 1;
            var toDoList = CreateToDoListDefault();
            var listServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => listServiceFake.UpdateToDoList(A<List>.Ignored))
                .ThrowsAsync(
                    new DbUpdateConcurrencyException(
                        "Update concurrency exception",
                        new List<IUpdateEntry> { A.Fake<IUpdateEntry>() }));

            A.CallTo(() => listServiceFake.ToDoListExists(A<int>.Ignored))
                .Returns(true);

            var sut = CreateSut(listServiceFake);

            //Act
            Func<Task<IActionResult>> action = () => sut.Edit(toDoListId, toDoList);

            //Assert
            action
                .Should()
                .Throw<DbUpdateConcurrencyException>();
        }
    }
}