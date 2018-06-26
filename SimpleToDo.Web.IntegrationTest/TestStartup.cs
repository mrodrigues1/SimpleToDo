
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleToDo.Model.Entities;
using SimpleToDo.Repository.Contracts;
using SimpleToDo.Repository.Implementations;
using SimpleToDo.Service.Contracts;
using SimpleToDo.Service.Implementations;

namespace SimpleToDo.Web.IntegrationTest
{
    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<ToDoDbContext>(
                options => 
                    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SimpleToDo;Trusted_Connection=True;MultipleActiveResultSets=true"),
                ServiceLifetime.Singleton
            );

            services.AddTransient<IToDoListService, ToDoListService>();
            services.AddTransient<ITaskService, TaskService>();

            services.AddTransient<IToDoListRepository, ToDoListRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
        }

        public void Configure(IApplicationBuilder app, ToDoDbContext dbContext)
        {
            dbContext.Database.Migrate();
            app.UseMvc();
        }
    }
}