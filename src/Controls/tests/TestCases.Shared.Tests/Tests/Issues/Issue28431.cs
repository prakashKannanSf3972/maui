using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue28431 : _IssuesUITest
{
	public Issue28431(TestDevice testDevice) : base(testDevice)
	{
	}
	public override string Issue => "In the template of CollectionView, the margin settings of the table are displayed inconsistently";

	[Test, Order(1)]
	[Category(UITestCategories.CollectionView)]
	public void VerifyCollectionTemplateInitialMargin()
	{
		App.WaitForElement("MainGrid");
		VerifyScreenshot();
	}

	[Test, Order(2)]
	[Category(UITestCategories.CollectionView)]
	public void VerifyCollectionTemplateDynamicMarginUpdate()
	{
		App.WaitForElement("ChangeMargin");
		App.Click("ChangeMargin");
		VerifyScreenshot();
	}
}
