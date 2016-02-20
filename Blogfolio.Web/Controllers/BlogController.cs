using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blogfolio.Models;
using Blogfolio.Models.Blog;
using Blogfolio.Web.Areas.Admin.ViewModels;
using Blogfolio.Web.ViewModels;
using ErenPinaz.Common.ActionResults;
using ErenPinaz.Common.Helpers;
using ErenPinaz.Common.Services.Settings;
using PagedList;

namespace Blogfolio.Web.Controllers
{
    public class BlogController : BaseController
    {
        private readonly string _siteTitle;
        private readonly string _siteTagline;
        private readonly int _pageSize;
        private readonly int _feedSize;

        public BlogController(IUnitOfWork unitOfWork, ISettingsService settingsService)
            : base(unitOfWork, settingsService)
        {
            var siteSettings = SettingsService.GetByName<SiteSettingsEditModel>("site-settings");

            _siteTitle = siteSettings.Title;
            _siteTagline = siteSettings.Tagline;
            _pageSize = siteSettings.PageSize;
            _feedSize = siteSettings.FeedSize;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int? page)
        {
            var posts = await UnitOfWork.PostRepository.GetPostsAsync();
            return View(new PostListModel()
            {
                Posts = posts.Select(p => new PostItemModel()
                {
                    Title = p.Title,
                    Author = p.User.Name,
                    Content = p.Content,
                    Slug = p.Slug,
                    CommentsEnabled = p.CommentsEnabled,
                    DateCreated = p.DateCreated,
                    Categories = p.Categories.ToList()
                }).ToPagedList(page ?? 1, _pageSize)
            });
        }

        [HttpGet]
        public async Task<ActionResult> Post(int year, int month, string slug)
        {
            var post = await UnitOfWork.PostRepository.GetPostAsync(year, month, slug);
            if (post == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(new PostItemModel()
            {
                Id = post.PostId,
                Title = post.Title,
                Author = post.User.Name,
                Slug = post.Slug,
                Content = post.Content,
                CommentsEnabled = post.CommentsEnabled,
                DateCreated = post.DateCreated,
                Categories = post.Categories.ToList()
            });
        }

        [HttpGet]
        public async Task<ActionResult> Category(string slug, int? page)
        {
            var category = await UnitOfWork.CategoryRepository.GetCategoryAsync(slug);
            if (category == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(new CategoryListModel()
            {
                Name = category.Name,
                Slug = category.Slug,
                Posts = category.Posts
                    .Where(p => p.Status == PostStatus.Published)
                    .OrderByDescending(p => p.DateCreated)
                    .Select(p => new PostItemModel()
                    {
                        Title = p.Title,
                        Author = p.User.Name,
                        Content = p.Content,
                        Slug = p.Slug,
                        CommentsEnabled = p.CommentsEnabled,
                        DateCreated = p.DateCreated,
                        Categories = p.Categories.ToList()
                    }).ToPagedList(page ?? 1, _pageSize)
            });
        }

        [HttpGet]
        public async Task<ActionResult> Feed()
        {
            // Get posts
            var posts = await UnitOfWork.PostRepository.GetPostsAsync();

            // Create feed items
            var feedItems = new List<SyndicationItem>();
            foreach (var post in posts.Take(_feedSize))
            {
                var feedItem = new SyndicationItem
                {
                    Title = SyndicationContent.CreatePlaintextContent(post.Title),
                    Summary = SyndicationContent.CreatePlaintextContent(post.Summary),
                    Content = SyndicationContent.CreateHtmlContent(post.Content),
                    Authors = {new SyndicationPerson(post.User.Name)},
                    Links =
                    {
                        SyndicationLink.CreateAlternateLink(new Uri(Url.QualifiedAction("Post", "Blog", new
                        {
                            year = post.DateCreated.Year,
                            month = post.DateCreated.Month,
                            slug = post.Slug
                        })))
                    },
                    PublishDate = post.DateCreated
                };
                foreach (var category in post.Categories)
                {
                    feedItem.Categories.Add(new SyndicationCategory(category.Name));
                }

                feedItems.Add(feedItem);
            }

            // Create feed
            var feed = new SyndicationFeed
            {
                Title = SyndicationContent.CreatePlaintextContent(_siteTitle),
                Description = SyndicationContent.CreatePlaintextContent(_siteTagline),
                Links = {SyndicationLink.CreateAlternateLink(new Uri(Url.QualifiedAction("Index", "Blog")))},
                Items = feedItems
            };

            // Serve the feed
            return new RssFeedResult(feed);
        }
    }
}