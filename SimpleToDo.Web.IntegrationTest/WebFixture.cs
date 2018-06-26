using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Storage;
using SimpleToDo.Model.Entities;

namespace SimpleToDo.Web.IntegrationTest
{
    public class WebFixture<TStartup> : IDisposable where TStartup : class
    {
        private readonly IServiceProvider _services;
        private readonly IDbContextTransaction Transaction;

        protected HttpClient Client;
        private WebApplicationFactory<TStartup> _factory;

        protected ToDoDbContext DbContext { get; }

        public WebFixture()
        {
            _factory = new WebApplicationFactory<TStartup>();
            _factory.ClientOptions.AllowAutoRedirect = false;
            _factory.WithWebHostBuilder(ConfigureWebHostBuilder);
            _services = _factory.Server.Host.Services;

            Client = _factory.CreateClient();
            DbContext = GetService<ToDoDbContext>();
            Transaction = DbContext.Database.BeginTransaction();
        }
        protected T GetService<T>() => (T)_services.GetService(typeof(T));

        private static void ConfigureWebHostBuilder(IWebHostBuilder builder) =>
                      builder.UseStartup<TStartup>();
        public void Dispose()
        {
            if (Transaction == null) return;

            Transaction.Rollback();
            Transaction.Dispose();
        }
    }
}