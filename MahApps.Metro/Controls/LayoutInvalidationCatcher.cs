using System;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class LayoutInvalidationCatcher : Decorator
	{
		public Planerator PlaParent
		{
			get
			{
				return base.Parent as Planerator;
			}
		}

		public LayoutInvalidationCatcher()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		protected override Size ArrangeOverride(Size arrangeSize)
		{
			Planerator plaParent = this.PlaParent;
			if (plaParent != null)
			{
				plaParent.InvalidateArrange();
			}
			return base.ArrangeOverride(arrangeSize);
		}

		protected override Size MeasureOverride(Size constraint)
		{
			Planerator plaParent = this.PlaParent;
			if (plaParent != null)
			{
				plaParent.InvalidateMeasure();
			}
			return base.MeasureOverride(constraint);
		}
	}
}