using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public static class DataGridRowHelper
	{
		public readonly static DependencyProperty SelectionUnitProperty;

		static DataGridRowHelper()
		{
			Class6.yDnXvgqzyB5jw();
			DataGridRowHelper.SelectionUnitProperty = DependencyProperty.RegisterAttached("SelectionUnit", typeof(DataGridSelectionUnit), typeof(DataGridRowHelper), new FrameworkPropertyMetadata((object)DataGridSelectionUnit.FullRow));
		}

		[AttachedPropertyBrowsableForType(typeof(DataGridRow))]
		[Category("MahApps.Metro")]
		public static DataGridSelectionUnit GetSelectionUnit(UIElement element)
		{
			return (DataGridSelectionUnit)element.GetValue(DataGridRowHelper.SelectionUnitProperty);
		}

		public static void SetSelectionUnit(UIElement element, DataGridSelectionUnit value)
		{
			element.SetValue(DataGridRowHelper.SelectionUnitProperty, value);
		}
	}
}