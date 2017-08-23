using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Stratus.ViewModels;

namespace Stratus
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            DataContext = new WindowViewModel();

            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBarOnLayoutMetricsChanged;

            TitleBar.Height = coreTitleBar.Height;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            //Window.Current.SetTitleBar is used to identify which control will handle user inputs (Grab and move)
            Window.Current.SetTitleBar(SystemButtonGutter);

            var view = ApplicationView.GetForCurrentView();

            view.TitleBar.BackgroundColor = Colors.DarkGreen;
            view.TitleBar.ForegroundColor = Colors.White;

            view.TitleBar.ButtonBackgroundColor = Colors.DarkGreen;
            view.TitleBar.ButtonForegroundColor = Colors.White;

            view.TitleBar.ButtonHoverBackgroundColor = Colors.Green;
            view.TitleBar.ButtonHoverForegroundColor = Colors.White;

            view.TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 0, 120, 0);
            view.TitleBar.ButtonPressedForegroundColor = Colors.White;

            view.TitleBar.ButtonInactiveBackgroundColor = Colors.DarkGray;
            view.TitleBar.ButtonInactiveForegroundColor = Colors.Gray;

            view.TitleBar.InactiveBackgroundColor = Colors.DarkGreen;
            view.TitleBar.InactiveForegroundColor = Colors.Gray;


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

 
    }
}
