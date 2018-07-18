using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleToDo.Model.Entities;
using SimpleToDo.Repository.Contracts;
using SimpleToDo.Repository.Implementations;
using SimpleToDo.Service.Contracts;
using SimpleToDo.Service.Implementations;

namespace SimpleToDo.Web.IntegrationTest.Stub
{
    public class StartupStub
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddDbContext<ToDoDbContext>(
                options =>
                {
                    options.UseSqlServer(
                        "Server=(localdb)\\MSSQLLocalDB;Database=SimpleToDo;Trusted_Connection=True;MultipleActiveResultSets=true");
                },
                ServiceLifetime.Singleton,
                ServiceLifetime.Singleton);

            services.AddTransient<IToDoListService, ToDoListService>();
            services.AddTransient<ITaskService, TaskService>();

            services.AddTransient<IToDoListRepository, ToDoListRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, ToDoDbContext dbContext)
        {
            dbContext.Database.Migrate();            

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=ToDoList}/{action=Index}/{id?}");
            });
        }
    }

}