using Windows.System;
using Windows.UI.Xaml.Controls;
using Uno.UI.Samples.Controls;

namespace UITests.Shared.Windows_UI_Xaml_Input.Keyboard
{
	[SampleControlInfo("Keyboard", nameof(Keyboard_Events))]
	public sealed partial class Keyboard_Events : Page
	{
		public Keyboard_Events()
		{
			this.InitializeComponent();

			input.PreviewKeyDown += (snd, evt) =>
			{
				var filtered = filterPreviewKeyDown.IsChecked ?? false;
				Write("PreviewKeyDown received" + (filtered ? " HANDLED" : ""));
				evt.Handled = filtered;
			};
			input.KeyDown += (snd, evt) =>
			{
				var filtered = filterKeyDown.IsChecked ?? false;
				Write("KeyDown received" + (filtered ? " HANDLED" : ""));
				evt.Handled = filtered;
			};
			input.PreviewKeyUp += (snd, evt) =>
			{
				var filtered = filterPreviewKeyUp.IsChecked ?? false;
				Write("PreviewKeyUp received" + (filtered ? " HANDLED" : ""));
				evt.Handled = filtered;
			};
			input.KeyUp += (snd, evt) =>
			{
				var filtered = filterKeyUp.IsChecked ?? false;
				Write("KeyUp received" + (filtered ? " HANDLED" : ""));
				evt.Handled = filtered;
			};

			PreviewKeyDown += (snd, evt) => Write("Control PreviewKeyDown");
			KeyDown += (snd, evt) => Write("Control KeyDown");
			PreviewKeyUp += (snd, evt) => Write("Control PreviewKeyUp");
			KeyUp += (snd, evt) => Write("Control KeyUp");
		}

		private void Write(string msg)
		{
			output.Text += msg + "\n";
		}
	}
}
