using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CG.Commons.Extensions;
using Prism.Windows.Mvvm;
using Stratus.Extensions;
using Stratus.Services;

namespace Stratus.ViewModels
{
    class SettingsViewModel : ViewModelBase
    {
        private readonly SettingsService _settingsService;
        private string _homePage;
        private bool _crashReporting;
        public ObservableCollection<ExtensionViewModel> Extensions { get; } = new ObservableCollection<ExtensionViewModel>();

        public SettingsViewModel(IList<BaseSiteExtension> extensions, SettingsService settingsService)
        {
            _settingsService = settingsService;
            var extensionViewModels = extensions.Select(e => new ExtensionViewModel(e, settingsService));
            Extensions.AddRange(extensionViewModels);
            _homePage = _settingsService.CurrentSettings.HomePage;
            _crashReporting = _settingsService.CurrentSettings.CrashReporting;
        }

        public string HomePage
        {
            get => _homePage;
            set
            {
                SetProperty(ref _homePage, value);
                _settingsService.CurrentSettings.HomePage = value;
                _settingsService.Save(_settingsService.CurrentSettings);
            }
        }

        public bool CrashReporting
        {
            get => _crashReporting;
            set
            {
                SetProperty(ref _crashReporting, value);
                _settingsService.CurrentSettings.CrashReporting = value;
                _settingsService.Save(_settingsService.CurrentSettings);
            }
        }
    }
}
