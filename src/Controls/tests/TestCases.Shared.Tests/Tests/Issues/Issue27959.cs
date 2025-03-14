#if TEST_FAILS_ON_WINDOWS
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
		App.WaitForElement("EmptyViewButton").Click();
		App.WaitForElement("ToggleHeaderButton").Click();
		App.WaitForNoElement("Header");
		App.Click("ToggleHeaderButton");
		App.WaitForElement("Header");
		App.Click("ToggleFooterButton");
		App.WaitForNoElement("Footer");
		App.Click("ToggleFooterButton");
		App.WaitForElement("Footer");
		App.Back();
	}

	[Test, Order(2)]
	[Category(UITestCategories.CollectionView)]
	public void TemplateHeaderFooterToggleNullToNonNull()
	{
		App.WaitForElement("EmptyViewViewTemplatedButton").Click();
		App.WaitForElement("ToggleHeaderTemplateButton").Click();
		App.WaitForNoElement("HeaderTemplate");
		App.Click("ToggleHeaderTemplateButton");
		App.WaitForElement("HeaderTemplate");
		App.Back();
	}

	[Test, Order(3)]
	[Category(UITestCategories.CollectionView)]
	public void ItemsViewHeaderFooterToggleNullToNonNull()
	{
		App.WaitForElement("ItemsViewTemplatedButton").Click();
		App.WaitForElement("ToggleHeaderTemplateButton").Click();
		App.WaitForNoElement("ItemsHeaderTemplate");
		App.Click("ToggleHeaderTemplateButton");
		App.WaitForElement("ItemsHeaderTemplate");
		App.Click("ToggleFooterTemplateButton");
		App.WaitForNoElement("ItemsFooterTemplate");
		App.Click("ToggleFooterTemplateButton");
		App.WaitForElement("ItemsFooterTemplate");
	}
}
#endif