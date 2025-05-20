#nullable disable
namespace Maui.Controls.Sample;

using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

public partial class MainPage : ContentPage
{
	//bool isShadowApplied = false;

	//Border imageFrame;
	//Label labelFrame;
	//BoxView boxFrame;

	//public MainPage()
	//{
	//	// Common shadow to apply initially
	//	var initialShadow = new Shadow
	//	{
	//		Brush = Brush.Yellow,
	//		Offset = new Point(4, 4),
	//		Radius = 10,
	//		Opacity = 0.5f
	//	};

	//	// Create Image in Frame with shadow
	//	imageFrame = new Border
	//	{
	//		Content = new Image
	//		{
	//			Source = "dotnet_bot.png",
	//			WidthRequest = 80,
	//			HeightRequest = 80
	//		},
	//		BackgroundColor = Colors.Transparent,
	//		Shadow = initialShadow,
	//		HorizontalOptions = LayoutOptions.Center,
	//		VerticalOptions = LayoutOptions.Center
	//	};

	//	// Create Label in Frame with shadow
	//	labelFrame = new Label
	//	{

	//			Text = "Hello MAUI",
	//			FontSize = 18,
	//			HorizontalOptions = LayoutOptions.Center,
	//			VerticalOptions = LayoutOptions.Center

	//	};

	//	// Create BoxView in Frame with shadow
	//	boxFrame = new BoxView
	//	{
	//		Color = Colors.Green,
	//		WidthRequest = 60,
	//		HeightRequest = 60,
	//		//HorizontalOptions = LayoutOptions.Center,
	//		//VerticalOptions = LayoutOptions.Center
	//	};

	//	// Grid with 3 equal columns
	//	//var grid = new Grid
	//	//{
	//	//	ColumnDefinitions = new ColumnDefinitionCollection
	//	//	{
	//	//		new ColumnDefinition { Width = GridLength.Star },
	//	//		new ColumnDefinition { Width = GridLength.Star },
	//	//		new ColumnDefinition { Width = GridLength.Star }
	//	//	},
	//	//	HorizontalOptions = LayoutOptions.Fill,
	//	//	VerticalOptions = LayoutOptions.Center,
	//	//	HeightRequest = 150
	//	//};

	//	StackLayout grid = new StackLayout() { HeightRequest = 150 };
	//	//grid.Children.Add(imageFrame);
	//	//grid.Children.Add(labelFrame);
	//	grid.Children.Add(boxFrame);

	//	//grid.SetColumn(imageFrame, 2);
	//	//grid.SetColumn(labelFrame, 1);
	//	//grid.SetColumn(boxFrame, 0);

	//	// Toggle Button
	//	var toggleButton = new Button
	//	{
	//		Text = "Toggle Shadows",
	//		HorizontalOptions = LayoutOptions.Center
	//	};
	//	toggleButton.Clicked += ToggleShadows;

	//	Content = new VerticalStackLayout
	//	{
	//		Spacing = 30,
	//		Padding = 20,
	//		Children =
	//		{
	//			grid,
	//			toggleButton
	//		}
	//	};
	//}

	//private void ToggleShadows(object sender, EventArgs e)
	//{
	//	if (isShadowApplied)
	//	{
	//		//RemoveShadow(imageFrame);
	//		//RemoveShadow(labelFrame);
	//		RemoveShadow(boxFrame);
	//	}
	//	else
	//	{
	//		//ApplyShadow(imageFrame);
	//		//ApplyShadow(labelFrame);
	//		ApplyShadow(boxFrame);
	//	}

	//	isShadowApplied = !isShadowApplied;
	//}

	//private void ApplyShadow(View frame)
	//{
	//	Debug.WriteLine("Apply Shadow");
	//	if(frame.Shadow == null)
	//	{
	//		frame.Shadow = new Shadow
	//		{
	//			Brush = Brush.Black,
	//			Offset = new Point(4, 4),
	//			Radius = 10,
	//			Opacity = 0.5f
	//		};
	//	}
	//}

	//private void RemoveShadow(View frame)
	//{
	//	Debug.WriteLine("Remove Shadow");
	//	if(frame.Shadow != null)
	//		frame.Shadow = null;
	//}

	const double Fps = 60;

	bool _clip;
	bool _shadow;
	bool _benchmark;

	DateTime _lastUpdateDateTime;
	readonly IDispatcherTimer _timer;

	event Action timerUpdateEvent;

	public MainPage()
	{
		InitializeComponent();

		_shadow = true;
		_lastUpdateDateTime = DateTime.Now;

		_timer = Dispatcher.CreateTimer();
		_timer.Interval = TimeSpan.FromSeconds(1 / Fps);
		double time = 0;
		DateTime currentTickDateTime = DateTime.Now;
		double deltaTime = 0;

		_timer.Tick += delegate
		{
			deltaTime = (DateTime.Now - currentTickDateTime).TotalSeconds;
			currentTickDateTime = DateTime.Now;
			time += deltaTime;
			timerUpdateEvent?.Invoke();
		};

		timerUpdateEvent += delegate
		{
			double sinVal = (Math.Sin(time) + 1) * 0.5;
			const double minSize = 20;
			double width = minSize + sinVal * 100;

			BorderShadow.WidthRequest = ImageShadow.WidthRequest = LabelShadow.WidthRequest = width;
		};

		ViewModel = new ShadowViewModel();

		BindingContext = ViewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		ShadowContainer.SizeChanged += OnShadowSizeChanged;
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();

		ShadowContainer.SizeChanged -= OnShadowSizeChanged;
		_timer.Stop();
	}

	void OnShadowSizeChanged(object sender, EventArgs e)
	{
		if (_lastUpdateDateTime != DateTime.Now)
		{
			string fps = Math.Round(1 / (DateTime.Now - _lastUpdateDateTime).TotalSeconds, 2).ToString();
			FpsLabel.Text = fps;

			_lastUpdateDateTime = DateTime.Now;
		}
	}

	public ShadowViewModel ViewModel { get; private set; }

	void OnColorChanged(object sender, TextChangedEventArgs e)
	{
		Color.TryParse(ColorEntry.Text, out Color color);

		if (color is not null)
			ViewModel.Color = color;
	}

	void OnOffsetXChanged(object sender, TextChangedEventArgs e)
	{
		if (double.TryParse(OffsetXEntry.Text, out double offsetX))
		{
			ViewModel.OffsetX = offsetX;
		}
	}

	void OnOffsetYChanged(object sender, TextChangedEventArgs e)
	{
		if (double.TryParse(OffsetYEntry.Text, out double offsetY))
		{
			ViewModel.OffsetY = offsetY;
		}
	}

	void OnRadiusChanged(object sender, TextChangedEventArgs e)
	{
		if (double.TryParse(RadiusEntry.Text, out double radius))
		{
			ViewModel.Radius = radius;
		}
	}

	void OnOpacityChanged(object sender, TextChangedEventArgs e)
	{
		if (double.TryParse(OpacityEntry.Text, out double opacity))
		{
			ViewModel.Opacity = (float)opacity;
		}
	}

	void OnFlowDirectionChanged(object sender, EventArgs e)
	{
		ViewModel.FlowDirection = FlowDirectionLTR.IsChecked ? FlowDirection.LeftToRight : FlowDirection.RightToLeft;
	}

	void OnIsEnabledCheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		ViewModel.IsEnabled = IsEnabledTrueRadio.IsChecked;
	}

	void OnIsVisibleCheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		ViewModel.IsVisible = IsVisibleTrueRadio.IsChecked;
	}

	void OnBenchmarkClicked(object sender, EventArgs e)
	{
		if (_benchmark)
		{
			FpsLabel.IsVisible = false;
			_timer.Stop();
			_benchmark = false;
		}
		else
		{
			FpsLabel.IsVisible = true;
			_timer.Start();
			_benchmark = true;
		}
	}

	void OnClipClicked(object sender, EventArgs e)
	{
		if (_clip)
		{
			//ClipButton.Text = "Add Clip";
			BorderShadow.Clip = ImageShadow.Clip = LabelShadow.Clip = null;
			_clip = false;
		}
		else
		{
			//ClipButton.Text = "Remove Clip";
			BorderShadow.Clip = ImageShadow.Clip = LabelShadow.Clip = new EllipseGeometry
			{
				Center = new Point(40, 40),
				RadiusX = 20,
				RadiusY = 20
			};
			_clip = true;
		}
	}

	void OnShadowClicked(object sender, EventArgs e)
	{
		if (_shadow)
		{
			ShadowButton.Text = "Add Shadow";
			BorderShadow.Shadow = ImageShadow.Shadow = LabelShadow.Shadow = null;
			_shadow = false;
		}
		else
		{
			ShadowButton.Text = "Remove Shadow";

			var newShadow = new Shadow();

			newShadow.SetBinding(Shadow.BrushProperty, "Color");
			newShadow.SetBinding(Shadow.OffsetProperty, "Offset");
			newShadow.SetBinding(Shadow.RadiusProperty, "Radius");
			newShadow.SetBinding(Shadow.OpacityProperty, "Opacity");

			BorderShadow.Shadow = ImageShadow.Shadow = LabelShadow.Shadow = newShadow;
			_shadow = true;
		}
	}

	void OnResetClicked(object sender, EventArgs e)
	{
		FpsLabel.IsVisible = false;
		_timer.Stop();
		_benchmark = false;

		ColorEntry.Text = "#000000";

		IsEnabledTrueRadio.IsChecked = true;
		IsVisibleTrueRadio.IsChecked = true;
		FlowDirectionLTR.IsChecked = true;

		BorderShadow.Clip = ImageShadow.Clip = LabelShadow.Clip = null;
		BorderShadow.WidthRequest = ImageShadow.WidthRequest = LabelShadow.WidthRequest = 80;
	}
}


public class ShadowViewModel : BindableObject
{
	Color _color;
	double _offsetX;
	double _offsetY;
	Point _offset;
	double _radius;
	double _opacity;
	FlowDirection _flowDirection;
	bool _isEnabled;
	bool _isVisible;

	public ShadowViewModel()
	{
		Reset();
	}

	public Color Color
	{
		get => _color;
		set
		{
			if (_color != value)
			{
				_color = value;
				OnPropertyChanged();
			}
		}
	}

	public double OffsetX
	{
		get => _offsetX;
		set
		{
			if (_offsetX != value)
			{
				_offsetX = value;
				UpdateOffset();
				OnPropertyChanged();
			}
		}
	}

	public double OffsetY
	{
		get => _offsetY;
		set
		{
			if (_offsetY != value)
			{
				_offsetY = value;
				UpdateOffset();
				OnPropertyChanged();
			}
		}
	}

	public Point Offset
	{
		get => _offset;
		set
		{
			if (_offset != value)
			{
				_offset = value;
				OnPropertyChanged();
			}
		}
	}

	public double Radius
	{
		get => _radius;
		set
		{
			if (_radius != value)
			{
				_radius = value;
				OnPropertyChanged();
			}
		}
	}

	public double Opacity
	{
		get => _opacity;
		set
		{
			if (_opacity != value)
			{
				_opacity = value;
				OnPropertyChanged();
			}
		}
	}

	public bool IsEnabled
	{
		get => _isEnabled;
		set
		{
			if (_isEnabled != value)
			{
				_isEnabled = value;
				OnPropertyChanged();
			}
		}
	}

	public bool IsVisible
	{
		get => _isVisible;
		set
		{
			if (_isVisible != value)
			{
				_isVisible = value;
				OnPropertyChanged();
			}
		}
	}
	public FlowDirection FlowDirection
	{
		get => _flowDirection;
		set
		{
			if (_flowDirection != value)
			{
				_flowDirection = value;
				OnPropertyChanged();
			}
		}
	}

	public ICommand ResetCommand => new Command(Reset);

	void UpdateOffset()
	{
		Offset = new Point(OffsetX, OffsetY);
	}

	void Reset()
	{
		Color = Colors.Black;
		OffsetX = 10;
		OffsetY = 10;
		Offset = new Point(OffsetX, OffsetY);
		Radius = 12;
		Opacity = 1;
		FlowDirection = FlowDirection.LeftToRight;
		IsEnabled = true;
		IsVisible = true;
	}
}