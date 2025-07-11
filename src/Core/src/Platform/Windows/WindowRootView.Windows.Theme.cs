using System;
using Microsoft.UI.Xaml.Media;

namespace Microsoft.Maui.Platform
{
    public partial class WindowRootView
    {
        void RegisterThemeChangedHandlers()
        {
            _viewSettings.ColorValuesChanged += OnUISettingsColorValuesChanged;
            ActualThemeChanged += OnWindowRootViewThemeChanged;
        }

        void UnregisterThemeChangedHandlers()
        {
            _viewSettings.ColorValuesChanged -= OnUISettingsColorValuesChanged;
            ActualThemeChanged -= OnWindowRootViewThemeChanged;
        }

        private void OnUISettingsColorValuesChanged(Windows.UI.ViewManagement.UISettings sender, object args)
        {
            UpdateTitleBarOnThemeChange();
        }

        private void OnWindowRootViewThemeChanged(object sender, UI.Xaml.FrameworkElement.ActualThemeChangedEventArgs args)
        {
            UpdateTitleBarOnThemeChange();
        }

        private void UpdateTitleBarOnThemeChange()
        {
            // Update the TitleBar background if it's using system defaults (Background is null)
            if (_titleBar is not null && _iTitleBarRef.TryGetTarget(out IView? iTitleBar) && iTitleBar?.Background is null)
            {
                // Get the correct background color based on current theme
                var isDarkTheme = ActualTheme == UI.Xaml.ElementTheme.Dark;
                UI.Color backgroundColor = isDarkTheme
                    ? UI.Colors.Black // Dark theme background
                    : UI.Colors.White; // Light theme background

                // Update the ButtonHolderGrid background color to match the theme
                if (NavigationViewControl?.ButtonHolderGrid is not null)
                {
                    NavigationViewControl.ButtonHolderGrid.Background = new SolidColorBrush(backgroundColor);
                }

                // Also update the WindowTitleBarContent background to match the theme
                if (WindowTitleBarContent is not null)
                {
                    WindowTitleBarContent.Background = new SolidColorBrush(backgroundColor);
                }
            }
        }
    }
}
