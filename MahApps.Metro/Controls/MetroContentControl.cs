using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public class MetroContentControl : ContentControl
	{
		public readonly static DependencyProperty ReverseTransitionProperty;

		public readonly static DependencyProperty TransitionsEnabledProperty;

		public readonly static DependencyProperty OnlyLoadTransitionProperty;

		private bool transitionLoaded;

		public bool OnlyLoadTransition
		{
			get
			{
				return (bool)base.GetValue(MetroContentControl.OnlyLoadTransitionProperty);
			}
			set
			{
				base.SetValue(MetroContentControl.OnlyLoadTransitionProperty, value);
			}
		}

		public bool ReverseTransition
		{
			get
			{
				return (bool)base.GetValue(MetroContentControl.ReverseTransitionProperty);
			}
			set
			{
				base.SetValue(MetroContentControl.ReverseTransitionProperty, value);
			}
		}

		public bool TransitionsEnabled
		{
			get
			{
				return (bool)base.GetValue(MetroContentControl.TransitionsEnabledProperty);
			}
			set
			{
				base.SetValue(MetroContentControl.TransitionsEnabledProperty, value);
			}
		}

		static MetroContentControl()
		{
			Class6.yDnXvgqzyB5jw();
			MetroContentControl.ReverseTransitionProperty = DependencyProperty.Register("ReverseTransition", typeof(bool), typeof(MetroContentControl), new FrameworkPropertyMetadata(false));
			MetroContentControl.TransitionsEnabledProperty = DependencyProperty.Register("TransitionsEnabled", typeof(bool), typeof(MetroContentControl), new FrameworkPropertyMetadata(true));
			MetroContentControl.OnlyLoadTransitionProperty = DependencyProperty.Register("OnlyLoadTransition", typeof(bool), typeof(MetroContentControl), new FrameworkPropertyMetadata(false));
		}

		public MetroContentControl()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			base.DefaultStyleKey = typeof(MetroContentControl);
			base.Loaded += new RoutedEventHandler(this.MetroContentControlLoaded);
			base.Unloaded += new RoutedEventHandler(this.MetroContentControlUnloaded);
		}

		private void MetroContentControlIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (this.TransitionsEnabled && !this.transitionLoaded)
			{
				if (!base.IsVisible)
				{
					VisualStateManager.GoToState(this, (this.ReverseTransition ? "AfterUnLoadedReverse" : "AfterUnLoaded"), false);
					return;
				}
				VisualStateManager.GoToState(this, (this.ReverseTransition ? "AfterLoadedReverse" : "AfterLoaded"), true);
			}
		}

		private void MetroContentControlLoaded(object sender, RoutedEventArgs e)
		{
			if (this.TransitionsEnabled)
			{
				if (!this.transitionLoaded)
				{
					this.transitionLoaded = this.OnlyLoadTransition;
					VisualStateManager.GoToState(this, (this.ReverseTransition ? "AfterLoadedReverse" : "AfterLoaded"), true);
				}
				base.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.MetroContentControlIsVisibleChanged);
				base.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.MetroContentControlIsVisibleChanged);
				return;
			}
			Grid templateChild = (Grid)base.GetTemplateChild("root");
			templateChild.Opacity = 1;
			TranslateTransform renderTransform = (TranslateTransform)templateChild.RenderTransform;
			if (!renderTransform.IsFrozen)
			{
				renderTransform.X = 0;
				return;
			}
			TranslateTransform translateTransform = renderTransform.Clone();
			translateTransform.X = 0;
			templateChild.RenderTransform = translateTransform;
		}

		private void MetroContentControlUnloaded(object sender, RoutedEventArgs e)
		{
			if (this.TransitionsEnabled)
			{
				if (this.transitionLoaded)
				{
					VisualStateManager.GoToState(this, (this.ReverseTransition ? "AfterUnLoadedReverse" : "AfterUnLoaded"), false);
				}
				base.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.MetroContentControlIsVisibleChanged);
			}
		}

		public void Reload()
		{
			if (!this.TransitionsEnabled || this.transitionLoaded)
			{
				return;
			}
			if (this.ReverseTransition)
			{
				VisualStateManager.GoToState(this, "BeforeLoaded", true);
				VisualStateManager.GoToState(this, "AfterUnLoadedReverse", true);
				return;
			}
			VisualStateManager.GoToState(this, "BeforeLoaded", true);
			VisualStateManager.GoToState(this, "AfterLoaded", true);
		}
	}
}