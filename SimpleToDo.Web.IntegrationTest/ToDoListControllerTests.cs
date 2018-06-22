using System.Collections.Generic;
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
            response.Content.Headers.ContentType.ToString().Should().Be("text/html; charset=utf-8");
        }

        [Fact]
        public async Task Create_ValidToDoList_RedirectToIndexAction()
        {
            //Arrange 
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions {AllowAutoRedirect = true});
            var responseGet = await client.GetAsync("/ToDoList/Create");
            responseGet.EnsureSuccessStatusCode();

            string antiForgeryToken = await AntiForgeryHelper.ExtractAntiForgeryTokenAsync(responseGet);

            var formData = new Dictionary<string, string>
            {
                { "__RequestVerificationToken",  antiForgeryToken },
                { nameof(ToDoList.Name), "To Do List 1" }
            };

            //Act
            var response = await client.PostAsync(
                "/ToDoList/Create",
                new FormUrlEncodedContent(formData));

            //Assert            
            response.RequestMessage.RequestUri.OriginalString.Should().Be("http://localhost/");
        }
    }
}