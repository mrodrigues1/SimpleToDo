using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Storage;
using SimpleToDo.Model.Entities;

namespace SimpleToDo.Web.IntegrationTest
{
    public class WebFixture<TStartup> : IDisposable where TStartup : class
    {
        private readonly IServiceProvider _services;
        private readonly IDbContextTransaction _transaction;

        protected HttpClient Client;
        protected ToDoDbContext DbContext { get; }

        public WebFixture()
        {
            var factory = new WebApplicationFactory<TStartup>();
            Client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _services = factory.Server.Host.Services;

            DbContext = GetService<ToDoDbContext>();
            _transaction = DbContext.Database.BeginTransaction();
        }

        protected T GetService<T>() => (T)_services.GetService(typeof(T));

        public void Dispose()
        {
            if (_transaction == null) return;

            _transaction.Rollback();
            _transaction.Dispose();
        }
    }
}