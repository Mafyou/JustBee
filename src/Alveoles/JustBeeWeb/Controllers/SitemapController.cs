using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace JustBeeWeb.Controllers;

[ApiController]
[Route("[controller]")]
public class SitemapController : ControllerBase
{
    private readonly ILogger<SitemapController> _logger;

    public SitemapController(ILogger<SitemapController> logger)
    {
        _logger = logger;
    }

    [HttpGet("sitemap.xml")]
    [ResponseCache(Duration = 86400)] // Cache for 24 hours
    public IActionResult GetSitemap()
    {
        try
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var sitemap = GenerateSitemap(baseUrl);

            return Content(sitemap, "application/xml", Encoding.UTF8);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating sitemap");
            return StatusCode(500);
        }
    }

    [HttpGet("robots.txt")]
    [ResponseCache(Duration = 86400)] // Cache for 24 hours
    public IActionResult GetRobotsTxt()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var robotsTxt = $@"User-agent: *
Allow: /

# Sitemap
Sitemap: {baseUrl}/sitemap/sitemap.xml

# Crawl-delay for respectful crawling
Crawl-delay: 1

# Specific directives for search engines
User-agent: Googlebot
Allow: /

User-agent: Bingbot
Allow: /

# Disallow admin areas if any
Disallow: /api/
Disallow: /_framework/

# Allow important assets
Allow: /css/
Allow: /js/
Allow: /img/
Allow: /lib/";

        return Content(robotsTxt, "text/plain", Encoding.UTF8);
    }

    private string GenerateSitemap(string baseUrl)
    {
        var sitemap = new StringBuilder();
        sitemap.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sitemap.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

        // Main pages with their priorities and change frequencies
        var pages = new[]
       {
            new { Url = "/", Priority = "1.0", ChangeFreq = "daily", LastMod = DateTime.UtcNow },
 new { Url = "/MapBee", Priority = "0.9", ChangeFreq = "daily", LastMod = DateTime.UtcNow },
      new { Url = "/CreerAlveole", Priority = "0.8", ChangeFreq = "weekly", LastMod = DateTime.UtcNow },
  new { Url = "/Privacy", Priority = "0.6", ChangeFreq = "monthly", LastMod = DateTime.UtcNow },
    new { Url = "/VerifierEmail", Priority = "0.5", ChangeFreq = "monthly", LastMod = DateTime.UtcNow },
         new { Url = "/VerifierAlveole", Priority = "0.5", ChangeFreq = "monthly", LastMod = DateTime.UtcNow },
      new { Url = "/MapVille", Priority = "0.7", ChangeFreq = "weekly", LastMod = DateTime.UtcNow },
     new { Url = "/ApiDemo", Priority = "0.4", ChangeFreq = "monthly", LastMod = DateTime.UtcNow }
    };

        foreach (var page in pages)
        {
            sitemap.AppendLine("  <url>");
            sitemap.AppendLine($"    <loc>{baseUrl}{page.Url}</loc>");
            sitemap.AppendLine($" <lastmod>{page.LastMod:yyyy-MM-dd}</lastmod>");
            sitemap.AppendLine($"    <changefreq>{page.ChangeFreq}</changefreq>");
            sitemap.AppendLine($"    <priority>{page.Priority}</priority>");
            sitemap.AppendLine("  </url>");
        }

        // Add important images for image SEO
        var images = new[]
        {
            "/img/alveole.png",
   "/img/slogan_main.png",
    "/img/slogan_fra.png",
      "/img/slogan.png",
    "/img/rubrixcube.png",
            "/img/heart.png"
        };

        foreach (var image in images)
        {
            sitemap.AppendLine("  <url>");
            sitemap.AppendLine($"    <loc>{baseUrl}{image}</loc>");
            sitemap.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
            sitemap.AppendLine("    <changefreq>yearly</changefreq>");
            sitemap.AppendLine("    <priority>0.3</priority>");
            sitemap.AppendLine("  </url>");
        }

        sitemap.AppendLine("</urlset>");

        return sitemap.ToString();
    }
}