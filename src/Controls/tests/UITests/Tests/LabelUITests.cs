using Maui.Controls.Sample;
using Microsoft.Maui.Appium;
using NUnit.Framework;

namespace Microsoft.Maui.AppiumTests;

public class LabelUITests : _ViewUITests
{
	static readonly string Label = "android.widget.TextView";
	const string LabelGallery = "* marked:'Label Gallery'";

	public LabelUITests(TestDevice device)
		: base(device)
	{
		PlatformViewType = Label;
	}

	protected override void NavigateToGallery() =>
		App.NavigateToGallery(LabelGallery);

	[Test]
	public void SpanTapped()
	{
		var remote = new EventViewContainerRemote(UITestContext, Test.FormattedString.SpanTapped, PlatformViewType);
		remote.GoTo();

		var textBeforeClick = remote.GetEventLabel().Text;
		Assert.AreEqual("Event: SpanTapped (none)", textBeforeClick);

		var r = remote.GetView().Rect;
		App.TapCoordinates(r.X + (r.Height * 3), r.CenterY); // 3 "heights" in from the left

		var textAfterClick = remote.GetEventLabel().Text;
		Assert.AreEqual("Event: SpanTapped (fired 1)", textAfterClick);
	}
}
