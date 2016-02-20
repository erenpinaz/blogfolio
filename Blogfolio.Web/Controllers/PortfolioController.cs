using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blogfolio.Models;
using Blogfolio.Web.ViewModels;
using ErenPinaz.Common.Services.Settings;

namespace Blogfolio.Web.Controllers
{
    public class PortfolioController : BaseController
    {
        public PortfolioController(IUnitOfWork unitOfWork, ISettingsService settingsService)
            : base(unitOfWork, settingsService)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var projects = await UnitOfWork.ProjectRepository.GetProjectsAsync();
            return View(new ProjectListModel()
            {
                Projects = projects.Select(p => new ProjectItemModel()
                {
                    Name = p.Name,
                    Image = p.Image,
                    Description = p.Description,
                    Slug = p.Slug
                }).ToList()
            });
        }

        [HttpGet]
        public async Task<ActionResult> Project(string slug)
        {
            if (!Request.IsAjaxRequest() || slug == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = await UnitOfWork.ProjectRepository.GetProjectAsync(slug);
            if (project == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return PartialView("_Project", new ProjectItemModel()
            {
                Name = project.Name,
                Image = project.Image,
                Description = project.Description,
                Slug = project.Slug
            });
        }
    }
}