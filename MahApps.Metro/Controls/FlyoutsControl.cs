using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	[StyleTypedProperty(Property="ItemContainerStyle", StyleTargetType=typeof(Flyout))]
	public class FlyoutsControl : ItemsControl
	{
		public readonly static DependencyProperty OverrideExternalCloseButtonProperty;

		public readonly static DependencyProperty OverrideIsPinnedProperty;

		public MouseButton? OverrideExternalCloseButton
		{
			get
			{
				return (MouseButton?)base.GetValue(FlyoutsControl.OverrideExternalCloseButtonProperty);
			}
			set
			{
				base.SetValue(FlyoutsControl.OverrideExternalCloseButtonProperty, value);
			}
		}

		public bool OverrideIsPinned
		{
			get
			{
				return (bool)base.GetValue(FlyoutsControl.OverrideIsPinnedProperty);
			}
			set
			{
				base.SetValue(FlyoutsControl.OverrideIsPinnedProperty, value);
			}
		}

		static FlyoutsControl()
		{
			Class6.yDnXvgqzyB5jw();
			FlyoutsControl.OverrideExternalCloseButtonProperty = DependencyProperty.Register("OverrideExternalCloseButton", typeof(MouseButton?), typeof(FlyoutsControl), new PropertyMetadata(null));
			FlyoutsControl.OverrideIsPinnedProperty = DependencyProperty.Register("OverrideIsPinned", typeof(bool), typeof(FlyoutsControl), new PropertyMetadata(false));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FlyoutsControl), new FrameworkPropertyMetadata(typeof(FlyoutsControl)));
		}

		public FlyoutsControl()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		private void AttachHandlers(Flyout flyout)
		{
			PropertyChangeNotifier propertyChangeNotifier = new PropertyChangeNotifier(flyout, Flyout.IsOpenProperty);
			propertyChangeNotifier.ValueChanged += new EventHandler(this.FlyoutStatusChanged);
			flyout.IsOpenPropertyChangeNotifier = propertyChangeNotifier;
			PropertyChangeNotifier propertyChangeNotifier1 = new PropertyChangeNotifier(flyout, Flyout.ThemeProperty);
			propertyChangeNotifier1.ValueChanged += new EventHandler(this.FlyoutStatusChanged);
			flyout.ThemePropertyChangeNotifier = propertyChangeNotifier1;
		}

		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			((Flyout)element).CleanUp(this);
			base.ClearContainerForItemOverride(element, item);
		}

		private void FlyoutStatusChanged(object sender, EventArgs e)
		{
			Flyout flyout = this.GetFlyout(sender);
			this.HandleFlyoutStatusChange(flyout, this.TryFindParent<MetroWindow>());
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new Flyout();
		}

		private Flyout GetFlyout(object item)
		{
			Flyout flyout = item as Flyout;
			if (flyout != null)
			{
				return flyout;
			}
			return (Flyout)base.ItemContainerGenerator.ContainerFromItem(item);
		}

		internal IEnumerable<Flyout> GetFlyouts()
		{
			return this.GetFlyouts(base.Items);
		}

		private IEnumerable<Flyout> GetFlyouts(IEnumerable items)
		{
			return 
				from object item in items
				select this.GetFlyout(item);
		}

		internal void HandleFlyoutStatusChange(Flyout flyout, MetroWindow parentWindow)
		{
			if (flyout == null || parentWindow == null)
			{
				return;
			}
			this.ReorderZIndices(flyout);
			IOrderedEnumerable<Flyout> flyouts = (
				from i in this.GetFlyouts(base.Items)
				where i.IsOpen
				select i).OrderBy<Flyout, int>(new Func<Flyout, int>(Panel.GetZIndex));
			parentWindow.HandleFlyoutStatusChange(flyout, flyouts);
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is Flyout;
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			this.AttachHandlers((Flyout)element);
		}

		private void ReorderZIndices(Flyout lastChanged)
		{
			IOrderedEnumerable<Flyout> flyouts = this.GetFlyouts(base.Items).Where<Flyout>((Flyout i) => {
				if (!i.IsOpen)
				{
					return false;
				}
				return i != lastChanged;
			}).OrderBy<Flyout, int>(new Func<Flyout, int>(Panel.GetZIndex));
			int num = 0;
			foreach (Flyout flyout in flyouts)
			{
				Panel.SetZIndex(flyout, num);
				num++;
			}
			if (lastChanged.IsOpen)
			{
				Panel.SetZIndex(lastChanged, num);
			}
		}
	}
}