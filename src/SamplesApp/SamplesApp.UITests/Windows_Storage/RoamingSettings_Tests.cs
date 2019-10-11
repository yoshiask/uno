using NUnit.Framework;
using SamplesApp.UITests.TestFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uno.UITest.Helpers;
using Uno.UITest.Helpers.Queries;



namespace SamplesApp.UITests.Windows_Storage
{
	[ActivePlatforms(Platform.Android, Platform.Browser, Platform.iOS)]
	[TestFixture]
	public class RoamingSettings_Tests : SampleControlUITestBase
	{
		[Test]
		[AutoRetry]
		public void ClearAddContainsRemove()
		{
			// Navigate to this x:Class control name
			Run("UITests.Shared.Windows_Storage_ApplicationData.LocalSettings");

			// Define elements that will be interacted with at the start of the test
			var containerName = _app.Marked("ContainerName");
			var clearButton = _app.Marked("ClearButton");
			var addButton = _app.Marked("AddButton");
			var containsButton = _app.Marked("ContainsButton");
			var removeButton = _app.Marked("RemoveButton");
			var output = _app.Marked("Output");

			// Specify what user interface element to wait on before starting test execution
			_app.WaitForElement(clearButton);

			// Take an initial screenshot
			_app.Screenshot(nameof(ClearAddContainsRemove) + " - Initial State");

			// Assert initial state
			Assert.AreEqual("Local", containerName.GetDependencyPropertyValue("Text")?.ToString());
			Assert.AreEqual(string.Empty, output.GetDependencyPropertyValue("Text")?.ToString());

			{
				_app.Tap(clearButton);
				_app.Screenshot(nameof(ClearAddContainsRemove) + " - Clear Button");
				_app.WaitForDependencyPropertyValue(output, "Text", 0);
			}

			{
				_app.Tap(addButton);
				_app.Screenshot(nameof(ClearAddContainsRemove) + " - Add Button");
				_app.WaitForDependencyPropertyValue(output, "Text", 1);
			}

			{
				_app.Tap(containsButton);
				_app.Screenshot(nameof(ClearAddContainsRemove) + " - Contains Button");
				_app.WaitForDependencyPropertyValue(output, "Text", "True");
			}

			{
				_app.Tap(removeButton);
				_app.Screenshot(nameof(ClearAddContainsRemove) + " - Remove Button");
				_app.WaitForDependencyPropertyValue(output, "Text", 0);
			}
		}
	}
}
