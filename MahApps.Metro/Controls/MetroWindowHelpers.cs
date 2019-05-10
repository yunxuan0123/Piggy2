using Microsoft.Windows.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	internal static class MetroWindowHelpers
	{
		private static void ChangeAllWindowCommandsBrush(this MetroWindow window, Brush brush, Position position = 2)
		{
			if (brush == null)
			{
				window.InvokeActionOnWindowCommands((Control x) => x.SetValue(WindowCommands.ThemeProperty, Theme.Light), (Control x) => x.SetValue(WindowButtonCommands.ThemeProperty, Theme.Light), position);
				window.InvokeActionOnWindowCommands((Control x) => x.ClearValue(Control.ForegroundProperty), null, position);
				return;
			}
			Color color = ((SolidColorBrush)brush).Color;
			float r = (float)color.R / 255f;
			float g = (float)color.G / 255f;
			float b = (float)color.B / 255f;
			float single = r;
			float single1 = r;
			if (g > single)
			{
				single = g;
			}
			if (b > single)
			{
				single = b;
			}
			if (g < single1)
			{
				single1 = g;
			}
			if (b < single1)
			{
				single1 = b;
			}
			if ((double)((single + single1) / 2f) <= 0.1)
			{
				window.InvokeActionOnWindowCommands((Control x) => x.SetValue(WindowCommands.ThemeProperty, Theme.Dark), (Control x) => x.SetValue(WindowButtonCommands.ThemeProperty, Theme.Dark), position);
			}
			else
			{
				window.InvokeActionOnWindowCommands((Control x) => x.SetValue(WindowCommands.ThemeProperty, Theme.Light), (Control x) => x.SetValue(WindowButtonCommands.ThemeProperty, Theme.Light), position);
			}
			window.InvokeActionOnWindowCommands((Control x) => x.SetValue(Control.ForegroundProperty, brush), null, position);
		}

		public static void HandleWindowCommandsForFlyouts(this MetroWindow window, IEnumerable<Flyout> flyouts, Brush resetBrush = null)
		{
			IEnumerable<Flyout> flyouts1 = 
				from x in flyouts
				where x.IsOpen
				select x;
			if (!flyouts1.Any<Flyout>((Flyout x) => x.Position != Position.Bottom))
			{
				if (resetBrush != null)
				{
					window.ChangeAllWindowCommandsBrush(resetBrush, Position.Top);
				}
				else
				{
					window.ResetAllWindowCommandsBrush();
				}
			}
			Flyout flyout = (
				from x in flyouts1
				where x.Position == Position.Top
				select x).OrderByDescending<Flyout, int>(new Func<Flyout, int>(Panel.GetZIndex)).FirstOrDefault<Flyout>();
			if (flyout != null)
			{
				window.UpdateWindowCommandsForFlyout(flyout);
				return;
			}
			Flyout flyout1 = (
				from x in flyouts1
				where x.Position == Position.Left
				select x).OrderByDescending<Flyout, int>(new Func<Flyout, int>(Panel.GetZIndex)).FirstOrDefault<Flyout>();
			if (flyout1 != null)
			{
				window.UpdateWindowCommandsForFlyout(flyout1);
			}
			Flyout flyout2 = (
				from x in flyouts1
				where x.Position == Position.Right
				select x).OrderByDescending<Flyout, int>(new Func<Flyout, int>(Panel.GetZIndex)).FirstOrDefault<Flyout>();
			if (flyout2 != null)
			{
				window.UpdateWindowCommandsForFlyout(flyout2);
			}
		}

		private static void InvokeActionOnWindowCommands(this MetroWindow window, Action<Control> action1, Action<Control> action2 = null, Position position = 2)
		{
			if (window.LeftWindowCommandsPresenter == null || window.RightWindowCommandsPresenter == null || window.WindowButtonCommands == null)
			{
				return;
			}
			if (position == Position.Left || position == Position.Top)
			{
				action1(window.LeftWindowCommands);
			}
			if (position == Position.Right || position == Position.Top)
			{
				action1(window.RightWindowCommands);
				if (action2 == null)
				{
					action1(window.WindowButtonCommands);
					return;
				}
				action2(window.WindowButtonCommands);
			}
		}

		public static void ResetAllWindowCommandsBrush(this MetroWindow window)
		{
			window.ChangeAllWindowCommandsBrush(window.OverrideDefaultWindowCommandsBrush, Position.Top);
		}

		public static void SetIsHitTestVisibleInChromeProperty<T>(this MetroWindow window, string name)
		where T : DependencyObject
		{
			if (window == null)
			{
				return;
			}
			T part = window.GetPart<T>(name);
			if (part != null)
			{
				part.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, (object)true);
			}
		}

		public static void SetWindowChromeResizeGripDirection(this MetroWindow window, string name, ResizeGripDirection direction)
		{
			if (window == null)
			{
				return;
			}
			IInputElement part = window.GetPart(name) as IInputElement;
			if (part != null)
			{
				WindowChrome.SetResizeGripDirection(part, direction);
			}
		}

		public static void UpdateWindowCommandsForFlyout(this MetroWindow window, Flyout flyout)
		{
			window.ChangeAllWindowCommandsBrush(flyout.Foreground, flyout.Position);
		}
	}
}