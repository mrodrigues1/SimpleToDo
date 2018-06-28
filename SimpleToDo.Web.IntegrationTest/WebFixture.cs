using System;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
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
            var factory = new WebApplicationFactory<TStartup>
            {
                ClientOptions =
                {
                    AllowAutoRedirect = false
                }
            };
            //factory.WithWebHostBuilder(ConfigureWebHostBuilder);

            Client = factory.CreateClient();
            _services = factory.Server.Host.Services;



            DbContext = GetService<ToDoDbContext>();
            Transaction = DbContext.Database.BeginTransaction();
        }

        protected T GetService<T>() => (T)_services.GetService(typeof(T));

        private static void ConfigureWebHostBuilder(IWebHostBuilder builder) =>
                      builder.UseStartup<TStartup>();

        protected virtual IWebHostBuilder CreateWebHostBuilder() => 
            WebHost.CreateDefaultBuilder()
                .UseStartup<TStartup>();

        public void Dispose()
        {
            if (Transaction == null) return;

            Transaction.Rollback();
            Transaction.Dispose();
        }
    }
}