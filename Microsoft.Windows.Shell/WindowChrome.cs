using Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.Windows.Shell
{
	public class WindowChrome : Freezable
	{
		public readonly static DependencyProperty WindowChromeProperty;

		public readonly static DependencyProperty IsHitTestVisibleInChromeProperty;

		public readonly static DependencyProperty ResizeGripDirectionProperty;

		public readonly static DependencyProperty CaptionHeightProperty;

		public readonly static DependencyProperty ResizeBorderThicknessProperty;

		public readonly static DependencyProperty GlassFrameThicknessProperty;

		public readonly static DependencyProperty UseAeroCaptionButtonsProperty;

		public readonly static DependencyProperty IgnoreTaskbarOnMaximizeProperty;

		public readonly static DependencyProperty UseNoneWindowStyleProperty;

		public readonly static DependencyProperty CornerRadiusProperty;

		public readonly static DependencyProperty SacrificialEdgeProperty;

		private readonly static Microsoft.Windows.Shell.SacrificialEdge SacrificialEdge_All;

		private readonly static List<WindowChrome._SystemParameterBoundProperty> _BoundProperties;

		public double CaptionHeight
		{
			get
			{
				return (double)base.GetValue(WindowChrome.CaptionHeightProperty);
			}
			set
			{
				base.SetValue(WindowChrome.CaptionHeightProperty, value);
			}
		}

		public System.Windows.CornerRadius CornerRadius
		{
			get
			{
				return (System.Windows.CornerRadius)base.GetValue(WindowChrome.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(WindowChrome.CornerRadiusProperty, value);
			}
		}

		public static Thickness GlassFrameCompleteThickness
		{
			get
			{
				return new Thickness(-1);
			}
		}

		public Thickness GlassFrameThickness
		{
			get
			{
				return (Thickness)base.GetValue(WindowChrome.GlassFrameThicknessProperty);
			}
			set
			{
				base.SetValue(WindowChrome.GlassFrameThicknessProperty, value);
			}
		}

		public bool IgnoreTaskbarOnMaximize
		{
			get
			{
				return (bool)base.GetValue(WindowChrome.IgnoreTaskbarOnMaximizeProperty);
			}
			set
			{
				base.SetValue(WindowChrome.IgnoreTaskbarOnMaximizeProperty, value);
			}
		}

		public Thickness ResizeBorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(WindowChrome.ResizeBorderThicknessProperty);
			}
			set
			{
				base.SetValue(WindowChrome.ResizeBorderThicknessProperty, value);
			}
		}

		public Microsoft.Windows.Shell.SacrificialEdge SacrificialEdge
		{
			get
			{
				return (Microsoft.Windows.Shell.SacrificialEdge)base.GetValue(WindowChrome.SacrificialEdgeProperty);
			}
			set
			{
				base.SetValue(WindowChrome.SacrificialEdgeProperty, value);
			}
		}

		public bool UseAeroCaptionButtons
		{
			get
			{
				return (bool)base.GetValue(WindowChrome.UseAeroCaptionButtonsProperty);
			}
			set
			{
				base.SetValue(WindowChrome.UseAeroCaptionButtonsProperty, value);
			}
		}

		public bool UseNoneWindowStyle
		{
			get
			{
				return (bool)base.GetValue(WindowChrome.UseNoneWindowStyleProperty);
			}
			set
			{
				base.SetValue(WindowChrome.UseNoneWindowStyleProperty, value);
			}
		}

		static WindowChrome()
		{
			Class6.yDnXvgqzyB5jw();
			WindowChrome.WindowChromeProperty = DependencyProperty.RegisterAttached("WindowChrome", typeof(WindowChrome), typeof(WindowChrome), new PropertyMetadata(null, new PropertyChangedCallback(WindowChrome._OnChromeChanged)));
			WindowChrome.IsHitTestVisibleInChromeProperty = DependencyProperty.RegisterAttached("IsHitTestVisibleInChrome", typeof(bool), typeof(WindowChrome), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));
			WindowChrome.ResizeGripDirectionProperty = DependencyProperty.RegisterAttached("ResizeGripDirection", typeof(ResizeGripDirection), typeof(WindowChrome), new FrameworkPropertyMetadata((object)ResizeGripDirection.None, FrameworkPropertyMetadataOptions.Inherits));
			WindowChrome.CaptionHeightProperty = DependencyProperty.Register("CaptionHeight", typeof(double), typeof(WindowChrome), new PropertyMetadata((object)0, (DependencyObject d, DependencyPropertyChangedEventArgs e) => ((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint()), (object value) => (double)value >= 0);
			Type type = typeof(Thickness);
			Type type1 = typeof(WindowChrome);
			Thickness thickness = new Thickness();
			WindowChrome.ResizeBorderThicknessProperty = DependencyProperty.Register("ResizeBorderThickness", type, type1, new PropertyMetadata((object)thickness), (object value) => ((Thickness)value).IsNonNegative());
			Type type2 = typeof(Thickness);
			Type type3 = typeof(WindowChrome);
			thickness = new Thickness();
			WindowChrome.GlassFrameThicknessProperty = DependencyProperty.Register("GlassFrameThickness", type2, type3, new PropertyMetadata((object)thickness, (DependencyObject d, DependencyPropertyChangedEventArgs e) => ((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint(), (DependencyObject d, object o) => WindowChrome._CoerceGlassFrameThickness((Thickness)o)));
			WindowChrome.UseAeroCaptionButtonsProperty = DependencyProperty.Register("UseAeroCaptionButtons", typeof(bool), typeof(WindowChrome), new FrameworkPropertyMetadata(true));
			WindowChrome.IgnoreTaskbarOnMaximizeProperty = DependencyProperty.Register("IgnoreTaskbarOnMaximize", typeof(bool), typeof(WindowChrome), new FrameworkPropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => ((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint()));
			WindowChrome.UseNoneWindowStyleProperty = DependencyProperty.Register("UseNoneWindowStyle", typeof(bool), typeof(WindowChrome), new FrameworkPropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => ((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint()));
			Type type4 = typeof(System.Windows.CornerRadius);
			Type type5 = typeof(WindowChrome);
			System.Windows.CornerRadius cornerRadiu = new System.Windows.CornerRadius();
			WindowChrome.CornerRadiusProperty = DependencyProperty.Register("CornerRadius", type4, type5, new PropertyMetadata((object)cornerRadiu, (DependencyObject d, DependencyPropertyChangedEventArgs e) => ((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint()), (object value) => ((System.Windows.CornerRadius)value).IsValid());
			WindowChrome.SacrificialEdgeProperty = DependencyProperty.Register("SacrificialEdge", typeof(Microsoft.Windows.Shell.SacrificialEdge), typeof(WindowChrome), new PropertyMetadata((object)Microsoft.Windows.Shell.SacrificialEdge.None, (DependencyObject d, DependencyPropertyChangedEventArgs e) => ((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint()), new ValidateValueCallback(WindowChrome._IsValidSacrificialEdge));
			WindowChrome.SacrificialEdge_All = Microsoft.Windows.Shell.SacrificialEdge.Left | Microsoft.Windows.Shell.SacrificialEdge.Top | Microsoft.Windows.Shell.SacrificialEdge.Right | Microsoft.Windows.Shell.SacrificialEdge.Bottom | Microsoft.Windows.Shell.SacrificialEdge.Office;
			List<WindowChrome._SystemParameterBoundProperty> _SystemParameterBoundProperties = new List<WindowChrome._SystemParameterBoundProperty>();
			WindowChrome._SystemParameterBoundProperty __SystemParameterBoundProperty = new WindowChrome._SystemParameterBoundProperty()
			{
				DependencyProperty = WindowChrome.CornerRadiusProperty,
				SystemParameterPropertyName = "WindowCornerRadius"
			};
			_SystemParameterBoundProperties.Add(__SystemParameterBoundProperty);
			__SystemParameterBoundProperty = new WindowChrome._SystemParameterBoundProperty()
			{
				DependencyProperty = WindowChrome.CaptionHeightProperty,
				SystemParameterPropertyName = "WindowCaptionHeight"
			};
			_SystemParameterBoundProperties.Add(__SystemParameterBoundProperty);
			__SystemParameterBoundProperty = new WindowChrome._SystemParameterBoundProperty()
			{
				DependencyProperty = WindowChrome.ResizeBorderThicknessProperty,
				SystemParameterPropertyName = "WindowResizeBorderThickness"
			};
			_SystemParameterBoundProperties.Add(__SystemParameterBoundProperty);
			__SystemParameterBoundProperty = new WindowChrome._SystemParameterBoundProperty()
			{
				DependencyProperty = WindowChrome.GlassFrameThicknessProperty,
				SystemParameterPropertyName = "WindowNonClientFrameThickness"
			};
			_SystemParameterBoundProperties.Add(__SystemParameterBoundProperty);
			WindowChrome._BoundProperties = _SystemParameterBoundProperties;
		}

		public WindowChrome()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			foreach (WindowChrome._SystemParameterBoundProperty _BoundProperty in WindowChrome._BoundProperties)
			{
				DependencyProperty dependencyProperty = _BoundProperty.DependencyProperty;
				Binding binding = new Binding()
				{
					Path = new PropertyPath(string.Concat("(SystemParameters.", _BoundProperty.SystemParameterPropertyName, ")"), new object[0]),
					Mode = BindingMode.OneWay,
					UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
				};
				BindingOperations.SetBinding(this, dependencyProperty, binding);
			}
		}

		private static object _CoerceGlassFrameThickness(Thickness thickness)
		{
			if (!thickness.IsNonNegative())
			{
				return WindowChrome.GlassFrameCompleteThickness;
			}
			return thickness;
		}

		private static bool _IsValidSacrificialEdge(object value)
		{
			Microsoft.Windows.Shell.SacrificialEdge sacrificialEdge = Microsoft.Windows.Shell.SacrificialEdge.None;
			try
			{
				sacrificialEdge = (Microsoft.Windows.Shell.SacrificialEdge)value;
			}
			catch (InvalidCastException invalidCastException)
			{
				return false;
			}
			if (sacrificialEdge == Microsoft.Windows.Shell.SacrificialEdge.None)
			{
				return true;
			}
			if ((sacrificialEdge | WindowChrome.SacrificialEdge_All) != WindowChrome.SacrificialEdge_All)
			{
				return false;
			}
			if (sacrificialEdge == WindowChrome.SacrificialEdge_All)
			{
				return false;
			}
			return true;
		}

		private static void _OnChromeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (DesignerProperties.GetIsInDesignMode(d))
			{
				return;
			}
			Window window = (Window)d;
			WindowChrome newValue = (WindowChrome)e.NewValue;
			WindowChromeWorker windowChromeWorker = WindowChromeWorker.GetWindowChromeWorker(window);
			if (windowChromeWorker == null)
			{
				windowChromeWorker = new WindowChromeWorker();
				WindowChromeWorker.SetWindowChromeWorker(window, windowChromeWorker);
			}
			windowChromeWorker.SetWindowChrome(newValue);
		}

		private void _OnPropertyChangedThatRequiresRepaint()
		{
			EventHandler eventHandler = this.PropertyChangedThatRequiresRepaint;
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		protected override Freezable CreateInstanceCore()
		{
			return new WindowChrome();
		}

		[Category("MahApps.Metro")]
		public static bool GetIsHitTestVisibleInChrome(IInputElement inputElement)
		{
			Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
			DependencyObject dependencyObject = inputElement as DependencyObject;
			if (dependencyObject == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			return (bool)dependencyObject.GetValue(WindowChrome.IsHitTestVisibleInChromeProperty);
		}

		[Category("MahApps.Metro")]
		public static ResizeGripDirection GetResizeGripDirection(IInputElement inputElement)
		{
			Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
			DependencyObject dependencyObject = inputElement as DependencyObject;
			if (dependencyObject == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			return (ResizeGripDirection)dependencyObject.GetValue(WindowChrome.ResizeGripDirectionProperty);
		}

		[Category("MahApps.Metro")]
		public static WindowChrome GetWindowChrome(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			return (WindowChrome)window.GetValue(WindowChrome.WindowChromeProperty);
		}

		public static void SetIsHitTestVisibleInChrome(IInputElement inputElement, bool hitTestVisible)
		{
			Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
			DependencyObject dependencyObject = inputElement as DependencyObject;
			if (dependencyObject == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			dependencyObject.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, hitTestVisible);
		}

		public static void SetResizeGripDirection(IInputElement inputElement, ResizeGripDirection direction)
		{
			Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
			DependencyObject dependencyObject = inputElement as DependencyObject;
			if (dependencyObject == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			dependencyObject.SetValue(WindowChrome.ResizeGripDirectionProperty, direction);
		}

		public static void SetWindowChrome(Window window, WindowChrome chrome)
		{
			Verify.IsNotNull<Window>(window, "window");
			window.SetValue(WindowChrome.WindowChromeProperty, chrome);
		}

		internal event EventHandler PropertyChangedThatRequiresRepaint;

		private struct _SystemParameterBoundProperty
		{
			public DependencyProperty DependencyProperty
			{
				get;
				set;
			}

			public string SystemParameterPropertyName
			{
				get;
				set;
			}
		}
	}
}