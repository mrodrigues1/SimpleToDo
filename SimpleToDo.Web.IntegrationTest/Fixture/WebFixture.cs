using System;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Storage;
using SimpleToDo.Model.Entities;

namespace SimpleToDo.Web.IntegrationTest
{
    public class WebFixture<TStartup> : Fixture, IDisposable where TStartup : class
    {
        private readonly IServiceProvider _services;

        protected HttpClient Client;

        protected ToDoDbContext DbContext { get; }

        protected IDbContextTransaction Transaction;

        public WebFixture()
        {
            var builder = WebHost.CreateDefaultBuilder()
                .UseStartup<TStartup>();
            var server = new TestServer(builder);
            Client = server.CreateClient();
            _services = server.Host.Services;

            DbContext = GetService<ToDoDbContext>();
            Transaction = DbContext.Database.BeginTransaction();
        }

        protected T GetService<T>() => (T) _services.GetService(typeof(T));

        public void Dispose()
        {
            if (Transaction == null) return;

            Transaction.Rollback();
            Transaction.Dispose();
        }
    }
}