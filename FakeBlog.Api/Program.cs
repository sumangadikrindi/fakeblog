using System.Text.Json.Serialization;
using FakeBlog.Api.MappingProfiles;
using FakeBlog.Blogs.Api;
using FakeBlog.Users.Api;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using static FakeBlog.Blogs.Api.BlogService;
using static FakeBlog.Users.Api.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("BlogCreaterPolicy", policyBuilder =>
        policyBuilder.Requirements.Add(new ScopeAuthorizationRequirement() { RequiredScopesConfigurationKey = $"AzureAd:BlogCreaterScopes" }));
    
    config.AddPolicy("BlogReaderPolicy", policyBuilder =>
        policyBuilder.Requirements.Add(new ScopeAuthorizationRequirement() { RequiredScopesConfigurationKey = $"AzureAd:BlogReaderScopes" }));
});

builder.Services.AddAutoMapper(configuration => {
    configuration.AddProfile(new MappingProfile());
});

builder.Services.AddControllers().AddJsonOptions(options =>
        {
            // Ignore null values during serialization
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<UserServiceClient>(provider => 
    new UserService.UserServiceClient(
        GrpcChannel.ForAddress(
            Environment.GetEnvironmentVariable("FakeBlog_Users_GRPC_URL")!
        )
    )
);

builder.Services.AddSingleton<BlogServiceClient>(provider => 
    new BlogService.BlogServiceClient(
        GrpcChannel.ForAddress(
            Environment.GetEnvironmentVariable("FakeBlog_Blogs_GRPC_URL")!
        )
    )
);

var app = builder.Build();
app.UseGrpcWeb();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
