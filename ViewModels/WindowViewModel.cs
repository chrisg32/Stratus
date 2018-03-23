using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Windows.Mvvm;
using System.Text.RegularExpressions;

namespace Stratus.ViewModels
{
    class WindowViewModel : ViewModelBase
    {
        private Uri _source = new Uri("https://stackoverflow.com/questions/34888906/move-to-next-textbox-when-enter-key-is-pressed-instead-of-submit-windows-phon");
        private string _address;


        public ICommand FullScreenCommand { get; }
        public ICommand PipCommand { get; }


        public DelegateCommand<string> NavigateCommand { get; }

        public Uri Source
        {
            get => _source;
            set
            {
                Address = value.ToString();
                SetProperty(ref _source, value);
            }
        }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        public WindowViewModel()
        {
            NavigateCommand = new DelegateCommand<string>(ExecuteNavigate);
        }

        private void ExecuteNavigate(string url)
        {
            if (!Regex.IsMatch(url, @"^(?:https?:\/\/)"))
            {
                url = "https://" + url;
            }
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
            {
                Source = uri;
            }
        }


    }
}
