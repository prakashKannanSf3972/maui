namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 20404, "Dynamic Grid.Row changes don't trigger layout update on Windows until window resize", PlatformAffected.UWP)]
public class Issue20404 : TestContentPage
{
	Grid dynamicGrid;
	int toggleCount;
	protected override void Init()
	{
		Grid mainGrid = new Grid
		{
			RowSpacing = 8,
			RowDefinitions =
			{
				new RowDefinition { Height = GridLength.Star },
				new RowDefinition { Height = new GridLength(4, GridUnitType.Star) },
				new RowDefinition { Height = GridLength.Star }
			}
		};

		Grid topGrid = new Grid();
		topGrid.Children.Add(new Button { Background = Colors.LightBlue });
		Grid.SetRow(topGrid, 0);
		mainGrid.Children.Add(topGrid);

		dynamicGrid = new Grid();
		dynamicGrid.Children.Add(new Button { Background = Colors.Green });
		Grid.SetRow(dynamicGrid, 1);
		mainGrid.Children.Add(dynamicGrid);

		Button toggleButton = new Button
		{
			Text = "Toggle Grid.Row",
			AutomationId = "ToggleButton",
			HorizontalOptions = LayoutOptions.Start,
			VerticalOptions = LayoutOptions.Start
		};
		toggleButton.Clicked += (_, __) =>
		{
			Grid.SetRow(dynamicGrid, toggleCount % 2 == 0 ? 0 : 1);
			toggleButton.Text = $"Grid.Row is now {(toggleCount % 2 == 0 ? 0 : 1)}";
			toggleCount++;
		};

		VerticalStackLayout buttonStack = new VerticalStackLayout { toggleButton };
		Grid.SetRow(buttonStack, 3);
		mainGrid.Children.Add(buttonStack);

		Content = mainGrid;
	}
}