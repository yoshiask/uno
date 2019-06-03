using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Uno.UI.Tests.Windows_UI_Xaml
{
	[TestClass]
	public partial class Given_VisualStateGroup
	{
		[TestMethod]
		public void When_GoToState_Setters_Reset()
		{
			var sut = new Sut();
			var state1 = new VisualState
			{
				Setters = { new Setter(Sut.Property1Property, "Property1_State1"), }
			};
			var state2 = new VisualState();
			var group = new VisualStateGroup {States = {state1, state2}};

			group.GoToState(sut, originalState: null, state: state1, useTransitions: true, onStateChanged: () => { });
			group.GoToState(sut, originalState: state1, state: state2, useTransitions: true, onStateChanged: () => { });

			sut.Propery1ShouldBe("Property1_Default", changed: 2);
			sut.Propery2ShouldBe("Property2_Default", changed: 0);
		}

		[TestMethod]
		public void When_GoToState_Setters_DontResetIfUseless()
		{
			var sut = new Sut();
			var state1 = new VisualState
			{
				Setters = { new Setter(Sut.Property1Property, "Property1_State1"), }
			};
			var state2 = new VisualState
			{
				Setters = { new Setter(Sut.Property1Property, "Property1_State2"), }
			};
			var group = new VisualStateGroup { States = { state1, state2 } };

			group.GoToState(sut, originalState: state1, state: state2, useTransitions: true, onStateChanged: () => { });

			sut.Propery1ShouldBe("Property1_State2", changed: 2);
			sut.Propery2ShouldBe("Property2_Default", changed: 0);
		}

		[TestMethod]
		public void When_GoToState_Setters_MixResetAndDontResetIfUseless()
		{
			var sut = new Sut();
			var state1 = new VisualState
			{
				Setters =
				{
					new Setter(Sut.Property1Property, "Property1_State1"),
					new Setter(Sut.Property1Property, "Property2_State1")
				}
			};
			var state2 = new VisualState
			{
				Setters = { new Setter(Sut.Property1Property, "Property1_State2"), }
			};
			var group = new VisualStateGroup { States = { state1, state2 } };

			group.GoToState(sut, originalState: null, state: state1, useTransitions: true, onStateChanged: () => { });

			sut.Propery1ShouldBe("Property1_State1", changed: 1);
			sut.Propery2ShouldBe("Property2_State1", changed: 1);

			group.GoToState(sut, originalState: state1, state: state2, useTransitions: true, onStateChanged: () => { });

			sut.Propery1ShouldBe("Property1_State2", changed: 2);
			sut.Propery2ShouldBe("Property2_Default", changed: 2);
		}

		private partial class Sut : Control
		{
			public static readonly DependencyProperty Property1Property = DependencyProperty.Register(
				"Property1",
				typeof(string),
				typeof(Sut),
				new PropertyMetadata("Property1_Default", (snd, args) => ((Sut)snd).Property1ChangedCount++));

			public string Property1
			{
				get { return (string)GetValue(Property1Property); }
				set { SetValue(Property1Property, value); }
			}

			public int Property1ChangedCount { get; private set; }

			public static readonly DependencyProperty Property2Property = DependencyProperty.Register(
				"Property2",
				typeof(string),
				typeof(Sut),
				new PropertyMetadata("Property2_Default", (snd, args) => ((Sut)snd).Property2ChangedCount++));

			public string Property2
			{
				get { return (string)GetValue(Property2Property); }
				set { SetValue(Property2Property, value); }
			}

			public int Property2ChangedCount { get; private set; }

			public void Propery1ShouldBe(string value, int changed)
			{
				Property1.Should().Be(value);
				Property1ChangedCount.Should().Be(1);
			}

			public void Propery2ShouldBe(string value, int changed)
			{
				Property2.Should().Be(value);
				Property2ChangedCount.Should().Be(changed);
			}
		}
	}
}
