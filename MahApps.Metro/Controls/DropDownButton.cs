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
	[TemplatePart(Name="PART_Button", Type=typeof(Button))]
	[TemplatePart(Name="PART_ButtonContent", Type=typeof(ContentControl))]
	[TemplatePart(Name="PART_Image", Type=typeof(Image))]
	[TemplatePart(Name="PART_Menu", Type=typeof(System.Windows.Controls.ContextMenu))]
	public class DropDownButton : ItemsControl
	{
		public readonly static RoutedEvent ClickEvent;

		public readonly static DependencyProperty IsExpandedProperty;

		public readonly static DependencyProperty ExtraTagProperty;

		public readonly static DependencyProperty OrientationProperty;

		public readonly static DependencyProperty IconProperty;

		public readonly static DependencyProperty IconTemplateProperty;

		public readonly static DependencyProperty CommandProperty;

		public readonly static DependencyProperty CommandTargetProperty;

		public readonly static DependencyProperty CommandParameterProperty;

		public readonly static DependencyProperty ContentProperty;

		public readonly static DependencyProperty ButtonStyleProperty;

		public readonly static DependencyProperty MenuStyleProperty;

		public readonly static DependencyProperty ArrowBrushProperty;

		public readonly static DependencyProperty ArrowVisibilityProperty;

		private Button clickButton;

		private System.Windows.Controls.ContextMenu menu;

		public Brush ArrowBrush
		{
			get
			{
				return (Brush)base.GetValue(DropDownButton.ArrowBrushProperty);
			}
			set
			{
				base.SetValue(DropDownButton.ArrowBrushProperty, value);
			}
		}

		public System.Windows.Visibility ArrowVisibility
		{
			get
			{
				return (System.Windows.Visibility)base.GetValue(DropDownButton.ArrowVisibilityProperty);
			}
			set
			{
				base.SetValue(DropDownButton.ArrowVisibilityProperty, value);
			}
		}

		public System.Windows.Style ButtonStyle
		{
			get
			{
				return (System.Windows.Style)base.GetValue(DropDownButton.ButtonStyleProperty);
			}
			set
			{
				base.SetValue(DropDownButton.ButtonStyleProperty, value);
			}
		}

		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(DropDownButton.CommandProperty);
			}
			set
			{
				base.SetValue(DropDownButton.CommandProperty, value);
			}
		}

		public object CommandParameter
		{
			get
			{
				return base.GetValue(DropDownButton.CommandParameterProperty);
			}
			set
			{
				base.SetValue(DropDownButton.CommandParameterProperty, value);
			}
		}

		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)base.GetValue(DropDownButton.CommandTargetProperty);
			}
			set
			{
				base.SetValue(DropDownButton.CommandTargetProperty, value);
			}
		}

		public object Content
		{
			get
			{
				return base.GetValue(DropDownButton.ContentProperty);
			}
			set
			{
				base.SetValue(DropDownButton.ContentProperty, value);
			}
		}

		public object ExtraTag
		{
			get
			{
				return base.GetValue(DropDownButton.ExtraTagProperty);
			}
			set
			{
				base.SetValue(DropDownButton.ExtraTagProperty, value);
			}
		}

		[Bindable(true)]
		public object Icon
		{
			get
			{
				return base.GetValue(DropDownButton.IconProperty);
			}
			set
			{
				base.SetValue(DropDownButton.IconProperty, value);
			}
		}

		[Bindable(true)]
		public DataTemplate IconTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DropDownButton.IconTemplateProperty);
			}
			set
			{
				base.SetValue(DropDownButton.IconTemplateProperty, value);
			}
		}

		public bool IsExpanded
		{
			get
			{
				return (bool)base.GetValue(DropDownButton.IsExpandedProperty);
			}
			set
			{
				base.SetValue(DropDownButton.IsExpandedProperty, value);
			}
		}

		public System.Windows.Style MenuStyle
		{
			get
			{
				return (System.Windows.Style)base.GetValue(DropDownButton.MenuStyleProperty);
			}
			set
			{
				base.SetValue(DropDownButton.MenuStyleProperty, value);
			}
		}

		public System.Windows.Controls.Orientation Orientation
		{
			get
			{
				return (System.Windows.Controls.Orientation)base.GetValue(DropDownButton.OrientationProperty);
			}
			set
			{
				base.SetValue(DropDownButton.OrientationProperty, value);
			}
		}

		static DropDownButton()
		{
			Class6.yDnXvgqzyB5jw();
			DropDownButton.ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropDownButton));
			DropDownButton.IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(DropDownButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(DropDownButton.IsExpandedPropertyChangedCallback)));
			DropDownButton.ExtraTagProperty = DependencyProperty.Register("ExtraTag", typeof(object), typeof(DropDownButton));
			DropDownButton.OrientationProperty = DependencyProperty.Register("Orientation", typeof(System.Windows.Controls.Orientation), typeof(DropDownButton), new FrameworkPropertyMetadata((object)System.Windows.Controls.Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));
			DropDownButton.IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(DropDownButton));
			DropDownButton.IconTemplateProperty = DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(DropDownButton));
			DropDownButton.CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(DropDownButton));
			DropDownButton.CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(DropDownButton));
			DropDownButton.CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(DropDownButton));
			DropDownButton.ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(DropDownButton));
			DropDownButton.ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(System.Windows.Style), typeof(DropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
			DropDownButton.MenuStyleProperty = DependencyProperty.Register("MenuStyle", typeof(System.Windows.Style), typeof(DropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
			DropDownButton.ArrowBrushProperty = DependencyProperty.Register("ArrowBrush", typeof(Brush), typeof(DropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
			DropDownButton.ArrowVisibilityProperty = DependencyProperty.Register("ArrowVisibility", typeof(System.Windows.Visibility), typeof(DropDownButton), new FrameworkPropertyMetadata((object)System.Windows.Visibility.Visible, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton), new FrameworkPropertyMetadata(typeof(DropDownButton)));
		}

		public DropDownButton()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		private void ButtonClick(object sender, RoutedEventArgs e)
		{
			this.IsExpanded = true;
			e.RoutedEvent = DropDownButton.ClickEvent;
			base.RaiseEvent(e);
		}

		private void DropDownButton_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
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

		private void InitializeVisualElementsContainer()
		{
			base.MouseRightButtonUp -= new MouseButtonEventHandler(this.DropDownButton_MouseRightButtonUp);
			this.clickButton.Click -= new RoutedEventHandler(this.ButtonClick);
			base.MouseRightButtonUp += new MouseButtonEventHandler(this.DropDownButton_MouseRightButtonUp);
			this.clickButton.Click += new RoutedEventHandler(this.ButtonClick);
		}

		private static void IsExpandedPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			DropDownButton dropDownButton = (DropDownButton)dependencyObject;
			if (dropDownButton.clickButton != null)
			{
				dropDownButton.menu.Placement = PlacementMode.Bottom;
				dropDownButton.menu.PlacementTarget = dropDownButton.clickButton;
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.clickButton = this.EnforceInstance<Button>("PART_Button");
			this.menu = this.EnforceInstance<System.Windows.Controls.ContextMenu>("PART_Menu");
			this.InitializeVisualElementsContainer();
		}

		public event RoutedEventHandler Click
		{
			add
			{
				base.AddHandler(DropDownButton.ClickEvent, value);
			}
			remove
			{
				base.RemoveHandler(DropDownButton.ClickEvent, value);
			}
		}
	}
}