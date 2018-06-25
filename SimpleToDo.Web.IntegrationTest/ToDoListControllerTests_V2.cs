using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SimpleToDo.Model.Entities;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace SimpleToDo.Web.IntegrationTest
{
    public class ToDoListControllerTests_V2 : WebFixture<TestStartup>
    {       
        [Fact]
        public async Task Index_ResponseReturnsSuccessStatusCode()
        {
            //Arrange             

            //Act
            var response = await Client.GetAsync("/");

            //Assert
            response.EnsureSuccessStatusCode();
            response.Content.Should().BeOfType<List<ToDoList>>();
            response.Content.Headers.ContentType.ToString().Should().Be("text/html; charset=utf-8");
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
                { "__RequestVerificationToken",  await AntiForgeryHelper.ExtractAntiForgeryTokenAsync(client) },
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
    }
}