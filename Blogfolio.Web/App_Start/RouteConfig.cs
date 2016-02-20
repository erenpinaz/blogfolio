using System.Web.Mvc;
using System.Web.Routing;

namespace Blogfolio.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Force lowercase URLs
            routes.LowercaseUrls = true;

            // Route: Project
            routes.MapRoute(
                "Project",
                "portfolio/{slug}",
                new {controller = "Portfolio", action = "Project"});

            // Route: Category
            routes.MapRoute(
                "Category",
                "blog/category/{slug}",
                new {controller = "Blog", action = "Category", page = 1});

            // Route: Category Paged
            routes.MapRoute(
                "CategoryPaged",
                "blog/category/{slug}/page/{page}",
                new {controller = "Blog", action = "Category"},
                new {page = @"\d+"});

            // Route: Post
            routes.MapRoute(
                "Post",
                "blog/post/{year}/{month}/{slug}",
                new {controller = "Blog", action = "Post"},
                new {year = @"\d+", month = @"\d+"});

            // Route: Blog
            routes.MapRoute(
                "Blog",
                "",
                new {controller = "Blog", action = "Index", page = 1});

            // Route: Blog Paged
            routes.MapRoute(
                "BlogPaged",
                "page/{page}",
                new {controller = "Blog", action = "Index"},
                new {page = @"\d+"});

            // Route: Sitemap
            routes.MapRoute(
                "Sitemap",
                "sitemap.xml",
                new {controller = "Sitemap", action = "Index"});

            // Route: Default
            routes.MapRoute(
                "Default",
                "{controller}/{action}",
                new {controller = "Blog", action = "Index"},
                new[] {"Blogfolio.Web.Controllers"}).DataTokens["UseNamespaceFallback"] = false;
        }
    }
}