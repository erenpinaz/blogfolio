using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using ErenPinaz.Common.SEO.Sitemap;

/* Credits: https://github.com/benfoster/Fabrik.Common */

namespace ErenPinaz.Common.ActionResults
{
    public class SitemapResult : ActionResult
    {
        private readonly IEnumerable<SitemapItem> _items;
        private readonly ISitemapGenerator _generator;

        /// <summary>
        ///     An action result for generating XML sitemap
        /// </summary>
        /// <param name="items"></param>
        public SitemapResult(IEnumerable<SitemapItem> items) : this(items, new SitemapGenerator())
        {
        }

        /// <summary>
        ///     An action result for generating XML sitemap
        /// </summary>
        /// <param name="items"></param>
        /// <param name="generator"></param>
        public SitemapResult(IEnumerable<SitemapItem> items, ISitemapGenerator generator)
        {
            _items = items;
            _generator = generator;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            response.ContentType = "text/xml";
            response.ContentEncoding = Encoding.UTF8;

            using (var writer = new XmlTextWriter(response.Output))
            {
                writer.Formatting = Formatting.Indented;
                var sitemap = _generator.GenerateSiteMap(_items);

                sitemap.WriteTo(writer);
            }
        }
    }
}