using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Stratus.Extensions.SiteHandlers
{
    class HuluSiteHandler : BaseSiteHandler
    {
        public override bool Filter(string url)
        {
            return Regex.IsMatch(url, @"^(?:https?:\/\/)?(?:www\.)?hulu\.com\/watch\/(\d{3,})#?.*?$");
        }

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
            var html = await document.GetHtml();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var nodes = doc.DocumentNode.SelectNodes("//img[contains(@class, 'thumbnail-background')]").ToList();
            var url = nodes.First().Attributes["src"].Value;
            var match = Regex.Match(url, @".?video\/(\d{5,})\?size");
            if (match.Success)
            {
                var videoId = match.Groups[1].Value;
                document.Navigate($@"https://www.hulu.com/html_stand_alone/{videoId}");
            }
        }
    }
}
