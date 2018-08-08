using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        static StartupStub()
        {
            Configuration = GetConfiguration();
        }

        protected static IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ToDoDbContext>(
                options => options.UseSqlServer(Configuration["DbConnection"]),
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

        private static IConfiguration GetConfiguration()
            => new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    }

}