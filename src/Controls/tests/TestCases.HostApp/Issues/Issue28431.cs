using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Maui.Controls.Sample.Issues;
public class Issue28431 : ContentPage
{
	ObservableCollection<Issue28431ViewItem> Items;

	public Issue28431()
	{
		Items = new ObservableCollection<Issue28431ViewItem>
			{
				new Issue28431ViewItem { Name = "UltraWidget", Margin = new Thickness(20) },
				new Issue28431ViewItem { Name = "ProGadget", Margin = new Thickness(20) },
				new Issue28431ViewItem { Name = "MaxTool", Margin = new Thickness(20) },
				new Issue28431ViewItem { Name = "EliteComponent", Margin = new Thickness(20) },
				new Issue28431ViewItem { Name = "PrimeUtility", Margin = new Thickness(20) }
			};
		Content = CreateLayout();
	}

	Grid CreateLayout()
	{
		Grid mainGrid = new Grid
		{
			AutomationId = "MainGrid",
			RowDefinitions =
			{
				new RowDefinition { Height = GridLength.Auto },
				new RowDefinition { Height = GridLength.Star }
			}
		};

		Button changeMarginButton = new Button
		{
			Text = "Change Margin",
			AutomationId = "ChangeMargin"
		};
		changeMarginButton.Clicked += OnChangeMarginClicked;

		CollectionView collectionView = new CollectionView
		{
			AutomationId = "CollectionView",
			ItemsSource = Items,
			ItemTemplate = CreateItemTemplate()
		};

		mainGrid.Add(changeMarginButton, 0, 0);
		mainGrid.Add(collectionView, 0, 1);

		return mainGrid;
	}

	DataTemplate CreateItemTemplate()
	{
		return new DataTemplate(() =>
		{
			Label label = new Label
			{
				FontSize = 20,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center
			};

			label.SetBinding(Label.TextProperty, nameof(Issue28431ViewItem.Name));
			label.SetBinding(Label.MarginProperty, nameof(Issue28431ViewItem.Margin));

			return new Grid
			{
				Children = { label }
			};
		});
	}

	void OnChangeMarginClicked(object sender, EventArgs e)
	{
		foreach (var item in Items)
		{
			item.Margin = new Thickness(60);
		}
	}
}

class Issue28431ViewItem : INotifyPropertyChanged
{
	public string Name { get; set; }

	Thickness _margin;
	public Thickness Margin
	{
		get => _margin;
		set
		{
			if (_margin != value)
			{
				_margin = value;
				OnPropertyChanged(nameof(Margin));
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}