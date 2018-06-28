using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleToDo.Model.Entities;
using SimpleToDo.Web;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
{
    protected ToDoDbContext DbContext;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            services.AddDbContext<ToDoDbContext>(
                options =>
                {
                    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SimpleToDo;Trusted_Connection=True;MultipleActiveResultSets=true");
                    options.UseInternalServiceProvider(serviceProvider);
                },
                ServiceLifetime.Singleton
            );

            DbContext = serviceProvider.GetService<ToDoDbContext>();
        });
    }
}