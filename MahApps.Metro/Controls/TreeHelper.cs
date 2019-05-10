using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public static class TreeHelper
	{
		public static T FindChild<T>(this DependencyObject parent, string childName)
		where T : DependencyObject
		{
			if (parent == null)
			{
				return default(T);
			}
			T t = default(T);
			int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
			int num = 0;
			while (true)
			{
				if (num < childrenCount)
				{
					DependencyObject child = VisualTreeHelper.GetChild(parent, num);
					if ((T)(child as T) == null)
					{
						t = child.FindChild<T>(childName);
						if (t != null)
						{
							break;
						}
					}
					else if (string.IsNullOrEmpty(childName))
					{
						t = (T)child;
						break;
					}
					else
					{
						FrameworkElement frameworkElement = child as FrameworkElement;
						if (frameworkElement != null && frameworkElement.Name == childName)
						{
							t = (T)child;
							break;
						}
					}
					num++;
				}
				else
				{
					break;
				}
			}
			return t;
		}

		public static IEnumerable<T> FindChildren<T>(this DependencyObject source, bool forceUsingTheVisualTreeHelper = false)
		where T : DependencyObject
		{
			return new TreeHelper.<FindChildren>d__3<T>(-2)
			{
				<>3__source = source,
				<>3__forceUsingTheVisualTreeHelper = forceUsingTheVisualTreeHelper
			};
		}

		public static IEnumerable<DependencyObject> GetChildObjects(DependencyObject parent, bool forceUsingTheVisualTreeHelper = false)
		{
			return new TreeHelper.<GetChildObjects>d__4(-2)
			{
				<>3__parent = parent,
				<>3__forceUsingTheVisualTreeHelper = forceUsingTheVisualTreeHelper
			};
		}

		public static DependencyObject GetParentObject(this DependencyObject child)
		{
			if (child == null)
			{
				return null;
			}
			ContentElement contentElement = child as ContentElement;
			if (contentElement == null)
			{
				FrameworkElement frameworkElement = child as FrameworkElement;
				if (frameworkElement != null)
				{
					DependencyObject parent = frameworkElement.Parent;
					if (parent != null)
					{
						return parent;
					}
				}
				return VisualTreeHelper.GetParent(child);
			}
			DependencyObject dependencyObject = ContentOperations.GetParent(contentElement);
			if (dependencyObject != null)
			{
				return dependencyObject;
			}
			FrameworkContentElement frameworkContentElement = contentElement as FrameworkContentElement;
			if (frameworkContentElement == null)
			{
				return null;
			}
			return frameworkContentElement.Parent;
		}

		public static T TryFindFromPoint<T>(UIElement reference, Point point)
		where T : DependencyObject
		{
			DependencyObject dependencyObject = reference.InputHitTest(point) as DependencyObject;
			if (dependencyObject == null)
			{
				return default(T);
			}
			if (dependencyObject is T)
			{
				return (T)dependencyObject;
			}
			return dependencyObject.TryFindParent<T>();
		}

		public static T TryFindParent<T>(this DependencyObject child)
		where T : DependencyObject
		{
			DependencyObject parentObject = child.GetParentObject();
			if (parentObject == null)
			{
				return default(T);
			}
			T t = (T)(parentObject as T);
			if (t == null)
			{
				t = parentObject.TryFindParent<T>();
			}
			return t;
		}
	}
}