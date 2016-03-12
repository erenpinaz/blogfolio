using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Mvc;
using System.Xml;

/* Credits: https://github.com/benfoster/Fabrik.Common */

namespace ErenPinaz.Common.ActionResults
{
    public class FeedResult : ActionResult
    {
        /// <summary>
        ///     Base result for atom/rss feeds
        /// </summary>
        /// <param name="formatter"></param>
        /// <param name="contentType"></param>
        public FeedResult(SyndicationFeedFormatter formatter, string contentType)
        {
            Formatter = formatter;
            ContentType = contentType;
            Encoding = Encoding.UTF8;
        }

        /// <summary>
        ///     Output formatter (atom/rss)
        /// </summary>
        public SyndicationFeedFormatter Formatter { get; }

        /// <summary>
        ///     Output content type
        /// </summary>
        public string ContentType { get; }

        /// <summary>
        ///     Output encoding
        /// </summary>
        public Encoding Encoding { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            response.ContentType = ContentType;
            response.ContentEncoding = Encoding;

            using (var writer = new XmlTextWriter(response.Output))
            {
                writer.Formatting = Formatting.Indented;
                Formatter.WriteTo(writer);
            }
        }
    }
}