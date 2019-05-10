using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MahApps.Metro.Converters
{
	public static class TreeViewItemExtensions
	{
		public static int GetDepth(this TreeViewItem item)
		{
			TreeViewItem parent = TreeViewItemExtensions.GetParent(item);
			TreeViewItem treeViewItem = parent;
			if (parent == null)
			{
				return 0;
			}
			return treeViewItem.GetDepth() + 1;
		}

		private static TreeViewItem GetParent(TreeViewItem item)
		{
			DependencyObject parent;
			if (item != null)
			{
				parent = VisualTreeHelper.GetParent(item);
			}
			else
			{
				parent = null;
			}
			DependencyObject dependencyObject = parent;
			while (dependencyObject != null && !(dependencyObject is TreeViewItem) && !(dependencyObject is TreeView))
			{
				dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
			}
			return dependencyObject as TreeViewItem;
		}
	}
}