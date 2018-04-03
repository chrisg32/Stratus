using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CG.Commons.Extensions;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Stratus.Extensions;

namespace Stratus.ViewModels
{
    class SettingsViewModel : ViewModelBase
    {
        public ObservableCollection<ExtensionViewModel> Extensions { get; } = new ObservableCollection<ExtensionViewModel>();

        public SettingsViewModel(IList<BaseSiteExtension> extensions)
        {
            var extensionViewModels = extensions.Select(e => new ExtensionViewModel(e));
            Extensions.AddRange(extensionViewModels);
        }
    }
}
