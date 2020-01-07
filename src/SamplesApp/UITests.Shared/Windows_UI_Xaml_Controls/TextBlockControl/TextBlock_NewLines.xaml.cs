using System;
using Windows.UI.Xaml.Controls;
using Uno.UI.Samples.Controls;

namespace UITests.Shared.Windows_UI_Xaml_Controls.TextBlockControl
{
    [SampleControlInfo]
    public sealed partial class TextBlock_NewLines : Page
    {
        public TextBlock_NewLines()
        {
            this.InitializeComponent();
            txtAssignedInCodeBehind.Text = string.Format("Line 1{0}Line 2{0}Line 3{0}Line 4", Environment.NewLine);
        }
    }
}
