using Microsoft.EntityFrameworkCore;

namespace NexaWorks.Data;

internal static class DbInitializerExtension
{
    public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();

        try
        {
            context.Database.Migrate();
            SeedData.Initialize(context);
        }
        catch (Exception exception)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(exception, "Erreur pendant l'application des migrations ou le remplissage de la base.");
            throw;
        }

        return app;
    }
}
