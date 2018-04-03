using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Prism.Windows.Mvvm;
using Stratus.Extensions;

namespace Stratus.ViewModels
{
    class ExtensionViewModel : ViewModelBase
    {
        private readonly BaseSiteExtension _extension;

        public string Name => _extension.Name;
        public string Description => _extension.Description;
        public ImageSource Icon { get; set; }
        public bool Enabled { get; set; }

        public ExtensionViewModel(BaseSiteExtension extension)
        {
            _extension = extension;
        }
    }
}
