using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class DataGridCellHelper
	{
		public readonly static DependencyProperty SaveDataGridProperty;

		public readonly static DependencyProperty DataGridProperty;

		static DataGridCellHelper()
		{
			Class6.yDnXvgqzyB5jw();
			DataGridCellHelper.SaveDataGridProperty = DependencyProperty.RegisterAttached("SaveDataGrid", typeof(bool), typeof(DataGridCellHelper), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridCellHelper.CellPropertyChangedCallback)));
			DataGridCellHelper.DataGridProperty = DependencyProperty.RegisterAttached("DataGrid", typeof(DataGrid), typeof(DataGridCellHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));
		}

		public DataGridCellHelper()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		private static void CellPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			DataGridCell dataGridCell = dependencyObject as DataGridCell;
			if (dataGridCell != null && e.NewValue != e.OldValue && e.NewValue is bool)
			{
				dataGridCell.Loaded -= new RoutedEventHandler(DataGridCellHelper.DataGridCellLoaded);
				dataGridCell.Unloaded -= new RoutedEventHandler(DataGridCellHelper.DataGridCellUnloaded);
				DataGrid dataGrid = null;
				if ((bool)e.NewValue)
				{
					dataGrid = dataGridCell.TryFindParent<DataGrid>();
					dataGridCell.Loaded += new RoutedEventHandler(DataGridCellHelper.DataGridCellLoaded);
					dataGridCell.Unloaded += new RoutedEventHandler(DataGridCellHelper.DataGridCellUnloaded);
				}
				DataGridCellHelper.SetDataGrid(dataGridCell, dataGrid);
			}
		}

		private static void DataGridCellLoaded(DataGridCell sender, RoutedEventArgs e)
		{
			DataGridCell dataGridCell = (DataGridCell)sender;
			if (DataGridCellHelper.GetDataGrid(dataGridCell) == null)
			{
				DataGridCellHelper.SetDataGrid(dataGridCell, dataGridCell.TryFindParent<DataGrid>());
			}
		}

		private static void DataGridCellUnloaded(DataGridCell sender, RoutedEventArgs e)
		{
			DataGridCellHelper.SetDataGrid((DataGridCell)sender, null);
		}

		[AttachedPropertyBrowsableForType(typeof(DataGridCell))]
		[Category("MahApps.Metro")]
		public static DataGrid GetDataGrid(UIElement element)
		{
			return (DataGrid)element.GetValue(DataGridCellHelper.DataGridProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(DataGridCell))]
		[Category("MahApps.Metro")]
		public static bool GetSaveDataGrid(UIElement element)
		{
			return (bool)element.GetValue(DataGridCellHelper.SaveDataGridProperty);
		}

		public static void SetDataGrid(UIElement element, DataGrid value)
		{
			element.SetValue(DataGridCellHelper.DataGridProperty, value);
		}

		public static void SetSaveDataGrid(UIElement element, bool value)
		{
			element.SetValue(DataGridCellHelper.SaveDataGridProperty, value);
		}
	}
}