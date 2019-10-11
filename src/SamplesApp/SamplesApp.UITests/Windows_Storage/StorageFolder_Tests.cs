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
	[ActivePlatforms(Platform.iOS, Platform.Android, Platform.Browser)]
	[TestFixture]
	public class StorageFolder_Tests : SampleControlUITestBase
	{
		[Test]
		[AutoRetry]
		public void WorkInProgress()
		{
			Run("UITests.Shared.Windows_Storage.StorageFolder.WorkInProgress");

			_app.Repl();

		}

	}
}
