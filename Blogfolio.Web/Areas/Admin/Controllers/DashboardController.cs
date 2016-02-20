using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Blogfolio.Models;
using Blogfolio.Models.Blog;
using Blogfolio.Models.Library;
using Blogfolio.Models.Portfolio;
using Blogfolio.Web.Areas.Admin.ViewModels;
using Microsoft.AspNet.Identity;

namespace Blogfolio.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private const string MediaDirectory = "/assets/images/media/";
        private const string ThumbDirectory = "/assets/uploads/thumbs/";
        private const string UploadDirectory = "/assets/uploads/";

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Post

        [HttpGet]
        public ActionResult Posts()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddPost()
        {
            return View("AddUpdatePost", MapPostEntityToModel(new Post()
            {
                PostId = Guid.NewGuid()
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPost(PostEditModel model, Guid[] selectedCategories)
        {
            if (ModelState.IsValid)
            {
                if (selectedCategories != null)
                {
                    try
                    {
                        // Get the current user (No need to check if null because of the Authorize attribute)
                        var currentUser =
                            await _unitOfWork.UserRepository.FindByUserNameAsync(User.Identity.GetUserName());

                        // Create post entity
                        var post = MapPostModelToEntity(new Post(), model);

                        if (await IsPostSlugExists(model.Slug, post.PostId))
                        {
                            // Populate unmapped properties
                            post.Slug = model.Slug;
                            post.User = currentUser;
                            post.DateCreated = DateTime.UtcNow;

                            // Categorize
                            foreach (var categoryId in selectedCategories)
                            {
                                var category = await _unitOfWork.CategoryRepository.FindByIdAsync(categoryId);
                                if (category != null && !post.Categories.Contains(category))
                                {
                                    post.Categories.Add(category);
                                }
                            }

                            // Add
                            _unitOfWork.PostRepository.Add(post);
                            await _unitOfWork.SaveChangesAsync();

                            return RedirectToAction("Posts", "Dashboard");
                        }
                        ModelState.AddModelError("slug", "Slug must be unique");
                    }
                    catch
                    {
                        ModelState.AddModelError("", "An error occurred while adding the post");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Category is required");
                }
            }

            return View("AddUpdatePost", model);
        }

        [HttpGet]
        public async Task<ActionResult> UpdatePost(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = await _unitOfWork.PostRepository.FindByIdAsync(id);
            if (post == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View("AddUpdatePost", MapPostEntityToModel(post));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePost(PostEditModel model, Guid[] selectedCategories)
        {
            if (ModelState.IsValid)
            {
                if (selectedCategories != null)
                {
                    try
                    {
                        // Get the post entity
                        var post = MapPostModelToEntity(await _unitOfWork.PostRepository.FindByIdAsync(model.PostId),
                            model);

                        if (await IsPostSlugExists(model.Slug, post.PostId))
                        {
                            // Update unmapped properties
                            post.Slug = model.Slug;
                            post.DateModified = DateTime.UtcNow;

                            // Re categorize
                            post.Categories.Clear();
                            foreach (var categoryId in selectedCategories)
                            {
                                var category = await _unitOfWork.CategoryRepository.FindByIdAsync(categoryId);
                                if (category != null && !post.Categories.Contains(category))
                                {
                                    post.Categories.Add(category);
                                }
                            }

                            // Update
                            _unitOfWork.PostRepository.Update(post);
                            await _unitOfWork.SaveChangesAsync();

                            return RedirectToAction("Posts", "Dashboard");
                        }
                        ModelState.AddModelError("slug", "Slug must be unique");
                    }
                    catch
                    {
                        ModelState.AddModelError("", "An error occurred while updating the post");
                    }
                }
                else
                {
                    ModelState.AddModelError("category", "Category is required");
                }
            }

            return View("AddUpdatePost", model);
        }

        [HttpPost]
        public async Task<ActionResult> DeletePost(Guid? id)
        {
            if (!Request.IsAjaxRequest() || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var post = await _unitOfWork.PostRepository.FindByIdAsync(id);
                if (post != null)
                {
                    _unitOfWork.PostRepository.Remove(post);
                    await _unitOfWork.SaveChangesAsync();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult> BulkDeletePost(Guid[] ids)
        {
            if (!Request.IsAjaxRequest() || ids.Count() == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                foreach (var id in ids)
                {
                    var post = await _unitOfWork.PostRepository.FindByIdAsync(id);
                    if (post != null)
                    {
                        _unitOfWork.PostRepository.Remove(post);
                    }
                }
                await _unitOfWork.SaveChangesAsync();

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult> PopulatePosts()
        {
            if (!Request.IsAjaxRequest())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var posts = await _unitOfWork.PostRepository.GetAllAsync();
            var json = posts.OrderByDescending(p => p.DateCreated).Select(p => new
            {
                postid = p.PostId,
                title = p.Title,
                summary = p.Summary,
                author = p.User.Name,
                categories = p.Categories.Select(c => new
                {
                    name = c.Name
                }),
                commentsenabled = p.CommentsEnabled,
                status = p.Status.ToString(),
                created = p.DateCreated,
                updated = p.DateModified
            });

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> PopulatePostCategories(Guid? id)
        {
            if (!Request.IsAjaxRequest())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = await _unitOfWork.PostRepository.FindByIdAsync(id) ?? new Post();

            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            var postCategories = new HashSet<Guid>(post.Categories?.Select(c => c.CategoryId) ?? new Guid[] {});
            var json = categories.Select(c => new
            {
                id = c.CategoryId,
                name = c.Name,
                ischecked = postCategories.Contains(c.CategoryId),
                postcount = c.Posts.Count()
            });

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Category

        [HttpGet]
        public ActionResult Categories()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            if (!Request.IsAjaxRequest())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return PartialView("_AddUpdateCategory", MapCategoryEntityToModel(new Category()
            {
                CategoryId = Guid.NewGuid()
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCategory(CategoryEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Create category entity
                    var category = MapCategoryModelToEntity(new Category(), model);

                    if (await IsCategorySlugExists(model.Slug, category.CategoryId))
                    {
                        // Populate unmapped properties
                        category.Slug = model.Slug;
                        category.DateCreated = DateTime.UtcNow;

                        // Add
                        _unitOfWork.CategoryRepository.Add(category);
                        await _unitOfWork.SaveChangesAsync();

                        return Json(new {success = true});
                    }
                    ModelState.AddModelError("slug", "Slug must be unique");
                }
                catch
                {
                    ModelState.AddModelError("", "An error occurred while adding the category");
                }
            }
            return PartialView("_AddUpdateCategory", model);
        }

        [HttpGet]
        public async Task<ActionResult> UpdateCategory(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = await _unitOfWork.CategoryRepository.FindByIdAsync(id);
            if (category == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return PartialView("_AddUpdateCategory", MapCategoryEntityToModel(category));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateCategory(CategoryEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Get the category entity
                    var category =
                        MapCategoryModelToEntity(await _unitOfWork.CategoryRepository.FindByIdAsync(model.CategoryId),
                            model);

                    if (await IsCategorySlugExists(model.Slug, category.CategoryId))
                    {
                        // Update unmapped properties
                        category.Slug = model.Slug;
                        category.DateModified = DateTime.UtcNow;

                        // Update
                        _unitOfWork.CategoryRepository.Update(category);
                        await _unitOfWork.SaveChangesAsync();

                        return Json(new {success = true});
                    }
                    ModelState.AddModelError("slug", "Slug must be unique");
                }
                catch
                {
                    ModelState.AddModelError("", "An error occurred while updating the category");
                }
            }

            return PartialView("_AddUpdateCategory", model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCategory(Guid? id)
        {
            if (!Request.IsAjaxRequest() || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var category = await _unitOfWork.CategoryRepository.FindByIdAsync(id);
                if (category != null)
                {
                    foreach (var post in category.Posts.ToList().Where(post => post.Categories.Count == 1))
                    {
                        _unitOfWork.PostRepository.Remove(post);
                    }

                    _unitOfWork.CategoryRepository.Remove(category);
                    await _unitOfWork.SaveChangesAsync();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult> BulkDeleteCategory(Guid[] ids)
        {
            if (!Request.IsAjaxRequest() || ids.Count() == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                foreach (var id in ids)
                {
                    var category = await _unitOfWork.CategoryRepository.FindByIdAsync(id);
                    if (category != null)
                    {
                        foreach (var post in category.Posts.ToList().Where(post => post.Categories.Count == 1))
                        {
                            _unitOfWork.PostRepository.Remove(post);
                        }
                        _unitOfWork.CategoryRepository.Remove(category);
                    }
                }
                await _unitOfWork.SaveChangesAsync();

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult> PopulateCategories()
        {
            if (!Request.IsAjaxRequest())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            var json = categories.OrderByDescending(c => c.Posts.Count).Select(c => new
            {
                categoryid = c.CategoryId,
                name = c.Name,
                slug = c.Slug,
                totalposts = c.Posts.Count,
                created = c.DateCreated,
                updated = c.DateModified
            });

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Project

        public ActionResult Projects()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddProject()
        {
            return View("AddUpdateProject", MapProjectEntityToModel(new Project()
            {
                ProjectId = Guid.NewGuid(),
                Image = "/assets/images/placeholder.png"
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProject(ProjectEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Create project entity
                    var project = MapProjectModelToEntity(new Project(), model);

                    if (await IsProjectSlugExists(model.Slug, project.ProjectId))
                    {
                        // Populate unmapped properties
                        project.Slug = model.Slug;
                        project.DateCreated = DateTime.UtcNow;

                        // Add
                        _unitOfWork.ProjectRepository.Add(project);
                        await _unitOfWork.SaveChangesAsync();

                        return RedirectToAction("Projects", "Dashboard");
                    }
                    ModelState.AddModelError("slug", "Slug must be unique");
                }
                catch
                {
                    ModelState.AddModelError("", "An error occurred while adding the project");
                }
            }

            return View("AddUpdateProject", model);
        }

        [HttpGet]
        public async Task<ActionResult> UpdateProject(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = await _unitOfWork.ProjectRepository.FindByIdAsync(id);
            if (project == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View("AddUpdateProject", MapProjectEntityToModel(project));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateProject(ProjectEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Get the project entity
                    var project =
                        MapProjectModelToEntity(await _unitOfWork.ProjectRepository.FindByIdAsync(model.ProjectId),
                            model);

                    if (await IsProjectSlugExists(model.Slug, project.ProjectId))
                    {
                        // Update unmapped properties
                        project.Slug = model.Slug;
                        project.DateModified = DateTime.UtcNow;

                        // Update
                        _unitOfWork.ProjectRepository.Update(project);
                        await _unitOfWork.SaveChangesAsync();

                        return RedirectToAction("Projects", "Dashboard");
                    }
                    ModelState.AddModelError("slug", "Slug must be unique.");
                }
                catch
                {
                    ModelState.AddModelError("", "An error occurred while updating the project.");
                }
            }

            return View("AddUpdateProject", model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteProject(Guid? id)
        {
            if (!Request.IsAjaxRequest() || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var project = await _unitOfWork.ProjectRepository.FindByIdAsync(id);
                if (project != null)
                {
                    _unitOfWork.ProjectRepository.Remove(project);
                    await _unitOfWork.SaveChangesAsync();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult> BulkDeleteProject(Guid[] ids)
        {
            if (!Request.IsAjaxRequest() || ids.Count() == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                foreach (var id in ids)
                {
                    var project = await _unitOfWork.ProjectRepository.FindByIdAsync(id);
                    if (project != null)
                    {
                        _unitOfWork.ProjectRepository.Remove(project);
                    }
                }
                await _unitOfWork.SaveChangesAsync();

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult> PopulateProjects()
        {
            if (!Request.IsAjaxRequest())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var projects = await _unitOfWork.ProjectRepository.GetAllAsync();
            var json = projects.OrderByDescending(p => p.DateCreated).Select(p => new
            {
                projectid = p.ProjectId,
                name = p.Name,
                image = p.Image,
                description = p.Description,
                status = p.Status.ToString(),
                created = p.DateCreated,
                updated = p.DateModified
            });

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Media

        [HttpGet]
        public ActionResult MediaLibrary()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetMedias()
        {
            if (!Request.IsAjaxRequest())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var mediaFiles = await _unitOfWork.MediaRepository.GetAllAsync();
            var json = mediaFiles.OrderByDescending(m => m.DateCreated).Select(m => new
            {
                mediaid = m.MediaId,
                name = m.Name,
                path = m.Path
            });

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UploadMedia()
        {
            try
            {
                var file = Request.Files[0];
                if (file != null)
                {
                    var fileName = GenerateUniqueFileName(file.FileName);

                    var uploadPath = Path.Combine(UploadDirectory, fileName);
                    var thumbPath = GetThumbnail(file.ContentType, fileName);

                    // Create media entity
                    var media = new Media()
                    {
                        MediaId = Guid.NewGuid(),
                        Name = file.FileName,
                        Path = uploadPath,
                        ThumbPath = thumbPath,
                        Type = file.ContentType,
                        Size = file.ContentLength.ToString(),
                        DateCreated = DateTime.UtcNow
                    };

                    // Save media file to disk
                    if (await SaveFile(file, media))
                    {
                        _unitOfWork.MediaRepository.Add(media);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    return Json(new
                    {
                        success = true,
                        id = media.MediaId,
                        path = media.Path,
                        thumbpath = media.ThumbPath,
                        size = media.Size,
                        type = media.Type,
                        created = media.DateCreated
                    });
                }
                return Json(new {success = false});
            }
            catch
            {
                return Json(new {success = false});
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteMedia(Guid? id)
        {
            if (!Request.IsAjaxRequest() || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var media = await _unitOfWork.MediaRepository.FindByIdAsync(id);
                if (media != null)
                {
                    await DeleteFileFromDiskAsync(media.Path);

                    if (!media.ThumbPath.StartsWith(MediaDirectory))
                    {
                        await DeleteFileFromDiskAsync(media.ThumbPath);
                    }

                    _unitOfWork.MediaRepository.Remove(media);
                    await _unitOfWork.SaveChangesAsync();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult> BulkDeleteMedia(Guid[] ids)
        {
            if (!Request.IsAjaxRequest() || ids.Count() == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                foreach (var id in ids)
                {
                    var media = await _unitOfWork.MediaRepository.FindByIdAsync(id);
                    if (media != null)
                    {
                        await DeleteFileFromDiskAsync(media.Path);

                        if (!media.ThumbPath.StartsWith(MediaDirectory))
                        {
                            await DeleteFileFromDiskAsync(media.ThumbPath);
                        }

                        _unitOfWork.MediaRepository.Remove(media);
                    }
                }
                await _unitOfWork.SaveChangesAsync();

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult> PopulateMedias()
        {
            if (!Request.IsAjaxRequest())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var medias = await _unitOfWork.MediaRepository.GetAllAsync();
            var json = medias.OrderByDescending(m => m.DateCreated).Select(m => new
            {
                mediaid = m.MediaId,
                name = m.Name,
                path = m.Path,
                thumbpath = m.ThumbPath,
                size = m.Size,
                type = m.Type,
                created = m.DateCreated
            });

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region MediaBrowser

        [HttpGet]
        public async Task<ActionResult> GetFilesByType(string type)
        {
            if (!Request.IsAjaxRequest() || string.IsNullOrWhiteSpace(type))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var medias = await _unitOfWork.MediaRepository.GetAllAsync();
            var json = medias.Where(m => m.Type.StartsWith(type))
                .OrderByDescending(i => i.DateCreated).Select(i => new
                {
                    mediaid = i.MediaId,
                    name = i.Name,
                    path = i.Path,
                    thumbpath = i.ThumbPath,
                    type = i.Type
                });

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Maps post entity to post edit model
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>A <see cref="PostEditModel" /></returns>
        private PostEditModel MapPostEntityToModel(Post entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = new PostEditModel
            {
                PostId = entity.PostId,
                Title = entity.Title,
                Summary = entity.Summary,
                Content = entity.Content,
                Slug = entity.Slug,
                Status = entity.Status,
                CommentsEnabled = entity.CommentsEnabled
            };

            return model;
        }

        /// <summary>
        /// Maps post edit model to post entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        /// <returns>A <see cref="Post" /></returns>
        private Post MapPostModelToEntity(Post entity, PostEditModel model)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            entity.PostId = model.PostId;
            entity.Title = model.Title;
            entity.Summary = model.Summary;
            entity.Content = model.Content;
            entity.Status = model.Status;
            entity.CommentsEnabled = model.CommentsEnabled;

            return entity;
        }

        /// <summary>
        /// Maps category entity to category edit model
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>A <see cref="CategoryEditModel" /></returns>
        private CategoryEditModel MapCategoryEntityToModel(Category entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = new CategoryEditModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Slug = entity.Slug
            };

            return model;
        }

        /// <summary>
        /// Maps category edit model to category entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        /// <returns>A <see cref="Category" /></returns>
        private Category MapCategoryModelToEntity(Category entity, CategoryEditModel model)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            entity.CategoryId = model.CategoryId;
            entity.Name = model.Name;

            return entity;
        }

        /// <summary>
        /// Maps project entity to project edit model
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private ProjectEditModel MapProjectEntityToModel(Project entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = new ProjectEditModel()
            {
                ProjectId = entity.ProjectId,
                Name = entity.Name,
                Image = entity.Image,
                Description = entity.Description,
                Slug = entity.Slug,
                Status = entity.Status
            };

            return model;
        }

        /// <summary>
        /// Maps project edit model to project entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private Project MapProjectModelToEntity(Project entity, ProjectEditModel model)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            entity.ProjectId = model.ProjectId;
            entity.Name = model.Name;
            entity.Image = model.Image;
            entity.Description = model.Description;
            entity.Status = model.Status;

            return entity;
        }

        /// <summary>
        /// Checks if unique post slug field is valid
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="postId"></param>
        /// <returns>A <see cref="bool" /></returns>
        private async Task<bool> IsPostSlugExists(string slug, Guid postId)
        {
            var posts = await _unitOfWork.PostRepository.GetAllAsync();
            var post = posts.FirstOrDefault(p => p.PostId == postId);
            if (post != null && post.Slug == slug)
            {
                return posts.Count(p => p.Slug == slug) <= 1;
            }
            return posts.Count(p => p.Slug == slug) == 0;
        }

        /// <summary>
        /// Checks if unique category slug field is valid
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="categoryId"></param>
        /// <returns>A <see cref="bool" /></returns>
        private async Task<bool> IsCategorySlugExists(string slug, Guid categoryId)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            var category = categories.FirstOrDefault(c => c.CategoryId == categoryId);
            if (category != null && category.Slug == slug)
            {
                return categories.Count(c => c.Slug == slug) <= 1;
            }
            return categories.Count(c => c.Slug == slug) == 0;
        }

        /// <summary>
        /// Checks if unique project slug field is valid
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="projectId"></param>
        /// <returns>A <see cref="bool" /></returns>
        private async Task<bool> IsProjectSlugExists(string slug, Guid projectId)
        {
            var projects = await _unitOfWork.ProjectRepository.GetAllAsync();
            var project = projects.FirstOrDefault(p => p.ProjectId == projectId);
            if (project != null && project.Slug == slug)
            {
                return projects.Count(p => p.Slug == slug) <= 1;
            }
            return projects.Count(p => p.Slug == slug) == 0;
        }

        /// <summary>
        /// Checks and creates server directories
        /// </summary>
        /// <param name="virtualPaths"></param>
        private static void CheckCreateDirectories(IEnumerable<string> virtualPaths)
        {
            foreach (var path in virtualPaths)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        /// <summary>
        /// Asynchronously saves given file to disk
        /// </summary>
        /// <param name="file"></param>
        /// <param name="media"></param>
        /// <returns>A <see cref="bool"/></returns>
        private async Task<bool> SaveFile(HttpPostedFileBase file, Media media)
        {
            try
            {
                if (file == null)
                    throw new ArgumentNullException(nameof(file));

                if (media == null)
                    throw new ArgumentNullException(nameof(media));

                // Check if required directories are present
                CheckCreateDirectories(new[]
                {
                    Server.MapPath(UploadDirectory),
                    Server.MapPath(ThumbDirectory)
                });

                // Save with a thumbnail if the file is an image file
                if (file.ContentType.StartsWith("image"))
                {
                    var result = SaveImage(file.InputStream, Server.MapPath(media.Path), 1280) &&
                                 SaveImage(file.InputStream, Server.MapPath(media.ThumbPath), 300);
                    return result;
                }

                // Save the file
                using (var fs = new FileStream(Server.MapPath(media.Path), FileMode.Create, FileAccess.ReadWrite))
                {
                    // Create empty buffer
                    var buffer = new byte[file.InputStream.Length];

                    // Store the file stream in the buffer
                    await file.InputStream.ReadAsync(buffer, 0, buffer.Length);

                    // Write the buffer to the disk
                    await fs.WriteAsync(buffer, 0, buffer.Length);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a resized bitmap from the given stream. Resizes the image by 
        /// creating an aspect ratio safe image. Image is sized to the larger size of width
        /// height and then smaller size is adjusted by aspect ratio.
        /// 
        /// Credits: https://github.com/RickStrahl/Westwind.plUploadHandler
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="outputFilename"></param>
        /// <param name="height"></param>
        /// <returns>A <see cref="bool"/></returns>
        private static bool SaveImage(Stream fileStream, string outputFilename, int height)
        {
            Bitmap bmp = null;
            Bitmap bmpOut = null;
            Graphics g = null;

            try
            {
                bmp = new Bitmap(fileStream);
                var format = bmp.RawFormat;

                var newWidth = 0;
                var newHeight = 0;

                if (bmp.Height < height)
                {
                    bmp.Save(outputFilename);
                    return true;
                }

                var ratio = (decimal) height/bmp.Height;
                newHeight = height;
                newWidth = Convert.ToInt32(bmp.Width*ratio);

                bmpOut = new Bitmap(newWidth, newHeight);
                g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                g.DrawImage(bmp, 0, 0, newWidth, newHeight);

                bmpOut.Save(outputFilename, format);
            }
            catch
            {
                return false;
            }
            finally
            {
                bmp?.Dispose();
                bmpOut?.Dispose();
                g?.Dispose();
            }

            return true;
        }

        /// <summary>
        /// Generates a unique name for a file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>A <see cref="string"/></returns>
        private static string GenerateUniqueFileName(string fileName)
        {
            return string.Format("UL_{0}{1}", Guid.NewGuid().ToString("N").ToUpper(),
                Path.GetExtension(fileName));
        }

        /// <summary>
        /// Asynchronously deletes a directory from the disk
        /// </summary>
        /// <param name="directory"></param>
        /// <returns>A <see cref="bool"/></returns>
        private async Task<bool> DeleteDirectoryFromDiskAsync(string directory)
        {
            try
            {
                if (directory == null)
                    throw new ArgumentNullException(nameof(directory));

                var serverDirectory = Server.MapPath(directory);
                if (Directory.Exists(serverDirectory))
                {
                    await Task.Factory.StartNew(() => Directory.Delete(serverDirectory, true));
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Asynchronously deletes a file from the disk
        /// </summary>
        /// <param name="file"></param>
        /// <returns>A <see cref="bool" /></returns>
        private async Task<bool> DeleteFileFromDiskAsync(string file)
        {
            try
            {
                if (file == null)
                    throw new ArgumentNullException(nameof(file));

                var serverFile = Server.MapPath(file);
                if (System.IO.File.Exists(serverFile))
                {
                    var fInfo = new FileInfo(serverFile);
                    await Task.Factory.StartNew(() => fInfo.Delete());
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns thumbnail path for an image
        /// Returns default thumbnail paths for known mime types
        /// </summary>
        /// <param name="mimeType"></param>
        /// <param name="fileName"></param>
        private string GetThumbnail(string mimeType, string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            if (mimeType == null)
                throw new ArgumentNullException(nameof(mimeType));

            // Default file thumbnail
            var thumbnail = Path.Combine(MediaDirectory, "default.png");

            if (mimeType.StartsWith("image")) // Images
            {
                thumbnail = Path.Combine(ThumbDirectory, fileName);
            }
            if (mimeType == ("application/zip") || mimeType == ("application/x-rar-compressed") ||
                mimeType == ("application/octet-stream")) // Archives
            {
                thumbnail = Path.Combine(MediaDirectory, "archive.png");
            }
            if (mimeType.StartsWith("text")) // Documents
            {
                thumbnail = Path.Combine(MediaDirectory, "text.png");
            }
            if (mimeType.StartsWith("audio")) // Audio files
            {
                thumbnail = Path.Combine(MediaDirectory, "audio.png");
            }
            if (mimeType.StartsWith("video")) // Video files
            {
                thumbnail = Path.Combine(MediaDirectory, "video.png");
            }

            return thumbnail;
        }

        #endregion
    }
}