using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Blogfolio.Models;
using ErenPinaz.Common.ActionResults;
using ErenPinaz.Common.Helpers;
using ErenPinaz.Common.SEO.Sitemap;

namespace Blogfolio.Web.Controllers
{
    public class SitemapController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SitemapController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult Index()
        {
            // Add main sections 
            var sitemapItems = new List<SitemapItem>
            {
                new SitemapItem(Url.QualifiedAction("Index", "Blog"), changeFrequency: SitemapChangeFrequency.Weekly,
                    priority: 1.0),
                new SitemapItem(Url.QualifiedAction("Index", "Portfolio"),
                    changeFrequency: SitemapChangeFrequency.Monthly,
                    priority: 1.0),
                new SitemapItem(Url.QualifiedAction("Index", "About"), changeFrequency: SitemapChangeFrequency.Yearly,
                    priority: 0.4),
                new SitemapItem(Url.QualifiedAction("Index", "Contact"), changeFrequency: SitemapChangeFrequency.Yearly,
                    priority: 0.4)
            };

            // Add posts 
            var posts = _unitOfWork.PostRepository.GetPosts();
            sitemapItems.AddRange(posts.Select(post => new SitemapItem(Url.QualifiedAction("Post", "Blog", new
            {
                year = post.DateCreated.Year,
                month = post.DateCreated.Month,
                slug = post.Slug
            }), changeFrequency: SitemapChangeFrequency.Weekly, priority: 0.9)));

            // Add categories 
            var categories = _unitOfWork.CategoryRepository.GetCategories();
            sitemapItems.AddRange(
                categories.Select(category => new SitemapItem(Url.QualifiedAction("Category", "Blog", new
                {
                    slug = category.Slug
                }), changeFrequency: SitemapChangeFrequency.Weekly, priority: 0.7)));

            // Add projects 
            var projects = _unitOfWork.ProjectRepository.GetProjects();
            sitemapItems.AddRange(
                projects.Select(project => new SitemapItem(Url.QualifiedAction("Project", "Portfolio", new
                {
                    slug = project.Slug
                }), changeFrequency: SitemapChangeFrequency.Yearly, priority: 0.8)));

            // Serve the sitemap 
            return new SitemapResult(sitemapItems);
        }
    }
}