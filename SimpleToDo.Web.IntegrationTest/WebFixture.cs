using System;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Storage;
using SimpleToDo.Model.Entities;
using SimpleToDo.Web;

public class WebFixture<TStartup> : Fixture, IDisposable where TStartup : class
{
    private readonly WebApplicationFactory<TStartup> _factory;
    private readonly IServiceProvider _services;

    protected HttpClient Client;
    protected ToDoDbContext DbContext { get; }
    protected IDbContextTransaction Transaction;

    public WebFixture()
    {
        _factory = new WebApplicationFactory<TStartup>();
        _factory.ClientOptions.AllowAutoRedirect = false;
        _factory.WithWebHostBuilder(ConfigureWebHostBuilder);
        _services = _factory.Server.Host.Services;

        Client = _factory.CreateClient();
        DbContext = GetService<ToDoDbContext>();
        Transaction = DbContext.Database.BeginTransaction();
    }

    private static void ConfigureWebHostBuilder(IWebHostBuilder builder) =>
               builder.UseStartup<TStartup>();

    protected T GetService<T>() => (T)_services.GetService(typeof(T));

    public void Dispose()
    {
        if (Transaction != null)
        {
            Transaction.Rollback();
            Transaction.Dispose();
        }
    }
}