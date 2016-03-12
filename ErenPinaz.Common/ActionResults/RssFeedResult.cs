using System.ServiceModel.Syndication;

/* Credits: https://github.com/benfoster/Fabrik.Common */

namespace ErenPinaz.Common.ActionResults
{
    public class RssFeedResult : FeedResult
    {
        /// <summary>
        ///     Action result for serving rss formatted feeds
        /// </summary>
        /// <param name="feed"></param>
        public RssFeedResult(SyndicationFeed feed)
            : base(new Rss20FeedFormatter(feed), "application/rss+xml")
        {
        }
    }
}