using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace MahApps.Metro.Actions
{
	public class SetFlyoutOpenAction : TargetedTriggerAction<FrameworkElement>
	{
		public readonly static DependencyProperty ValueProperty;

		public bool Value
		{
			get
			{
				return (bool)base.GetValue(SetFlyoutOpenAction.ValueProperty);
			}
			set
			{
				base.SetValue(SetFlyoutOpenAction.ValueProperty, value);
			}
		}

		static SetFlyoutOpenAction()
		{
			Class6.yDnXvgqzyB5jw();
			SetFlyoutOpenAction.ValueProperty = DependencyProperty.Register("Value", typeof(bool), typeof(SetFlyoutOpenAction), new PropertyMetadata(false));
		}

		public SetFlyoutOpenAction()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		protected override void Invoke(object parameter)
		{
			((Flyout)base.TargetObject).IsOpen = this.Value;
		}
	}
}