using System.Collections.Generic;

namespace Blogfolio.Web.ViewModels
{
    public class ProjectListModel : BaseModel
    {
        public List<ProjectItemModel> Projects;
    }

    public class ProjectItemModel : BaseModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
    }
}