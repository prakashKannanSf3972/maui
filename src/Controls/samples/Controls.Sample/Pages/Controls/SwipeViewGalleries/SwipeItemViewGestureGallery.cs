using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Maui.Controls.Sample.Pages.SwipeViewGalleries
{
    public class SwipeItemViewGestureGallery : ContentPage
    {
        public SwipeItemViewGestureGallery()
        {
            Title = "SwipeItemView Gesture Gallery";

            var swipeLayout = new StackLayout
            {
                Margin = new Thickness(12)
            };

            var instructions = new Label
            {
                BackgroundColor = Colors.Black,
                TextColor = Colors.White,
                Text = "Test TapGestureRecognizer commands in SwipeItemView. The Edit and Delete labels should respond to taps and show alerts."
            };

            swipeLayout.Children.Add(instructions);

            // Create a command for testing
            var editCommand = new Command<string>((item) =>
            {
                DisplayAlert("Edit Command", $"Edit command executed for: {item}", "OK");
            });

            var deleteCommand = new Command<string>((item) =>
            {
                DisplayAlert("Delete Command", $"Delete command executed for: {item}", "OK");
            });

            // Create SwipeItemView with gesture recognizers (the broken scenario)
            var editLabel = new Label
            {
                Text = "Edit",
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.Black,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                GestureRecognizers =
                {
                    new TapGestureRecognizer 
                    { 
                        Command = editCommand,
                        CommandParameter = "Test Item"
                    }
                }
            };

            var deleteLabel = new Label
            {
                Text = "Delete",
                BackgroundColor = Colors.Red,
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                GestureRecognizers =
                {
                    new TapGestureRecognizer 
                    { 
                        Command = deleteCommand,
                        CommandParameter = "Test Item"
                    }
                }
            };

            var swipeItemViewGrid = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(), new ColumnDefinition() },
                WidthRequest = 160,
                HeightRequest = 100,
                ColumnSpacing = 0
            };

            swipeItemViewGrid.Add(editLabel, 0);
            swipeItemViewGrid.Add(deleteLabel, 1);

            var swipeItemView = new SwipeItemView
            {
                Content = swipeItemViewGrid
            };

            var swipeItems = new SwipeItems { swipeItemView };

            // Create main content
            var mainContent = new Grid
            {
                BackgroundColor = Colors.LightBlue,
                HeightRequest = 60
            };

            var mainLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Text = "Swipe left to reveal SwipeItemView with gesture recognizers"
            };

            mainContent.Children.Add(mainLabel);

            var swipeView = new SwipeView
            {
                RightItems = swipeItems,
                Content = mainContent
            };

            swipeLayout.Children.Add(swipeView);

            // For comparison, add a SwipeItem version that should work
            var comparisonLabel = new Label
            {
                FontSize = 10,
                Text = "For comparison - SwipeItem (should work):",
                Margin = new Thickness(0, 20, 0, 0)
            };

            swipeLayout.Children.Add(comparisonLabel);

            var workingSwipeItem = new SwipeItem
            {
                Text = "Working Delete",
                BackgroundColor = Colors.Red,
                Command = deleteCommand,
                CommandParameter = "Working Item"
            };

            var workingSwipeItems = new SwipeItems { workingSwipeItem };

            var workingMainContent = new Grid
            {
                BackgroundColor = Colors.LightGreen,
                HeightRequest = 60
            };

            var workingMainLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Text = "Swipe left to reveal working SwipeItem"
            };

            workingMainContent.Children.Add(workingMainLabel);

            var workingSwipeView = new SwipeView
            {
                RightItems = workingSwipeItems,
                Content = workingMainContent
            };

            swipeLayout.Children.Add(workingSwipeView);

            Content = swipeLayout;
        }
    }
}