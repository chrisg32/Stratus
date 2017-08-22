using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Windows.UI.Popups;

namespace Stratus.ViewModels
{
    class TabViewModel
    {
        public string Title { get; set; }
        public ICommand BackCommand { get; set; }

        public TabViewModel()
        {
            BackCommand = new DelegateCommand(ExecuteGoBack, CanGoBack);
        }

        private bool CanGoBack()
        {
            return true;
            //TODO implement
        }

        private async void ExecuteGoBack()
        {
            await new MessageDialog("Back clicked").ShowAsync();
        }
    }
}
