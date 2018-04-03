namespace Stratus.Extensions.SiteExtensions
{
    class YouTubeSiteExtension : BaseSiteExtension
    {
        public override bool Filter(string url)
        {
            return false;
        }

        public override string Name => "YouTube";
        public override string Description => "Hosts youtube video in a fullscreen frame for certain view modes.";
        public override string IconUrl => @"https://s.ytimg.com/yts/img/favicon_48-vflVjB_Qk.png";
    }
}
