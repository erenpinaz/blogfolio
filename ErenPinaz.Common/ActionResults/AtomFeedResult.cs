using System.ServiceModel.Syndication;

/* Credits: https://github.com/benfoster/Fabrik.Common */

namespace ErenPinaz.Common.ActionResults
{
    public class AtomFeedResult : FeedResult
    {
        /// <summary>
        /// Action result for serving atom formatted feeds
        /// </summary>
        /// <param name="feed"></param>
        public AtomFeedResult(SyndicationFeed feed)
            : base(new Atom10FeedFormatter(feed), "application/atom+xml")
        {
        }
    }
}