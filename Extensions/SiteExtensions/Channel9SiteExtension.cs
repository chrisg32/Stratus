using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace Stratus.Extensions.SiteExtensions
{
    class Channel9SiteExtension : BaseSiteExtension
    {
        public override bool Filter(string url)
        {
            return Regex.IsMatch(url, @"^(?:https?:\/\/)?(?:www\.)?channel9\.msdn\.com\/", RegexOptions.IgnoreCase);
        }

        public override string Name => "Channel 9";
        public override string Description => "Hosts Channel 9 videos in a fullscreen frame for certain view modes.";
        public override string IconUrl => "https://channel9.msdn.com/favicon.ico";

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
            if (html.Contains("class=\"ch9tab embed\""))
            {
                var url = document.Url.ToLowerInvariant();
                if (!url.EndsWith("/player"))
                {
                    var playerUrl = document.Url + (url.EndsWith("/") ? string.Empty : "/") + "player";
                    document.Navigate(playerUrl);
                }
            }
            //youtube hosted videos
            else
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var playerNode = doc.DocumentNode.SelectNodes("//iframe[@class=\"playerIframe\"]").FirstOrDefault();
                var youtubeUrl = playerNode?.Attributes["src"].Value;
                if (!string.IsNullOrWhiteSpace(youtubeUrl))
                {
                    youtubeUrl += "?autoplay=1";
                    document.Navigate(youtubeUrl);
                }
            }
        }
    }


}
