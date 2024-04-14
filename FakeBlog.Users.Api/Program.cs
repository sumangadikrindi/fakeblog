using System.Reflection;
using AutoMapper;
using FakeBlog.Users.Api.Data;
using FakeBlog.Users.Api.MappingProfiles;
using FakeBlog.Users.Api.Repository.Contracts;
using FakeBlog.Users.Api.Repository.Implementations;
using FakeBlog.Users.Api.Services;
using FakeBlog.Users.Api.Services.QueryHandlers;
using FakeBlog.Users.Api.Services.CommandHandlers;
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
var mapper = new MapperConfiguration(configuration => {
    configuration.AddProfile(new MappingProfile());
}).CreateMapper();
builder.Services.AddSingleton<IMapper>(mapper);

builder.Services.AddDbContext<UsersDbContext>(options => {
    options.UseSqlServer(Environment.GetEnvironmentVariable("FakeBlog_Users_SQL_ConnectionString"), builder =>
        {
            builder.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
        });
});

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped<CreateUserCommandHandler>();
builder.Services.AddScoped<GetUserDetailsQueryHandler>();

// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
});

var app = builder.Build();
app.Services.ApplyPendingDbMigrations();

// Configure the HTTP request pipeline.
app.MapGrpcService<UserService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();