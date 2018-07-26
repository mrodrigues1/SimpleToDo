﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SimpleToDo.Model.Entities;
using SimpleToDo.Web.IntegrationTest.Factory;
using SimpleToDo.Web.IntegrationTest.Fixture;
using SimpleToDo.Web.IntegrationTest.Helper;
using SimpleToDo.Web.IntegrationTest.Stub;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace SimpleToDo.Web.IntegrationTest
{
    public class ToDoListControllerTests : WebFixture<StartupStub>
    {
        [Fact]
        public async Task Index_GetAsyncCall_ResponseReturnsSuccessStatusCode()
        {
            //Arrange
            var toDoList = ToDoListFactory.Create().Single();
            toDoList.Name = "Integration Test";
            await DbContext.ToDoList.AddAsync(toDoList);
            await DbContext.SaveChangesAsync();

            //Act
            var response = await Client.GetAsync("/");

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Index_GetAsyncCall_ReturnCreatedToDoListToView()
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
                .ReadAsStringAsync()
                .Result
                .Should()
                .Contain(toDoList.Name);
        }

        [Fact]
        public async Task Create_GetAsyncCall_RetunOkStatusCode()
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
        public async Task Create_PostAsyncCallWithValidToDoList_RedirectToIndexAction_Error()
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
            response.Headers.Location.ToString().Should().Be("/");
        }

        [Fact]
        public async Task Create_PostAsyncCallWithValidToDoList_RedirectToIndexAction()
        {
            //Arrange            
            var formData = new Dictionary<string, string>
            {
                {
                    "__RequestVerificationToken",
                    await AntiForgeryHelper.EnsureAntiforgeryTokenAsync(Client)
                },
                { nameof(ToDoList.Name), "To Do List 1" }
            };

            //Act
            var response = await Client.PostAsync(
                "/ToDoList/Create",
                new FormUrlEncodedContent(formData));

            //Assert            
            response.Headers.Location.ToString().Should().Be("/");
        }

        [Fact]
        public async Task Edit_GetAsyncCallWithNewToDoListId_ReturnToDoListToView()
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
                .ReadAsStringAsync()
                .Result
                .Should()
                .Contain(toDoList.Id.ToString());
        }

        [Fact]
        public async Task Edit_GetAsyncCallWithNullId_ReturnNotFoundStatusCode()
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
        public async Task Edit_PostAsyncCallWithValidIdAndToDoList_RedirectToIndexView()
        {
            //Arrange
            var toDoList = ToDoListFactory.Create().Single();
            await DbContext.ToDoList.AddAsync(toDoList);
            await DbContext.SaveChangesAsync();

            var formData = new Dictionary<string, string>
            {
                {
                    "__RequestVerificationToken",
                    await AntiForgeryHelper.EnsureAntiforgeryTokenAsync(Client)
                },
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
            response.Headers.Location.ToString().Should().Be("/");
        }

        [Fact]
        public async Task Edit_PostAsyncCallWithModelStateInvalid_ReturnModelToEditView()
        {
            //Arrange
            var toDoList = ToDoListFactory.Create().Single();
            await DbContext.ToDoList.AddAsync(toDoList);
            await DbContext.SaveChangesAsync();

            var formData = new Dictionary<string, string>
            {
                {
                    "__RequestVerificationToken",
                    await AntiForgeryHelper.EnsureAntiforgeryTokenAsync(Client)
                },
                { "id", toDoList.Id.ToString() },
                { "Id", toDoList.Id.ToString() }
            };

            //Act
            var response = await Client
                .PostAsync(
                    "/ToDoList/Edit/",
                    new FormUrlEncodedContent(formData));

            //Assert                        
            response
                .Content
                .ReadAsStringAsync()
                .Result
                .Should()
                .Contain($"value=\"{toDoList.Id.ToString()}\"");
        }

        [Fact]
        public async Task Edit_PostAsyncCallWithConcurrency_ThrowDbConcurrencyException()
        {
            //Arrange
            var toDoList = ToDoListFactory.Create().Single();
            await DbContext.ToDoList.AddAsync(toDoList);
            await DbContext.SaveChangesAsync();

            var toDoListDb = await DbContext.ToDoList.FirstOrDefaultAsync(x => x.Id == toDoList.Id);
            toDoListDb.Name = "Concurrency";

            var formData = new Dictionary<string, string>
            {
                {
                    "__RequestVerificationToken",
                    await AntiForgeryHelper.EnsureAntiforgeryTokenAsync(Client)
                },
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
        public async Task DeleteConfirmed_PostAsyncCallWithValidIdAndToDoList_RedirectToIndexView()
        {
            //Arrange
            var toDoList = ToDoListFactory.Create().Single();
            await DbContext.ToDoList.AddAsync(toDoList);
            await DbContext.SaveChangesAsync();

            var formData = new Dictionary<string, string>
            {
                {
                    "__RequestVerificationToken",
                    await AntiForgeryHelper.EnsureAntiforgeryTokenAsync(Client)
                },
                { "id", toDoList.Id.ToString() }
            };

            //Act
            var response = await Client
                .PostAsync(
                    $"/ToDoList/Delete/",
                    new FormUrlEncodedContent(formData));

            //Assert            
            response.StatusCode.Should().Be(HttpStatusCode.Found);
            response.Headers.Location.ToString().Should().Be("/");
        }
    }
}