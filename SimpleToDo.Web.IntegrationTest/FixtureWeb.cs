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
    public class FixtureWeb : IDisposable, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly IServiceProvider _services;

        protected HttpClient Client;

        protected ToDoDbContext DbContext { get; }

        protected IDbContextTransaction Transaction;

        public FixtureWeb(WebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _services = factory.Server.Host.Services;

            using (var scope = _services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;


                DbContext = scopedServices.GetService<ToDoDbContext>();

                DbContext.Database.EnsureCreated();
                DbContext.Database.Migrate();
                Transaction = DbContext.Database.BeginTransaction();
            }
        }

        private static void ConfigureWebHostBuilder(IWebHostBuilder builder) =>
            builder.Build();

        protected T GetService<T>() => (T)_services.GetService(typeof(T));

        public void Dispose()
        {
            if (Transaction == null) return;

            Transaction.Rollback();
            Transaction.Dispose();
        }
    }
}