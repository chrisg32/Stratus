using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stratus.Extensions.SiteExtensions
{
    class OnlineStopwatchSiteExtension : BaseSiteExtension
    {
        #region Overrides of BaseSiteExtension

        public override bool Filter(string url)
        {
            return Regex.IsMatch(url, @"^(?:https?:\/\/)?(?:www\.)?online-stopwatch\.com\/.*", RegexOptions.IgnoreCase);
        }

        public override string Name => "Online-Stopwatch.com";
        public override string Description => "Allows online-stopwatch.com to run in PIP mode.";
        public override string IconUrl => @"https://www.online-stopwatch.com/favicon.ico";

        #endregion

        public override async Task OnPictureInPicture(Document document)
        {
            await RedirectToPopoutVersion(document);
        }

        public override async Task OnFullScreen(Document document)
        {
            await RedirectToPopoutVersion(document);
        }

        private async Task RedirectToPopoutVersion(Document document)
        {
            var vector = await document.RunJavascript(@"OSapp.vector");
            if (!string.IsNullOrWhiteSpace(vector))
            {
                document.Navigate(@"https://www.online-stopwatch.com/html5/" + vector);
            }
        }
    }
}
