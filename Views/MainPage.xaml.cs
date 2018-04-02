using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Autofac;
using Stratus.Extensions;
using Stratus.ViewModels;

namespace Stratus.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        private readonly WindowViewModel _viewModel;
        private bool _isFullScreen;
        private Document _document;
        private IList<BaseSiteHandler> _extensions;

        public MainPage()
        {
            InitializeComponent();

            _viewModel = App.Container.Resolve<WindowViewModel>();
            _extensions = App.Container.Resolve<IList<BaseSiteHandler>>();
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

            view.VisibleBoundsChanged += View_VisibleBoundsChanged;

            CreateJumpList();

            _document = new Document(_viewModel, WebView);
        }

        private async void CreateJumpList()
        {
            if (!JumpList.IsSupported()) return;
            const string viewModesGroup = "ViewModes";

            var jl = await JumpList.LoadCurrentAsync();

            jl.Items.Clear();

            var pip = JumpListItem.CreateWithArguments("pip", "Pip");
            pip.Description = "Toggle PIP mode.";
            pip.GroupName = viewModesGroup;
            //TODO create assets for icons and set logo url
            //pip.Logo = new Uri("ms-appx:///images/your-images.png");

            
            jl.Items.Add(pip);
            try
            {
                await jl.SaveAsync();
            }
            catch (Exception e)
            {
                //TODO log exception
            }
        }

        private void View_VisibleBoundsChanged(ApplicationView sender, object args)
        {
            if (_isFullScreen && !sender.IsFullScreenMode)
            {
                _isFullScreen = false;
                ShowTitleBar();
            }
        }

        #region Event Handlers

        private void CoreTitleBarOnLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar.Height = sender.Height;
            SystemButtonGutter.Width = sender.SystemOverlayRightInset;
            MagicButton.Width = sender.SystemOverlayRightInset / 4;
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
            _viewModel.OnNavigate(args.Uri);
        }


        private void Pip_Click(object sender, RoutedEventArgs e)
        {
            var extension = _extensions.FirstOrDefault(ext => ext.Filter(_document.Url));
            extension?.OnPictureInPicture(_document);
            TogglePictureInPicture();
        }

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            var extension = _extensions.FirstOrDefault(ext => ext.Filter(_document.Url));
            extension?.OnFullScreen(_document);
            EnterFullScreen();
        }

        #endregion

        #region View Modes

        private async void TogglePictureInPicture()
        {
            if (ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay))
            {
                ViewModePreferences compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
                compactOptions.CustomSize = new Windows.Foundation.Size(320, 200);
                var result = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);
                if (result)
                {
                    HideTitleBar();
                }
            }
        }

        private void EnterFullScreen()
        {
            var view = ApplicationView.GetForCurrentView();
            if (view.TryEnterFullScreenMode())
            {
                _isFullScreen = true;
                HideTitleBar();
            }
        }

        private void HideTitleBar()
        {
            TitleBar.Visibility = Visibility.Collapsed;
        }

        private void ShowTitleBar()
        {
            TitleBar.Visibility = Visibility.Visible;
        }

        #endregion


    }
}
