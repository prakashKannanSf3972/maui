using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue29484 : _IssuesUITest
{
	public Issue29484(TestDevice testDevice) : base(testDevice)
	{
	}

	public override string Issue => "CollectionView Selected state does not work on the selected item when combined with PointerOver";

	[Test]
	[Category(UITestCategories.CollectionView)]
	public void CollectionViewPointerOverAndSelectedState()
	{
		App.WaitForElement("PointerOverSelectView");
		App.Tap("Item 1");
		App.Tap("Item 3");
		VerifyScreenshot();
	}
}