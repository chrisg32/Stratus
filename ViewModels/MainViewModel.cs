using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using Prism.Windows.Mvvm;
using System.Text.RegularExpressions;
using Stratus.Services;

namespace Stratus.ViewModels
{

    class MainViewModel : ViewModelBase
    {
        private Uri _source;
        private string _address;

        private readonly Stack<Uri> _backStack = new Stack<Uri>();
        private readonly Stack<Uri> _forwardStack = new Stack<Uri>();
        private Uri _currentUri;
        public Action<string> ShowHtml { get; set; }

        #region Commands

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

        public MainViewModel(SettingsService settingsService)
        {
            NavigateCommand = new DelegateCommand<string>(ExecuteNavigate);
            BackCommand = new DelegateCommand(ExecuteBack, () => _backStack.Any());
            ForwardCommand = new DelegateCommand(ExecuteForward, () => _forwardStack.Any());
            var homepage = string.IsNullOrWhiteSpace(settingsService.CurrentSettings.HomePage)
                ? "https://eff.org"
                : settingsService.CurrentSettings.HomePage;
            _source = new Uri(homepage);
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
            if (url == null) return;
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
            if(uri == null) return;
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
