using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleToDo.Model.Entities;

namespace SimpleToDo.Web.IntegrationTest
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var contextOptions = new DbContextOptionsBuilder()
               .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SimpleToDo;Trusted_Connection=True;MultipleActiveResultSets=true")
               .Options;


                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkSqlServer()
                    .BuildServiceProvider();

                services.AddDbContext<ToDoDbContext>(
                    options =>
                    {
                        options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SimpleToDo;Trusted_Connection=True;MultipleActiveResultSets=true");
                        options.UseInternalServiceProvider(serviceProvider);
                    },
                    ServiceLifetime.Scoped,
                    ServiceLifetime.Singleton
                );
            });
        }
    }
}