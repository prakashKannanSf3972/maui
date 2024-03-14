﻿/*
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace UITests
{
	public class Bugzilla31255 : IssuesUITest
	{
		public Bugzilla31255(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "Flyout's page Icon cause memory leak after FlyoutPage is popped out by holding on page";

		[Test]
		[Ignore("The sample is crashing.")]
		[Category(UITestCategories.Navigation)]
		public async Task Bugzilla31255Test()
		{
			this.IgnoreIfPlatforms([TestDevice.Android, TestDevice.iOS, TestDevice.Mac, TestDevice.Windows], "The sample is crashing. More information: https://github.com/dotnet/maui/issues/21206");

			RunningApp.Screenshot("I am at Bugzilla 31255");
			await Task.Delay(5000);
			RunningApp.WaitForNoElement("Page1. But Page2 IsAlive = False");
		}
	}
}
*/