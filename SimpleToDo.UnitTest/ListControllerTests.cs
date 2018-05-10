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
            IToDoListService toDoListServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => toDoListServiceFake.ToDoLists())
                .Returns(new List<ToDoList>());

            ToDoListController sut = CreateSut(toDoListServiceFake);

            //Act
            IActionResult result = sut.Index().Result;

            //Assert
            result
                .Should()
                .BeOfType<ViewResult>();
        }

        private ToDoListController CreateSut(IToDoListService toDoListServiceFake)
        {
            return new ToDoListController(toDoListServiceFake);
        }

        [Fact]
        public void Index_ReturnTypeIsListOfToDoList()
        {
            //Arrange
            IToDoListService toDoListServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => toDoListServiceFake.ToDoLists())
                .Returns(new List<ToDoList>());

            ToDoListController sut = CreateSut(toDoListServiceFake);

            //Act
            IActionResult result = sut.Index().Result;

            //Assert
            result
                .As<ViewResult>()
                .Model
                .Should()
                .BeOfType<List<ToDoList>>();
        }

        [Fact]
        public void Index_ReturnDefaultViewName()
        {
            //Arrange
            IToDoListService toDoListServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => toDoListServiceFake.ToDoLists())
                .Returns(new List<ToDoList>());

            ToDoListController sut = CreateSut(toDoListServiceFake);

            //Act
            IActionResult result = sut.Index().Result;

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
            IToDoListService toDoListServiceFake = A.Fake<IToDoListService>();
            List<ToDoList> toDoLists = new List<ToDoList>
            {
                CreateToDoListDefault()
            };

            A.CallTo(() => toDoListServiceFake.ToDoLists())
                .Returns(toDoLists);

            ToDoListController sut = CreateSut(toDoListServiceFake);

            //Act
            IActionResult result = sut.Index().Result;

            //Assert
            result
                .As<ViewResult>()
                .Model
                .As<List<ToDoList>>()
                .Should()
                .HaveCount(1);
        }

        private ToDoList CreateToDoListDefault()
        {
            return new ToDoList
            {
                Id = 1,
                Name = "ToDo"
            };
        }

        [Fact]
        public void Edit_ReceiveNullId_ReturnNotFoundStatusCode()
        {
            //Arrange
            int notFoundStatusCode = 404;
            IToDoListService toDoListServiceFake = A.Fake<IToDoListService>();
            ToDoListController sut = CreateSut(toDoListServiceFake);

            //Act
            IActionResult result = sut.Details(null).Result;

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
            int toDoListId = 1;
            int notFoundStatusCode = 404;
            IToDoListService toDoListServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => toDoListServiceFake.FindById(A<int>.Ignored))
                .Returns(System.Threading.Tasks.Task.FromResult((ToDoList)null));

            ToDoListController sut = CreateSut(toDoListServiceFake);

            //Act
            IActionResult result = sut.Edit(toDoListId).Result;

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
            int toDoListId = 1;
            ToDoList toDoList = CreateToDoListDefault();
            IToDoListService toDoListServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => toDoListServiceFake.FindById(A<int>.Ignored))
                .Returns(toDoList);

            ToDoListController sut = CreateSut(toDoListServiceFake);

            //Act
            IActionResult result = sut.Edit(toDoListId).Result;

            //Assert
            result
                .As<ViewResult>()
                .Model
                .Should()
                .BeOfType<ToDoList>();
        }

        [Fact]
        public void Edit_ToDoListExists_ReturnToDoList()
        {
            //Arrange
            int toDoListId = 1;
            ToDoList toDoList = CreateToDoListDefault();
            IToDoListService toDoListServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => toDoListServiceFake.FindById(A<int>.Ignored))
                .Returns(toDoList);

            ToDoListController sut = CreateSut(toDoListServiceFake);

            //Act
            IActionResult result = sut.Edit(toDoListId).Result;

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
            int toDoListId = 1;
            ToDoList toDoList = CreateToDoListDefault();
            IToDoListService toDoListServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => toDoListServiceFake.FindById(A<int>.Ignored))
                .Returns(toDoList);

            ToDoListController sut = CreateSut(toDoListServiceFake);

            //Act
            IActionResult result = sut.Edit(toDoListId).Result;

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
            int toDoListId = 1;
            string toDoName = "ToDo List Unit Test";
            IToDoListService toDoListServiceFake = A.Fake<IToDoListService>();
            A.CallTo(() => toDoListServiceFake.Remove(A<int>.Ignored))
                .Returns(toDoName);

            ToDoListController sut = CreateSut(toDoListServiceFake);
            sut.TempData = new TempDataDictionary(A.Fake<HttpContext>(), A.Fake<ITempDataProvider>());

            //Act
            IActionResult result = sut.DeleteConfirmed(toDoListId).Result;

            //Assert
            result
                .Should()
                .BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public void EditPost_ConcurrencyInUpdateAndToDoListExists_ThrowsDbUpdateConcurrencyException()
        {
            //Arrange
            const int toDoListId = 1;
            ToDoList toDoList = CreateToDoListDefault();
            IToDoListService toDoListServiceFake = A.Fake<IToDoListService>();
            DbUpdateConcurrencyException dbUpdateConcurrencyException = 
                new DbUpdateConcurrencyException(
                    "Update concurrency exception",
                    new List<IUpdateEntry> { A.Fake<IUpdateEntry>() });

            A.CallTo(() => toDoListServiceFake.Update(A<ToDoList>.Ignored))
                .ThrowsAsync(dbUpdateConcurrencyException);
            A.CallTo(() => toDoListServiceFake.Exists(A<int>.Ignored))
                .Returns(true);

            ToDoListController sut = CreateSut(toDoListServiceFake);

            //Act
            Func<Task<IActionResult>> action = () => sut.Edit(toDoListId, toDoList);

            //Assert
            action
                .Should()
                .Throw<DbUpdateConcurrencyException>();
        }

        [Fact]
        public void EditPost_InvalidModalState_ReturnViewWithOneModelStateError()
        {
            //Arrange
            int toDoListId = 1;
            ToDoList toDoList = new ToDoList
            {
                Id = 1
            };

            IToDoListService toDoListServiceFake = A.Fake<IToDoListService>();

            ToDoListController sut = CreateSut(toDoListServiceFake);

            string modelStateErrorKey = "Name";
            sut.ModelState.AddModelError(modelStateErrorKey, "Name is required.");

            //Act
            IActionResult result = sut.Edit(toDoListId, toDoList).Result;

            //Assert
            result
                .As<ViewResult>()
                .ViewData
                .ModelState[modelStateErrorKey]
                .Errors
                .Should()
                .HaveCount(1);
        }
    }
}