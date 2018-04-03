using System.Threading.Tasks;

namespace Stratus.Extensions
{
    public abstract class BaseSiteExtension
    {
        /// <summary>
        /// Returns true if the site handler should apply to the current site.
        /// </summary>
        /// <param name="url">The current site url.</param>
        /// <returns>Bool if the filter should apply to the current site.</returns>
        public abstract bool Filter(string url);

        public virtual async Task OnPictureInPicture(Document document)
        {
            await Task.Run(() => { });
        }

        public virtual async Task OnFullScreen(Document document)
        {
            await Task.Run(() => { });
        }

        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string IconUrl { get; }
    }
}
