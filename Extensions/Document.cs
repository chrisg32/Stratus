using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Stratus.ViewModels;

namespace Stratus.Extensions
{
    public class Document
    {
        private readonly MainViewModel _viewModel;
        private readonly WebView _webView;

        internal Document(MainViewModel viewModel, WebView webView)
        {
            _viewModel = viewModel;
            _webView = webView;
        }

        public string Url => _viewModel.Address;

        public void Navigate(string url)
        {
            if (_viewModel.NavigateCommand.CanExecute(url))
            {
                _viewModel.NavigateCommand.Execute(url);
            }
        }

        public void ShowHtml(string html)
        {
            _viewModel.ShowHtml(html);
        }

        public void Back()
        {
            if(_viewModel.BackCommand.CanExecute())
            {
                _viewModel.BackCommand.Execute();
            }
        }

        public void Forward()
        {
            if (_viewModel.ForwardCommand.CanExecute())
            {
                _viewModel.ForwardCommand.Execute();
            }
        }

        public async Task<string> GetHtml()
        {
            return await _webView.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });
        }
    }
}
