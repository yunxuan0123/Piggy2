using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Native;
using Microsoft.Windows.Shell;
using Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name="PART_FlyoutModal", Type=typeof(Rectangle))]
	[TemplatePart(Name="PART_Icon", Type=typeof(UIElement))]
	[TemplatePart(Name="PART_LeftWindowCommands", Type=typeof(WindowCommands))]
	[TemplatePart(Name="PART_MetroActiveDialogContainer", Type=typeof(Grid))]
	[TemplatePart(Name="PART_MetroInactiveDialogsContainer", Type=typeof(Grid))]
	[TemplatePart(Name="PART_OverlayBox", Type=typeof(Grid))]
	[TemplatePart(Name="PART_RightWindowCommands", Type=typeof(WindowCommands))]
	[TemplatePart(Name="PART_TitleBar", Type=typeof(UIElement))]
	[TemplatePart(Name="PART_WindowButtonCommands", Type=typeof(MahApps.Metro.Controls.WindowButtonCommands))]
	[TemplatePart(Name="PART_WindowTitleBackground", Type=typeof(UIElement))]
	[TemplatePart(Name="PART_WindowTitleThumb", Type=typeof(Thumb))]
	public class MetroWindow : Window
	{
		private const string PART_Icon = "PART_Icon";

		private const string PART_TitleBar = "PART_TitleBar";

		private const string PART_WindowTitleBackground = "PART_WindowTitleBackground";

		private const string PART_WindowTitleThumb = "PART_WindowTitleThumb";

		private const string PART_LeftWindowCommands = "PART_LeftWindowCommands";

		private const string PART_RightWindowCommands = "PART_RightWindowCommands";

		private const string PART_WindowButtonCommands = "PART_WindowButtonCommands";

		private const string PART_OverlayBox = "PART_OverlayBox";

		private const string PART_MetroActiveDialogContainer = "PART_MetroActiveDialogContainer";

		private const string PART_MetroInactiveDialogsContainer = "PART_MetroInactiveDialogsContainer";

		private const string PART_FlyoutModal = "PART_FlyoutModal";

		public readonly static DependencyProperty ShowIconOnTitleBarProperty;

		public readonly static DependencyProperty IconEdgeModeProperty;

		public readonly static DependencyProperty IconBitmapScalingModeProperty;

		public readonly static DependencyProperty ShowTitleBarProperty;

		public readonly static DependencyProperty ShowMinButtonProperty;

		public readonly static DependencyProperty ShowMaxRestoreButtonProperty;

		public readonly static DependencyProperty ShowCloseButtonProperty;

		public readonly static DependencyProperty IsMinButtonEnabledProperty;

		public readonly static DependencyProperty IsMaxRestoreButtonEnabledProperty;

		public readonly static DependencyProperty IsCloseButtonEnabledProperty;

		public readonly static DependencyProperty ShowSystemMenuOnRightClickProperty;

		public readonly static DependencyProperty TitlebarHeightProperty;

		public readonly static DependencyProperty TitleCapsProperty;

		public readonly static DependencyProperty TitleAlignmentProperty;

		public readonly static DependencyProperty SaveWindowPositionProperty;

		public readonly static DependencyProperty WindowPlacementSettingsProperty;

		public readonly static DependencyProperty TitleForegroundProperty;

		public readonly static DependencyProperty IgnoreTaskbarOnMaximizeProperty;

		public readonly static DependencyProperty FlyoutsProperty;

		public readonly static DependencyProperty WindowTransitionsEnabledProperty;

		public readonly static DependencyProperty MetroDialogOptionsProperty;

		public readonly static DependencyProperty WindowTitleBrushProperty;

		public readonly static DependencyProperty GlowBrushProperty;

		public readonly static DependencyProperty NonActiveGlowBrushProperty;

		public readonly static DependencyProperty NonActiveBorderBrushProperty;

		public readonly static DependencyProperty NonActiveWindowTitleBrushProperty;

		public readonly static DependencyProperty IconTemplateProperty;

		public readonly static DependencyProperty TitleTemplateProperty;

		public readonly static DependencyProperty LeftWindowCommandsProperty;

		public readonly static DependencyProperty RightWindowCommandsProperty;

		public readonly static DependencyProperty LeftWindowCommandsOverlayBehaviorProperty;

		public readonly static DependencyProperty RightWindowCommandsOverlayBehaviorProperty;

		public readonly static DependencyProperty WindowButtonCommandsOverlayBehaviorProperty;

		public readonly static DependencyProperty IconOverlayBehaviorProperty;

		[Obsolete("This property will be deleted in the next release. You should use LightMinButtonStyle or DarkMinButtonStyle in WindowButtonCommands to override the style.")]
		public readonly static DependencyProperty WindowMinButtonStyleProperty;

		[Obsolete("This property will be deleted in the next release. You should use LightMaxButtonStyle or DarkMaxButtonStyle in WindowButtonCommands to override the style.")]
		public readonly static DependencyProperty WindowMaxButtonStyleProperty;

		[Obsolete("This property will be deleted in the next release. You should use LightCloseButtonStyle or DarkCloseButtonStyle in WindowButtonCommands to override the style.")]
		public readonly static DependencyProperty WindowCloseButtonStyleProperty;

		public readonly static DependencyProperty UseNoneWindowStyleProperty;

		public readonly static DependencyProperty OverrideDefaultWindowCommandsBrushProperty;

		[Obsolete("This property will be deleted in the next release. You should use BorderThickness=\"0\" and a GlowBrush=\"Black\" to get a drop shadow around the Window.")]
		public readonly static DependencyProperty EnableDWMDropShadowProperty;

		public readonly static DependencyProperty IsWindowDraggableProperty;

		private UIElement icon;

		private UIElement titleBar;

		private UIElement titleBarBackground;

		private Thumb windowTitleThumb;

		internal ContentPresenter LeftWindowCommandsPresenter;

		internal ContentPresenter RightWindowCommandsPresenter;

		internal MahApps.Metro.Controls.WindowButtonCommands WindowButtonCommands;

		internal Grid overlayBox;

		internal Grid metroActiveDialogContainer;

		internal Grid metroInactiveDialogContainer;

		private Storyboard overlayStoryboard;

		private Rectangle flyoutModal;

		public readonly static RoutedEvent FlyoutsStatusChangedEvent;

		protected new IntPtr CriticalHandle
		{
			get
			{
				object value = typeof(Window).GetProperty("CriticalHandle", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this, new object[0]);
				return (IntPtr)value;
			}
		}

		[Obsolete("This property will be deleted in the next release. You should use BorderThickness=\"0\" and a GlowBrush=\"Black\" to get a drop shadow around the Window.")]
		public bool EnableDWMDropShadow
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.EnableDWMDropShadowProperty);
			}
			set
			{
				base.SetValue(MetroWindow.EnableDWMDropShadowProperty, value);
			}
		}

		public FlyoutsControl Flyouts
		{
			get
			{
				return (FlyoutsControl)base.GetValue(MetroWindow.FlyoutsProperty);
			}
			set
			{
				base.SetValue(MetroWindow.FlyoutsProperty, value);
			}
		}

		public Brush GlowBrush
		{
			get
			{
				return (Brush)base.GetValue(MetroWindow.GlowBrushProperty);
			}
			set
			{
				base.SetValue(MetroWindow.GlowBrushProperty, value);
			}
		}

		public BitmapScalingMode IconBitmapScalingMode
		{
			get
			{
				return (BitmapScalingMode)base.GetValue(MetroWindow.IconBitmapScalingModeProperty);
			}
			set
			{
				base.SetValue(MetroWindow.IconBitmapScalingModeProperty, value);
			}
		}

		public EdgeMode IconEdgeMode
		{
			get
			{
				return (EdgeMode)base.GetValue(MetroWindow.IconEdgeModeProperty);
			}
			set
			{
				base.SetValue(MetroWindow.IconEdgeModeProperty, value);
			}
		}

		public WindowCommandsOverlayBehavior IconOverlayBehavior
		{
			get
			{
				return (WindowCommandsOverlayBehavior)base.GetValue(MetroWindow.IconOverlayBehaviorProperty);
			}
			set
			{
				base.SetValue(MetroWindow.IconOverlayBehaviorProperty, value);
			}
		}

		public DataTemplate IconTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(MetroWindow.IconTemplateProperty);
			}
			set
			{
				base.SetValue(MetroWindow.IconTemplateProperty, value);
			}
		}

		public bool IgnoreTaskbarOnMaximize
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.IgnoreTaskbarOnMaximizeProperty);
			}
			set
			{
				base.SetValue(MetroWindow.IgnoreTaskbarOnMaximizeProperty, value);
			}
		}

		public bool IsCloseButtonEnabled
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.IsCloseButtonEnabledProperty);
			}
			set
			{
				base.SetValue(MetroWindow.IsCloseButtonEnabledProperty, value);
			}
		}

		public bool IsMaxRestoreButtonEnabled
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.IsMaxRestoreButtonEnabledProperty);
			}
			set
			{
				base.SetValue(MetroWindow.IsMaxRestoreButtonEnabledProperty, value);
			}
		}

		public bool IsMinButtonEnabled
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.IsMinButtonEnabledProperty);
			}
			set
			{
				base.SetValue(MetroWindow.IsMinButtonEnabledProperty, value);
			}
		}

		public bool IsWindowDraggable
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.IsWindowDraggableProperty);
			}
			set
			{
				base.SetValue(MetroWindow.IsWindowDraggableProperty, value);
			}
		}

		public WindowCommands LeftWindowCommands
		{
			get
			{
				return (WindowCommands)base.GetValue(MetroWindow.LeftWindowCommandsProperty);
			}
			set
			{
				base.SetValue(MetroWindow.LeftWindowCommandsProperty, value);
			}
		}

		public WindowCommandsOverlayBehavior LeftWindowCommandsOverlayBehavior
		{
			get
			{
				return (WindowCommandsOverlayBehavior)base.GetValue(MetroWindow.LeftWindowCommandsOverlayBehaviorProperty);
			}
			set
			{
				base.SetValue(MetroWindow.LeftWindowCommandsOverlayBehaviorProperty, value);
			}
		}

		public MetroDialogSettings MetroDialogOptions
		{
			get
			{
				return (MetroDialogSettings)base.GetValue(MetroWindow.MetroDialogOptionsProperty);
			}
			set
			{
				base.SetValue(MetroWindow.MetroDialogOptionsProperty, value);
			}
		}

		public Brush NonActiveBorderBrush
		{
			get
			{
				return (Brush)base.GetValue(MetroWindow.NonActiveBorderBrushProperty);
			}
			set
			{
				base.SetValue(MetroWindow.NonActiveBorderBrushProperty, value);
			}
		}

		public Brush NonActiveGlowBrush
		{
			get
			{
				return (Brush)base.GetValue(MetroWindow.NonActiveGlowBrushProperty);
			}
			set
			{
				base.SetValue(MetroWindow.NonActiveGlowBrushProperty, value);
			}
		}

		public Brush NonActiveWindowTitleBrush
		{
			get
			{
				return (Brush)base.GetValue(MetroWindow.NonActiveWindowTitleBrushProperty);
			}
			set
			{
				base.SetValue(MetroWindow.NonActiveWindowTitleBrushProperty, value);
			}
		}

		public SolidColorBrush OverrideDefaultWindowCommandsBrush
		{
			get
			{
				return (SolidColorBrush)base.GetValue(MetroWindow.OverrideDefaultWindowCommandsBrushProperty);
			}
			set
			{
				base.SetValue(MetroWindow.OverrideDefaultWindowCommandsBrushProperty, value);
			}
		}

		public WindowCommands RightWindowCommands
		{
			get
			{
				return (WindowCommands)base.GetValue(MetroWindow.RightWindowCommandsProperty);
			}
			set
			{
				base.SetValue(MetroWindow.RightWindowCommandsProperty, value);
			}
		}

		public WindowCommandsOverlayBehavior RightWindowCommandsOverlayBehavior
		{
			get
			{
				return (WindowCommandsOverlayBehavior)base.GetValue(MetroWindow.RightWindowCommandsOverlayBehaviorProperty);
			}
			set
			{
				base.SetValue(MetroWindow.RightWindowCommandsOverlayBehaviorProperty, value);
			}
		}

		public bool SaveWindowPosition
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.SaveWindowPositionProperty);
			}
			set
			{
				base.SetValue(MetroWindow.SaveWindowPositionProperty, value);
			}
		}

		public bool ShowCloseButton
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.ShowCloseButtonProperty);
			}
			set
			{
				base.SetValue(MetroWindow.ShowCloseButtonProperty, value);
			}
		}

		public bool ShowIconOnTitleBar
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.ShowIconOnTitleBarProperty);
			}
			set
			{
				base.SetValue(MetroWindow.ShowIconOnTitleBarProperty, value);
			}
		}

		public bool ShowMaxRestoreButton
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.ShowMaxRestoreButtonProperty);
			}
			set
			{
				base.SetValue(MetroWindow.ShowMaxRestoreButtonProperty, value);
			}
		}

		public bool ShowMinButton
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.ShowMinButtonProperty);
			}
			set
			{
				base.SetValue(MetroWindow.ShowMinButtonProperty, value);
			}
		}

		public bool ShowSystemMenuOnRightClick
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.ShowSystemMenuOnRightClickProperty);
			}
			set
			{
				base.SetValue(MetroWindow.ShowSystemMenuOnRightClickProperty, value);
			}
		}

		public bool ShowTitleBar
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.ShowTitleBarProperty);
			}
			set
			{
				base.SetValue(MetroWindow.ShowTitleBarProperty, value);
			}
		}

		public System.Windows.HorizontalAlignment TitleAlignment
		{
			get
			{
				return (System.Windows.HorizontalAlignment)base.GetValue(MetroWindow.TitleAlignmentProperty);
			}
			set
			{
				base.SetValue(MetroWindow.TitleAlignmentProperty, value);
			}
		}

		public int TitlebarHeight
		{
			get
			{
				return (int)base.GetValue(MetroWindow.TitlebarHeightProperty);
			}
			set
			{
				base.SetValue(MetroWindow.TitlebarHeightProperty, value);
			}
		}

		public bool TitleCaps
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.TitleCapsProperty);
			}
			set
			{
				base.SetValue(MetroWindow.TitleCapsProperty, value);
			}
		}

		public Brush TitleForeground
		{
			get
			{
				return (Brush)base.GetValue(MetroWindow.TitleForegroundProperty);
			}
			set
			{
				base.SetValue(MetroWindow.TitleForegroundProperty, value);
			}
		}

		public DataTemplate TitleTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(MetroWindow.TitleTemplateProperty);
			}
			set
			{
				base.SetValue(MetroWindow.TitleTemplateProperty, value);
			}
		}

		public bool UseNoneWindowStyle
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.UseNoneWindowStyleProperty);
			}
			set
			{
				base.SetValue(MetroWindow.UseNoneWindowStyleProperty, value);
			}
		}

		public WindowCommandsOverlayBehavior WindowButtonCommandsOverlayBehavior
		{
			get
			{
				return (WindowCommandsOverlayBehavior)base.GetValue(MetroWindow.WindowButtonCommandsOverlayBehaviorProperty);
			}
			set
			{
				base.SetValue(MetroWindow.WindowButtonCommandsOverlayBehaviorProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release. You should use LightCloseButtonStyle or DarkCloseButtonStyle in WindowButtonCommands to override the style.")]
		public System.Windows.Style WindowCloseButtonStyle
		{
			get
			{
				return (System.Windows.Style)base.GetValue(MetroWindow.WindowCloseButtonStyleProperty);
			}
			set
			{
				base.SetValue(MetroWindow.WindowCloseButtonStyleProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release. You should use LightMaxButtonStyle or DarkMaxButtonStyle in WindowButtonCommands to override the style.")]
		public System.Windows.Style WindowMaxButtonStyle
		{
			get
			{
				return (System.Windows.Style)base.GetValue(MetroWindow.WindowMaxButtonStyleProperty);
			}
			set
			{
				base.SetValue(MetroWindow.WindowMaxButtonStyleProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release. You should use LightMinButtonStyle or DarkMinButtonStyle in WindowButtonCommands to override the style.")]
		public System.Windows.Style WindowMinButtonStyle
		{
			get
			{
				return (System.Windows.Style)base.GetValue(MetroWindow.WindowMinButtonStyleProperty);
			}
			set
			{
				base.SetValue(MetroWindow.WindowMinButtonStyleProperty, value);
			}
		}

		public IWindowPlacementSettings WindowPlacementSettings
		{
			get
			{
				return (IWindowPlacementSettings)base.GetValue(MetroWindow.WindowPlacementSettingsProperty);
			}
			set
			{
				base.SetValue(MetroWindow.WindowPlacementSettingsProperty, value);
			}
		}

		public string WindowTitle
		{
			get
			{
				if (!this.TitleCaps)
				{
					return base.Title;
				}
				return base.Title.ToUpper();
			}
		}

		public Brush WindowTitleBrush
		{
			get
			{
				return (Brush)base.GetValue(MetroWindow.WindowTitleBrushProperty);
			}
			set
			{
				base.SetValue(MetroWindow.WindowTitleBrushProperty, value);
			}
		}

		public bool WindowTransitionsEnabled
		{
			get
			{
				return (bool)base.GetValue(MetroWindow.WindowTransitionsEnabledProperty);
			}
			set
			{
				base.SetValue(MetroWindow.WindowTransitionsEnabledProperty, value);
			}
		}

		static MetroWindow()
		{
			Class6.yDnXvgqzyB5jw();
			MetroWindow.ShowIconOnTitleBarProperty = DependencyProperty.Register("ShowIconOnTitleBar", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true, new PropertyChangedCallback(MetroWindow.OnShowIconOnTitleBarPropertyChangedCallback)));
			MetroWindow.IconEdgeModeProperty = DependencyProperty.Register("IconEdgeMode", typeof(EdgeMode), typeof(MetroWindow), new PropertyMetadata((object)EdgeMode.Aliased));
			MetroWindow.IconBitmapScalingModeProperty = DependencyProperty.Register("IconBitmapScalingMode", typeof(BitmapScalingMode), typeof(MetroWindow), new PropertyMetadata((object)BitmapScalingMode.HighQuality));
			MetroWindow.ShowTitleBarProperty = DependencyProperty.Register("ShowTitleBar", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true, new PropertyChangedCallback(MetroWindow.OnShowTitleBarPropertyChangedCallback), new CoerceValueCallback(MetroWindow.OnShowTitleBarCoerceValueCallback)));
			MetroWindow.ShowMinButtonProperty = DependencyProperty.Register("ShowMinButton", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			MetroWindow.ShowMaxRestoreButtonProperty = DependencyProperty.Register("ShowMaxRestoreButton", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			MetroWindow.ShowCloseButtonProperty = DependencyProperty.Register("ShowCloseButton", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			MetroWindow.IsMinButtonEnabledProperty = DependencyProperty.Register("IsMinButtonEnabled", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			MetroWindow.IsMaxRestoreButtonEnabledProperty = DependencyProperty.Register("IsMaxRestoreButtonEnabled", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			MetroWindow.IsCloseButtonEnabledProperty = DependencyProperty.Register("IsCloseButtonEnabled", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			MetroWindow.ShowSystemMenuOnRightClickProperty = DependencyProperty.Register("ShowSystemMenuOnRightClick", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			MetroWindow.TitlebarHeightProperty = DependencyProperty.Register("TitlebarHeight", typeof(int), typeof(MetroWindow), new PropertyMetadata((object)30, new PropertyChangedCallback(MetroWindow.TitlebarHeightPropertyChangedCallback)));
			MetroWindow.TitleCapsProperty = DependencyProperty.Register("TitleCaps", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			MetroWindow.TitleAlignmentProperty = DependencyProperty.Register("TitleAlignment", typeof(System.Windows.HorizontalAlignment), typeof(MetroWindow), new PropertyMetadata((object)System.Windows.HorizontalAlignment.Stretch));
			MetroWindow.SaveWindowPositionProperty = DependencyProperty.Register("SaveWindowPosition", typeof(bool), typeof(MetroWindow), new PropertyMetadata(false));
			MetroWindow.WindowPlacementSettingsProperty = DependencyProperty.Register("WindowPlacementSettings", typeof(IWindowPlacementSettings), typeof(MetroWindow), new PropertyMetadata(null));
			MetroWindow.TitleForegroundProperty = DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(MetroWindow));
			MetroWindow.IgnoreTaskbarOnMaximizeProperty = DependencyProperty.Register("IgnoreTaskbarOnMaximize", typeof(bool), typeof(MetroWindow), new PropertyMetadata(false));
			MetroWindow.FlyoutsProperty = DependencyProperty.Register("Flyouts", typeof(FlyoutsControl), typeof(MetroWindow), new PropertyMetadata(null));
			MetroWindow.WindowTransitionsEnabledProperty = DependencyProperty.Register("WindowTransitionsEnabled", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			MetroWindow.MetroDialogOptionsProperty = DependencyProperty.Register("MetroDialogOptions", typeof(MetroDialogSettings), typeof(MetroWindow), new PropertyMetadata(new MetroDialogSettings()));
			MetroWindow.WindowTitleBrushProperty = DependencyProperty.Register("WindowTitleBrush", typeof(Brush), typeof(MetroWindow), new PropertyMetadata(Brushes.Transparent));
			MetroWindow.GlowBrushProperty = DependencyProperty.Register("GlowBrush", typeof(Brush), typeof(MetroWindow), new PropertyMetadata(null));
			MetroWindow.NonActiveGlowBrushProperty = DependencyProperty.Register("NonActiveGlowBrush", typeof(Brush), typeof(MetroWindow), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(153, 153, 153))));
			MetroWindow.NonActiveBorderBrushProperty = DependencyProperty.Register("NonActiveBorderBrush", typeof(Brush), typeof(MetroWindow), new PropertyMetadata(Brushes.Gray));
			MetroWindow.NonActiveWindowTitleBrushProperty = DependencyProperty.Register("NonActiveWindowTitleBrush", typeof(Brush), typeof(MetroWindow), new PropertyMetadata(Brushes.Gray));
			MetroWindow.IconTemplateProperty = DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(MetroWindow), new PropertyMetadata(null));
			MetroWindow.TitleTemplateProperty = DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(MetroWindow), new PropertyMetadata(null));
			MetroWindow.LeftWindowCommandsProperty = DependencyProperty.Register("LeftWindowCommands", typeof(WindowCommands), typeof(MetroWindow), new PropertyMetadata(null));
			MetroWindow.RightWindowCommandsProperty = DependencyProperty.Register("RightWindowCommands", typeof(WindowCommands), typeof(MetroWindow), new PropertyMetadata(null));
			MetroWindow.LeftWindowCommandsOverlayBehaviorProperty = DependencyProperty.Register("LeftWindowCommandsOverlayBehavior", typeof(WindowCommandsOverlayBehavior), typeof(MetroWindow), new PropertyMetadata((object)WindowCommandsOverlayBehavior.Always));
			MetroWindow.RightWindowCommandsOverlayBehaviorProperty = DependencyProperty.Register("RightWindowCommandsOverlayBehavior", typeof(WindowCommandsOverlayBehavior), typeof(MetroWindow), new PropertyMetadata((object)WindowCommandsOverlayBehavior.Always));
			MetroWindow.WindowButtonCommandsOverlayBehaviorProperty = DependencyProperty.Register("WindowButtonCommandsOverlayBehavior", typeof(WindowCommandsOverlayBehavior), typeof(MetroWindow), new PropertyMetadata((object)WindowCommandsOverlayBehavior.Always));
			MetroWindow.IconOverlayBehaviorProperty = DependencyProperty.Register("IconOverlayBehavior", typeof(WindowCommandsOverlayBehavior), typeof(MetroWindow), new PropertyMetadata((object)WindowCommandsOverlayBehavior.Never));
			MetroWindow.WindowMinButtonStyleProperty = DependencyProperty.Register("WindowMinButtonStyle", typeof(System.Windows.Style), typeof(MetroWindow), new PropertyMetadata(null, new PropertyChangedCallback(MetroWindow.OnWindowButtonStyleChanged)));
			MetroWindow.WindowMaxButtonStyleProperty = DependencyProperty.Register("WindowMaxButtonStyle", typeof(System.Windows.Style), typeof(MetroWindow), new PropertyMetadata(null, new PropertyChangedCallback(MetroWindow.OnWindowButtonStyleChanged)));
			MetroWindow.WindowCloseButtonStyleProperty = DependencyProperty.Register("WindowCloseButtonStyle", typeof(System.Windows.Style), typeof(MetroWindow), new PropertyMetadata(null, new PropertyChangedCallback(MetroWindow.OnWindowButtonStyleChanged)));
			MetroWindow.UseNoneWindowStyleProperty = DependencyProperty.Register("UseNoneWindowStyle", typeof(bool), typeof(MetroWindow), new PropertyMetadata(false, new PropertyChangedCallback(MetroWindow.OnUseNoneWindowStylePropertyChangedCallback)));
			MetroWindow.OverrideDefaultWindowCommandsBrushProperty = DependencyProperty.Register("OverrideDefaultWindowCommandsBrush", typeof(SolidColorBrush), typeof(MetroWindow));
			MetroWindow.EnableDWMDropShadowProperty = DependencyProperty.Register("EnableDWMDropShadow", typeof(bool), typeof(MetroWindow), new PropertyMetadata(false, new PropertyChangedCallback(MetroWindow.OnEnableDWMDropShadowPropertyChangedCallback)));
			MetroWindow.IsWindowDraggableProperty = DependencyProperty.Register("IsWindowDraggable", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			MetroWindow.FlyoutsStatusChangedEvent = EventManager.RegisterRoutedEvent("FlyoutsStatusChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MetroWindow));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroWindow), new FrameworkPropertyMetadata(typeof(MetroWindow)));
		}

		public MetroWindow()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			base.Loaded += new RoutedEventHandler(this.MetroWindow_Loaded);
		}

		private void ClearWindowEvents()
		{
			if (this.windowTitleThumb != null)
			{
				this.windowTitleThumb.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.WindowTitleThumbOnPreviewMouseLeftButtonUp);
				this.windowTitleThumb.DragDelta -= new DragDeltaEventHandler(this.WindowTitleThumbMoveOnDragDelta);
				this.windowTitleThumb.MouseDoubleClick -= new MouseButtonEventHandler(this.WindowTitleThumbChangeWindowStateOnMouseDoubleClick);
				this.windowTitleThumb.MouseRightButtonUp -= new MouseButtonEventHandler(this.WindowTitleThumbSystemMenuOnMouseRightButtonUp);
			}
			if (this.icon != null)
			{
				this.icon.MouseDown -= new MouseButtonEventHandler(this.IconMouseDown);
			}
			base.SizeChanged -= new SizeChangedEventHandler(this.MetroWindow_SizeChanged);
		}

		internal static void DoWindowTitleThumbChangeWindowStateOnMouseDoubleClick(MetroWindow window, MouseButtonEventArgs mouseButtonEventArgs)
		{
			if (mouseButtonEventArgs.ChangedButton == MouseButton.Left)
			{
				bool flag = (window.ResizeMode == System.Windows.ResizeMode.CanResizeWithGrip ? true : window.ResizeMode == System.Windows.ResizeMode.CanResize);
				if (flag & (Mouse.GetPosition(window).Y > (double)window.TitlebarHeight ? false : window.TitlebarHeight > 0))
				{
					if (window.WindowState != System.Windows.WindowState.Maximized)
					{
						Microsoft.Windows.Shell.SystemCommands.MaximizeWindow(window);
					}
					else
					{
						Microsoft.Windows.Shell.SystemCommands.RestoreWindow(window);
					}
					mouseButtonEventArgs.Handled = true;
				}
			}
		}

		internal static void DoWindowTitleThumbMoveOnDragDelta(MetroWindow window, DragDeltaEventArgs dragDeltaEventArgs)
		{
			if (!window.IsWindowDraggable || Math.Abs(dragDeltaEventArgs.HorizontalChange) <= 2 && Math.Abs(dragDeltaEventArgs.VerticalChange) <= 2)
			{
				return;
			}
			window.VerifyAccess();
			Standard.POINT cursorPos = NativeMethods.GetCursorPos();
			bool windowState = window.WindowState == System.Windows.WindowState.Maximized;
			if ((Mouse.GetPosition(window.windowTitleThumb).Y > (double)window.TitlebarHeight ? 0 : (int)(window.TitlebarHeight > 0)) == 0 & windowState)
			{
				return;
			}
			UnsafeNativeMethods.ReleaseCapture();
			if (windowState)
			{
				window.Top = 2;
				MetroWindow metroWindow = window;
				double num = (double)cursorPos.x;
				Rect restoreBounds = window.RestoreBounds;
				metroWindow.Left = Math.Max(num - restoreBounds.Width / 2, 0);
				EventHandler eventHandler = null;
				eventHandler = (object sender, EventArgs e) => {
					window.StateChanged -= eventHandler;
					if (window.WindowState == System.Windows.WindowState.Normal)
					{
						Mouse.Capture(window.windowTitleThumb, CaptureMode.Element);
					}
				};
				window.StateChanged += eventHandler;
			}
			IntPtr criticalHandle = window.CriticalHandle;
			NativeMethods.SendMessage(criticalHandle, WM.SYSCOMMAND, (IntPtr)61458, IntPtr.Zero);
			NativeMethods.SendMessage(criticalHandle, WM.LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
		}

		internal static void DoWindowTitleThumbOnPreviewMouseLeftButtonUp(MetroWindow window, MouseButtonEventArgs mouseButtonEventArgs)
		{
			Mouse.Capture(null);
		}

		internal static void DoWindowTitleThumbSystemMenuOnMouseRightButtonUp(MetroWindow window, MouseButtonEventArgs e)
		{
			if (window.ShowSystemMenuOnRightClick)
			{
				Point position = e.GetPosition(window);
				if (position.Y <= (double)window.TitlebarHeight && window.TitlebarHeight > 0 || window.UseNoneWindowStyle && window.TitlebarHeight <= 0)
				{
					MetroWindow.ShowSystemMenuPhysicalCoordinates(window, window.PointToScreen(position));
				}
			}
		}

		private void FlyoutsPreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			Func<Flyout, bool> func = null;
			DependencyObject originalSource = e.OriginalSource as DependencyObject;
			if (originalSource != null && (originalSource.TryFindParent<Flyout>() != null || object.Equals(originalSource.TryFindParent<ContentControl>(), this.icon) || originalSource.TryFindParent<WindowCommands>() != null || originalSource.TryFindParent<MahApps.Metro.Controls.WindowButtonCommands>() != null))
			{
				return;
			}
			MouseButton? overrideExternalCloseButton = this.Flyouts.OverrideExternalCloseButton;
			if (overrideExternalCloseButton.HasValue)
			{
				overrideExternalCloseButton = this.Flyouts.OverrideExternalCloseButton;
				MouseButton changedButton = e.ChangedButton;
				if ((overrideExternalCloseButton.GetValueOrDefault() == changedButton ? overrideExternalCloseButton.HasValue : false))
				{
					foreach (Flyout flyout in this.Flyouts.GetFlyouts().Where<Flyout>((Flyout x) => {
						if (!x.IsOpen)
						{
							return false;
						}
						if (!x.IsPinned)
						{
							return true;
						}
						return this.Flyouts.OverrideIsPinned;
					}))
					{
						flyout.IsOpen = false;
						e.Handled = true;
					}
				}
			}
			else
			{
				IEnumerable<Flyout> flyouts = this.Flyouts.GetFlyouts();
				Func<Flyout, bool> func1 = func;
				if (func1 == null)
				{
					Func<Flyout, bool> isOpen = (Flyout x) => {
						if (!x.IsOpen || x.ExternalCloseButton != e.ChangedButton)
						{
							return false;
						}
						if (!x.IsPinned)
						{
							return true;
						}
						return this.Flyouts.OverrideIsPinned;
					};
					Func<Flyout, bool> func2 = isOpen;
					func = isOpen;
					func1 = func2;
				}
				foreach (Flyout flyout1 in flyouts.Where<Flyout>(func1))
				{
					flyout1.IsOpen = false;
					e.Handled = true;
				}
			}
		}

		internal T GetPart<T>(string name)
		where T : DependencyObject
		{
			return (T)(base.GetTemplateChild(name) as T);
		}

		internal DependencyObject GetPart(string name)
		{
			return base.GetTemplateChild(name);
		}

		public virtual IWindowPlacementSettings GetWindowPlacementSettings()
		{
			object windowPlacementSettings = this.WindowPlacementSettings;
			if (windowPlacementSettings == null)
			{
				windowPlacementSettings = new WindowApplicationSettings(this);
			}
			return windowPlacementSettings;
		}

		internal void HandleFlyoutStatusChange(Flyout flyout, IEnumerable<Flyout> visibleFlyouts)
		{
			if (flyout.Position == Position.Left || flyout.Position == Position.Right || flyout.Position == Position.Top)
			{
				int num = (flyout.IsOpen ? Panel.GetZIndex(flyout) + 3 : visibleFlyouts.Count<Flyout>() + 2);
				if (this.icon != null)
				{
					this.icon.SetValue(Panel.ZIndexProperty, (this.IconOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.Flyouts) ? num : 1));
				}
				if (this.LeftWindowCommandsPresenter != null)
				{
					this.LeftWindowCommandsPresenter.SetValue(Panel.ZIndexProperty, (this.LeftWindowCommandsOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.Flyouts) ? num : 1));
				}
				if (this.RightWindowCommandsPresenter != null)
				{
					this.RightWindowCommandsPresenter.SetValue(Panel.ZIndexProperty, (this.RightWindowCommandsOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.Flyouts) ? num : 1));
				}
				if (this.WindowButtonCommands != null)
				{
					this.WindowButtonCommands.SetValue(Panel.ZIndexProperty, (this.WindowButtonCommandsOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.Flyouts) ? num : 1));
				}
				this.HandleWindowCommandsForFlyouts(visibleFlyouts, null);
			}
			this.flyoutModal.Visibility = (visibleFlyouts.Any<Flyout>((Flyout x) => x.IsModal) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden);
			base.RaiseEvent(new MetroWindow.FlyoutStatusChangedRoutedEventArgs(MetroWindow.FlyoutsStatusChangedEvent, this)
			{
				ChangedFlyout = flyout
			});
		}

		public void HideOverlay()
		{
			this.overlayBox.SetCurrentValue(UIElement.OpacityProperty, 0);
			this.overlayBox.Visibility = System.Windows.Visibility.Hidden;
		}

		public Task HideOverlayAsync()
		{
			if (this.overlayBox == null)
			{
				throw new InvalidOperationException("OverlayBox can not be founded in this MetroWindow's template. Are you calling this before the window has loaded?");
			}
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			if (this.overlayBox.Visibility == System.Windows.Visibility.Visible && this.overlayBox.Opacity == 0)
			{
				taskCompletionSource.SetResult(null);
				return taskCompletionSource.Task;
			}
			base.Dispatcher.VerifyAccess();
			Storyboard item = (Storyboard)base.Template.Resources["OverlayFastSemiFadeOut"];
			item = item.Clone();
			EventHandler visibility = null;
			visibility = (object sender, EventArgs e) => {
				item.Completed -= visibility;
				if (this.overlayStoryboard == item)
				{
					this.overlayBox.Visibility = System.Windows.Visibility.Hidden;
					this.overlayStoryboard = null;
				}
				taskCompletionSource.TrySetResult(null);
			};
			item.Completed += visibility;
			this.overlayBox.BeginStoryboard(item);
			this.overlayStoryboard = item;
			return taskCompletionSource.Task;
		}

		private void IconMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				if (e.ClickCount == 2)
				{
					base.Close();
					return;
				}
				MetroWindow.ShowSystemMenuPhysicalCoordinates(this, base.PointToScreen(new Point(0, (double)this.TitlebarHeight)));
			}
		}

		public bool IsOverlayVisible()
		{
			if (this.overlayBox == null)
			{
				throw new InvalidOperationException("OverlayBox can not be founded in this MetroWindow's template. Are you calling this before the window has loaded?");
			}
			if (this.overlayBox.Visibility != System.Windows.Visibility.Visible)
			{
				return false;
			}
			return this.overlayBox.Opacity >= 0.7;
		}

		private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
		{
			if (this.EnableDWMDropShadow)
			{
				this.UseDropShadow();
			}
			if (this.WindowTransitionsEnabled)
			{
				VisualStateManager.GoToState(this, "AfterLoaded", true);
			}
			this.ToggleNoneWindowStyle(this.UseNoneWindowStyle);
			if (this.Flyouts == null)
			{
				this.Flyouts = new FlyoutsControl();
			}
			this.ResetAllWindowCommandsBrush();
			ThemeManager.IsThemeChanged += new EventHandler<OnThemeChangedEventArgs>(this.ThemeManagerOnIsThemeChanged);
			base.Unloaded += new RoutedEventHandler((object argument0, RoutedEventArgs argument1) => ThemeManager.IsThemeChanged -= new EventHandler<OnThemeChangedEventArgs>(this.ThemeManagerOnIsThemeChanged));
		}

		private void MetroWindow_SizeChanged(object sender, RoutedEventArgs e)
		{
			Grid grid = (Grid)this.titleBar;
			ContentControl content = (ContentControl)((Label)grid.Children[0]).Content;
			ContentControl contentControl = (ContentControl)this.icon;
			double width = base.Width / 2;
			double actualWidth = content.ActualWidth / 2;
			double num = contentControl.ActualWidth + this.LeftWindowCommands.ActualWidth;
			double actualWidth1 = this.WindowButtonCommands.ActualWidth + this.RightWindowCommands.ActualWidth;
			if (num + actualWidth + 5 < width && actualWidth1 + actualWidth + 5 < width)
			{
				Grid.SetColumn(grid, 0);
				Grid.SetColumnSpan(grid, 5);
				return;
			}
			Grid.SetColumn(grid, 2);
			Grid.SetColumnSpan(grid, 1);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this.LeftWindowCommands == null)
			{
				this.LeftWindowCommands = new WindowCommands();
			}
			if (this.RightWindowCommands == null)
			{
				this.RightWindowCommands = new WindowCommands();
			}
			this.LeftWindowCommands.ParentWindow = this;
			this.RightWindowCommands.ParentWindow = this;
			this.LeftWindowCommandsPresenter = base.GetTemplateChild("PART_LeftWindowCommands") as ContentPresenter;
			this.RightWindowCommandsPresenter = base.GetTemplateChild("PART_RightWindowCommands") as ContentPresenter;
			this.WindowButtonCommands = base.GetTemplateChild("PART_WindowButtonCommands") as MahApps.Metro.Controls.WindowButtonCommands;
			if (this.WindowButtonCommands != null)
			{
				this.WindowButtonCommands.ParentWindow = this;
			}
			this.overlayBox = base.GetTemplateChild("PART_OverlayBox") as Grid;
			this.metroActiveDialogContainer = base.GetTemplateChild("PART_MetroActiveDialogContainer") as Grid;
			this.metroInactiveDialogContainer = base.GetTemplateChild("PART_MetroInactiveDialogsContainer") as Grid;
			this.flyoutModal = (Rectangle)base.GetTemplateChild("PART_FlyoutModal");
			this.flyoutModal.PreviewMouseDown += new MouseButtonEventHandler(this.FlyoutsPreviewMouseDown);
			base.PreviewMouseDown += new MouseButtonEventHandler(this.FlyoutsPreviewMouseDown);
			this.icon = base.GetTemplateChild("PART_Icon") as UIElement;
			this.titleBar = base.GetTemplateChild("PART_TitleBar") as UIElement;
			this.titleBarBackground = base.GetTemplateChild("PART_WindowTitleBackground") as UIElement;
			this.windowTitleThumb = base.GetTemplateChild("PART_WindowTitleThumb") as Thumb;
			this.SetVisibiltyForAllTitleElements(this.TitlebarHeight > 0);
		}

		private static void OnEnableDWMDropShadowPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue && (bool)e.NewValue)
			{
				((MetroWindow)d).UseDropShadow();
			}
		}

		private static void OnShowIconOnTitleBarPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MetroWindow metroWindow = (MetroWindow)d;
			if (e.NewValue != e.OldValue)
			{
				metroWindow.SetVisibiltyForIcon();
			}
		}

		private static object OnShowTitleBarCoerceValueCallback(DependencyObject d, object value)
		{
			if (!((MetroWindow)d).UseNoneWindowStyle)
			{
				return value;
			}
			return false;
		}

		private static void OnShowTitleBarPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MetroWindow metroWindow = (MetroWindow)d;
			if (e.NewValue != e.OldValue)
			{
				metroWindow.SetVisibiltyForAllTitleElements((bool)e.NewValue);
			}
		}

		private static void OnUseNoneWindowStylePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
			{
				bool newValue = (bool)e.NewValue;
				((MetroWindow)d).ToggleNoneWindowStyle(newValue);
			}
		}

		public static void OnWindowButtonStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == e.OldValue)
			{
				return;
			}
			MetroWindow metroWindow = (MetroWindow)d;
			if (metroWindow.WindowButtonCommands != null)
			{
				metroWindow.WindowButtonCommands.ApplyTheme();
			}
		}

		private void SetVisibiltyForAllTitleElements(bool visible)
		{
			this.SetVisibiltyForIcon();
			System.Windows.Visibility visibility = (!visible || !this.ShowTitleBar ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible);
			if (this.titleBar != null)
			{
				this.titleBar.Visibility = visibility;
			}
			if (this.titleBarBackground != null)
			{
				this.titleBarBackground.Visibility = visibility;
			}
			if (this.LeftWindowCommandsPresenter != null)
			{
				this.LeftWindowCommandsPresenter.Visibility = (this.LeftWindowCommandsOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.HiddenTitleBar) ? System.Windows.Visibility.Visible : visibility);
			}
			if (this.RightWindowCommandsPresenter != null)
			{
				this.RightWindowCommandsPresenter.Visibility = (this.RightWindowCommandsOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.HiddenTitleBar) ? System.Windows.Visibility.Visible : visibility);
			}
			if (this.WindowButtonCommands != null)
			{
				this.WindowButtonCommands.Visibility = (this.WindowButtonCommandsOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.HiddenTitleBar) ? System.Windows.Visibility.Visible : visibility);
			}
			this.SetWindowEvents();
		}

		private void SetVisibiltyForIcon()
		{
			bool flag;
			if (this.icon != null)
			{
				if (!this.IconOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.HiddenTitleBar) || this.ShowTitleBar)
				{
					flag = (!this.ShowIconOnTitleBar ? false : this.ShowTitleBar);
				}
				else
				{
					flag = true;
				}
				this.icon.Visibility = (flag ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed);
			}
		}

		private void SetWindowEvents()
		{
			this.ClearWindowEvents();
			if (this.icon != null && this.icon.Visibility == System.Windows.Visibility.Visible)
			{
				this.icon.MouseDown += new MouseButtonEventHandler(this.IconMouseDown);
			}
			if (this.windowTitleThumb != null)
			{
				this.windowTitleThumb.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.WindowTitleThumbOnPreviewMouseLeftButtonUp);
				this.windowTitleThumb.DragDelta += new DragDeltaEventHandler(this.WindowTitleThumbMoveOnDragDelta);
				this.windowTitleThumb.MouseDoubleClick += new MouseButtonEventHandler(this.WindowTitleThumbChangeWindowStateOnMouseDoubleClick);
				this.windowTitleThumb.MouseRightButtonUp += new MouseButtonEventHandler(this.WindowTitleThumbSystemMenuOnMouseRightButtonUp);
			}
			if (this.titleBar != null && this.titleBar.GetType() == typeof(Grid))
			{
				base.SizeChanged += new SizeChangedEventHandler(this.MetroWindow_SizeChanged);
			}
		}

		public void ShowOverlay()
		{
			this.overlayBox.Visibility = System.Windows.Visibility.Visible;
			this.overlayBox.SetCurrentValue(UIElement.OpacityProperty, 0.7);
		}

		public Task ShowOverlayAsync()
		{
			if (this.overlayBox == null)
			{
				throw new InvalidOperationException("OverlayBox can not be founded in this MetroWindow's template. Are you calling this before the window has loaded?");
			}
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			if (this.IsOverlayVisible() && this.overlayStoryboard == null)
			{
				taskCompletionSource.SetResult(null);
				return taskCompletionSource.Task;
			}
			base.Dispatcher.VerifyAccess();
			this.overlayBox.Visibility = System.Windows.Visibility.Visible;
			Storyboard item = (Storyboard)base.Template.Resources["OverlayFastSemiFadeIn"];
			item = item.Clone();
			EventHandler eventHandler = null;
			eventHandler = (object sender, EventArgs e) => {
				item.Completed -= eventHandler;
				if (this.overlayStoryboard == item)
				{
					this.overlayStoryboard = null;
				}
				taskCompletionSource.TrySetResult(null);
			};
			item.Completed += eventHandler;
			this.overlayBox.BeginStoryboard(item);
			this.overlayStoryboard = item;
			return taskCompletionSource.Task;
		}

		private static void ShowSystemMenuPhysicalCoordinates(Window window, Point physicalScreenLocation)
		{
			if (window == null)
			{
				return;
			}
			IntPtr handle = (new WindowInteropHelper(window)).Handle;
			if (handle == IntPtr.Zero || !UnsafeNativeMethods.IsWindow(handle))
			{
				return;
			}
			IntPtr systemMenu = UnsafeNativeMethods.GetSystemMenu(handle, false);
			uint num = UnsafeNativeMethods.TrackPopupMenuEx(systemMenu, 256, (int)physicalScreenLocation.X, (int)physicalScreenLocation.Y, handle, IntPtr.Zero);
			if (num != 0)
			{
				UnsafeNativeMethods.PostMessage(handle, 274, new IntPtr((long)num), IntPtr.Zero);
			}
		}

		private void ThemeManagerOnIsThemeChanged(object sender, OnThemeChangedEventArgs e)
		{
			if (e.Accent != null)
			{
				List<Flyout> list = this.Flyouts.GetFlyouts().ToList<Flyout>();
				List<FlyoutsControl> flyoutsControls = (base.Content as DependencyObject).FindChildren<FlyoutsControl>(true).ToList<FlyoutsControl>();
				if (flyoutsControls.Any<FlyoutsControl>())
				{
					list.AddRange(flyoutsControls.SelectMany<FlyoutsControl, Flyout>((FlyoutsControl flyoutsControl) => flyoutsControl.GetFlyouts()));
				}
				if (!list.Any<Flyout>())
				{
					this.ResetAllWindowCommandsBrush();
					return;
				}
				foreach (Flyout flyout in list)
				{
					flyout.ChangeFlyoutTheme(e.Accent, e.AppTheme);
				}
				this.HandleWindowCommandsForFlyouts(list, null);
			}
		}

		private static void TitlebarHeightPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			MetroWindow metroWindow = (MetroWindow)dependencyObject;
			if (e.NewValue != e.OldValue)
			{
				metroWindow.SetVisibiltyForAllTitleElements((int)e.NewValue > 0);
			}
		}

		private void ToggleNoneWindowStyle(bool useNoneWindowStyle)
		{
			if (useNoneWindowStyle)
			{
				this.ShowTitleBar = false;
			}
			if (this.LeftWindowCommandsPresenter != null)
			{
				this.LeftWindowCommandsPresenter.Visibility = (useNoneWindowStyle ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible);
			}
			if (this.RightWindowCommandsPresenter != null)
			{
				this.RightWindowCommandsPresenter.Visibility = (useNoneWindowStyle ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible);
			}
		}

		private void UseDropShadow()
		{
			base.BorderThickness = new Thickness(0);
			base.BorderBrush = null;
			this.GlowBrush = Brushes.Black;
		}

		private static void WindowCommandsPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue && e.NewValue != null)
			{
				((MetroWindow)dependencyObject).RightWindowCommands = (WindowCommands)e.NewValue;
			}
		}

		private void WindowTitleThumbChangeWindowStateOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			MetroWindow.DoWindowTitleThumbChangeWindowStateOnMouseDoubleClick(this, e);
		}

		private void WindowTitleThumbMoveOnDragDelta(object sender, DragDeltaEventArgs e)
		{
			MetroWindow.DoWindowTitleThumbMoveOnDragDelta(this, e);
		}

		private void WindowTitleThumbOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			MetroWindow.DoWindowTitleThumbOnPreviewMouseLeftButtonUp(this, e);
		}

		private void WindowTitleThumbSystemMenuOnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			MetroWindow.DoWindowTitleThumbSystemMenuOnMouseRightButtonUp(this, e);
		}

		public event RoutedEventHandler FlyoutsStatusChanged
		{
			add
			{
				base.AddHandler(MetroWindow.FlyoutsStatusChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MetroWindow.FlyoutsStatusChangedEvent, value);
			}
		}

		public class FlyoutStatusChangedRoutedEventArgs : RoutedEventArgs
		{
			public Flyout ChangedFlyout
			{
				get;
				internal set;
			}

			internal FlyoutStatusChangedRoutedEventArgs(RoutedEvent rEvent, object source)
			{
				Class6.yDnXvgqzyB5jw();
				base(rEvent, source);
			}
		}
	}
}