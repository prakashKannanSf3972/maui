#nullable disable
namespace Maui.Controls.Sample;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

public partial class MainPage : ContentPage
{
	bool isShadowApplied = true;

	Border imageFrame;
	Border labelFrame;
	Border boxFrame;

	public MainPage()
	{
		// Common shadow to apply initially
		var initialShadow = new Shadow
		{
			Brush = Brush.Black,
			Offset = new Point(4, 4),
			Radius = 10,
			Opacity = 0.5f
		};

		// Create Image in Frame with shadow
		imageFrame = new Border
		{
			Content = new Image
			{
				Source = "dotnet_bot.png",
				WidthRequest = 80,
				HeightRequest = 80
			},
			BackgroundColor = Colors.Transparent,
			Shadow = initialShadow,
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};

		// Create Label in Frame with shadow
		labelFrame = new Border
		{
			Content = new Label
			{
				Text = "Hello MAUI",
				FontSize = 18,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			},
			BackgroundColor = Colors.LightGray,
			Shadow = initialShadow,
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};

		// Create BoxView in Frame with shadow
		boxFrame = new Border
		{
			Content = new BoxView
			{
				Color = Colors.Green,
				WidthRequest = 60,
				HeightRequest = 60,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			},
			BackgroundColor = Colors.Transparent,
			Shadow = initialShadow,
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};

		// Grid with 3 equal columns
		var grid = new Grid
		{
			ColumnDefinitions = new ColumnDefinitionCollection
			{
				new ColumnDefinition { Width = GridLength.Star },
				new ColumnDefinition { Width = GridLength.Star },
				new ColumnDefinition { Width = GridLength.Star }
			},
			HorizontalOptions = LayoutOptions.Fill,
			VerticalOptions = LayoutOptions.Center,
			HeightRequest = 150
		};

		grid.Children.Add(imageFrame);
		grid.Children.Add(labelFrame);
		grid.Children.Add(boxFrame);

		grid.SetColumn(imageFrame, 0);
		grid.SetColumn(labelFrame, 1);
		grid.SetColumn(boxFrame, 2);

		// Toggle Button
		var toggleButton = new Button
		{
			Text = "Toggle Shadows",
			HorizontalOptions = LayoutOptions.Center
		};
		toggleButton.Clicked += ToggleShadows;

		Content = new VerticalStackLayout
		{
			Spacing = 30,
			Padding = 20,
			Children =
			{
				grid,
				toggleButton
			}
		};
	}

	private void ToggleShadows(object sender, EventArgs e)
	{
		if (isShadowApplied)
		{
			RemoveShadow(imageFrame);
			RemoveShadow(labelFrame);
			RemoveShadow(boxFrame);
		}
		else
		{
			ApplyShadow(imageFrame);
			ApplyShadow(labelFrame);
			ApplyShadow(boxFrame);
		}

		isShadowApplied = !isShadowApplied;
	}

	private void ApplyShadow(Border frame)
	{
		frame.Shadow = new Shadow
		{
			Brush = Brush.Black,
			Offset = new Point(4, 4),
			Radius = 10,
			Opacity = 0.5f
		};
	}

	private void RemoveShadow(Border frame)
	{
		frame.Shadow = null;
	}
}