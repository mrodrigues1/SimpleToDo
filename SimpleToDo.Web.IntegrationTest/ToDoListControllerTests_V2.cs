using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using SimpleToDo.Model.Entities;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace SimpleToDo.Web.IntegrationTest
{
    public class ToDoListControllerTests_V2 : FixtureWeb
    {
        public ToDoListControllerTests_V2(CustomWebApplicationFactory<Startup> fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Index_ResponseReturnsSuccessStatusCode()
        {
            //Arrange             

            //Act
            var response = await Client.GetAsync("/");

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Index_ReturnToDoListToView()
        {
            //Arrange
            var toDoList = ToDoListFactory.Create().Single();
            await DbContext.ToDoList.AddAsync(toDoList);
            await DbContext.SaveChangesAsync();

            //Act
            var response = await Client.GetAsync("/");

            //Assert            
            response
                 .Content
                 .Should()
                 .As<List<ToDoList>>()
                 .Should()
                 .Contain(toDoList);
        }

        [Fact]
        public async Task Create_ReturnToDoListToView()
        {
            //Arrange            

            //Act
            var response = await Client.GetAsync("/ToDoList/Create");

            //Assert 
            response
                .StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
        }


        [Fact]
        public async Task Create_ValidToDoList_RedirectToIndexAction_Error()
        {
            //Arrange            
            var formData = new Dictionary<string, string>
            {
                { nameof(ToDoList.Name), "To Do List 1" }
            };

            //Act
            var response = await Client.PostAsync(
                "/ToDoList/Create",
                new FormUrlEncodedContent(formData));

            //Assert            
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            response.Headers.Location.OriginalString.Should().StartWith("/");
        }

        [Fact]
        public async Task Create_ValidToDoList_RedirectToIndexAction()
        {
            //Arrange            
            var formData = new Dictionary<string, string>
            {
                { "__RequestVerificationToken",  await AntiForgeryHelper.ExtractAntiForgeryTokenAsync(Client) },
                { nameof(ToDoList.Name), "To Do List 1" }
            };

            //Act
            var response = await Client.PostAsync(
                "/ToDoList/Create",
                new FormUrlEncodedContent(formData));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            response.Headers.Location.OriginalString.Should().StartWith("/");
        }

        [Fact]
        public async Task EditGet_IdEqualsOne_ReturnToDoListToView()
        {
            //Arrange
            var toDoList = ToDoListFactory.Create().Single();
            await DbContext.ToDoList.AddAsync(toDoList);
            await DbContext.SaveChangesAsync();

            //Act
            var response = await Client.GetAsync($"/ToDoList/Edit/{toDoList.Id}");

            //Assert            
            response
                .Content
                .Should()
                .As<ToDoList>()
                .Should()
                .Be(toDoList);
        }

        [Fact]
        public async Task EditGet_IdEqualsNull_ReturnNotFoundHttpStatusCode()
        {
            //Arrange

            //Act
            var response = await Client.GetAsync("/ToDoList/Edit/");

            //Assert            
            response
                .StatusCode
                .Should()
                .Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task EditPost_ValidIdAndToDoList_RedirectToIndexView()
        {
            //Arrange
            var toDoList = ToDoListFactory.Create().Single();
            await DbContext.ToDoList.AddAsync(toDoList);
            await DbContext.SaveChangesAsync();

            var formData = new Dictionary<string, string>
            {
                { "id", toDoList.Id.ToString() },
                { "Id", toDoList.Id.ToString() },
                { "Name", "ToDoList Test 1" }
            };

            //Act
            var response = await Client
                .PostAsync(
                    "/ToDoList/Edit/",
                    new FormUrlEncodedContent(formData));

            //Assert            
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            response.Headers.Location.OriginalString.Should().StartWith("/");
        }

        [Fact]
        public async Task EditPost_ModelStateInvalid_ReturnModelToEditView()
        {
            //Arrange
            var toDoList = ToDoListFactory.Create().Single();
            await DbContext.ToDoList.AddAsync(toDoList);
            await DbContext.SaveChangesAsync();

            var formData = new Dictionary<string, string>
            {
                { "id", toDoList.Id.ToString() },
                { "Id", toDoList.Id.ToString() }
            };

            //Act
            var response = await Client
                .PostAsync(
                    "/ToDoList/Edit/",
                    new FormUrlEncodedContent(formData));

            //Assert            
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            response.Headers.Location.OriginalString.Should().StartWith("/");
        }

        [Fact]
        public async Task EditGet_ThrowDbConcurrencyException()
        {
            //Arrange
            var toDoList = ToDoListFactory.Create().Single();
            await DbContext.ToDoList.AddAsync(toDoList);
            await DbContext.SaveChangesAsync();

            var toDoListDb = await DbContext.ToDoList.FirstOrDefaultAsync(x => x.Id == toDoList.Id);
            toDoListDb.Name = "Concurrency";

            var formData = new Dictionary<string, string>
            {
                { "id", toDoList.Id.ToString() },
                { "Id", toDoList.Id.ToString() },
                { "Name", "ToDoList Test 1" }
            };

            //Act
            Func<Task<HttpResponseMessage>> action = () => Client
                .PostAsync(
                    "/ToDoList/Edit/",
                    new FormUrlEncodedContent(formData));

            //Assert            
            action
                .Should()
                .Throw<DbUpdateConcurrencyException>();
        }

        [Fact]
        public async Task DeleteConfirmed_ValidIdAndToDoList_RedirectToIndexView()
        {
            //Arrange
            var toDoList = ToDoListFactory.Create().Single();
            await DbContext.ToDoList.AddAsync(toDoList);
            await DbContext.SaveChangesAsync();

            var formData = new Dictionary<string, string>
            {
                { "id", toDoList.Id.ToString() }
            };

            //Act
            var response = await Client
                .PostAsync(
                    "/ToDoList/DeleteConfirmed/",
                    new FormUrlEncodedContent(formData));

            //Assert            
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            response.Headers.Location.OriginalString.Should().StartWith("/");
        }
    }
}