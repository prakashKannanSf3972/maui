namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 29484, "CollectionView Selected state does not work on the selected item when combined with PointerOver", PlatformAffected.UWP | PlatformAffected.macOS)]
public class Issue29484 : TestContentPage
{
	protected override void Init()
	{
		Style pointerOverSelectedItemStyle = new Style(typeof(Label))
		{
			Setters =
				{
					new Setter { Property = BackgroundColorProperty, Value = Colors.Transparent }
				}
		};

		VisualStateGroupList visualStateGroupList = new VisualStateGroupList();
		VisualStateGroup commonStatesGroup = new VisualStateGroup { Name = "CommonStates" };

		VisualState normalState = new VisualState { Name = "Normal" };
		commonStatesGroup.States.Add(normalState);

		VisualState pointerOverState = new VisualState { Name = "PointerOver" };
		pointerOverState.Setters.Add(new Setter { Property = BackgroundColorProperty, Value = Colors.DarkTurquoise });
		commonStatesGroup.States.Add(pointerOverState);

		VisualState selectedState = new VisualState { Name = "Selected" };
		selectedState.Setters.Add(new Setter { Property = BackgroundColorProperty, Value = Colors.DarkBlue });
		commonStatesGroup.States.Add(selectedState);

		visualStateGroupList.Add(commonStatesGroup);

		pointerOverSelectedItemStyle.Setters.Add(new Setter
		{
			Property = VisualStateManager.VisualStateGroupsProperty,
			Value = visualStateGroupList
		});

		DataTemplate pointerOverSelectedItemTemplate = new DataTemplate(() =>
		{
			Label label = new Label
			{
				Style = pointerOverSelectedItemStyle
			};
			label.SetBinding(Label.TextProperty, ".");
			return label;
		});

		Resources.Add("PointerOverSelectedItemStyle", pointerOverSelectedItemStyle);
		Resources.Add("PointerOverSelectedItemTemplate", pointerOverSelectedItemTemplate);

		Grid grid = new Grid
		{
			ColumnSpacing = 10,
			HorizontalOptions = LayoutOptions.Center,
			RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Star }
				},
			ColumnDefinitions =
				{
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = GridLength.Auto }
				}
		};

		Label headerLabel = new Label
		{
			Text = "PointerOver + Selected"
		};
		grid.Add(headerLabel, 0, 0);
		CollectionView collectionView = new CollectionView
		{
			AutomationId = "PointerOverSelectView",
			ItemTemplate = pointerOverSelectedItemTemplate,
			SelectionMode = SelectionMode.Single,
			ItemsSource = new List<string>
			{
				"Item 1",
				"Item 2",
				"Item 3",
				"Item 4"
			}
		};
		grid.Add(collectionView, 0, 1);
		Grid.SetRowSpan(collectionView, 2);
		Content = grid;
	}
}