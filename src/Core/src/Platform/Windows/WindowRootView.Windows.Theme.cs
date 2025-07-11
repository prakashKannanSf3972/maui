using System;
using Microsoft.UI.Xaml.Media;
using Microsoft.Maui.Controls;

namespace Microsoft.Maui.Platform
{
    public partial class WindowRootView
    {
        void RegisterThemeChangedHandlers()
        {
            _viewSettings.ColorValuesChanged += OnUISettingsColorValuesChanged;
            ActualThemeChanged += OnWindowRootViewThemeChanged;

            // Also listen for MAUI Application UserAppTheme changes
            if (Microsoft.Maui.Controls.Application.Current != null)
            {
                Microsoft.Maui.Controls.Application.Current.RequestedThemeChanged += OnMauiRequestedThemeChanged;
            }
        }

        void UnregisterThemeChangedHandlers()
        {
            _viewSettings.ColorValuesChanged -= OnUISettingsColorValuesChanged;
            ActualThemeChanged -= OnWindowRootViewThemeChanged;

            // Remove MAUI Application theme change handler
            if (Microsoft.Maui.Controls.Application.Current != null)
            {
                Microsoft.Maui.Controls.Application.Current.RequestedThemeChanged -= OnMauiRequestedThemeChanged;
            }
        }

        private void OnUISettingsColorValuesChanged(Windows.UI.ViewManagement.UISettings sender, object args)
        {
            UpdateTitleBarOnThemeChange();
        }

        private void OnWindowRootViewThemeChanged(object sender, UI.Xaml.FrameworkElement.ActualThemeChangedEventArgs args)
        {
            UpdateTitleBarOnThemeChange();
        }

        private void OnMauiRequestedThemeChanged(object sender, AppThemeChangedEventArgs args)
        {
            // Handle MAUI Application.Current.UserAppTheme changes
            UpdateTitleBarOnThemeChange();
        }

        private void UpdateTitleBarOnThemeChange()
        {
            // Update the TitleBar background if it's using system defaults (Background is null)
            if (_titleBar is not null && _iTitleBarRef.TryGetTarget(out IView? iTitleBar))
            {
                // Check if we need to update the theme - only update if background is not explicitly set
                bool shouldUpdateTheme = iTitleBar?.Background is null;

                if (shouldUpdateTheme)
                {
                    // Determine the current theme - first check MAUI app theme, then fall back to system theme
                    bool isDarkTheme;

                    // First check if MAUI app has an explicit theme set
                    if (Microsoft.Maui.Controls.Application.Current?.RequestedTheme != AppTheme.Unspecified)
                    {
                        isDarkTheme = Microsoft.Maui.Controls.Application.Current.RequestedTheme == AppTheme.Dark;
                    }
                    else
                    {
                        // Fall back to system theme
                        isDarkTheme = ActualTheme == UI.Xaml.ElementTheme.Dark;
                    }

                    // Get the appropriate background color based on theme
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
}
