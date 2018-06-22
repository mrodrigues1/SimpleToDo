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

public class WebFixture<TStartup> : IDisposable where TStartup : class
{
    private readonly WebApplicationFactory<TStartup> _factory;
    private readonly IServiceProvider _services;
    private readonly IDbContextTransaction _transaction;

    protected HttpClient Client;
    protected ToDoDbContext DbContext { get; }

    public WebFixture()
    {        
        _factory = new WebApplicationFactory<TStartup>();
        Client = _factory.CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

        _services = _factory.Server.Host.Services;

        DbContext = GetService<ToDoDbContext>();
        _transaction = DbContext.Database.BeginTransaction();
    }

    protected T GetService<T>() => (T)_services.GetService(typeof(T));

    public void Dispose()
    {
        if (_transaction != null)
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }
    }
}