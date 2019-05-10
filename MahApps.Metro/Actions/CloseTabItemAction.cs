using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace MahApps.Metro.Actions
{
	public class CloseTabItemAction : TriggerAction<DependencyObject>
	{
		public readonly static DependencyProperty TabControlProperty;

		public readonly static DependencyProperty TabItemProperty;

		public System.Windows.Controls.TabControl TabControl
		{
			get
			{
				return (System.Windows.Controls.TabControl)base.GetValue(CloseTabItemAction.TabControlProperty);
			}
			set
			{
				base.SetValue(CloseTabItemAction.TabControlProperty, value);
			}
		}

		public System.Windows.Controls.TabItem TabItem
		{
			get
			{
				return (System.Windows.Controls.TabItem)base.GetValue(CloseTabItemAction.TabItemProperty);
			}
			set
			{
				base.SetValue(CloseTabItemAction.TabItemProperty, value);
			}
		}

		static CloseTabItemAction()
		{
			Class6.yDnXvgqzyB5jw();
			CloseTabItemAction.TabControlProperty = DependencyProperty.Register("TabControl", typeof(System.Windows.Controls.TabControl), typeof(CloseTabItemAction), new PropertyMetadata(null));
			CloseTabItemAction.TabItemProperty = DependencyProperty.Register("TabItem", typeof(System.Windows.Controls.TabItem), typeof(CloseTabItemAction), new PropertyMetadata(null));
		}

		public CloseTabItemAction()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		protected override void Invoke(object parameter)
		{
			this.TabControl.Items.Remove(this.TabItem);
		}
	}
}