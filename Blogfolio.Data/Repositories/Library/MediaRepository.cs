using Blogfolio.Models.Library;
using Blogfolio.Models.Repositories.Library;

namespace Blogfolio.Data.Repositories.Library
{
    /// <summary>
    /// Entity framework implementation of <see cref="IMediaRepository" />
    /// </summary>
    internal class MediaRepository : Repository<Media>, IMediaRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        internal MediaRepository(BlogfolioContext context)
            : base(context)
        {
        }
    }
}