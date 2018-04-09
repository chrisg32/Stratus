using System;
using Windows.UI.Xaml.Media.Imaging;
using Prism.Windows.Mvvm;
using Stratus.Extensions;
using Stratus.Services;

namespace Stratus.ViewModels
{
    class ExtensionViewModel : ViewModelBase
    {
        private readonly BaseSiteExtension _extension;
        private readonly SettingsService _settingsService;

        private bool _enabled;

        public string Name => _extension.Name;
        public string Description => _extension.Description;
        public BitmapImage Icon { get; }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                SetProperty(ref _enabled, value);
                _settingsService.CurrentSettings.ExtensionsEnabled[Name] = value;
                _settingsService.Save(_settingsService.CurrentSettings);
            }
        }

        public ExtensionViewModel(BaseSiteExtension extension, SettingsService settingsService)
        {
            _extension = extension;
            _settingsService = settingsService;
            Icon = new BitmapImage(new Uri(extension.IconUrl));
            _enabled = _settingsService.CurrentSettings.ExtensionsEnabled[Name];
        }
    }
}
