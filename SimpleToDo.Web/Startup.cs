using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleToDo.Model.Entities;
using SimpleToDo.Repository.Contracts;
using SimpleToDo.Repository.Implementations;
using SimpleToDo.Service.Contracts;
using SimpleToDo.Service.Implementations;

namespace SimpleToDo.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ToDoDbContext>(
                options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SimpleToDo")));

            services.AddTransient<IToDoListService, ToDoListService>();
            services.AddTransient<IToDoListRepository, ToDoListRepository>();

            services.AddMemoryCache();
            services.AddSession();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=List}/{action=Index}/{id?}");
            });
        }
    }
}
