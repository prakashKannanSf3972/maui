using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	public class DynamicResourceNestedBindableObjectTests : BaseTestFixture
	{
		// Test classes that replicate the issue scenario
		public class CustomEntryStyle : BindableObject
		{
			public static readonly BindableProperty TextColorProperty =
				BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CustomEntryStyle), Colors.Black);

			public Color TextColor
			{
				get { return (Color)this.GetValue(TextColorProperty); }
				set { this.SetValue(TextColorProperty, value); }
			}
		}

		public class CustomEntry : Entry
		{
			public CustomEntry()
			{
				this.CustomEntryTextStyle = new CustomEntryStyle();
			}

			public static readonly BindableProperty CustomEntryTextStyleProperty =
				BindableProperty.Create(nameof(CustomEntryTextStyle), typeof(CustomEntryStyle), typeof(CustomEntry), null, BindingMode.TwoWay, null, propertyChanged: OnCustomTextStylePropertyChanged);

			private static void OnCustomTextStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
			{
				if (bindable is CustomEntry entry)
				{
					entry.TextColor = entry.CustomEntryTextStyle.TextColor;
				}
			}

			public CustomEntryStyle CustomEntryTextStyle
			{
				get { return (CustomEntryStyle)GetValue(CustomEntryTextStyleProperty); }
				set { SetValue(CustomEntryTextStyleProperty, value); }
			}
		}

		public DynamicResourceNestedBindableObjectTests()
		{
			Application.Current = new MockApplication();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Application.Current = null;
			}

			base.Dispose(disposing);
		}

		[Fact]
		public void StaticResourceWorksForNestedBindableObject()
		{
			// Arrange
			var customEntry = new CustomEntry();
			var page = new ContentPage
			{
				Resources = new ResourceDictionary
				{
					{ "redColor", Colors.Red }
				},
				Content = customEntry
			};

			// Act - Set StaticResource (this should work)
			customEntry.CustomEntryTextStyle.TextColor = (Color)page.Resources["redColor"];

			// Assert
			Assert.Equal(Colors.Red, customEntry.CustomEntryTextStyle.TextColor);
			Assert.Equal(Colors.Red, customEntry.TextColor);
		}

		[Fact]
		public void DynamicResourceShouldWorkForNestedBindableObject()
		{
			// Arrange
			var customEntry = new CustomEntry();
			var page = new ContentPage
			{
				Resources = new ResourceDictionary
				{
					{ "redColor", Colors.Red }
				},
				Content = customEntry
			};

			// Act - Set DynamicResource (this is currently broken)
			customEntry.CustomEntryTextStyle.SetDynamicResource(CustomEntryStyle.TextColorProperty, "redColor");

			// Assert - This test should currently fail, proving the issue
			Assert.Equal(Colors.Red, customEntry.CustomEntryTextStyle.TextColor);
			Assert.Equal(Colors.Red, customEntry.TextColor);
		}

		[Fact]
		public void DynamicResourceShouldUpdateWhenResourceChanges()
		{
			// Arrange
			var customEntry = new CustomEntry();
			var page = new ContentPage
			{
				Resources = new ResourceDictionary
				{
					{ "dynamicColor", Colors.Red }
				},
				Content = customEntry
			};

			// Set DynamicResource
			customEntry.CustomEntryTextStyle.SetDynamicResource(CustomEntryStyle.TextColorProperty, "dynamicColor");

			// Verify initial value
			Assert.Equal(Colors.Red, customEntry.CustomEntryTextStyle.TextColor);

			// Act - Change the resource value
			page.Resources["dynamicColor"] = Colors.Blue;

			// Assert - Should update automatically (currently broken)
			Assert.Equal(Colors.Blue, customEntry.CustomEntryTextStyle.TextColor);
			Assert.Equal(Colors.Blue, customEntry.TextColor);
		}

		[Fact]
		public void DynamicResourceShouldWorkWhenResourceAddedLater()
		{
			// Arrange
			var customEntry = new CustomEntry();
			var page = new ContentPage
			{
				Resources = new ResourceDictionary(),
				Content = customEntry
			};

			// Act - Set DynamicResource before resource exists
			customEntry.CustomEntryTextStyle.SetDynamicResource(CustomEntryStyle.TextColorProperty, "laterColor");

			// Verify initial value is default
			Assert.Equal(Colors.Black, customEntry.CustomEntryTextStyle.TextColor);

			// Add the resource later
			page.Resources["laterColor"] = Colors.Green;

			// Assert - Should update when resource is added (currently broken)
			Assert.Equal(Colors.Green, customEntry.CustomEntryTextStyle.TextColor);
			Assert.Equal(Colors.Green, customEntry.TextColor);
		}
	}
}