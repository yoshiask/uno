using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Uno.UI.Samples.Controls;
using System.Reactive.Concurrency;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UITests.Shared.Windows_UI_Xaml_Media_Animation
{
	private int _clickTotal = 0;

	private RotateTransform _renderRotateTransform = new RotateTransform();
	private SkewTransform _renderSkewTransform = new SkewTransform();
	private ScaleTransform _renderScaleTransform = new ScaleTransform();
	private TranslateTransform _renderTranslateTransform = new TranslateTransform();
	private TransformGroup _renderTransformGroup = new TransformGroup();

	private EasingFunctionBase _easingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseIn };
	private Duration _duration = new Duration(TimeSpan.FromSeconds(0.5));
	private Storyboard _storyboard = new Storyboard();
	private IScheduler _uiScheduler;

	[SampleControlInfo("Animations", nameof(ButtonAnimationTest_Composite_Automated))]
	public sealed partial class ButtonAnimationTest_Composite_Automated : UserControl
	{
		public ButtonAnimationTest_Composite_Automated()
		{
			this.InitializeComponent();
			this.DataContext = this;

			this.Loaded += Initialize;

#if XAMARIN
			_uiScheduler = CoreDispatcherScheduler.MainNormal;
#else
			_uiScheduler = new CoreDispatcherScheduler(Dispatcher);
#endif
		}

		private void Initialize(object sender, RoutedEventArgs e)
		{
			this.Loaded -= Initialize;
			this.StartAnimationButton.Command = new Common.DelegateCommand(AnimateButton_Click);
		}

		private void IncreaseClick(object sender, RoutedEventArgs e)
		{
			_clickTotal++;
			TotalClicks.Text = _clickTotal.ToString();
		}

		private void AnimateButton_Click()
		{
			PrepareAnimationTarget();
			LaunchAnimation();
		}

		private void PrepareAnimationTarget()
		{
			_renderTransformGroup.Children.Add(_renderTranslateTransform);
			_renderTransformGroup.Children.Add(_renderRotateTransform);
			_renderTransformGroup.Children.Add(_renderScaleTransform);
			_renderTransformGroup.Children.Add(_renderSkewTransform);

			this.MyCompositeButton.RenderTransform = _renderTransformGroup;
		}

		private void LaunchAnimation()
		{
			_storyboard.Stop();

			var anim_rotation = new DoubleAnimation()
			{
				From = 0,
				To = 15,
				EasingFunction = _easingFunction,
				Duration = _duration
			};

			var anim_skew = new DoubleAnimation()
			{
				From = 0,
				To = 15,
				EasingFunction = _easingFunction,
				Duration = _duration
			};

			var anim_translate = new DoubleAnimation()
			{
				From = 0,
				To = 15,
				EasingFunction = _easingFunction,
				Duration = _duration
			};

			var anim_scale = new DoubleAnimation()
			{
				From = 1,
				To = 1.5,
				EasingFunction = _easingFunction,
				Duration = _duration
			};

			Storyboard.SetTarget(anim_rotation, _renderRotateTransform);
			Storyboard.SetTargetProperty(anim_rotation, "Angle");

			Storyboard.SetTarget(anim_skew, _renderSkewTransform);
			Storyboard.SetTargetProperty(anim_skew, "AngleY");

			Storyboard.SetTarget(anim_translate, _renderTranslateTransform);
			Storyboard.SetTargetProperty(anim_translate, "Y");

			Storyboard.SetTarget(anim_scale, _renderScaleTransform);
			Storyboard.SetTargetProperty(anim_scale, "ScaleY");

			_storyboard.Children.Add(anim_rotation);
			_storyboard.Children.Add(anim_skew);
			_storyboard.Children.Add(anim_translate);
			_storyboard.Children.Add(anim_scale);

			_storyboard.Begin();
		}

		private void ResetAnimation()
		{
			if (_storyboard == null)
			{
				return;
			}

			_storyboard.Stop();
		}
	}
}
