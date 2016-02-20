using Blogfolio.Web.Areas.Admin.ViewModels;

namespace Blogfolio.Web.ViewModels
{
    public class BaseModel
    {
        public SiteSettingsEditModel SiteSettings { get; set; }
        public SocialSettingsEditModel SocialSettings { get; set; }
    }
}