using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue27959 : _IssuesUITest
{
	public override string Issue => "Dynamically toggling the Header/Footer between a null and a non-null value in CollectionView is not working";

	public Issue27959(TestDevice device) : base(device)
	{
	}

	[Test, Order(1)]
	[Category(UITestCategories.CollectionView)]
	public void HeaderFooterToggleNullToNonNull()
	{
		App.WaitForElement("CollectionViewButton");
		App.Click("CollectionViewButton");
		App.WaitForElement("CollectionView");
		App.Click("ToggleHeaderButton");
		App.WaitForNoElement("Header");
		App.Click("ToggleHeaderButton");
		App.WaitForElement("Header");
		App.Click("ToggleFooterButton");
		App.WaitForNoElement("Footer");
		App.Click("ToggleFooterButton");
		App.WaitForElement("Footer");
	}

	[Test, Order(2)]
	[Category(UITestCategories.CollectionView)]
	public void TemplateHeaderFooterToggleNullToNonNull()
	{
		App.WaitForElement("CollectionViewTemplatedButton");
		App.Click("CollectionViewTemplatedButton");
		App.WaitForElement("CollectionViewTemplate");
		App.Click("ToggleHeaderTemplateButton");
		App.WaitForNoElement("HeaderTemplate");
		App.Click("ToggleHeaderTemplateButton");
		App.WaitForElement("HeaderTemplate");
	}
}
