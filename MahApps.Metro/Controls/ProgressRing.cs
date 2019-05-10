using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	[TemplateVisualState(Name="Active", GroupName="ActiveStates")]
	[TemplateVisualState(Name="Inactive", GroupName="ActiveStates")]
	[TemplateVisualState(Name="Large", GroupName="SizeStates")]
	[TemplateVisualState(Name="Small", GroupName="SizeStates")]
	public class ProgressRing : Control
	{
		public readonly static DependencyProperty BindableWidthProperty;

		public readonly static DependencyProperty IsActiveProperty;

		public readonly static DependencyProperty IsLargeProperty;

		public readonly static DependencyProperty MaxSideLengthProperty;

		public readonly static DependencyProperty EllipseDiameterProperty;

		public readonly static DependencyProperty EllipseOffsetProperty;

		private List<Action> _deferredActions;

		public double BindableWidth
		{
			get
			{
				return (double)base.GetValue(ProgressRing.BindableWidthProperty);
			}
			private set
			{
				base.SetValue(ProgressRing.BindableWidthProperty, value);
			}
		}

		public double EllipseDiameter
		{
			get
			{
				return (double)base.GetValue(ProgressRing.EllipseDiameterProperty);
			}
			private set
			{
				base.SetValue(ProgressRing.EllipseDiameterProperty, value);
			}
		}

		public Thickness EllipseOffset
		{
			get
			{
				return (Thickness)base.GetValue(ProgressRing.EllipseOffsetProperty);
			}
			private set
			{
				base.SetValue(ProgressRing.EllipseOffsetProperty, value);
			}
		}

		public bool IsActive
		{
			get
			{
				return (bool)base.GetValue(ProgressRing.IsActiveProperty);
			}
			set
			{
				base.SetValue(ProgressRing.IsActiveProperty, value);
			}
		}

		public bool IsLarge
		{
			get
			{
				return (bool)base.GetValue(ProgressRing.IsLargeProperty);
			}
			set
			{
				base.SetValue(ProgressRing.IsLargeProperty, value);
			}
		}

		public double MaxSideLength
		{
			get
			{
				return (double)base.GetValue(ProgressRing.MaxSideLengthProperty);
			}
			private set
			{
				base.SetValue(ProgressRing.MaxSideLengthProperty, value);
			}
		}

		static ProgressRing()
		{
			Class6.yDnXvgqzyB5jw();
			ProgressRing.BindableWidthProperty = DependencyProperty.Register("BindableWidth", typeof(double), typeof(ProgressRing), new PropertyMetadata((object)0, new PropertyChangedCallback(ProgressRing.BindableWidthCallback)));
			ProgressRing.IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ProgressRing), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ProgressRing.IsActiveChanged)));
			ProgressRing.IsLargeProperty = DependencyProperty.Register("IsLarge", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true, new PropertyChangedCallback(ProgressRing.IsLargeChangedCallback)));
			ProgressRing.MaxSideLengthProperty = DependencyProperty.Register("MaxSideLength", typeof(double), typeof(ProgressRing), new PropertyMetadata((object)0));
			ProgressRing.EllipseDiameterProperty = DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(ProgressRing), new PropertyMetadata((object)0));
			Type type = typeof(Thickness);
			Type type1 = typeof(ProgressRing);
			Thickness thickness = new Thickness();
			ProgressRing.EllipseOffsetProperty = DependencyProperty.Register("EllipseOffset", type, type1, new PropertyMetadata((object)thickness));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRing), new FrameworkPropertyMetadata(typeof(ProgressRing)));
			UIElement.VisibilityProperty.OverrideMetadata(typeof(ProgressRing), new FrameworkPropertyMetadata((DependencyObject ringObject, DependencyPropertyChangedEventArgs e) => {
				if (e.NewValue != e.OldValue)
				{
					ProgressRing progressRing = (ProgressRing)ringObject;
					if ((System.Windows.Visibility)e.NewValue != System.Windows.Visibility.Visible)
					{
						progressRing.SetCurrentValue(ProgressRing.IsActiveProperty, (object)false);
						return;
					}
					progressRing.IsActive = true;
				}
			}));
		}

		public ProgressRing()
		{
			Class6.yDnXvgqzyB5jw();
			this._deferredActions = new List<Action>();
			base();
			base.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
		}

		private static void BindableWidthCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			ProgressRing progressRing = dependencyObject as ProgressRing;
			if (progressRing == null)
			{
				return;
			}
			Action action = () => {
				progressRing.SetEllipseDiameter((double)dependencyPropertyChangedEventArgs.NewValue);
				progressRing.SetEllipseOffset((double)dependencyPropertyChangedEventArgs.NewValue);
				progressRing.SetMaxSideLength((double)dependencyPropertyChangedEventArgs.NewValue);
			};
			if (progressRing._deferredActions == null)
			{
				action();
				return;
			}
			progressRing._deferredActions.Add(action);
		}

		private static void IsActiveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			ProgressRing progressRing = dependencyObject as ProgressRing;
			if (progressRing == null)
			{
				return;
			}
			progressRing.UpdateActiveState();
		}

		private static void IsLargeChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			ProgressRing progressRing = dependencyObject as ProgressRing;
			if (progressRing == null)
			{
				return;
			}
			progressRing.UpdateLargeState();
		}

		public override void OnApplyTemplate()
		{
			this.UpdateLargeState();
			this.UpdateActiveState();
			base.OnApplyTemplate();
			if (this._deferredActions != null)
			{
				foreach (Action _deferredAction in this._deferredActions)
				{
					_deferredAction();
				}
			}
			this._deferredActions = null;
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.BindableWidth = base.ActualWidth;
		}

		private void SetEllipseDiameter(double width)
		{
			this.EllipseDiameter = width / 8;
		}

		private void SetEllipseOffset(double width)
		{
			this.EllipseOffset = new Thickness(0, width / 2, 0, 0);
		}

		private void SetMaxSideLength(double width)
		{
			this.MaxSideLength = (width <= 20 ? 20 : width);
		}

		private void UpdateActiveState()
		{
			Action action;
			action = (!this.IsActive ? new Action(() => VisualStateManager.GoToState(this, "Inactive", true)) : new Action(() => VisualStateManager.GoToState(this, "Active", true)));
			if (this._deferredActions == null)
			{
				action();
				return;
			}
			this._deferredActions.Add(action);
		}

		private void UpdateLargeState()
		{
			Action action;
			action = (!this.IsLarge ? new Action(() => VisualStateManager.GoToState(this, "Small", true)) : new Action(() => VisualStateManager.GoToState(this, "Large", true)));
			if (this._deferredActions == null)
			{
				action();
				return;
			}
			this._deferredActions.Add(action);
		}
	}
}