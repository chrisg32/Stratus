using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
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
        private readonly MainViewModel _viewModel;
        private bool _isFullScreen;
        private readonly Document _document;
        private readonly IList<BaseSiteExtension> _extensions;
        private const string ViewModesGroup = "ViewModes";
        private readonly ApplicationView _applicationView;

        public MainPage()
        {
            InitializeComponent();
            _viewModel = App.Container.Resolve<MainViewModel>();
            _viewModel.ShowHtml = html => Task.Run(() => WebView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => WebView.NavigateToString(html)));
            _extensions = App.Container.Resolve<IList<BaseSiteExtension>>();
            SettingsDialog.DataContext = App.Container.Resolve<SettingsViewModel>();
            DataContext = _viewModel;

            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBarOnLayoutMetricsChanged;

            TitleBar.Height = coreTitleBar.Height;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            //Window.Current.SetTitleBar is used to identify which control will handle user inputs (Grab and move)
            Window.Current.SetTitleBar(SystemButtonGutter);

            var view = ApplicationView.GetForCurrentView();
            _applicationView = view;

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

            WebView.PermissionRequested += WebView_PermissionRequested;

            _document = new Document(_viewModel, WebView);
        }

        private async void WebView_PermissionRequested(WebView sender, WebViewPermissionRequestedEventArgs args)
        {
            if (args.PermissionRequest.PermissionType == WebViewPermissionType.Geolocation)
            {
                var locationPromptDialog = new ContentDialog
                {
                    Title = $"Let {args.PermissionRequest.Uri} use your location?",
                    CloseButtonText = "Block",
                    PrimaryButtonText = "Allow"
                };

                var result = await locationPromptDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    args.PermissionRequest.Allow();
                }
                else
                {
                    args.PermissionRequest.Deny();
                }
            }
        }

        private async void CreateJumpList()
        {
            if (!JumpList.IsSupported()) return;


            var jl = await JumpList.LoadCurrentAsync();
            jl.Items.Clear();

            if (jl.Items.All(i => i.GroupName != ViewModesGroup))
            {
                CreateViewModesJumpListItems(jl);
            }

            try
            {
                await jl.SaveAsync();
            }
            catch (Exception e)
            {
                //TODO log exception
            }
        }

        private void CreateViewModesJumpListItems(JumpList jl)
        {
            var pip = JumpListItem.CreateWithArguments("pip", "PIP");
            pip.Description = "Toggle PIP mode.";
            pip.GroupName = ViewModesGroup;
            pip.Logo = new Uri("ms-appx:///Assets/pip_icon_80.png");
            jl.Items.Add(pip);

            var fs = JumpListItem.CreateWithArguments("fullscreen", "Full Screen");
            fs.Description = "Toggle Full Screen mode.";
            fs.GroupName = ViewModesGroup;
            fs.Logo = new Uri("ms-appx:///Assets/fullscreen_icon_80.png");
            jl.Items.Add(fs);


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
            PipButton.Margin = new Thickness(0, 0, sender.SystemOverlayRightInset, 0);
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


        private async void Pip_Click(object sender, RoutedEventArgs e)
        {
            var extension = _extensions.FirstOrDefault(ext => ext.Filter(_document.Url));
            try
            {
                var task = extension?.OnPictureInPicture(_document);
                if (task != null) await task;
            }
            catch (Exception ex)
            {
                //TODO log exception
            }
            await TogglePip();
        }

        private async void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            var extension = _extensions.FirstOrDefault(ext => ext.Filter(_document.Url));
            try
            {
                var task = extension?.OnFullScreen(_document);
                if (task != null) await task;
            }
            catch (Exception ex)
            {
                //TODO log exception
            }
            EnterFullScreen();
        }

        private async void Settings_OnClick(object sender, RoutedEventArgs e)
        {
            await SettingsDialog.ShowAsync();
        }

        #endregion

        #region View Modes

        private bool IsPip => _applicationView.ViewMode == ApplicationViewMode.CompactOverlay;

        private async Task EnterPip()
        {
            if (_applicationView.IsViewModeSupported(ApplicationViewMode.CompactOverlay))
            {
                var compactOptions = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
                var result = await _applicationView.TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, compactOptions);
                if (result)
                {
                    HideTitleBar(true);
                }
            }
        }

        private async Task ExitPip()
        {
            if (_applicationView.IsViewModeSupported(ApplicationViewMode.CompactOverlay))
            {
                var result = await _applicationView.TryEnterViewModeAsync(ApplicationViewMode.Default);
                if (result)
                {
                    ShowTitleBar();
                }
            }
        }

        private async Task TogglePip()
        {
            if (_applicationView.IsFullScreenMode)
            {
                ExitFullScreen();
            }
            if (IsPip)
            {
                await ExitPip();
            }
            else
            {
                await EnterPip();
            }
        }

        private void ExitFullScreen()
        {
            _applicationView.ExitFullScreenMode();
            _isFullScreen = false;
            ShowTitleBar();
        }

        private void EnterFullScreen()
        {
            if (_applicationView.TryEnterFullScreenMode())
            {
                _isFullScreen = true;
                HideTitleBar(false);
            }
        }

        private async Task ToggleFullscreen()
        {
            if (IsPip)
            {
                await ExitPip();
            }
            if (_applicationView.IsFullScreenMode)
            {
                ExitFullScreen();
            }
            else
            {
                EnterFullScreen();
            }
        }

        private void HideTitleBar(bool grip)
        {
            PipTitleBar.Visibility = grip ? Visibility.Visible : Visibility.Collapsed;
            PipTitleBar.Height = TitleBar.ActualHeight;
            TitleBar.Visibility = Visibility.Collapsed;
            Window.Current.SetTitleBar(PipGrid);
        }

        private void ShowTitleBar()
        {
            TitleBar.Visibility = Visibility.Visible;
            PipTitleBar.Visibility = Visibility.Collapsed;
            Window.Current.SetTitleBar(SystemButtonGutter);
        }

        #endregion


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!string.IsNullOrEmpty(App.LaunchParam))
            {
                await HandleJump(App.LaunchParam);
                App.LaunchParam = null;
            }
        }

        public async Task HandleJump(string jumpArg)
        {
            switch (jumpArg)
            {
                case "pip":
                    await TogglePip();
                    break;
                case "fullscreen":
                    await ToggleFullscreen();
                    break;
            }
        }

    }
}
