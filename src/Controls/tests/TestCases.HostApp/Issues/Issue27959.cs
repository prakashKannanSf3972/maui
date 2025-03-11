using System.Collections.ObjectModel;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 27959, "Dynamically toggling the Header/Footer between a null and a non-null value in CollectionView is not working", PlatformAffected.UWP | PlatformAffected.Android)]
public class Issue27959_NavigationPage : TestNavigationPage
{
	protected override void Init()
	{
		var root = CreateRootContentPage();
		PushAsync(root);
	}

	ContentPage CreateRootContentPage()
	{
		var ContentPage = new ContentPage();
		VerticalStackLayout rootLayout = new VerticalStackLayout
		{
			Spacing = 10,
			Padding = new Thickness(10),
		};

		Button collectionViewButton = new Button() { Text = "CollectionView Header/Footer Toggle", AutomationId = "CollectionViewButton" };
		collectionViewButton.Clicked += (s, e) => Navigation.PushAsync(new Issue27959_View());
		Button collectionViewTemplatedButton = new Button() { Text = "CollectionViewTemplated Header/Footer Toggle", AutomationId = "CollectionViewTemplatedButton" };
		collectionViewTemplatedButton.Clicked += (s, e) => Navigation.PushAsync(new Issue27959_TemplatedView());
		rootLayout.Add(collectionViewButton);
		rootLayout.Add(collectionViewTemplatedButton);
		ContentPage.Content = rootLayout;
		return ContentPage;
	}
}

public class Issue27959_View : TestContentPage
{
	CollectionView _collectionView;
	object _storedHeader;
	object _storedFooter;

	protected override void Init()
	{
		Title = "CollectionView Header/Footer Toggle";

		Grid layoutGrid = new Grid
		{
			RowDefinitions =
			{
				new RowDefinition { Height = GridLength.Auto },
				new RowDefinition { Height = GridLength.Star },
			}
		};

		Button _toggleHeaderButton = new Button
		{
			AutomationId = "ToggleHeaderButton",
			Text = "Toggle Header"
		};
		_toggleHeaderButton.Clicked += ToggleHeader;

		Button _toggleFooterButton = new Button
		{
			AutomationId = "ToggleFooterButton",
			Text = "Toggle Footer"
		};
		_toggleFooterButton.Clicked += ToggleFooter;

		HorizontalStackLayout buttonsLayout = new HorizontalStackLayout
		{
			Padding = 20,
			HorizontalOptions = LayoutOptions.Center,
			Spacing = 20,
			Children =
			{
				_toggleHeaderButton,
				_toggleFooterButton
			}
		};

		layoutGrid.Add(buttonsLayout, 0, 0);

		_collectionView = new CollectionView
		{
			AutomationId = "CollectionView",
			Header = new Label
			{
				Padding = 10,
				FontAttributes = FontAttributes.Bold,
				FontSize = 24,
				Text = "This Is A Header",
				AutomationId = "Header"
			},
			EmptyView = new Label
			{
				Padding = new Thickness(20, 5, 5, 5),
				Text = "Empty"
			},
			Footer = new Label
			{
				Padding = 10,
				FontAttributes = FontAttributes.Bold,
				FontSize = 24,
				Text = "This Is A Footer",
				AutomationId = "Footer"
			},
			ItemsSource = new ObservableCollection<string>()
		};

		layoutGrid.Add(_collectionView, 0, 1);

		Content = layoutGrid;
	}

	private void ToggleHeader(object sender, System.EventArgs e)
	{
		_storedHeader = _collectionView.Header ?? _storedHeader;

		if (_collectionView.Header == null)
		{
			_collectionView.Header = _storedHeader;
		}
		else
		{
			_collectionView.Header = null;
		}
	}

	private void ToggleFooter(object sender, System.EventArgs e)
	{
		_storedFooter = _collectionView.Footer ?? _storedFooter;
		if (_collectionView.Footer == null)
		{
			_collectionView.Footer = _storedFooter;
		}
		else
		{
			_collectionView.Footer = null;
		}
	}
}

public class Issue27959_TemplatedView : TestContentPage
{
	CollectionView _templatedCollectionView;
	DataTemplate _storedHeaderTemplate;
	DataTemplate _storedFooterTemplate;

	protected override void Init()
	{
		Title = "CollectionViewTemplated Header/Footer Toggle";

		Grid layoutGrid = new Grid
		{
			RowDefinitions =
			{
				new RowDefinition { Height = GridLength.Auto },
				new RowDefinition { Height = GridLength.Star },
			}
		};

		Button _toggleHeaderButton = new Button
		{
			AutomationId = "ToggleHeaderTemplateButton",
			Text = "Toggle HeaderTemplate"
		};
		_toggleHeaderButton.Clicked += ToggleHeader;

		Button _toggleFooterButton = new Button
		{
			AutomationId = "ToggleFooterTemplateButton",
			Text = "Toggle FooterTemplate"
		};
		_toggleFooterButton.Clicked += ToggleFooter;

		HorizontalStackLayout buttonsLayout = new HorizontalStackLayout
		{
			Padding = 20,
			HorizontalOptions = LayoutOptions.Center,
			Spacing = 20,
			Children =
			{
				_toggleHeaderButton,
				_toggleFooterButton
			}
		};

		layoutGrid.Add(buttonsLayout, 0, 0);

		_templatedCollectionView = new CollectionView
		{
			AutomationId = "CollectionViewTemplate",
			HeaderTemplate = new DataTemplate(() =>
				new Label
				{
					Padding = 10,
					FontAttributes = FontAttributes.Bold,
					FontSize = 24,
					Text = "This Is A HeaderTemplate",
					AutomationId = "HeaderTemplate"
				}),
			EmptyViewTemplate = new DataTemplate(() =>
				new Label
				{
					Padding = new Thickness(20, 5, 5, 5),
					Text = "Empty Template"
				}),
			FooterTemplate = new DataTemplate(() =>
				new Label
				{
					Padding = 10,
					FontAttributes = FontAttributes.Bold,
					FontSize = 24,
					Text = "This Is A FooterTemplate",
					AutomationId = "FooterTemplate"
				})
		};

		layoutGrid.Add(_templatedCollectionView, 0, 1);
		Content = layoutGrid;
	}

	private void ToggleHeader(object sender, System.EventArgs e)
	{
		_storedHeaderTemplate = _templatedCollectionView.HeaderTemplate ?? _storedHeaderTemplate;

		if (_templatedCollectionView.HeaderTemplate == null)
		{
			_templatedCollectionView.HeaderTemplate = _storedHeaderTemplate;
		}
		else
		{
			_templatedCollectionView.HeaderTemplate = null;
		}
	}

	private void ToggleFooter(object sender, System.EventArgs e)
	{
		_storedFooterTemplate = _templatedCollectionView.FooterTemplate ?? _storedFooterTemplate;

		if (_templatedCollectionView.FooterTemplate == null)
		{
			_templatedCollectionView.FooterTemplate = _storedFooterTemplate;
		}
		else
		{
			_templatedCollectionView.FooterTemplate = null;
		}
	}
}
