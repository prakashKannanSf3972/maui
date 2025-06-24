#nullable enable
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WRect = global::Windows.Foundation.Rect;
using WSize = global::Windows.Foundation.Size;
using WSolidColorBrush = Microsoft.UI.Xaml.Media.SolidColorBrush;

namespace Microsoft.Maui.Platform
{
	public class LayoutPanel : MauiPanel
	{
		public bool ClipsToBounds { get; set; }

		// TODO: Possibly reconcile this code with ViewHandlerExtensions.LayoutVirtualView
		// If you make changes here please review if those changes should also
		// apply to ViewHandlerExtensions.LayoutVirtualView
		protected override WSize ArrangeOverride(WSize finalSize)
		{
			var actual = base.ArrangeOverride(finalSize);

			if (!(Parent is ContentPanel contentPanel && contentPanel.BorderStroke?.Shape is not null))
			{
				Clip = ClipsToBounds ? new RectangleGeometry { Rect = new WRect(0, 0, finalSize.Width, finalSize.Height) } : null;
			}

			return actual;
		}

		public void UpdateInputTransparent(bool inputTransparent, Brush? background)
		{
			// Set hit test visibility based on input transparency
			IsHitTestVisible = !inputTransparent;

			SetBackground(background);
		}

		void SetBackground(Brush? background)
		{
			if (background is null)
			{
				// We can't have a null background, because that would allow input through
				// So we'll make the background color transparent (visually the same as null, but consumes input)
				Background = new WSolidColorBrush(UI.Colors.Transparent);
			}
			else
			{
				Background = background;
			}
		}
	}
}