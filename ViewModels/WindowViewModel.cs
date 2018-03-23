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
        private Uri _source = new Uri("https://eff.org");
        private string _address;

        private readonly Stack<Uri> _backStack = new Stack<Uri>();
        private readonly Stack<Uri> _forwardStack = new Stack<Uri>();
        private Uri _currentUri;

        #region Commands

        public ICommand FullScreenCommand { get; }
        public ICommand PipCommand { get; }


        public DelegateCommand<string> NavigateCommand { get; }
        public DelegateCommand BackCommand { get; }
        public DelegateCommand ForwardCommand { get; }

        #endregion



        public Uri Source
        {
            get => _source;
            private set
            {
                Address = value.ToString();
                SetProperty(ref _source, value);
            }
        }

        public string Address
        {
            get => _address;
            private set => SetProperty(ref _address, value);
        }

        public WindowViewModel()
        {
            NavigateCommand = new DelegateCommand<string>(ExecuteNavigate);
            BackCommand = new DelegateCommand(ExecuteBack, () => _backStack.Any());
            ForwardCommand = new DelegateCommand(ExecuteForward, () => _forwardStack.Any());
        }

        #region Command Implementation

        private void ExecuteBack()
        {
            if (_currentUri != null)
            {
                _forwardStack.Push(_currentUri);
                ForwardCommand.RaiseCanExecuteChanged();
            }
            _currentUri = null;
            Source = _backStack.Pop();
            Address = Source.ToString();
            BackCommand.RaiseCanExecuteChanged();
        }

        private void ExecuteForward()
        {
            Source = _forwardStack.Pop();
            Address = Source.ToString();
            ForwardCommand.RaiseCanExecuteChanged();
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
            _forwardStack.Clear();
            ForwardCommand.RaiseCanExecuteChanged();
        }

        #endregion


        public void OnNavigate(Uri uri)
        {
            if (_currentUri != null)
            {
                _backStack.Push(_currentUri);
                BackCommand.RaiseCanExecuteChanged();
            }

            _currentUri = uri;
            Address = uri.ToString();
        }

    }
}
