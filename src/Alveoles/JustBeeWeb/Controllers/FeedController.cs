using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace JustBeeWeb.Controllers;

[ApiController]
[Route("[controller]")]
public class FeedController(ILogger<FeedController> logger) : ControllerBase
{
    private readonly ILogger<FeedController> _logger = logger;

    [HttpGet("rss.xml")]
    [ResponseCache(Duration = 3600)] // Cache for 1 hour
    public IActionResult GetRssFeed()
    {
        try
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var rssXml = GenerateRssFeed(baseUrl);

            return Content(rssXml, "application/rss+xml", Encoding.UTF8);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating RSS feed");
            return StatusCode(500);
        }
    }

    [HttpGet("atom.xml")]
    [ResponseCache(Duration = 3600)] // Cache for 1 hour
    public IActionResult GetAtomFeed()
    {
        try
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var atomXml = GenerateAtomFeed(baseUrl);

            return Content(atomXml, "application/atom+xml", Encoding.UTF8);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating Atom feed");
            return StatusCode(500);
        }
    }

    private string GenerateRssFeed(string baseUrl)
    {
        var rss = new StringBuilder();
        rss.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        rss.AppendLine("<rss version=\"2.0\" xmlns:atom=\"http://www.w3.org/2005/Atom\">");
        rss.AppendLine("<channel>");
        rss.AppendLine($"<title>Plan B Ruche Démocratique - Actualités</title>");
        rss.AppendLine($"<description>Les dernières actualités de l'innovation démocratique citoyenne avec les alvéoles participatives du Plan B</description>");
        rss.AppendLine($"<link>{baseUrl}</link>");
        rss.AppendLine($"<atom:link href=\"{baseUrl}/feed/rss.xml\" rel=\"self\" type=\"application/rss+xml\" />");
        rss.AppendLine($"<language>fr-FR</language>");
        rss.AppendLine($"<lastBuildDate>{DateTime.UtcNow:R}</lastBuildDate>");
        rss.AppendLine($"<ttl>60</ttl>");
        rss.AppendLine($"<image>");
        rss.AppendLine($"  <url>{baseUrl}/img/alveole.png</url>");
        rss.AppendLine($"  <title>Plan B Ruche Démocratique</title>");
        rss.AppendLine($"  <link>{baseUrl}</link>");
        rss.AppendLine($"</image>");

        // Add news items (static for now, could be dynamic from database)
        var newsItems = GetNewsItems(baseUrl);
        foreach (var item in newsItems)
        {
            rss.AppendLine("<item>");
            rss.AppendLine($"  <title><![CDATA[{item.Title}]]></title>");
            rss.AppendLine($"  <description><![CDATA[{item.Description}]]></description>");
            rss.AppendLine($"  <link>{item.Link}</link>");
            rss.AppendLine($"  <guid isPermaLink=\"true\">{item.Link}</guid>");
            rss.AppendLine($"  <pubDate>{item.PublishDate:R}</pubDate>");
            rss.AppendLine($"  <category>Démocratie Participative</category>");
            rss.AppendLine("</item>");
        }

        rss.AppendLine("</channel>");
        rss.AppendLine("</rss>");

        return rss.ToString();
    }

    private string GenerateAtomFeed(string baseUrl)
    {
        var atom = new StringBuilder();
        atom.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        atom.AppendLine("<feed xmlns=\"http://www.w3.org/2005/Atom\">");
        atom.AppendLine($"<title>Plan B Ruche Démocratique - Actualités</title>");
        atom.AppendLine($"<subtitle>Innovation démocratique citoyenne avec alvéoles participatives</subtitle>");
        atom.AppendLine($"<link href=\"{baseUrl}\" />");
        atom.AppendLine($"<link href=\"{baseUrl}/feed/atom.xml\" rel=\"self\" />");
        atom.AppendLine($"<id>{baseUrl}/</id>");
        atom.AppendLine($"<updated>{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}</updated>");

        var newsItems = GetNewsItems(baseUrl);
        foreach (var item in newsItems)
        {
            atom.AppendLine("<entry>");
            var encodedTitle = System.Net.WebUtility.HtmlEncode(item.Title);
            var encodedDescription = System.Net.WebUtility.HtmlEncode(item.Description);
            atom.AppendLine($"  <title>{encodedTitle}</title>");
            atom.AppendLine($"  <link href=\"{item.Link}\" />");
            atom.AppendLine($"  <id>{item.Link}</id>");
            atom.AppendLine($"  <updated>{item.PublishDate:yyyy-MM-ddTHH:mm:ssZ}</updated>");
            atom.AppendLine($"  <summary>{encodedDescription}</summary>");
            atom.AppendLine($"  <category term=\"Démocratie Participative\" />");
            atom.AppendLine("</entry>");
        }

        atom.AppendLine("</feed>");
        return atom.ToString();
    }

    private List<NewsItem> GetNewsItems(string baseUrl)
    {
        // Static news items - in a real application, these would come from a database
        return new List<NewsItem>
     {
            new NewsItem
            {
       Title = "Lancement du Plan B Ruche Démocratique - 10 Septembre 2025",
   Description = "Début de l'action citoyenne avec la mise en place des alvéoles participatives dans toute la France. Un nouveau modèle de démocratie locale voit le jour.",
                Link = $"{baseUrl}#lancement-plan-b",
       PublishDate = new DateTime(2025, 9, 10)
        },
       new NewsItem
            {
     Title = "Première Carte Interactive des Alvéoles Citoyennes",
                Description = "Découvrez notre cartographie interactive permettant de visualiser l'essaimage démocratique en temps réel à travers les territoires français.",
 Link = $"{baseUrl}/MapBee",
    PublishDate = new DateTime(2025, 9, 12)
        },
       new NewsItem
  {
  Title = "Ouverture de la Création d'Alvéoles pour Tous les Citoyens",
           Description = "Chaque citoyen peut désormais créer son alvéole locale et rejoindre le mouvement de démocratie participative. Simple, transparent et efficace.",
         Link = $"{baseUrl}/CreerAlveole",
        PublishDate = new DateTime(2025, 9, 15)
            },
            new NewsItem
            {
       Title = "Conformité RGPD et Protection des Données Citoyennes",
    Description = "Plan B s'engage dans une protection maximale des données personnelles avec une politique de confidentialité conforme au RGPD européen.",
     Link = $"{baseUrl}/Privacy",
     PublishDate = new DateTime(2025, 9, 18)
     },
      new NewsItem
        {
       Title = "Alvéole du Vivant : Droit de Veto Écologique",
                Description = "Les professionnels du vivant obtiennent un droit de veto symbolique sur les décisions impactant l'écosystème. L'expertise terrain prévaut sur les décisions de bureau.",
 Link = $"{baseUrl}#alveole-vivant",
        PublishDate = new DateTime(2025, 9, 20)
       }
        };
    }

    private class NewsItem
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Link { get; set; } = "";
        public DateTime PublishDate { get; set; }
    }
}