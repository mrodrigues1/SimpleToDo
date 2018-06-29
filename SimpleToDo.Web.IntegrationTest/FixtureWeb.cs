using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using SimpleToDo.Model.Entities;
using Xunit;

namespace SimpleToDo.Web.IntegrationTest
{
    public class FixtureWeb : IDisposable, IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly IServiceProvider _services;

        protected HttpClient Client;

        protected ToDoDbContext DbContext { get; }

        protected IDbContextTransaction Transaction;

        private IServiceScope _scope;

        public FixtureWeb(CustomWebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _services = factory.Server.Host.Services;

            _scope = _services.CreateScope();

            var scopedServices = _scope.ServiceProvider;
            DbContext = scopedServices.GetRequiredService<ToDoDbContext>();

            DbContext.Database.EnsureCreated();
            DbContext.Database.Migrate();

            Transaction = DbContext.Database.BeginTransaction();
        }

        protected T GetService<T>() => (T)_services.GetService(typeof(T));

        public void Dispose()
        {
            if (Transaction == null) return;

            Transaction.Rollback();
            Transaction.Dispose();
            _scope.Dispose();
        }
    }
}