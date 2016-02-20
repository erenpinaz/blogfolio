using System;
using System.Collections.Generic;
using Blogfolio.Models.Blog;
using PagedList;

namespace Blogfolio.Web.ViewModels
{
    public class PostListModel : BaseModel
    {
        public IPagedList<PostItemModel> Posts;
    }

    public class PostItemModel : BaseModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public bool CommentsEnabled { get; set; }
        public DateTime DateCreated { get; set; }

        public List<Category> Categories { get; set; }
    }

    public class CategoryListModel : BaseModel
    {
        public string Name { get; set; }

        public string Slug { get; set; }

        public IPagedList<PostItemModel> Posts;
    }
}