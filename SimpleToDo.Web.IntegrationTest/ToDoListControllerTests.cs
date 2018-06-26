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
    public class ToDoListControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ToDoListControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;            
        }

        [Fact]
        public async Task Index_ResponseReturnsSuccessStatusCode()
        {
            //Arrange 
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync("/");

            //Assert
            response.EnsureSuccessStatusCode();
            response.Content.Should().BeOfType<List<ToDoList>>();
            response.Content.Headers.ContentType.ToString().Should().Be("text/html; charset=utf-8");
        }

        [Fact]
        public async Task Create_ValidToDoList_RedirectToIndexAction_Error()
        {
            //Arrange 
            var client = _factory
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
            var formData = new Dictionary<string, string>
            {
                { nameof(ToDoList.Name), "To Do List 1" }
            };

            //Act
            var response = await client.PostAsync(
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
            var client = _factory
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            var formData = new Dictionary<string, string>
            {
                { "__RequestVerificationToken",  await AntiForgeryHelper.ExtractAntiForgeryTokenAsync(client) },
                { nameof(ToDoList.Name), "To Do List 1" }
            };

            //Act
            var response = await client.PostAsync(
                "/ToDoList/Create",
                new FormUrlEncodedContent(formData));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            response.Headers.Location.OriginalString.Should().StartWith("/");
        }
    }
}