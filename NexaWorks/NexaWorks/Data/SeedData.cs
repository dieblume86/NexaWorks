using Microsoft.EntityFrameworkCore;
using NexaWorks.Models.Entities;

namespace NexaWorks.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        InitOs(context);

        //var product = context.Products
        //    .Include(item => item.ProductVersions)
        //        .ThenInclude(version => version.ProductVersionOs)
        //            .ThenInclude(association => association.Os)
        //    .SingleOrDefault(item => item.Name == "Mozilla Firefox");

        //if (product is null)
        //{
        //    product = new Product { Name = "Mozilla Firefox" };
        //    context.Products.Add(product);
        //}

        //var versionSpecifications = new[] { "131.0", "132.0" };
        //var versions = product.ProductVersions.ToDictionary(version => version.VersionNumber);

        //foreach (var versionNumber in versionSpecifications)
        //{
        //    if (!versions.TryGetValue(versionNumber, out var version))
        //    {
        //        version = new ProductVersion { VersionNumber = versionNumber };
        //        product.ProductVersions.Add(version);
        //        versions.Add(versionNumber, version);
        //    }

        //    foreach (var osName in osNames)
        //    {
        //        var os = operatingSystems[osName];
        //        if (!version.ProductVersionOs.Any(association =>
        //                association.Os?.Name == osName || (os.Id != 0 && association.OsId == os.Id)))
        //        {
        //            version.ProductVersionOs.Add(new ProductVersionOs { Os = os });
        //        }
        //    }
        //}

        context.SaveChanges();
    }

    private static void InitOs(ApplicationDbContext context)
    {
        if (context.Os.Any())
        {
            context.Os.RemoveRange(context.Os);
            context.SaveChanges();
        }

        var osNames = new[] { "Linux", "MacOS", "Windows", "Android", "iOS", "Windows Mobile" };

        foreach (var name in osNames)
        {
            var operatingSystem = new Os { Name = name };
            context.Os.Add(operatingSystem);
        }
    }

    private static void InitProducts(ApplicationDbContext context)
    {
        if (context.Products.Any())
        {
            context.Products.RemoveRange(context.Products);
            context.SaveChanges();
        }
        var productNames = new[] { "Tarder en Herbe", "Maître des Invectissements", "Planificateur d'Entraînement", "Planificateur d'Anxiété Sociale" };
        foreach (var name in productNames)
        {
            var product = new Product { Name = name };
            context.Products.Add(product);
        }
    }
}
