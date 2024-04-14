using System.Reflection;
using AutoMapper;
using FakeBlog.Blogs.Api.Data;
using FakeBlog.Blogs.Api.MappingProfiles;
using FakeBlog.Blogs.Api.Repository.Contracts;
using FakeBlog.Blogs.Api.Repository.Implementations;
using FakeBlog.Blogs.Api.Services;
using FakeBlog.Blogs.Api.Services.QueryHandlers;
using FakeBlog.Blogs.Api.Services.CommandHandlers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(80, listenOptions =>
        {
            // Enable HTTP/1
            listenOptions.Protocols = HttpProtocols.Http1;
        });
        options.ListenAnyIP(50051, listenOptions =>
        {
            // Enable HTTP/2
            listenOptions.Protocols = HttpProtocols.Http2;
        });
    }
);

// Add services to the container.
//AutoMapper configuation
var mapper = new MapperConfiguration(configuration =>{
    configuration.AddProfile(new MappingProfile());
}).CreateMapper();
builder.Services.AddSingleton<IMapper>(mapper);

builder.Services.AddDbContext<BlogsDbContext>(options => {
    options.UseSqlServer(Environment.GetEnvironmentVariable("FakeBlog_Blogs_SQL_ConnectionString"), builder =>
        {
            builder.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
        });
});

builder.Services.AddScoped<IBlogRepository, BlogRepository>();

builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped<CreateBlogCommandHandler>();
builder.Services.AddScoped<GetBlogQueryHandler>();

// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
});

var app = builder.Build();
app.Services.ApplyPendingDbMigrations();

// Configure the HTTP request pipeline.
app.MapGrpcService<BlogService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();