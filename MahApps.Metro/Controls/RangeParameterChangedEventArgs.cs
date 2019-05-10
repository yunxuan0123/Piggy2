using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MahApps.Metro.Controls
{
	public class RangeParameterChangedEventArgs : RoutedEventArgs
	{
		public double NewValue
		{
			get;
			private set;
		}

		public double OldValue
		{
			get;
			private set;
		}

		public RangeParameterChangeType ParameterType
		{
			get;
			private set;
		}

		internal RangeParameterChangedEventArgs(RangeParameterChangeType type, double _old, double _new)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.ParameterType = type;
			this.OldValue = _old;
			this.NewValue = _new;
		}
	}
}