using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	[ContentProperty("ItemsSource")]
	[DefaultEvent("SelectionChanged")]
	[TemplatePart(Name="PART_Button", Type=typeof(Button))]
	[TemplatePart(Name="PART_ButtonContent", Type=typeof(ContentControl))]
	[TemplatePart(Name="PART_Container", Type=typeof(Grid))]
	[TemplatePart(Name="PART_Expander", Type=typeof(Button))]
	[TemplatePart(Name="PART_ListBox", Type=typeof(ListBox))]
	[TemplatePart(Name="PART_Popup", Type=typeof(Popup))]
	public class SplitButton : ItemsControl
	{
		public readonly static RoutedEvent ClickEvent;

		public readonly static RoutedEvent SelectionChangedEvent;

		public readonly static DependencyProperty IsExpandedProperty;

		public readonly static DependencyProperty SelectedIndexProperty;

		public readonly static DependencyProperty SelectedItemProperty;

		public readonly static DependencyProperty ExtraTagProperty;

		public readonly static DependencyProperty OrientationProperty;

		public readonly static DependencyProperty IconProperty;

		public readonly static DependencyProperty IconTemplateProperty;

		public readonly static DependencyProperty CommandProperty;

		public readonly static DependencyProperty CommandTargetProperty;

		public readonly static DependencyProperty CommandParameterProperty;

		public readonly static DependencyProperty ButtonStyleProperty;

		public readonly static DependencyProperty ButtonArrowStyleProperty;

		public readonly static DependencyProperty ListBoxStyleProperty;

		public readonly static DependencyProperty ArrowBrushProperty;

		private Button _clickButton;

		private Button _expander;

		private ListBox _listBox;

		private Popup _popup;

		public Brush ArrowBrush
		{
			get
			{
				return (Brush)base.GetValue(SplitButton.ArrowBrushProperty);
			}
			set
			{
				base.SetValue(SplitButton.ArrowBrushProperty, value);
			}
		}

		public System.Windows.Style ButtonArrowStyle
		{
			get
			{
				return (System.Windows.Style)base.GetValue(SplitButton.ButtonArrowStyleProperty);
			}
			set
			{
				base.SetValue(SplitButton.ButtonArrowStyleProperty, value);
			}
		}

		public System.Windows.Style ButtonStyle
		{
			get
			{
				return (System.Windows.Style)base.GetValue(SplitButton.ButtonStyleProperty);
			}
			set
			{
				base.SetValue(SplitButton.ButtonStyleProperty, value);
			}
		}

		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(SplitButton.CommandProperty);
			}
			set
			{
				base.SetValue(SplitButton.CommandProperty, value);
			}
		}

		public object CommandParameter
		{
			get
			{
				return base.GetValue(SplitButton.CommandParameterProperty);
			}
			set
			{
				base.SetValue(SplitButton.CommandParameterProperty, value);
			}
		}

		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)base.GetValue(SplitButton.CommandTargetProperty);
			}
			set
			{
				base.SetValue(SplitButton.CommandTargetProperty, value);
			}
		}

		public object ExtraTag
		{
			get
			{
				return base.GetValue(SplitButton.ExtraTagProperty);
			}
			set
			{
				base.SetValue(SplitButton.ExtraTagProperty, value);
			}
		}

		[Bindable(true)]
		public object Icon
		{
			get
			{
				return base.GetValue(SplitButton.IconProperty);
			}
			set
			{
				base.SetValue(SplitButton.IconProperty, value);
			}
		}

		[Bindable(true)]
		public DataTemplate IconTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(SplitButton.IconTemplateProperty);
			}
			set
			{
				base.SetValue(SplitButton.IconTemplateProperty, value);
			}
		}

		public bool IsExpanded
		{
			get
			{
				return (bool)base.GetValue(SplitButton.IsExpandedProperty);
			}
			set
			{
				base.SetValue(SplitButton.IsExpandedProperty, value);
			}
		}

		public System.Windows.Style ListBoxStyle
		{
			get
			{
				return (System.Windows.Style)base.GetValue(SplitButton.ListBoxStyleProperty);
			}
			set
			{
				base.SetValue(SplitButton.ListBoxStyleProperty, value);
			}
		}

		public System.Windows.Controls.Orientation Orientation
		{
			get
			{
				return (System.Windows.Controls.Orientation)base.GetValue(SplitButton.OrientationProperty);
			}
			set
			{
				base.SetValue(SplitButton.OrientationProperty, value);
			}
		}

		public int SelectedIndex
		{
			get
			{
				return (int)base.GetValue(SplitButton.SelectedIndexProperty);
			}
			set
			{
				base.SetValue(SplitButton.SelectedIndexProperty, value);
			}
		}

		public object SelectedItem
		{
			get
			{
				return base.GetValue(SplitButton.SelectedItemProperty);
			}
			set
			{
				base.SetValue(SplitButton.SelectedItemProperty, value);
			}
		}

		static SplitButton()
		{
			Class6.yDnXvgqzyB5jw();
			SplitButton.ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SplitButton));
			SplitButton.SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(SelectionChangedEventHandler), typeof(SplitButton));
			SplitButton.IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(SplitButton));
			SplitButton.SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(SplitButton), new FrameworkPropertyMetadata((object)-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
			SplitButton.SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(SplitButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
			SplitButton.ExtraTagProperty = DependencyProperty.Register("ExtraTag", typeof(object), typeof(SplitButton));
			SplitButton.OrientationProperty = DependencyProperty.Register("Orientation", typeof(System.Windows.Controls.Orientation), typeof(SplitButton), new FrameworkPropertyMetadata((object)System.Windows.Controls.Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));
			SplitButton.IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(SplitButton));
			SplitButton.IconTemplateProperty = DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(SplitButton));
			SplitButton.CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(SplitButton));
			SplitButton.CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(SplitButton));
			SplitButton.CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(SplitButton));
			SplitButton.ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(System.Windows.Style), typeof(SplitButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
			SplitButton.ButtonArrowStyleProperty = DependencyProperty.Register("ButtonArrowStyle", typeof(System.Windows.Style), typeof(SplitButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
			SplitButton.ListBoxStyleProperty = DependencyProperty.Register("ListBoxStyle", typeof(System.Windows.Style), typeof(SplitButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
			SplitButton.ArrowBrushProperty = DependencyProperty.Register("ArrowBrush", typeof(Brush), typeof(SplitButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButton), new FrameworkPropertyMetadata(typeof(SplitButton)));
		}

		public SplitButton()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		private void ButtonClick(object sender, RoutedEventArgs e)
		{
			e.RoutedEvent = SplitButton.ClickEvent;
			base.RaiseEvent(e);
		}

		private T EnforceInstance<T>(string partName)
		where T : FrameworkElement, new()
		{
			T templateChild = (T)(base.GetTemplateChild(partName) as T);
			if (templateChild == null)
			{
				templateChild = Activator.CreateInstance<T>();
			}
			return templateChild;
		}

		private void ExpanderClick(object sender, RoutedEventArgs e)
		{
			this.IsExpanded = !this.IsExpanded;
		}

		private void InitializeVisualElementsContainer()
		{
			this._expander.Click -= new RoutedEventHandler(this.ExpanderClick);
			this._clickButton.Click -= new RoutedEventHandler(this.ButtonClick);
			this._listBox.SelectionChanged -= new SelectionChangedEventHandler(this.ListBoxSelectionChanged);
			this._listBox.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.ListBoxPreviewMouseLeftButtonDown);
			this._popup.Opened -= new EventHandler(this.PopupOpened);
			this._popup.Closed -= new EventHandler(this.PopupClosed);
			this._expander.Click += new RoutedEventHandler(this.ExpanderClick);
			this._clickButton.Click += new RoutedEventHandler(this.ButtonClick);
			this._listBox.SelectionChanged += new SelectionChangedEventHandler(this.ListBoxSelectionChanged);
			this._listBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.ListBoxPreviewMouseLeftButtonDown);
			this._popup.Opened += new EventHandler(this.PopupOpened);
			this._popup.Closed += new EventHandler(this.PopupClosed);
		}

		private void ListBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			DependencyObject originalSource = e.OriginalSource as DependencyObject;
			if (originalSource != null && ItemsControl.ContainerFromElement(this._listBox, originalSource) is ListBoxItem)
			{
				this.IsExpanded = false;
			}
		}

		private void ListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			e.RoutedEvent = SplitButton.SelectionChangedEvent;
			base.RaiseEvent(e);
			this.IsExpanded = false;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._clickButton = this.EnforceInstance<Button>("PART_Button");
			this._expander = this.EnforceInstance<Button>("PART_Expander");
			this._listBox = this.EnforceInstance<ListBox>("PART_ListBox");
			this._popup = this.EnforceInstance<Popup>("PART_Popup");
			this.InitializeVisualElementsContainer();
		}

		private void OutsideCapturedElementHandler(object sender, MouseButtonEventArgs e)
		{
			this.IsExpanded = false;
			Mouse.RemovePreviewMouseDownOutsideCapturedElementHandler(this, new MouseButtonEventHandler(this.OutsideCapturedElementHandler));
		}

		private void PopupClosed(object sender, EventArgs e)
		{
			base.ReleaseMouseCapture();
			if (this._expander != null)
			{
				this._expander.Focus();
			}
		}

		private void PopupOpened(object sender, EventArgs e)
		{
			Mouse.Capture(this, CaptureMode.SubTree);
			Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, new MouseButtonEventHandler(this.OutsideCapturedElementHandler));
		}

		public event RoutedEventHandler Click
		{
			add
			{
				base.AddHandler(SplitButton.ClickEvent, value);
			}
			remove
			{
				base.RemoveHandler(SplitButton.ClickEvent, value);
			}
		}

		public event SelectionChangedEventHandler SelectionChanged
		{
			add
			{
				base.AddHandler(SplitButton.SelectionChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(SplitButton.SelectionChangedEvent, value);
			}
		}
	}
}