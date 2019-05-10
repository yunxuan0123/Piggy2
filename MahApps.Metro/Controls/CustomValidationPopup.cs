using MahApps.Metro.Native;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;

namespace MahApps.Metro.Controls
{
	public class CustomValidationPopup : Popup
	{
		public readonly static DependencyProperty CloseOnMouseLeftButtonDownProperty;

		private Window hostWindow;

		private bool? appliedTopMost;

		private readonly static IntPtr HWND_TOPMOST;

		private readonly static IntPtr HWND_NOTOPMOST;

		private readonly static IntPtr HWND_TOP;

		private readonly static IntPtr HWND_BOTTOM;

		public bool CloseOnMouseLeftButtonDown
		{
			get
			{
				return (bool)base.GetValue(CustomValidationPopup.CloseOnMouseLeftButtonDownProperty);
			}
			set
			{
				base.SetValue(CustomValidationPopup.CloseOnMouseLeftButtonDownProperty, value);
			}
		}

		static CustomValidationPopup()
		{
			Class6.yDnXvgqzyB5jw();
			CustomValidationPopup.CloseOnMouseLeftButtonDownProperty = DependencyProperty.Register("CloseOnMouseLeftButtonDown", typeof(bool), typeof(CustomValidationPopup), new PropertyMetadata(true));
			CustomValidationPopup.HWND_TOPMOST = new IntPtr(-1);
			CustomValidationPopup.HWND_NOTOPMOST = new IntPtr(-2);
			CustomValidationPopup.HWND_TOP = new IntPtr(0);
			CustomValidationPopup.HWND_BOTTOM = new IntPtr(1);
		}

		public CustomValidationPopup()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			base.Loaded += new RoutedEventHandler(this.CustomValidationPopup_Loaded);
			base.Opened += new EventHandler(this.CustomValidationPopup_Opened);
		}

		private void CustomValidationPopup_Loaded(object sender, RoutedEventArgs e)
		{
			FrameworkElement placementTarget = base.PlacementTarget as FrameworkElement;
			if (placementTarget == null)
			{
				return;
			}
			this.hostWindow = Window.GetWindow(placementTarget);
			if (this.hostWindow == null)
			{
				return;
			}
			this.hostWindow.LocationChanged -= new EventHandler(this.hostWindow_SizeOrLocationChanged);
			this.hostWindow.LocationChanged += new EventHandler(this.hostWindow_SizeOrLocationChanged);
			this.hostWindow.SizeChanged -= new SizeChangedEventHandler(this.hostWindow_SizeOrLocationChanged);
			this.hostWindow.SizeChanged += new SizeChangedEventHandler(this.hostWindow_SizeOrLocationChanged);
			placementTarget.SizeChanged -= new SizeChangedEventHandler(this.hostWindow_SizeOrLocationChanged);
			placementTarget.SizeChanged += new SizeChangedEventHandler(this.hostWindow_SizeOrLocationChanged);
			this.hostWindow.StateChanged -= new EventHandler(this.hostWindow_StateChanged);
			this.hostWindow.StateChanged += new EventHandler(this.hostWindow_StateChanged);
			this.hostWindow.Activated -= new EventHandler(this.hostWindow_Activated);
			this.hostWindow.Activated += new EventHandler(this.hostWindow_Activated);
			this.hostWindow.Deactivated -= new EventHandler(this.hostWindow_Deactivated);
			this.hostWindow.Deactivated += new EventHandler(this.hostWindow_Deactivated);
			base.Unloaded -= new RoutedEventHandler(this.CustomValidationPopup_Unloaded);
			base.Unloaded += new RoutedEventHandler(this.CustomValidationPopup_Unloaded);
		}

		private void CustomValidationPopup_Opened(object sender, EventArgs e)
		{
			this.SetTopmostState(true);
		}

		private void CustomValidationPopup_Unloaded(object sender, RoutedEventArgs e)
		{
			FrameworkElement placementTarget = base.PlacementTarget as FrameworkElement;
			if (placementTarget != null)
			{
				placementTarget.SizeChanged -= new SizeChangedEventHandler(this.hostWindow_SizeOrLocationChanged);
			}
			if (this.hostWindow != null)
			{
				this.hostWindow.LocationChanged -= new EventHandler(this.hostWindow_SizeOrLocationChanged);
				this.hostWindow.SizeChanged -= new SizeChangedEventHandler(this.hostWindow_SizeOrLocationChanged);
				this.hostWindow.StateChanged -= new EventHandler(this.hostWindow_StateChanged);
				this.hostWindow.Activated -= new EventHandler(this.hostWindow_Activated);
				this.hostWindow.Deactivated -= new EventHandler(this.hostWindow_Deactivated);
			}
			base.Unloaded -= new RoutedEventHandler(this.CustomValidationPopup_Unloaded);
			base.Opened -= new EventHandler(this.CustomValidationPopup_Opened);
			this.hostWindow = null;
		}

		private void hostWindow_Activated(object sender, EventArgs e)
		{
			this.SetTopmostState(true);
		}

		private void hostWindow_Deactivated(object sender, EventArgs e)
		{
			this.SetTopmostState(false);
		}

		private void hostWindow_SizeOrLocationChanged(object sender, EventArgs e)
		{
			double horizontalOffset = base.HorizontalOffset;
			base.HorizontalOffset = horizontalOffset + 1;
			base.HorizontalOffset = horizontalOffset;
		}

		private void hostWindow_StateChanged(object sender, EventArgs e)
		{
			AdornedElementPlaceholder dataContext;
			if (this.hostWindow != null && this.hostWindow.WindowState != WindowState.Minimized)
			{
				FrameworkElement placementTarget = base.PlacementTarget as FrameworkElement;
				if (placementTarget != null)
				{
					dataContext = placementTarget.DataContext as AdornedElementPlaceholder;
				}
				else
				{
					dataContext = null;
				}
				AdornedElementPlaceholder adornedElementPlaceholder = dataContext;
				if (adornedElementPlaceholder != null && adornedElementPlaceholder.AdornedElement != null)
				{
					base.PopupAnimation = System.Windows.Controls.Primitives.PopupAnimation.None;
					base.IsOpen = false;
					object value = adornedElementPlaceholder.AdornedElement.GetValue(Validation.ErrorTemplateProperty);
					adornedElementPlaceholder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, null);
					adornedElementPlaceholder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, value);
				}
			}
		}

		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (this.CloseOnMouseLeftButtonDown)
			{
				base.IsOpen = false;
			}
		}

		private void SetTopmostState(bool isTop)
		{
			RECT rECT;
			if (this.appliedTopMost.HasValue)
			{
				bool? nullable = this.appliedTopMost;
				if ((nullable.GetValueOrDefault() == isTop ? nullable.HasValue : false))
				{
					return;
				}
			}
			if (base.Child == null)
			{
				return;
			}
			HwndSource hwndSource = PresentationSource.FromVisual(base.Child) as HwndSource;
			if (hwndSource == null)
			{
				return;
			}
			IntPtr handle = hwndSource.Handle;
			if (!UnsafeNativeMethods.GetWindowRect(handle, out rECT))
			{
				return;
			}
			int num = rECT.left;
			int num1 = rECT.top;
			int width = rECT.Width;
			int height = rECT.Height;
			if (!isTop)
			{
				UnsafeNativeMethods.SetWindowPos(handle, CustomValidationPopup.HWND_BOTTOM, num, num1, width, height, 1563);
				UnsafeNativeMethods.SetWindowPos(handle, CustomValidationPopup.HWND_TOP, num, num1, width, height, 1563);
				UnsafeNativeMethods.SetWindowPos(handle, CustomValidationPopup.HWND_NOTOPMOST, num, num1, width, height, 1563);
			}
			else
			{
				UnsafeNativeMethods.SetWindowPos(handle, CustomValidationPopup.HWND_TOPMOST, num, num1, width, height, 1563);
			}
			this.appliedTopMost = new bool?(isTop);
		}
	}
}