using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Windows.Mvvm;

namespace Stratus.ViewModels
{
    class WindowViewModel : ViewModelBase
    {
        private TabViewModel _currentTab;
        public ObservableCollection<TabViewModel> Tabs { get; }

        public TabViewModel CurrentTab
        {
            get => _currentTab;
            set => SetProperty(ref _currentTab, value);
        }

        public ICommand NewTabCommand { get; }

        public WindowViewModel()
        {
            Tabs = new ObservableCollection<TabViewModel>();
            NewTabCommand = new DelegateCommand(ExecuteNewTab);

            ExecuteNewTab();
            CurrentTab = Tabs.First();
        }

        private void ExecuteNewTab()
        {
            Tabs.Add(new TabViewModel
            {
                Title = Faker.Company.Bullshit()
            });
        }
    }
}
