using System;
using Windows.UI.Xaml.Media.Imaging;
using Prism.Windows.Mvvm;
using Stratus.Extensions;

namespace Stratus.ViewModels
{
    class ExtensionViewModel : ViewModelBase
    {
        private readonly BaseSiteExtension _extension;

        public string Name => _extension.Name;
        public string Description => _extension.Description;
        public BitmapImage Icon { get; }
        public bool Enabled { get; set; }

        public ExtensionViewModel(BaseSiteExtension extension)
        {
            _extension = extension;
            Icon = new BitmapImage(new Uri(extension.IconUrl));
        }
    }
}
