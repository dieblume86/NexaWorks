using Microsoft.CodeAnalysis;
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
        ArgumentNullException.ThrowIfNull(context);

        InitOs(context);
        InitProducts(context);
        InitProductVersions(context);
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
}
