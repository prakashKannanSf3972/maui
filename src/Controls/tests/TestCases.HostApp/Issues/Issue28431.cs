using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 28431, "In the template of CollectionView, the margin settings of the table are displayed inconsistently", PlatformAffected.UWP)]
public class Issue28431 : ContentPage
{
	private readonly ProductsViewModel _viewModel = new();

	public Issue28431()
	{
		BindingContext = _viewModel;

		Content = CreateLayout();
	}

	private Grid CreateLayout()
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
			ItemsSource = _viewModel.Products,
			ItemTemplate = CreateItemTemplate()
		};

		mainGrid.Add(changeMarginButton, 0, 0);
		mainGrid.Add(collectionView, 0, 1);

		return mainGrid;
	}

	private DataTemplate CreateItemTemplate()
	{
		return new DataTemplate(() =>
		{
			Label label = new Label
			{
				FontSize = 20,
				Margin = new Thickness(10, 0),
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center
			};
			label.SetBinding(Label.TextProperty, "Name");

			Grid paddingGrid = new Grid();
			paddingGrid.SetBinding(Grid.MarginProperty, "Margin");
			paddingGrid.Add(label);

			return paddingGrid;
		});
	}

	private void OnChangeMarginClicked(object sender, EventArgs e)
	{
		const int newMargin = 60;
		foreach (var product in _viewModel.Products)
		{
			product.Margin = new Thickness(newMargin);
		}
	}
}

public class Product : INotifyPropertyChanged
{
	private Thickness _margin = new Thickness(20);

	public string Name { get; set; }

	public Thickness Margin
	{
		get => _margin;
		set
		{
			if (_margin != value)
			{
				_margin = value;
				OnPropertyChanged();
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}

public class ProductsViewModel
{
	public ObservableCollection<Product> Products { get; }

	public ProductsViewModel()
	{
		Products = new ObservableCollection<Product>
		{
			new() { Name = "UltraWidget" },
			new() { Name = "ProGadget" },
			new() { Name = "MaxTool" },
			new() { Name = "EliteComponent" },
			new() { Name = "PrimeUtility" }
		};
	}
}
