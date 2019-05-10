using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public class DataGridNumericUpDownColumn : DataGridBoundColumn
	{
		private static Style _defaultEditingElementStyle;

		private static Style _defaultElementStyle;

		private double minimum;

		private double maximum;

		private double interval;

		private string stringFormat;

		private bool hideUpDownButtons;

		private double upDownButtonsWidth;

		public readonly static DependencyProperty FontFamilyProperty;

		public readonly static DependencyProperty FontSizeProperty;

		public readonly static DependencyProperty FontStyleProperty;

		public readonly static DependencyProperty FontWeightProperty;

		public readonly static DependencyProperty ForegroundProperty;

		public static Style DefaultEditingElementStyle
		{
			get
			{
				if (DataGridNumericUpDownColumn._defaultEditingElementStyle == null)
				{
					Style style = new Style(typeof(NumericUpDown));
					style.Setters.Add(new Setter(Control.BorderThicknessProperty, (object)(new Thickness(0))));
					style.Setters.Add(new Setter(Control.PaddingProperty, (object)(new Thickness(0))));
					style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, (object)VerticalAlignment.Top));
					style.Setters.Add(new Setter(ScrollViewer.HorizontalScrollBarVisibilityProperty, (object)ScrollBarVisibility.Disabled));
					style.Setters.Add(new Setter(ScrollViewer.VerticalScrollBarVisibilityProperty, (object)ScrollBarVisibility.Disabled));
					style.Setters.Add(new Setter(Control.VerticalContentAlignmentProperty, (object)VerticalAlignment.Center));
					style.Setters.Add(new Setter(FrameworkElement.MinHeightProperty, (object)0));
					style.Seal();
					DataGridNumericUpDownColumn._defaultEditingElementStyle = style;
				}
				return DataGridNumericUpDownColumn._defaultEditingElementStyle;
			}
		}

		public static Style DefaultElementStyle
		{
			get
			{
				if (DataGridNumericUpDownColumn._defaultElementStyle == null)
				{
					Style style = new Style(typeof(NumericUpDown));
					style.Setters.Add(new Setter(Control.BorderThicknessProperty, (object)(new Thickness(0))));
					style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, (object)VerticalAlignment.Top));
					style.Setters.Add(new Setter(UIElement.IsHitTestVisibleProperty, false));
					style.Setters.Add(new Setter(UIElement.FocusableProperty, false));
					style.Setters.Add(new Setter(NumericUpDown.HideUpDownButtonsProperty, true));
					style.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.Transparent));
					style.Setters.Add(new Setter(ScrollViewer.HorizontalScrollBarVisibilityProperty, (object)ScrollBarVisibility.Disabled));
					style.Setters.Add(new Setter(ScrollViewer.VerticalScrollBarVisibilityProperty, (object)ScrollBarVisibility.Disabled));
					style.Setters.Add(new Setter(Control.VerticalContentAlignmentProperty, (object)VerticalAlignment.Center));
					style.Setters.Add(new Setter(FrameworkElement.MinHeightProperty, (object)0));
					style.Setters.Add(new Setter(ControlsHelper.DisabledVisualElementVisibilityProperty, (object)System.Windows.Visibility.Collapsed));
					style.Seal();
					DataGridNumericUpDownColumn._defaultElementStyle = style;
				}
				return DataGridNumericUpDownColumn._defaultElementStyle;
			}
		}

		public System.Windows.Media.FontFamily FontFamily
		{
			get
			{
				return (System.Windows.Media.FontFamily)base.GetValue(DataGridNumericUpDownColumn.FontFamilyProperty);
			}
			set
			{
				base.SetValue(DataGridNumericUpDownColumn.FontFamilyProperty, value);
			}
		}

		[Localizability(LocalizationCategory.None)]
		[TypeConverter(typeof(FontSizeConverter))]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(DataGridNumericUpDownColumn.FontSizeProperty);
			}
			set
			{
				base.SetValue(DataGridNumericUpDownColumn.FontSizeProperty, value);
			}
		}

		public System.Windows.FontStyle FontStyle
		{
			get
			{
				return (System.Windows.FontStyle)base.GetValue(DataGridNumericUpDownColumn.FontStyleProperty);
			}
			set
			{
				base.SetValue(DataGridNumericUpDownColumn.FontStyleProperty, value);
			}
		}

		public System.Windows.FontWeight FontWeight
		{
			get
			{
				return (System.Windows.FontWeight)base.GetValue(DataGridNumericUpDownColumn.FontWeightProperty);
			}
			set
			{
				base.SetValue(DataGridNumericUpDownColumn.FontWeightProperty, value);
			}
		}

		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(DataGridNumericUpDownColumn.ForegroundProperty);
			}
			set
			{
				base.SetValue(DataGridNumericUpDownColumn.ForegroundProperty, value);
			}
		}

		public bool HideUpDownButtons
		{
			get
			{
				return this.hideUpDownButtons;
			}
			set
			{
				this.hideUpDownButtons = value;
			}
		}

		public double Interval
		{
			get
			{
				return this.interval;
			}
			set
			{
				this.interval = value;
			}
		}

		public double Maximum
		{
			get
			{
				return this.maximum;
			}
			set
			{
				this.maximum = value;
			}
		}

		public double Minimum
		{
			get
			{
				return this.minimum;
			}
			set
			{
				this.minimum = value;
			}
		}

		public string StringFormat
		{
			get
			{
				return this.stringFormat;
			}
			set
			{
				this.stringFormat = value;
			}
		}

		public double UpDownButtonsWidth
		{
			get
			{
				return this.upDownButtonsWidth;
			}
			set
			{
				this.upDownButtonsWidth = value;
			}
		}

		static DataGridNumericUpDownColumn()
		{
			Class6.yDnXvgqzyB5jw();
			DataGridNumericUpDownColumn.FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridNumericUpDownColumn.NotifyPropertyChangeForRefreshContent)));
			DataGridNumericUpDownColumn.FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata((object)SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridNumericUpDownColumn.NotifyPropertyChangeForRefreshContent)));
			DataGridNumericUpDownColumn.FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata((object)SystemFonts.MessageFontStyle, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridNumericUpDownColumn.NotifyPropertyChangeForRefreshContent)));
			DataGridNumericUpDownColumn.FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata((object)SystemFonts.MessageFontWeight, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridNumericUpDownColumn.NotifyPropertyChangeForRefreshContent)));
			DataGridNumericUpDownColumn.ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridNumericUpDownColumn.NotifyPropertyChangeForRefreshContent)));
			DataGridBoundColumn.ElementStyleProperty.OverrideMetadata(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata(DataGridNumericUpDownColumn.DefaultElementStyle));
			DataGridBoundColumn.EditingElementStyleProperty.OverrideMetadata(typeof(DataGridNumericUpDownColumn), new FrameworkPropertyMetadata(DataGridNumericUpDownColumn.DefaultEditingElementStyle));
		}

		public DataGridNumericUpDownColumn()
		{
			Class6.yDnXvgqzyB5jw();
			this.minimum = (double)NumericUpDown.MinimumProperty.DefaultMetadata.DefaultValue;
			this.maximum = (double)NumericUpDown.MaximumProperty.DefaultMetadata.DefaultValue;
			this.interval = (double)NumericUpDown.IntervalProperty.DefaultMetadata.DefaultValue;
			this.stringFormat = (string)NumericUpDown.StringFormatProperty.DefaultMetadata.DefaultValue;
			this.hideUpDownButtons = (bool)NumericUpDown.HideUpDownButtonsProperty.DefaultMetadata.DefaultValue;
			this.upDownButtonsWidth = (double)NumericUpDown.UpDownButtonsWidthProperty.DefaultMetadata.DefaultValue;
			base();
		}

		private static void ApplyBinding(BindingBase binding, DependencyObject target, DependencyProperty property)
		{
			if (binding == null)
			{
				BindingOperations.ClearBinding(target, property);
				return;
			}
			BindingOperations.SetBinding(target, property, binding);
		}

		private new void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkElement element)
		{
			Style style = this.PickStyle(isEditing, defaultToElementStyle);
			if (style != null)
			{
				element.Style = style;
			}
		}

		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			return this.GenerateNumericUpDown(true, cell);
		}

		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			NumericUpDown numericUpDown = this.GenerateNumericUpDown(false, cell);
			numericUpDown.HideUpDownButtons = true;
			return numericUpDown;
		}

		private NumericUpDown GenerateNumericUpDown(bool isEditing, DataGridCell cell)
		{
			NumericUpDown content;
			if (cell != null)
			{
				content = cell.Content as NumericUpDown;
			}
			else
			{
				content = null;
			}
			NumericUpDown numericUpDown = content ?? new NumericUpDown();
			this.SyncProperties(numericUpDown);
			this.ApplyStyle(isEditing, true, numericUpDown);
			DataGridNumericUpDownColumn.ApplyBinding(this.Binding, numericUpDown, NumericUpDown.ValueProperty);
			numericUpDown.Minimum = this.Minimum;
			numericUpDown.Maximum = this.Maximum;
			numericUpDown.StringFormat = this.StringFormat;
			numericUpDown.Interval = this.Interval;
			numericUpDown.InterceptArrowKeys = true;
			numericUpDown.InterceptMouseWheel = true;
			numericUpDown.Speedup = true;
			numericUpDown.HideUpDownButtons = this.HideUpDownButtons;
			numericUpDown.UpDownButtonsWidth = this.UpDownButtonsWidth;
			return numericUpDown;
		}

		private static bool IsDefaultValue(DependencyObject d, DependencyProperty dp)
		{
			return DependencyPropertyHelper.GetValueSource(d, dp).BaseValueSource == BaseValueSource.Default;
		}

		private static new void NotifyPropertyChangeForRefreshContent(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridNumericUpDownColumn)d).NotifyPropertyChanged(e.Property.Name);
		}

		private Style PickStyle(bool isEditing, bool defaultToElementStyle)
		{
			Style elementStyle = (isEditing ? base.EditingElementStyle : base.ElementStyle);
			if (isEditing & defaultToElementStyle && elementStyle == null)
			{
				elementStyle = base.ElementStyle;
			}
			return elementStyle;
		}

		protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
		{
			NumericUpDown numericUpDown = editingElement as NumericUpDown;
			if (numericUpDown == null)
			{
				return null;
			}
			numericUpDown.Focus();
			numericUpDown.SelectAll();
			return numericUpDown.Value;
		}

		protected override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			DataGridCell dataGridCell = element as DataGridCell;
			if (dataGridCell != null)
			{
				FrameworkElement content = dataGridCell.Content as FrameworkElement;
				if (content != null)
				{
					if (propertyName == "FontFamily")
					{
						DataGridNumericUpDownColumn.SyncColumnProperty(this, content, TextElement.FontFamilyProperty, DataGridNumericUpDownColumn.FontFamilyProperty);
					}
					else if (propertyName == "FontSize")
					{
						DataGridNumericUpDownColumn.SyncColumnProperty(this, content, TextElement.FontSizeProperty, DataGridNumericUpDownColumn.FontSizeProperty);
					}
					else if (propertyName == "FontStyle")
					{
						DataGridNumericUpDownColumn.SyncColumnProperty(this, content, TextElement.FontStyleProperty, DataGridNumericUpDownColumn.FontStyleProperty);
					}
					else if (propertyName == "FontWeight")
					{
						DataGridNumericUpDownColumn.SyncColumnProperty(this, content, TextElement.FontWeightProperty, DataGridNumericUpDownColumn.FontWeightProperty);
					}
					else if (propertyName == "Foreground")
					{
						DataGridNumericUpDownColumn.SyncColumnProperty(this, content, TextElement.ForegroundProperty, DataGridNumericUpDownColumn.ForegroundProperty);
					}
				}
			}
			base.RefreshCellContent(element, propertyName);
		}

		private static void SyncColumnProperty(DependencyObject column, DependencyObject content, DependencyProperty contentProperty, DependencyProperty columnProperty)
		{
			if (DataGridNumericUpDownColumn.IsDefaultValue(column, columnProperty))
			{
				content.ClearValue(contentProperty);
				return;
			}
			content.SetValue(contentProperty, column.GetValue(columnProperty));
		}

		private void SyncProperties(FrameworkElement e)
		{
			DataGridNumericUpDownColumn.SyncColumnProperty(this, e, TextElement.FontFamilyProperty, DataGridNumericUpDownColumn.FontFamilyProperty);
			DataGridNumericUpDownColumn.SyncColumnProperty(this, e, TextElement.FontSizeProperty, DataGridNumericUpDownColumn.FontSizeProperty);
			DataGridNumericUpDownColumn.SyncColumnProperty(this, e, TextElement.FontStyleProperty, DataGridNumericUpDownColumn.FontStyleProperty);
			DataGridNumericUpDownColumn.SyncColumnProperty(this, e, TextElement.FontWeightProperty, DataGridNumericUpDownColumn.FontWeightProperty);
			DataGridNumericUpDownColumn.SyncColumnProperty(this, e, TextElement.ForegroundProperty, DataGridNumericUpDownColumn.ForegroundProperty);
		}
	}
}