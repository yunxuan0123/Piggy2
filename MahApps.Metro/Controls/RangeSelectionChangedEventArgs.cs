using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MahApps.Metro.Controls
{
	public class RangeSelectionChangedEventArgs : RoutedEventArgs
	{
		public double NewLowerValue
		{
			get;
			set;
		}

		public double NewUpperValue
		{
			get;
			set;
		}

		public double OldLowerValue
		{
			get;
			set;
		}

		public double OldUpperValue
		{
			get;
			set;
		}

		internal RangeSelectionChangedEventArgs(double newLowerValue, double newUpperValue, double oldLowerValue, double oldUpperValue)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.NewLowerValue = newLowerValue;
			this.NewUpperValue = newUpperValue;
			this.OldLowerValue = oldLowerValue;
			this.OldUpperValue = oldUpperValue;
		}
	}
}