using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Stratus.ViewModels;
using Windows.UI.Xaml.Controls.Primitives;
using System.Runtime.CompilerServices;

namespace Stratus
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        private readonly WindowViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();

            _viewModel = new WindowViewModel();
            DataContext = _viewModel;

            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBarOnLayoutMetricsChanged;

            TitleBar.Height = coreTitleBar.Height;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            //Window.Current.SetTitleBar is used to identify which control will handle user inputs (Grab and move)
            Window.Current.SetTitleBar(SystemButtonGutter);

            var view = ApplicationView.GetForCurrentView();

            view.TitleBar.BackgroundColor = Colors.Transparent;
            view.TitleBar.ForegroundColor = Colors.White;

            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonForegroundColor = Colors.White;

            var hoverColor = (Color)Application.Current.Resources["UiMouseOverColor"];
            view.TitleBar.ButtonHoverBackgroundColor = hoverColor;
            view.TitleBar.ButtonHoverForegroundColor = Colors.White;

            var pressedColor = (Color)Application.Current.Resources["UiClickedColor"];
            view.TitleBar.ButtonPressedBackgroundColor = pressedColor;
            view.TitleBar.ButtonPressedForegroundColor = Colors.White;

            view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonInactiveForegroundColor = Colors.Transparent;

            view.TitleBar.InactiveBackgroundColor = Colors.Transparent;
            view.TitleBar.InactiveForegroundColor = Colors.Transparent;


            //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void CoreTitleBarOnLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar.Height = sender.Height;
            SystemButtonGutter.Width = sender.SystemOverlayRightInset;
            MagicButton.Width = sender.SystemOverlayRightInset / 4;
        }

        private async void DoPictureInPicture()
        {
            if (ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay))
            {
                ViewModePreferences compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
                compactOptions.CustomSize = new Windows.Foundation.Size(320, 200);
                var result = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);
            }
        }

        private void MagicButton_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void AddressBar_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (_viewModel.NavigateCommand.CanExecute(null))
                {
                    _viewModel.NavigateCommand.Execute(AddressBar.Text);
                }
            }
        }


        private void WebView_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            _viewModel.Address = args.Uri.ToString();
        }


    }
}
