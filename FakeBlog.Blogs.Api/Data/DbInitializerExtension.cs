using Microsoft.EntityFrameworkCore;

namespace FakeBlog.Blogs.Api.Data;

public static class DbInitializerExtension
{
    public static async void ApplyPendingDbMigrations(this IServiceProvider services)
    {
        using var servicesScope = services.CreateScope();
        using var dbContext = servicesScope.ServiceProvider.GetService<BlogsDbContext>();
        var logger = servicesScope.ServiceProvider.GetService<ILogger<BlogsDbContext>>();

        logger!.LogInformation("Checking for pending migrations");

        if (dbContext!.Database.GetPendingMigrations().Any())
        {
            logger!.LogInformation("There are pending migrations to execute. Starting to migrate now");
            await dbContext.Database.MigrateAsync();
            logger!.LogInformation("Migration complete");
        }
        else
        {
            logger!.LogInformation("Db is already up to date.");
        }
    }
}
