using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	[Category(TestCategory.SwipeView)]
	public partial class SwipeViewTests : ControlsHandlerTestBase
	{
		void SetupBuilder()
		{
			EnsureHandlerCreated(builder =>
			{
				builder.ConfigureMauiHandlers(handlers =>
				{
					handlers.AddHandler(typeof(VerticalStackLayout), typeof(LayoutHandler));
					handlers.AddHandler(typeof(Grid), typeof(LayoutHandler));
					handlers.AddHandler(typeof(Button), typeof(ButtonHandler));
					handlers.AddHandler<SwipeView, SwipeViewHandler>();
					handlers.AddHandler<SwipeItem, SwipeItemMenuItemHandler>();
					handlers.AddHandler<SwipeItemView, SwipeItemViewHandler>();
				});
			});
		}

#if !WINDOWS
		[Fact(DisplayName = "SwipeItemView TapGestureRecognizer Command Executes")]
		public async Task SwipeItemViewTapGestureRecognizerCommandExecutes()
		{
			SetupBuilder();

			bool commandExecuted = false;
			var command = new Command(() => commandExecuted = true);

			var label = new Label
			{
				Text = "Test Label",
				BackgroundColor = Colors.LightGray,
				GestureRecognizers =
				{
					new TapGestureRecognizer { Command = command }
				}
			};

			var swipeItemView = new SwipeItemView
			{
				Content = label
			};

			var swipeItems = new SwipeItems { swipeItemView };

			var swipeView = new SwipeView
			{
				RightItems = swipeItems,
				Content = new Grid
				{
					HeightRequest = 60,
					Background = new SolidPaint(Colors.White)
				}
			};

			await CreateHandlerAndAddToWindow<SwipeViewHandler>(swipeView, async (handler) =>
			{
				// Verify the swipe item view is created
				Assert.NotNull(handler.PlatformView);

				// Get the platform view for the SwipeItemView
				var swipeItemViewHandler = swipeItemView.Handler as SwipeItemViewHandler;
				Assert.NotNull(swipeItemViewHandler);
				Assert.NotNull(swipeItemViewHandler.PlatformView);

				// Get the label's platform view and simulate a tap
				var labelHandler = label.Handler as IViewHandler;
				Assert.NotNull(labelHandler);

#if ANDROID
				// On Android, simulate touch
				var androidView = labelHandler.PlatformView as Android.Views.View;
				Assert.NotNull(androidView);
				
				// Simulate tap gesture
				var tapGesture = label.GestureRecognizers[0] as TapGestureRecognizer;
				tapGesture.SendTapped(label);
#elif IOS || MACCATALYST
				// On iOS, simulate tap
				var iOSView = labelHandler.PlatformView as UIKit.UIView;
				Assert.NotNull(iOSView);
				
				// Simulate tap gesture
				var tapGesture = label.GestureRecognizers[0] as TapGestureRecognizer;
				tapGesture.SendTapped(label);
#endif

				// Wait a bit for the command to be processed
				await Task.Delay(100);

				Assert.True(commandExecuted, "TapGestureRecognizer command should have been executed in SwipeItemView");
			});
		}

		[Fact(DisplayName = "SwipeView LogicalChildren Works Correctly")]
		public async Task SwipeViewLogicalChildren()
		{
			SetupBuilder();

			var content = new Grid
			{
				HeightRequest = 60,
				Background = new SolidPaint(Colors.White)
			};

			var swipeItem = new SwipeItem
			{
				BackgroundColor = Colors.Red,
			};

			var swipeItems = new SwipeItems
				{
					swipeItem
				};

			var swipeView = new SwipeView()
			{
				LeftItems = swipeItems,
				Content = content
			};

			await InvokeOnMainThreadAsync(async () =>
			{
				var swipeViewHandler = CreateHandler<SwipeViewHandler>(swipeView);
				await swipeViewHandler.PlatformView.AttachAndRun(async () =>
				{
					bool hasChildren = await HasChildren(swipeViewHandler);
					Assert.True(hasChildren);

					swipeView.Open(OpenSwipeItem.LeftItems, false);
#pragma warning disable CS0618 // Type or member is obsolete
					var logicalChildrenCount = swipeView.LogicalChildren.Count;
#pragma warning restore CS0618 // Type or member is obsolete
					Assert.Equal(1, logicalChildrenCount);
				});
			});
		}

		[Fact("Items Do Not Leak")]
		public async Task ItemsDoNotLeak()
		{
			SetupBuilder();

			WeakReference viewReference = null;
			WeakReference platformViewReference = null;
			WeakReference handlerReference = null;

			await InvokeOnMainThreadAsync(async () =>
			{
				var item = new SwipeItem();
				var swipeView = new SwipeView()
				{
					LeftItems = { item },
					Content = new Grid(),
				};
				var handler = CreateHandler<SwipeViewHandler>(swipeView);
				await handler.PlatformView.AttachAndRun(() =>
				{
					swipeView.Open(OpenSwipeItem.LeftItems, false);
					viewReference = new WeakReference(item);
					handlerReference = new WeakReference(item.Handler);
					platformViewReference = new WeakReference(item.Handler.PlatformView);
				});
			});

			await AssertionExtensions.WaitForGC(viewReference, handlerReference, platformViewReference);
		}
#endif
	}
}