using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class PivotItem : ContentControl
	{
		public readonly static DependencyProperty HeaderProperty;

		public string Header
		{
			get
			{
				return (string)base.GetValue(PivotItem.HeaderProperty);
			}
			set
			{
				base.SetValue(PivotItem.HeaderProperty, value);
			}
		}

		static PivotItem()
		{
			Class6.yDnXvgqzyB5jw();
			PivotItem.HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(PivotItem), new PropertyMetadata(null));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PivotItem), new FrameworkPropertyMetadata(typeof(PivotItem)));
		}

		public PivotItem()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			base.RequestBringIntoView += new RequestBringIntoViewEventHandler((object sender, RequestBringIntoViewEventArgs e) => e.Handled = true);
		}
	}
}