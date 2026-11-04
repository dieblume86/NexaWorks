using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NexaWorks.Models.Entities;

namespace NexaWorks.Data;

public static class SeedData
{
    private static readonly string linux = "Linux";
    private static readonly string macos = "MacOS";
    private static readonly string windows = "Windows";
    private static readonly string android = "Android";
    private static readonly string ios = "iOS";
    private static readonly string windowsMobile = "Windows Mobile";

    private static readonly string productTrader = "Trader en Herbe";
    private static readonly string productMaster = "Maître des Invectissements";
    private static readonly string productTraining = "Planificateur d'Entraînement";
    private static readonly string productAnxiety = "Planificateur d'Anxiété Sociale";

    public static void Initialize(ApplicationDbContext context)
    {
        Reset(context);

        ArgumentNullException.ThrowIfNull(context);

        InitOs(context);
        InitProducts(context);
        InitProductVersions(context);
        InitTickets(context);
    }

    private static void InitOs(ApplicationDbContext context)
    {
        if (context.Os.Any())
        {
            context.Os.RemoveRange(context.Os);
        }

        var osNames = new[] { linux, macos, windows, android, ios, windowsMobile };

        foreach (var name in osNames)
        {
            var operatingSystem = new Os { Name = name };
            context.Os.Add(operatingSystem);
        }

        context.SaveChanges();
    }

    private static void InitProducts(ApplicationDbContext context)
    {
        if (context.Products.Any())
        {
            context.Products.RemoveRange(context.Products);
        }

        var productNames = new[] { productTrader, productMaster, productTraining, productAnxiety };

        foreach (var name in productNames)
        {
            var product = new Product { Name = name };
            context.Products.Add(product);
        }

        context.SaveChanges();
    }

    private static void InitProductVersions(ApplicationDbContext context)
    {
        if (context.ProductVersions.Any())
        {
            context.ProductVersions.RemoveRange(context.ProductVersions);
        }

        foreach (var product in context.Products)
        {
            if (product.Name.Equals(productTrader))
            {
                AddProductVersions(new[] { "1.0", "1.1", "1.2", "1.3" }, product.Id, context);
            }
            else if (product.Name.Equals(productMaster))
            {
                AddProductVersions(new[] { "1.0", "2.0", "2.1" }, product.Id, context);
            }
            else if (product.Name.Equals(productTraining))
            {
                AddProductVersions(new[] { "1.0", "1.1", "2.0" }, product.Id, context);
            }
            else if (product.Name.Equals(productAnxiety))
            {
                AddProductVersions(new[] { "1.0", "1.1" }, product.Id, context);
            }
        }

        context.SaveChanges();
    }
    private static void AddProductVersions(string[] versions, int productId, ApplicationDbContext context)
    {
        foreach (var version in versions)
        {
            context.ProductVersions.Add(new ProductVersion { VersionNumber = version, ProductId = productId });
        }
    }

    private static void InitTickets(ApplicationDbContext context)
    {
        Ticket1(context);
        Ticket2(context);

        context.SaveChanges();
    }
    private static void AddTicket(string poductName,
        string versionNumber,
        string osName,
        DateTimeOffset creationDate,
        DateTimeOffset? resolutionDate,
        string issue,
        string? resolution,
        ApplicationDbContext context)
    {
        var product = context.Products.FirstOrDefault(p => p.Name == poductName);
        var productVersion = context.ProductVersions.FirstOrDefault(pv => pv.ProductId == product.Id && pv.VersionNumber == versionNumber);
        var os = context.Os.FirstOrDefault(o => o.Name == osName);

        var productVersionOs = GetProductVersionOs(productVersion, os, context);

        if (productVersionOs == null)
        {
            context.ProductVersionOs.Add(new ProductVersionOs { ProductVersionId = productVersion.Id, OsId = os.Id });
            context.SaveChanges();
            productVersionOs = GetProductVersionOs(productVersion, os, context);
        }

        context.Tickets.Add(new Ticket
        {
            ProductVersionOs = productVersionOs,
            CreationDate = creationDate,
            ResolutionDate = resolutionDate,
            Issue = issue,
            Resolution = resolution,
            Status = string.IsNullOrEmpty(resolution) ? Models.Enums.ETicketStatus.InProgress : Models.Enums.ETicketStatus.Resolved
        });
    }
    private static ProductVersionOs GetProductVersionOs(ProductVersion productVersion, Os os, ApplicationDbContext context)
    {
        return context.ProductVersionOs.FirstOrDefault(pvo => pvo.ProductVersionId == productVersion.Id && pvo.OsId == os.Id);
    }

    private static void Reset(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        using var transaction = context.Database.BeginTransaction();

        //First remove Tickets because they depend on ProductVersionOs, which in turn depends on ProductVersions and Os.
        context.Database.ExecuteSqlRaw("DELETE FROM [dbo].[Tickets]");
        context.Database.ExecuteSqlRaw("DELETE FROM [dbo].[ProductVersionOs]");
        context.Database.ExecuteSqlRaw("DELETE FROM [dbo].[ProductVersions]");
        context.Database.ExecuteSqlRaw("DELETE FROM [dbo].[Products]");
        context.Database.ExecuteSqlRaw("DELETE FROM [dbo].[Os]");

        // Reset the identity columns to 0 for each table. This ensures that the next inserted record will have an ID of 1.
        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('[dbo].[Tickets]', RESEED, 0)");
        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('[dbo].[ProductVersionOs]', RESEED, 0)");
        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('[dbo].[ProductVersions]', RESEED, 0)");
        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('[dbo].[Products]', RESEED, 0)");
        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('[dbo].[Os]', RESEED, 0)");

        transaction.Commit();

        // The SQL commands bypass the EF Core change tracker.
        context.ChangeTracker.Clear();
    }


    private static void Ticket1(ApplicationDbContext context)
    {
        AddTicket(
           productTrader,
           "1.2",
           android,
           new DateTimeOffset(2025, 1, 12, 10, 30, 0, TimeSpan.Zero),
           new DateTimeOffset(2025, 1, 15, 14, 45, 0, TimeSpan.Zero),
           "L’utilisateur indique que le solde de son portefeuille ne se met pas à jour immédiatement après la vente d’actions. Le montant affiché reste celui d’avant la vente jusqu’à ce qu’il ferme puis rouvre l’application.",
           "L’équipe de développement a identifié un problème de rafraîchissement des données après la confirmation d’une transaction. Une mise à jour du portefeuille a été ajoutée après chaque vente réussie. Des tests ont confirmé que le nouveau solde s’affiche désormais sans redémarrer l’application",
           context
       );
    }
    private static void Ticket2(ApplicationDbContext context)
    {
        AddTicket(
           productMaster,
           "2.1",
           android,
           new DateTimeOffset(2025, 1, 18, 10, 30, 0, TimeSpan.Zero),
           null,
            "L’utilisateur signale que le graphique de répartition de ses investissements affiche des pourcentages dont la somme dépasse 100 %. Le problème apparaît après l’ajout de plusieurs nouveaux placements, alors que les montants individuels semblent corrects.",
            null,
            context
       );
    }
}