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
    private static readonly string productMaster = "Maître des Investissements";
    private static readonly string productTraining = "Planificateur d’Entraînement";
    private static readonly string productAnxiety = "Planificateur d’Anxiété Sociale";

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
        Ticket3(context);
        Ticket4(context);
        Ticket5(context);
        Ticket6(context);
        Ticket7(context);
        Ticket8(context);
        Ticket9(context);
        Ticket10(context);
        Ticket11(context);
        Ticket12(context);
        Ticket13(context);
        Ticket14(context);
        Ticket15(context);
        Ticket16(context);
        Ticket17(context);
        Ticket18(context);
        Ticket19(context);
        Ticket20(context);
        Ticket21(context);
        Ticket22(context);
        Ticket23(context);
        Ticket24(context);
        Ticket25(context);

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
    private static void Ticket3(ApplicationDbContext context)
    {
        AddTicket(
            productTraining,
            "1.1",
            ios,
            new DateTimeOffset(2025, 1, 22, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2025, 1, 24, 0, 0, 0, TimeSpan.Zero),
            "L’utilisateur indique que les séances d’entraînement programmées pour le lendemain disparaissent du calendrier après la fermeture de l’application. Les séances restent visibles tant que l’application demeure ouverte.",
            "L’équipe a découvert que les modifications du calendrier n’étaient pas toujours enregistrées avant la fermeture de l’application. La sauvegarde automatique a été corrigée et un message de confirmation s’affiche désormais lorsque les séances sont enregistrées avec succès.",
            context
        );
    }
    private static void Ticket4(ApplicationDbContext context)
    {
        AddTicket(
            productAnxiety,
            "1.0",
            macos,
            new DateTimeOffset(2025, 1, 27, 0, 0, 0, TimeSpan.Zero),
            null,
            "L’utilisateur explique que les exercices de préparation à une conversation ne s’affichent plus entièrement sur son écran. Sur certaines tailles de fenêtre, le bouton permettant de passer à l’exercice suivant est inaccessible, ce qui empêche de poursuivre le programme.",
            null,
            context
        );
    }
    private static void Ticket5(ApplicationDbContext context)
    {
        AddTicket(
            productTrader,
            "1.0",
            linux,
            new DateTimeOffset(2025, 2, 2, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2025, 2, 6, 0, 0, 0, TimeSpan.Zero),
            "L’utilisateur signale que l’application ne démarre plus après une mise à jour de son système Linux. Un message d’erreur apparaît au lancement et indique qu’une bibliothèque nécessaire au fonctionnement du programme est introuvable.",
            "L’analyse a révélé que le programme dépendait d’une bibliothèque qui n’était pas correctement incluse dans le paquet d’installation. Le paquet a été corrigé afin d’intégrer cette dépendance et les instructions d’installation ont été mises à jour.",
            context
        );
    }
    private static void Ticket6(ApplicationDbContext context)
    {
        AddTicket(
            productMaster,
            "1.0",
            ios,
            new DateTimeOffset(2025, 2, 5, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2025, 2, 9, 0, 0, 0, TimeSpan.Zero),
            "L’utilisateur indique que les alertes de prix qu’il a configurées ne sont pas déclenchées lorsque la valeur d’une action atteint le seuil défini. Il reçoit parfois la notification plusieurs heures après que le seuil a été dépassé.",
            "L’équipe a identifié un retard dans le traitement des alertes lorsque l’application fonctionne en arrière-plan. Le mécanisme de vérification des seuils a été optimisé et les notifications sont désormais envoyées dans un délai plus court.",
            context
        );
    }
    private static void Ticket7(ApplicationDbContext context)
    {
        AddTicket(
            productTraining,
            "2.0",
            windows,
            new DateTimeOffset(2025, 2, 10, 0, 0, 0, TimeSpan.Zero),
            null,
            "L’utilisateur constate que la durée totale de son programme d’entraînement est incorrecte lorsqu’il ajoute plusieurs exercices avec des temps de repos. Le programme semble additionner la durée des exercices, mais ne prend pas toujours en compte les temps de repos configurés.",
            null,
            context
        );
    }
    private static void Ticket8(ApplicationDbContext context)
    {
        AddTicket(
            productTrader,
            "1.3",
            macos,
            new DateTimeOffset(2025, 2, 14, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2025, 2, 17, 0, 0, 0, TimeSpan.Zero),
            "L’utilisateur indique que les noms de certaines actions sont coupés dans la liste de suivi lorsqu’ils sont particulièrement longs. Il ne peut donc pas distinguer facilement plusieurs entreprises dont les noms commencent de la même manière.",
            "Le problème provenait d’une largeur fixe appliquée aux éléments de la liste. L’interface a été adaptée pour afficher les noms sur plusieurs lignes lorsque cela est nécessaire, sans masquer les informations importantes.",
            context
        );
    }
    private static void Ticket9(ApplicationDbContext context)
    {
        AddTicket(
            productAnxiety,
            "1.1",
            android,
            new DateTimeOffset(2025, 2, 19, 0, 0, 0, TimeSpan.Zero),
            null,
            "L’utilisateur explique que les exercices de respiration guidée s’interrompent lorsque le téléphone reçoit un appel ou qu’une autre application passe temporairement au premier plan. Lorsqu’il revient dans le programme, le minuteur repart parfois de zéro.",
            null,
            context
        );
    }
    private static void Ticket10(ApplicationDbContext context)
    {
        AddTicket(
            productMaster,
            "2.0",
            macos,
            new DateTimeOffset(2025, 2, 23, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2025, 2, 27, 0, 0, 0, TimeSpan.Zero),
            "L’utilisateur signale que l’exportation de son portefeuille au format CSV produit un fichier dans lequel les montants contenant des décimales sont mal interprétés par son tableur. Certaines valeurs sont considérées comme du texte au lieu d’être reconnues comme des nombres.",
            "L’équipe a identifié une incompatibilité entre le séparateur décimal utilisé par l’application et les paramètres régionaux du système. L’exportation a été adaptée pour gérer correctement les paramètres régionaux et produire un fichier CSV exploitable dans les tableurs courants.",
            context
        );
    }
    private static void Ticket11(ApplicationDbContext context)
    {
        AddTicket(
            productTraining,
            "1.0",
            linux,
            new DateTimeOffset(2025, 3, 3, 0, 0, 0, TimeSpan.Zero),
            null,
            "L’utilisateur indique que les caractères accentués présents dans le nom de ses exercices personnalisés sont remplacés par des symboles incorrects après l’enregistrement. Le problème apparaît notamment lorsqu’il utilise des noms contenant des accents ou des caractères spéciaux.",
            null,
            context
        );
    }
    private static void Ticket12(ApplicationDbContext context)
    {
        AddTicket(
            productTrader,
            "1.1",
            windows,
            new DateTimeOffset(2025, 3, 7, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2025, 3, 11, 0, 0, 0, TimeSpan.Zero),
            "L’utilisateur signale que le filtre permettant d’afficher uniquement les actions d’un secteur donné ne fonctionne plus après avoir changé plusieurs fois de catégorie. La liste continue d’afficher des actions appartenant à d’autres secteurs.",
            "L’équipe a découvert que les anciens critères de filtrage n’étaient pas correctement supprimés lors d’un changement de catégorie. La logique de filtrage a été corrigée afin que chaque nouvelle sélection remplace les critères précédents.",
            context
        );
    }
    private static void Ticket13(ApplicationDbContext context)
    {
        AddTicket(
            productAnxiety,
            "1.0",
            windows,
            new DateTimeOffset(2025, 3, 12, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2025, 3, 16, 0, 0, 0, TimeSpan.Zero),
            "L’utilisateur explique que ses notes personnelles associées aux exercices disparaissent après avoir modifié le titre d’un exercice. Les exercices restent présents, mais les commentaires qu’il avait rédigés ne sont plus visibles.",
            "L’analyse a montré que la modification du titre déclenchait une sauvegarde qui ne reprenait pas les notes existantes. Le processus de mise à jour a été corrigé pour conserver toutes les données associées à l’exercice.",
            context
        );
    }
    private static void Ticket14(ApplicationDbContext context)
    {
        AddTicket(
            productMaster,
            "2.1",
            android,
            new DateTimeOffset(2025, 3, 18, 0, 0, 0, TimeSpan.Zero),
            null,
            "L’utilisateur indique que l’application se ferme inopinément lorsqu’il consulte l’historique de ses investissements sur plusieurs années. Le problème semble surtout apparaître lorsque le portefeuille contient un grand nombre de transactions.",
            null,
            context
        );
    }
    private static void Ticket15(ApplicationDbContext context)
    {
        AddTicket(
            productTrader,
            "1.2",
            windowsMobile,
            new DateTimeOffset(2025, 3, 21, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2025, 3, 25, 0, 0, 0, TimeSpan.Zero),
            "L’utilisateur constate que les cours des actions affichés dans les favoris ne se mettent plus à jour après le retour d’une période sans connexion Internet. Les cours restent figés même lorsque le téléphone retrouve une connexion réseau.",
            "Le mécanisme de reconnexion ne relançait pas correctement la récupération des cours après une perte de réseau. L’équipe a corrigé la détection du rétablissement de la connexion et ajouté une nouvelle tentative de synchronisation automatique.",
            context
        );
    }
    private static void Ticket16(ApplicationDbContext context)
    {
        AddTicket(
            productTraining,
            "1.1",
            macos,
            new DateTimeOffset(2025, 3, 28, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2025, 4, 2, 0, 0, 0, TimeSpan.Zero),
            "L’utilisateur signale que les séances récurrentes qu’il programme chaque semaine sont ajoutées deux fois à son calendrier après la modification de leur horaire. Il doit supprimer manuellement les séances en double.",
            "L’équipe a identifié un problème dans la gestion des événements récurrents lors de la modification d’une séance. La mise à jour a été corrigée afin de modifier l’événement existant au lieu d’en créer un nouveau.",
            context
        );
    }
    private static void Ticket17(ApplicationDbContext context)
    {
        AddTicket(
            productTrader,
            "1.3",
            ios,
            new DateTimeOffset(2025, 4, 4, 0, 0, 0, TimeSpan.Zero),
            null,
            "L’utilisateur explique que les graphiques boursiers deviennent difficiles à lire lorsqu’il utilise le zoom. Après plusieurs agrandissements successifs, les dates affichées sur l’axe horizontal se chevauchent et certaines valeurs disparaissent de l’écran.",
            null,
            context
        );
    }
    private static void Ticket18(ApplicationDbContext context)
    {
        AddTicket(
            productMaster,
            "2.0",
            android,
            new DateTimeOffset(2025, 4, 9, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2025, 4, 13, 0, 0, 0, TimeSpan.Zero),
            "L’utilisateur signale que la fonction de recherche d’investissement ne trouve pas une entreprise lorsque son nom contient un accent ou lorsqu’il saisit le nom en minuscules. La recherche fonctionne uniquement avec certaines variantes exactes du nom.",
            "L’équipe a modifié le système de recherche pour qu’il ne tienne pas compte des majuscules et des minuscules et qu’il normalise les caractères accentués. Les utilisateurs peuvent désormais retrouver les entreprises sans respecter exactement leur orthographe dans la recherche.",
            context
        );
    }
    private static void Ticket19(ApplicationDbContext context)
    {
        AddTicket(
            productAnxiety,
            "1.1",
            ios,
            new DateTimeOffset(2025, 4, 15, 0, 0, 0, TimeSpan.Zero),
            null,
            "L’utilisateur indique que les rappels quotidiens apparaissent à une heure différente de celle qu’il a configurée. Le problème survient après un voyage dans un autre fuseau horaire ou après un changement manuel de l’heure du téléphone.",
            null,
            context
        );
    }
    private static void Ticket20(ApplicationDbContext context)
    {
        AddTicket(
            productTraining,
            "2.0",
            android,
            new DateTimeOffset(2025, 4, 20, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2025, 4, 24, 0, 0, 0, TimeSpan.Zero),
            "L’utilisateur signale que le compteur de répétitions d’un exercice ne conserve pas sa dernière valeur lorsqu’il verrouille son téléphone pendant une séance. À la reprise, le compteur revient parfois à zéro, ce qui l’oblige à saisir à nouveau ses résultats.",
            "L’équipe a identifié que l’état de la séance n’était pas sauvegardé lors du passage de l’application en arrière-plan. Une sauvegarde intermédiaire a été ajoutée afin de conserver le nombre de répétitions et la progression de la séance lors d’une interruption.",
            context
        );
    }
    private static void Ticket21(ApplicationDbContext context)
    {
        AddTicket(
            productTrader,
            "1.2",
            linux,
            new DateTimeOffset(2025, 4, 26, 0, 0, 0, TimeSpan.Zero),
            null,
            "L’utilisateur indique que l’historique des transactions ne s’affiche pas correctement lorsqu’il sélectionne une période personnalisée. Certaines transactions réalisées pendant les premiers jours de la période sélectionnée sont absentes, alors qu’elles apparaissent lorsque l’utilisateur consulte l’historique complet.",
            null,
            context
        );
    }
    private static void Ticket22(ApplicationDbContext context)
    {
        AddTicket(
            productAnxiety,
            "1.0",
            android,
            new DateTimeOffset(2025, 5, 2, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2025, 5, 6, 0, 0, 0, TimeSpan.Zero),
            "L’utilisateur signale que les progrès réalisés dans les exercices de préparation aux situations sociales ne sont pas correctement représentés dans les statistiques. Certains exercices terminés restent marqués comme incomplets, même après avoir été validés.",
            "L’équipe a découvert que la validation d’un exercice n’actualisait pas toujours les statistiques associées. Le système de suivi de progression a été corrigé pour mettre à jour les résultats immédiatement après la validation d’un exercice.",
            context
        );
    }
    private static void Ticket23(ApplicationDbContext context)
    {
        AddTicket(
            productTrader,
            "1.1",
            macos,
            new DateTimeOffset(2025, 5, 8, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2025, 5, 12, 0, 0, 0, TimeSpan.Zero),
            "L’utilisateur explique que la fenêtre de confirmation d’achat reste ouverte après la validation d’une transaction. Même si l’achat a bien été effectué, il pense que l’opération a échoué et risque de recommencer la transaction.",
            "L’équipe a identifié un problème dans la fermeture de la fenêtre après réception de la confirmation du serveur. L’interface a été corrigée pour fermer automatiquement la fenêtre lorsque la transaction est confirmée et afficher un message indiquant que l’achat a été effectué.",
            context
        );
    }
    private static void Ticket24(ApplicationDbContext context)
    {
        AddTicket(
            productTraining,
            "1.1",
            windowsMobile,
            new DateTimeOffset(2025, 5, 14, 0, 0, 0, TimeSpan.Zero),
            null,
            "L’utilisateur indique que les notifications de rappel d’entraînement ne s’affichent pas lorsque le téléphone est en mode économie d’énergie. Les séances sont toujours présentes dans le calendrier, mais aucun rappel n’est reçu à l’heure prévue.",
            null,
            context
        );
    }
    private static void Ticket25(ApplicationDbContext context)
    {
        AddTicket(
            productMaster,
            "2.1",
            ios,
            new DateTimeOffset(2025, 5, 20, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2025, 5, 25, 0, 0, 0, TimeSpan.Zero),
            "L’utilisateur signale que le montant total de ses investissements affiché sur le tableau de bord diffère du montant obtenu en additionnant les valeurs individuelles de ses placements. L’écart apparaît après la modification de la devise d’affichage du portefeuille.",
            "L’équipe a identifié que certains montants étaient convertis dans la nouvelle devise pour les lignes individuelles, mais pas pour le calcul du total. Le calcul a été corrigé afin que toutes les valeurs utilisent la même devise avant d’être additionnées. Des tests ont également été ajoutés pour vérifier les changements de devise.",
            context
        );
    }
}