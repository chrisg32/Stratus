using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Stratus.Extensions.SiteExtensions
{
    class YouTubeSiteExtension : BaseSiteExtension
    {
        public override bool Filter(string url)
        {
            return Regex.IsMatch(url, @"^(?:https?:\/\/)?(?:www\.)?youtube\.com\/watch", RegexOptions.IgnoreCase);
        }

        public override string Name => "YouTube";
        public override string Description => "Hosts youtube video in a fullscreen frame for certain view modes.";
        public override string IconUrl => @"https://s.ytimg.com/yts/img/favicon_48-vflVjB_Qk.png";

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
            await Task.Run(() =>
            {
                var videoId = HttpUtility.ParseQueryString(document.Url)["v"] ?? HttpUtility.ParseQueryString(document.Url)[0];
                var html =
                    "<html><head></head><body style=\"margin: 0;\"><iframe src=\"https://www.youtube-nocookie.com/embed/{{videoId}}?autoplay=1\" style=\"border: 0; width: 100%; height: 100%\" allow=\"autoplay; encrypted-media\"></iframe></body></html>"
                        .Replace("{{videoId}}", videoId);
                document.ShowHtml(html);
            });
        }
    }
}
