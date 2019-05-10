using MahApps.Metro;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name="PART_BackButton", Type=typeof(Button))]
	[TemplatePart(Name="PART_Content", Type=typeof(ContentPresenter))]
	[TemplatePart(Name="PART_Header", Type=typeof(ContentPresenter))]
	[TemplatePart(Name="PART_WindowTitleThumb", Type=typeof(Thumb))]
	public class Flyout : ContentControl
	{
		public readonly static RoutedEvent IsOpenChangedEvent;

		public readonly static RoutedEvent ClosingFinishedEvent;

		public readonly static DependencyProperty HeaderProperty;

		public readonly static DependencyProperty PositionProperty;

		public readonly static DependencyProperty IsPinnedProperty;

		public readonly static DependencyProperty IsOpenProperty;

		public readonly static DependencyProperty AnimateOnPositionChangeProperty;

		public readonly static DependencyProperty AnimateOpacityProperty;

		public readonly static DependencyProperty IsModalProperty;

		public readonly static DependencyProperty HeaderTemplateProperty;

		public readonly static DependencyProperty CloseCommandProperty;

		public readonly static DependencyProperty ThemeProperty;

		public readonly static DependencyProperty ExternalCloseButtonProperty;

		public readonly static DependencyProperty CloseButtonVisibilityProperty;

		public readonly static DependencyProperty CloseButtonIsCancelProperty;

		public readonly static DependencyProperty TitleVisibilityProperty;

		public readonly static DependencyProperty AreAnimationsEnabledProperty;

		public readonly static DependencyProperty FocusedElementProperty;

		public readonly static DependencyProperty AllowFocusElementProperty;

		private MetroWindow parentWindow;

		private Grid root;

		private Storyboard hideStoryboard;

		private SplineDoubleKeyFrame hideFrame;

		private SplineDoubleKeyFrame hideFrameY;

		private SplineDoubleKeyFrame showFrame;

		private SplineDoubleKeyFrame showFrameY;

		private SplineDoubleKeyFrame fadeOutFrame;

		private ContentPresenter PART_Header;

		private ContentPresenter PART_Content;

		private Thumb windowTitleThumb;

		public bool AllowFocusElement
		{
			get
			{
				return (bool)base.GetValue(Flyout.AllowFocusElementProperty);
			}
			set
			{
				base.SetValue(Flyout.AllowFocusElementProperty, value);
			}
		}

		public bool AnimateOnPositionChange
		{
			get
			{
				return (bool)base.GetValue(Flyout.AnimateOnPositionChangeProperty);
			}
			set
			{
				base.SetValue(Flyout.AnimateOnPositionChangeProperty, value);
			}
		}

		public bool AnimateOpacity
		{
			get
			{
				return (bool)base.GetValue(Flyout.AnimateOpacityProperty);
			}
			set
			{
				base.SetValue(Flyout.AnimateOpacityProperty, value);
			}
		}

		public bool AreAnimationsEnabled
		{
			get
			{
				return (bool)base.GetValue(Flyout.AreAnimationsEnabledProperty);
			}
			set
			{
				base.SetValue(Flyout.AreAnimationsEnabledProperty, value);
			}
		}

		public bool CloseButtonIsCancel
		{
			get
			{
				return (bool)base.GetValue(Flyout.CloseButtonIsCancelProperty);
			}
			set
			{
				base.SetValue(Flyout.CloseButtonIsCancelProperty, value);
			}
		}

		public System.Windows.Visibility CloseButtonVisibility
		{
			get
			{
				return (System.Windows.Visibility)base.GetValue(Flyout.CloseButtonVisibilityProperty);
			}
			set
			{
				base.SetValue(Flyout.CloseButtonVisibilityProperty, value);
			}
		}

		public ICommand CloseCommand
		{
			get
			{
				return (ICommand)base.GetValue(Flyout.CloseCommandProperty);
			}
			set
			{
				base.SetValue(Flyout.CloseCommandProperty, value);
			}
		}

		public MouseButton ExternalCloseButton
		{
			get
			{
				return (MouseButton)base.GetValue(Flyout.ExternalCloseButtonProperty);
			}
			set
			{
				base.SetValue(Flyout.ExternalCloseButtonProperty, value);
			}
		}

		public FrameworkElement FocusedElement
		{
			get
			{
				return (FrameworkElement)base.GetValue(Flyout.FocusedElementProperty);
			}
			set
			{
				base.SetValue(Flyout.FocusedElementProperty, value);
			}
		}

		public string Header
		{
			get
			{
				return (string)base.GetValue(Flyout.HeaderProperty);
			}
			set
			{
				base.SetValue(Flyout.HeaderProperty, value);
			}
		}

		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(Flyout.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(Flyout.HeaderTemplateProperty, value);
			}
		}

		public bool IsModal
		{
			get
			{
				return (bool)base.GetValue(Flyout.IsModalProperty);
			}
			set
			{
				base.SetValue(Flyout.IsModalProperty, value);
			}
		}

		public bool IsOpen
		{
			get
			{
				return (bool)base.GetValue(Flyout.IsOpenProperty);
			}
			set
			{
				base.SetValue(Flyout.IsOpenProperty, value);
			}
		}

		internal PropertyChangeNotifier IsOpenPropertyChangeNotifier
		{
			get;
			set;
		}

		public bool IsPinned
		{
			get
			{
				return (bool)base.GetValue(Flyout.IsPinnedProperty);
			}
			set
			{
				base.SetValue(Flyout.IsPinnedProperty, value);
			}
		}

		private MetroWindow ParentWindow
		{
			get
			{
				MetroWindow metroWindow = this.parentWindow;
				if (metroWindow == null)
				{
					MetroWindow metroWindow1 = this.TryFindParent<MetroWindow>();
					MetroWindow metroWindow2 = metroWindow1;
					this.parentWindow = metroWindow1;
					metroWindow = metroWindow2;
				}
				return metroWindow;
			}
		}

		public MahApps.Metro.Controls.Position Position
		{
			get
			{
				return (MahApps.Metro.Controls.Position)base.GetValue(Flyout.PositionProperty);
			}
			set
			{
				base.SetValue(Flyout.PositionProperty, value);
			}
		}

		public FlyoutTheme Theme
		{
			get
			{
				return (FlyoutTheme)base.GetValue(Flyout.ThemeProperty);
			}
			set
			{
				base.SetValue(Flyout.ThemeProperty, value);
			}
		}

		internal PropertyChangeNotifier ThemePropertyChangeNotifier
		{
			get;
			set;
		}

		public System.Windows.Visibility TitleVisibility
		{
			get
			{
				return (System.Windows.Visibility)base.GetValue(Flyout.TitleVisibilityProperty);
			}
			set
			{
				base.SetValue(Flyout.TitleVisibilityProperty, value);
			}
		}

		static Flyout()
		{
			Class6.yDnXvgqzyB5jw();
			Flyout.IsOpenChangedEvent = EventManager.RegisterRoutedEvent("IsOpenChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Flyout));
			Flyout.ClosingFinishedEvent = EventManager.RegisterRoutedEvent("ClosingFinished", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Flyout));
			Flyout.HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(Flyout), new PropertyMetadata(null));
			Flyout.PositionProperty = DependencyProperty.Register("Position", typeof(MahApps.Metro.Controls.Position), typeof(Flyout), new PropertyMetadata((object)MahApps.Metro.Controls.Position.Left, new PropertyChangedCallback(Flyout.PositionChanged)));
			Flyout.IsPinnedProperty = DependencyProperty.Register("IsPinned", typeof(bool), typeof(Flyout), new PropertyMetadata(true));
			Flyout.IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(Flyout), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Flyout.IsOpenedChanged)));
			Flyout.AnimateOnPositionChangeProperty = DependencyProperty.Register("AnimateOnPositionChange", typeof(bool), typeof(Flyout), new PropertyMetadata(true));
			Flyout.AnimateOpacityProperty = DependencyProperty.Register("AnimateOpacity", typeof(bool), typeof(Flyout), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Flyout.AnimateOpacityChanged)));
			Flyout.IsModalProperty = DependencyProperty.Register("IsModal", typeof(bool), typeof(Flyout));
			Flyout.HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(Flyout));
			Flyout.CloseCommandProperty = DependencyProperty.RegisterAttached("CloseCommand", typeof(ICommand), typeof(Flyout), new UIPropertyMetadata(null));
			Flyout.ThemeProperty = DependencyProperty.Register("Theme", typeof(FlyoutTheme), typeof(Flyout), new FrameworkPropertyMetadata((object)FlyoutTheme.Dark, new PropertyChangedCallback(Flyout.ThemeChanged)));
			Flyout.ExternalCloseButtonProperty = DependencyProperty.Register("ExternalCloseButton", typeof(MouseButton), typeof(Flyout), new PropertyMetadata((object)MouseButton.Left));
			Flyout.CloseButtonVisibilityProperty = DependencyProperty.Register("CloseButtonVisibility", typeof(System.Windows.Visibility), typeof(Flyout), new FrameworkPropertyMetadata((object)System.Windows.Visibility.Visible));
			Flyout.CloseButtonIsCancelProperty = DependencyProperty.Register("CloseButtonIsCancel", typeof(bool), typeof(Flyout), new FrameworkPropertyMetadata(false));
			Flyout.TitleVisibilityProperty = DependencyProperty.Register("TitleVisibility", typeof(System.Windows.Visibility), typeof(Flyout), new FrameworkPropertyMetadata((object)System.Windows.Visibility.Visible));
			Flyout.AreAnimationsEnabledProperty = DependencyProperty.Register("AreAnimationsEnabled", typeof(bool), typeof(Flyout), new PropertyMetadata(true));
			Flyout.FocusedElementProperty = DependencyProperty.Register("FocusedElement", typeof(FrameworkElement), typeof(Flyout), new UIPropertyMetadata(null));
			Flyout.AllowFocusElementProperty = DependencyProperty.Register("AllowFocusElement", typeof(bool), typeof(Flyout), new PropertyMetadata(true));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Flyout), new FrameworkPropertyMetadata(typeof(Flyout)));
		}

		public Flyout()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			base.Loaded += new RoutedEventHandler((object sender, RoutedEventArgs e) => this.UpdateFlyoutTheme());
		}

		private static void AnimateOpacityChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			((Flyout)dependencyObject).UpdateOpacityChange();
		}

		internal void ApplyAnimation(MahApps.Metro.Controls.Position position, bool animateOpacity, bool resetShowFrame = true)
		{
			if (this.root == null || this.hideFrame == null || this.showFrame == null || this.hideFrameY == null || this.showFrameY == null || this.fadeOutFrame == null)
			{
				return;
			}
			if (this.Position == MahApps.Metro.Controls.Position.Left || this.Position == MahApps.Metro.Controls.Position.Right)
			{
				this.showFrame.Value = 0;
			}
			if (this.Position == MahApps.Metro.Controls.Position.Top || this.Position == MahApps.Metro.Controls.Position.Bottom)
			{
				this.showFrameY.Value = 0;
			}
			if (animateOpacity)
			{
				this.fadeOutFrame.Value = 0;
				if (!this.IsOpen)
				{
					this.root.Opacity = 0;
				}
			}
			else
			{
				this.fadeOutFrame.Value = 1;
				this.root.Opacity = 1;
			}
			switch (position)
			{
				case MahApps.Metro.Controls.Position.Right:
				{
					base.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
					base.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
					this.hideFrame.Value = this.root.ActualWidth;
					if (!resetShowFrame)
					{
						break;
					}
					this.root.RenderTransform = new TranslateTransform(this.root.ActualWidth, 0);
					return;
				}
				case MahApps.Metro.Controls.Position.Top:
				{
					base.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
					base.VerticalAlignment = System.Windows.VerticalAlignment.Top;
					this.hideFrameY.Value = -this.root.ActualHeight - 1;
					if (!resetShowFrame)
					{
						break;
					}
					this.root.RenderTransform = new TranslateTransform(0, -this.root.ActualHeight - 1);
					return;
				}
				case MahApps.Metro.Controls.Position.Bottom:
				{
					base.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
					base.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
					this.hideFrameY.Value = this.root.ActualHeight;
					if (!resetShowFrame)
					{
						break;
					}
					this.root.RenderTransform = new TranslateTransform(0, this.root.ActualHeight);
					break;
				}
				default:
				{
					base.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
					base.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
					this.hideFrame.Value = -this.root.ActualWidth;
					if (!resetShowFrame)
					{
						break;
					}
					this.root.RenderTransform = new TranslateTransform(-this.root.ActualWidth, 0);
					return;
				}
			}
		}

		internal void ChangeFlyoutTheme(Accent windowAccent, AppTheme windowTheme)
		{
			switch (this.Theme)
			{
				case FlyoutTheme.Adapt:
				{
					ThemeManager.ChangeAppStyle(base.Resources, windowAccent, windowTheme);
					return;
				}
				case FlyoutTheme.Inverse:
				{
					AppTheme inverseAppTheme = ThemeManager.GetInverseAppTheme(windowTheme);
					if (inverseAppTheme == null)
					{
						throw new InvalidOperationException("The inverse flyout theme only works if the window theme abides the naming convention. See ThemeManager.GetInverseAppTheme for more infos");
					}
					ThemeManager.ChangeAppStyle(base.Resources, windowAccent, inverseAppTheme);
					return;
				}
				case FlyoutTheme.Dark:
				{
					ThemeManager.ChangeAppStyle(base.Resources, windowAccent, ThemeManager.GetAppTheme("BaseDark"));
					return;
				}
				case FlyoutTheme.Light:
				{
					ThemeManager.ChangeAppStyle(base.Resources, windowAccent, ThemeManager.GetAppTheme("BaseLight"));
					return;
				}
				case FlyoutTheme.Accent:
				{
					ThemeManager.ChangeAppStyle(base.Resources, windowAccent, windowTheme);
					base.SetResourceReference(Control.BackgroundProperty, "HighlightBrush");
					base.SetResourceReference(Control.ForegroundProperty, "IdealForegroundColorBrush");
					return;
				}
				default:
				{
					return;
				}
			}
		}

		protected internal void CleanUp(FlyoutsControl flyoutsControl)
		{
			if (this.windowTitleThumb != null)
			{
				this.windowTitleThumb.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.WindowTitleThumbOnPreviewMouseLeftButtonUp);
				this.windowTitleThumb.DragDelta -= new DragDeltaEventHandler(this.WindowTitleThumbMoveOnDragDelta);
				this.windowTitleThumb.MouseDoubleClick -= new MouseButtonEventHandler(this.WindowTitleThumbChangeWindowStateOnMouseDoubleClick);
				this.windowTitleThumb.MouseRightButtonUp -= new MouseButtonEventHandler(this.WindowTitleThumbSystemMenuOnMouseRightButtonUp);
			}
			this.parentWindow = null;
		}

		private static Tuple<AppTheme, Accent> DetectTheme(Flyout flyout)
		{
			Tuple<AppTheme, Accent> tuple;
			Tuple<AppTheme, Accent> tuple1;
			if (flyout == null)
			{
				return null;
			}
			MetroWindow parentWindow = flyout.ParentWindow;
			if (parentWindow != null)
			{
				tuple = ThemeManager.DetectAppStyle(parentWindow);
			}
			else
			{
				tuple = null;
			}
			Tuple<AppTheme, Accent> tuple2 = tuple;
			if (tuple2 != null && tuple2.Item2 != null)
			{
				return tuple2;
			}
			if (Application.Current != null)
			{
				MetroWindow mainWindow = Application.Current.MainWindow as MetroWindow;
				if (mainWindow != null)
				{
					tuple1 = ThemeManager.DetectAppStyle(mainWindow);
				}
				else
				{
					tuple1 = null;
				}
				tuple2 = tuple1;
				if (tuple2 != null && tuple2.Item2 != null)
				{
					return tuple2;
				}
				tuple2 = ThemeManager.DetectAppStyle(Application.Current);
				if (tuple2 != null && tuple2.Item2 != null)
				{
					return tuple2;
				}
			}
			return null;
		}

		private void Hide()
		{
			base.Visibility = System.Windows.Visibility.Hidden;
			base.RaiseEvent(new RoutedEventArgs(Flyout.ClosingFinishedEvent));
		}

		private void HideStoryboard_Completed(object sender, EventArgs e)
		{
			this.hideStoryboard.Completed -= new EventHandler(this.HideStoryboard_Completed);
			this.Hide();
		}

		private static void IsOpenedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			Flyout flyout = (Flyout)dependencyObject;
			Action newValue = () => {
				if (e.NewValue != e.OldValue)
				{
					if (!flyout.AreAnimationsEnabled)
					{
						if (!(bool)e.NewValue)
						{
							flyout.Focus();
							flyout.Hide();
						}
						else
						{
							flyout.Visibility = System.Windows.Visibility.Visible;
							flyout.TryFocusElement();
						}
						VisualStateManager.GoToState(flyout, (!(bool)e.NewValue ? "HideDirect" : "ShowDirect"), true);
					}
					else
					{
						if (!(bool)e.NewValue)
						{
							flyout.Focus();
							if (flyout.hideStoryboard == null)
							{
								flyout.Hide();
							}
							else
							{
								flyout.hideStoryboard.Completed += new EventHandler(flyout.HideStoryboard_Completed);
							}
						}
						else
						{
							if (flyout.hideStoryboard != null)
							{
								flyout.hideStoryboard.Completed -= new EventHandler(flyout.HideStoryboard_Completed);
							}
							flyout.Visibility = System.Windows.Visibility.Visible;
							flyout.ApplyAnimation(flyout.Position, flyout.AnimateOpacity, true);
							flyout.TryFocusElement();
						}
						VisualStateManager.GoToState(flyout, (!(bool)e.NewValue ? "Hide" : "Show"), true);
					}
				}
				flyout.RaiseEvent(new RoutedEventArgs(Flyout.IsOpenChangedEvent));
			};
			flyout.Dispatcher.BeginInvoke(DispatcherPriority.Background, newValue);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.root = (Grid)base.GetTemplateChild("root");
			if (this.root == null)
			{
				return;
			}
			this.PART_Header = (ContentPresenter)base.GetTemplateChild("PART_Header");
			this.PART_Content = (ContentPresenter)base.GetTemplateChild("PART_Content");
			this.windowTitleThumb = base.GetTemplateChild("PART_WindowTitleThumb") as Thumb;
			if (this.windowTitleThumb != null)
			{
				this.windowTitleThumb.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.WindowTitleThumbOnPreviewMouseLeftButtonUp);
				this.windowTitleThumb.DragDelta -= new DragDeltaEventHandler(this.WindowTitleThumbMoveOnDragDelta);
				this.windowTitleThumb.MouseDoubleClick -= new MouseButtonEventHandler(this.WindowTitleThumbChangeWindowStateOnMouseDoubleClick);
				this.windowTitleThumb.MouseRightButtonUp -= new MouseButtonEventHandler(this.WindowTitleThumbSystemMenuOnMouseRightButtonUp);
				this.windowTitleThumb.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.WindowTitleThumbOnPreviewMouseLeftButtonUp);
				this.windowTitleThumb.DragDelta += new DragDeltaEventHandler(this.WindowTitleThumbMoveOnDragDelta);
				this.windowTitleThumb.MouseDoubleClick += new MouseButtonEventHandler(this.WindowTitleThumbChangeWindowStateOnMouseDoubleClick);
				this.windowTitleThumb.MouseRightButtonUp += new MouseButtonEventHandler(this.WindowTitleThumbSystemMenuOnMouseRightButtonUp);
			}
			this.hideStoryboard = (Storyboard)base.GetTemplateChild("HideStoryboard");
			this.hideFrame = (SplineDoubleKeyFrame)base.GetTemplateChild("hideFrame");
			this.hideFrameY = (SplineDoubleKeyFrame)base.GetTemplateChild("hideFrameY");
			this.showFrame = (SplineDoubleKeyFrame)base.GetTemplateChild("showFrame");
			this.showFrameY = (SplineDoubleKeyFrame)base.GetTemplateChild("showFrameY");
			this.fadeOutFrame = (SplineDoubleKeyFrame)base.GetTemplateChild("fadeOutFrame");
			if (this.hideFrame == null || this.showFrame == null || this.hideFrameY == null || this.showFrameY == null || this.fadeOutFrame == null)
			{
				return;
			}
			this.ApplyAnimation(this.Position, this.AnimateOpacity, true);
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			if (!this.IsOpen)
			{
				return;
			}
			if (!sizeInfo.WidthChanged && !sizeInfo.HeightChanged)
			{
				return;
			}
			if (this.root == null || this.hideFrame == null || this.showFrame == null || this.hideFrameY == null || this.showFrameY == null)
			{
				return;
			}
			if (this.Position == MahApps.Metro.Controls.Position.Left || this.Position == MahApps.Metro.Controls.Position.Right)
			{
				this.showFrame.Value = 0;
			}
			if (this.Position == MahApps.Metro.Controls.Position.Top || this.Position == MahApps.Metro.Controls.Position.Bottom)
			{
				this.showFrameY.Value = 0;
			}
			switch (this.Position)
			{
				case MahApps.Metro.Controls.Position.Right:
				{
					this.hideFrame.Value = this.root.ActualWidth;
					return;
				}
				case MahApps.Metro.Controls.Position.Top:
				{
					this.hideFrameY.Value = -this.root.ActualHeight - 1;
					return;
				}
				case MahApps.Metro.Controls.Position.Bottom:
				{
					this.hideFrameY.Value = this.root.ActualHeight;
					return;
				}
				default:
				{
					this.hideFrame.Value = -this.root.ActualWidth;
					return;
				}
			}
		}

		private static void PositionChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			Flyout flyout = (Flyout)dependencyObject;
			bool isOpen = flyout.IsOpen;
			bool flag = isOpen;
			if (!isOpen || !flyout.AnimateOnPositionChange)
			{
				flyout.ApplyAnimation((MahApps.Metro.Controls.Position)e.NewValue, flyout.AnimateOpacity, false);
			}
			else
			{
				flyout.ApplyAnimation((MahApps.Metro.Controls.Position)e.NewValue, flyout.AnimateOpacity, true);
				VisualStateManager.GoToState(flyout, "Hide", true);
			}
			if (flag && flyout.AnimateOnPositionChange)
			{
				flyout.ApplyAnimation((MahApps.Metro.Controls.Position)e.NewValue, flyout.AnimateOpacity, true);
				VisualStateManager.GoToState(flyout, "Show", true);
			}
		}

		private static void ThemeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			((Flyout)dependencyObject).UpdateFlyoutTheme();
		}

		private void TryFocusElement()
		{
			if (this.AllowFocusElement)
			{
				base.Focus();
				if (this.FocusedElement != null)
				{
					this.FocusedElement.Focus();
					return;
				}
				if ((this.PART_Content == null || !this.PART_Content.MoveFocus(new TraversalRequest(FocusNavigationDirection.First))) && this.PART_Header != null)
				{
					this.PART_Header.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
				}
			}
		}

		private void UpdateFlyoutTheme()
		{
			FlyoutsControl flyoutsControl = this.TryFindParent<FlyoutsControl>();
			if (DesignerProperties.GetIsInDesignMode(this))
			{
				base.Visibility = (flyoutsControl != null ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible);
			}
			MetroWindow parentWindow = this.ParentWindow;
			if (parentWindow != null)
			{
				Tuple<AppTheme, Accent> tuple = Flyout.DetectTheme(this);
				if (tuple != null && tuple.Item2 != null)
				{
					this.ChangeFlyoutTheme(tuple.Item2, tuple.Item1);
				}
				if (flyoutsControl != null && this.IsOpen)
				{
					flyoutsControl.HandleFlyoutStatusChange(this, parentWindow);
				}
			}
		}

		private void UpdateOpacityChange()
		{
			if (this.root == null || this.fadeOutFrame == null || DesignerProperties.GetIsInDesignMode(this))
			{
				return;
			}
			if (!this.AnimateOpacity)
			{
				this.fadeOutFrame.Value = 1;
				this.root.Opacity = 1;
				return;
			}
			this.fadeOutFrame.Value = 0;
			if (!this.IsOpen)
			{
				this.root.Opacity = 0;
			}
		}

		private void WindowTitleThumbChangeWindowStateOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			MetroWindow parentWindow = this.ParentWindow;
			if (parentWindow != null && this.Position != MahApps.Metro.Controls.Position.Bottom)
			{
				MetroWindow.DoWindowTitleThumbChangeWindowStateOnMouseDoubleClick(parentWindow, e);
			}
		}

		private void WindowTitleThumbMoveOnDragDelta(object sender, DragDeltaEventArgs e)
		{
			MetroWindow parentWindow = this.ParentWindow;
			if (parentWindow != null && this.Position != MahApps.Metro.Controls.Position.Bottom)
			{
				MetroWindow.DoWindowTitleThumbMoveOnDragDelta(parentWindow, e);
			}
		}

		private void WindowTitleThumbOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			MetroWindow parentWindow = this.ParentWindow;
			if (parentWindow != null && this.Position != MahApps.Metro.Controls.Position.Bottom)
			{
				MetroWindow.DoWindowTitleThumbOnPreviewMouseLeftButtonUp(parentWindow, e);
			}
		}

		private void WindowTitleThumbSystemMenuOnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			MetroWindow parentWindow = this.ParentWindow;
			if (parentWindow != null && this.Position != MahApps.Metro.Controls.Position.Bottom)
			{
				MetroWindow.DoWindowTitleThumbSystemMenuOnMouseRightButtonUp(parentWindow, e);
			}
		}

		public event RoutedEventHandler ClosingFinished
		{
			add
			{
				base.AddHandler(Flyout.ClosingFinishedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Flyout.ClosingFinishedEvent, value);
			}
		}

		public event RoutedEventHandler IsOpenChanged
		{
			add
			{
				base.AddHandler(Flyout.IsOpenChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Flyout.IsOpenChangedEvent, value);
			}
		}
	}
}