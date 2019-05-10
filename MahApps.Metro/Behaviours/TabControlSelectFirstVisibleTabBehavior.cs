using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace MahApps.Metro.Behaviours
{
	public class TabControlSelectFirstVisibleTabBehavior : Behavior<TabControl>
	{
		public TabControlSelectFirstVisibleTabBehavior()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		protected override void OnAttached()
		{
			base.AssociatedObject.SelectionChanged += new SelectionChangedEventHandler(this.OnSelectionChanged);
		}

		protected override void OnDetaching()
		{
			base.AssociatedObject.SelectionChanged -= new SelectionChangedEventHandler(this.OnSelectionChanged);
		}

		private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			List<TabItem> list = base.AssociatedObject.Items.Cast<TabItem>().ToList<TabItem>();
			TabItem selectedItem = base.AssociatedObject.SelectedItem as TabItem;
			if (selectedItem != null && selectedItem.Visibility == Visibility.Visible)
			{
				return;
			}
			TabItem tabItem = list.FirstOrDefault<TabItem>((TabItem t) => t.Visibility == Visibility.Visible);
			if (tabItem == null)
			{
				base.AssociatedObject.SelectedItem = null;
				return;
			}
			base.AssociatedObject.SelectedIndex = list.IndexOf(tabItem);
		}
	}
}