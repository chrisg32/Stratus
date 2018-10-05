using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stratus.Extensions.SiteExtensions
{
    class HuluSiteExtension : BaseSiteExtension
    {
        public override bool Filter(string url)
        {
            return Regex.IsMatch(url, @"^(?:https?:\/\/)?(?:www\.)?hulu\.com\/(?:watch\/([{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?))|live$", RegexOptions.IgnoreCase);
        }

        public override string Name => "Hulu";
        public override string Description => "Redirects Hulu to full screen viewer when entering certain view modes.";
        public override string IconUrl => @"https://www.hulu.com/fat-favicon.ico";

        public override async Task OnPictureInPicture(Document document)
        {
            await RedirectToPopoutVersion(document);
        }

        private async Task RedirectToPopoutVersion(Document document)
        {
            var javascript = "var x = document.getElementsByClassName(\"hulu-player-app\");"
                             + "var i;"
                             + "for (i = 0; i < x.length; i++)"
                             + "{"
                             + "x[i].style.minWidth = \"0px\";"
                             + "x[i].style.minHeight = \"0px\";"
                             + "}";
            await document.RunJavascript(javascript);
        }
    }
}
