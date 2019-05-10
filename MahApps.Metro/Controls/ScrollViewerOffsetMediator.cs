using System;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class ScrollViewerOffsetMediator : FrameworkElement
	{
		public readonly static DependencyProperty ScrollViewerProperty;

		public readonly static DependencyProperty HorizontalOffsetProperty;

		public double HorizontalOffset
		{
			get
			{
				return (double)base.GetValue(ScrollViewerOffsetMediator.HorizontalOffsetProperty);
			}
			set
			{
				base.SetValue(ScrollViewerOffsetMediator.HorizontalOffsetProperty, value);
			}
		}

		public System.Windows.Controls.ScrollViewer ScrollViewer
		{
			get
			{
				return (System.Windows.Controls.ScrollViewer)base.GetValue(ScrollViewerOffsetMediator.ScrollViewerProperty);
			}
			set
			{
				base.SetValue(ScrollViewerOffsetMediator.ScrollViewerProperty, value);
			}
		}

		static ScrollViewerOffsetMediator()
		{
			Class6.yDnXvgqzyB5jw();
			ScrollViewerOffsetMediator.ScrollViewerProperty = DependencyProperty.Register("ScrollViewer", typeof(System.Windows.Controls.ScrollViewer), typeof(ScrollViewerOffsetMediator), new PropertyMetadata(null, new PropertyChangedCallback(ScrollViewerOffsetMediator.OnScrollViewerChanged)));
			ScrollViewerOffsetMediator.HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(ScrollViewerOffsetMediator), new PropertyMetadata((object)0, new PropertyChangedCallback(ScrollViewerOffsetMediator.OnHorizontalOffsetChanged)));
		}

		public ScrollViewerOffsetMediator()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		private static void OnHorizontalOffsetChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			ScrollViewerOffsetMediator scrollViewerOffsetMediator = (ScrollViewerOffsetMediator)o;
			if (scrollViewerOffsetMediator.ScrollViewer != null)
			{
				scrollViewerOffsetMediator.ScrollViewer.ScrollToHorizontalOffset((double)e.NewValue);
			}
		}

		private static void OnScrollViewerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			ScrollViewerOffsetMediator scrollViewerOffsetMediator = (ScrollViewerOffsetMediator)o;
			System.Windows.Controls.ScrollViewer newValue = (System.Windows.Controls.ScrollViewer)e.NewValue;
			if (newValue != null)
			{
				newValue.ScrollToHorizontalOffset(scrollViewerOffsetMediator.HorizontalOffset);
			}
		}
	}
}