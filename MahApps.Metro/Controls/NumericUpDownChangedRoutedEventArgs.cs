using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MahApps.Metro.Controls
{
	public class NumericUpDownChangedRoutedEventArgs : RoutedEventArgs
	{
		public double Interval
		{
			get;
			set;
		}

		public NumericUpDownChangedRoutedEventArgs(System.Windows.RoutedEvent routedEvent, double interval)
		{
			Class6.yDnXvgqzyB5jw();
			base(routedEvent);
			this.Interval = interval;
		}
	}
}