using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name="PART_ContentPresenter", Type=typeof(UIElement))]
	[TemplatePart(Name="PART_Separator", Type=typeof(UIElement))]
	public class WindowCommandsItem : ContentControl
	{
		private const string PART_ContentPresenter = "PART_ContentPresenter";

		private const string PART_Separator = "PART_Separator";

		public readonly static DependencyProperty IsSeparatorVisibleProperty;

		public bool IsSeparatorVisible
		{
			get
			{
				return (bool)base.GetValue(WindowCommandsItem.IsSeparatorVisibleProperty);
			}
			set
			{
				base.SetValue(WindowCommandsItem.IsSeparatorVisibleProperty, value);
			}
		}

		internal PropertyChangeNotifier VisibilityPropertyChangeNotifier
		{
			get;
			set;
		}

		static WindowCommandsItem()
		{
			Class6.yDnXvgqzyB5jw();
			WindowCommandsItem.IsSeparatorVisibleProperty = DependencyProperty.Register("IsSeparatorVisible", typeof(bool), typeof(WindowCommandsItem), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
		}

		public WindowCommandsItem()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}
	}
}