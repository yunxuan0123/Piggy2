using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MahApps.Metro.Controls
{
	public class MetroProgressBar : ProgressBar
	{
		public readonly static DependencyProperty EllipseDiameterProperty;

		public readonly static DependencyProperty EllipseOffsetProperty;

		private readonly object lockme;

		private Storyboard indeterminateStoryboard;

		public double EllipseDiameter
		{
			get
			{
				return (double)base.GetValue(MetroProgressBar.EllipseDiameterProperty);
			}
			set
			{
				base.SetValue(MetroProgressBar.EllipseDiameterProperty, value);
			}
		}

		public double EllipseOffset
		{
			get
			{
				return (double)base.GetValue(MetroProgressBar.EllipseOffsetProperty);
			}
			set
			{
				base.SetValue(MetroProgressBar.EllipseOffsetProperty, value);
			}
		}

		static MetroProgressBar()
		{
			Class6.yDnXvgqzyB5jw();
			MetroProgressBar.EllipseDiameterProperty = DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(MetroProgressBar), new PropertyMetadata((object)0));
			MetroProgressBar.EllipseOffsetProperty = DependencyProperty.Register("EllipseOffset", typeof(double), typeof(MetroProgressBar), new PropertyMetadata((object)0));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroProgressBar), new FrameworkPropertyMetadata(typeof(MetroProgressBar)));
			ProgressBar.IsIndeterminateProperty.OverrideMetadata(typeof(MetroProgressBar), new FrameworkPropertyMetadata(new PropertyChangedCallback(MetroProgressBar.OnIsIndeterminateChanged)));
		}

		public MetroProgressBar()
		{
			Class6.yDnXvgqzyB5jw();
			this.lockme = new object();
			base();
			base.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.VisibleChangedHandler);
		}

		private double CalcContainerAnimEnd(double width)
		{
			double num = 0.4352 * width;
			if (width <= 180)
			{
				return num - 25.731;
			}
			if (width <= 280)
			{
				return num + 27.84;
			}
			return num + 58.862;
		}

		private double CalcContainerAnimStart(double width)
		{
			if (width <= 180)
			{
				return -34;
			}
			if (width <= 280)
			{
				return -50.5;
			}
			return -63;
		}

		private double CalcEllipseAnimEnd(double width)
		{
			return width * 2 / 3;
		}

		private double CalcEllipseAnimWell(double width)
		{
			return width * 1 / 3;
		}

		private VisualState GetIndeterminate()
		{
			FrameworkElement templateChild = base.GetTemplateChild("ContainingGrid") as FrameworkElement;
			if (templateChild == null)
			{
				base.ApplyTemplate();
				templateChild = base.GetTemplateChild("ContainingGrid") as FrameworkElement;
				if (templateChild == null)
				{
					return null;
				}
			}
			IList visualStateGroups = VisualStateManager.GetVisualStateGroups(templateChild);
			if (visualStateGroups == null)
			{
				return null;
			}
			return visualStateGroups.Cast<VisualStateGroup>().SelectMany<VisualStateGroup, VisualState>((VisualStateGroup group) => group.States.Cast<VisualState>()).FirstOrDefault<VisualState>((VisualState state) => state.Name == "Indeterminate");
		}

		private void LoadedHandler(object sender, RoutedEventArgs e)
		{
			base.Loaded -= new RoutedEventHandler(this.LoadedHandler);
			this.SizeChangedHandler(null, null);
			base.SizeChanged += new SizeChangedEventHandler(this.SizeChangedHandler);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			lock (this.lockme)
			{
				this.indeterminateStoryboard = base.TryFindResource("IndeterminateStoryboard") as Storyboard;
			}
			base.Loaded -= new RoutedEventHandler(this.LoadedHandler);
			base.Loaded += new RoutedEventHandler(this.LoadedHandler);
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			if (this.EllipseDiameter.Equals(0))
			{
				this.SetEllipseDiameter(base.ActualWidth);
			}
			if (this.EllipseOffset.Equals(0))
			{
				this.SetEllipseOffset(base.ActualWidth);
			}
		}

		private static void OnIsIndeterminateChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			MetroProgressBar.ToggleIndeterminate(dependencyObject as MetroProgressBar, (bool)e.OldValue, (bool)e.NewValue);
		}

		private void ResetStoryboard(double width, bool removeOldStoryboard)
		{
			DoubleKeyFrame item;
			DoubleKeyFrame doubleKeyFrame;
			DoubleKeyFrame item1;
			if (!base.IsIndeterminate)
			{
				return;
			}
			lock (this.lockme)
			{
				double num = this.CalcContainerAnimStart(width);
				double num1 = this.CalcContainerAnimEnd(width);
				double num2 = this.CalcEllipseAnimWell(width);
				double num3 = this.CalcEllipseAnimEnd(width);
				try
				{
					VisualState indeterminate = this.GetIndeterminate();
					if (indeterminate != null && this.indeterminateStoryboard != null)
					{
						Storyboard storyboard = this.indeterminateStoryboard.Clone();
						Timeline timeline = storyboard.Children.First<Timeline>((Timeline t) => t.Name == "MainDoubleAnim");
						timeline.SetValue(DoubleAnimation.FromProperty, num);
						timeline.SetValue(DoubleAnimation.ToProperty, num1);
						string[] strArrays = new string[] { "E1", "E2", "E3", "E4", "E5" };
						for (int i = 0; i < (int)strArrays.Length; i++)
						{
							string str = strArrays[i];
							DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrame = (DoubleAnimationUsingKeyFrames)storyboard.Children.First<Timeline>((Timeline t) => t.Name == string.Concat(str, "Anim"));
							if (str != "E1")
							{
								item = doubleAnimationUsingKeyFrame.KeyFrames[2];
								doubleKeyFrame = doubleAnimationUsingKeyFrame.KeyFrames[3];
								item1 = doubleAnimationUsingKeyFrame.KeyFrames[4];
							}
							else
							{
								item = doubleAnimationUsingKeyFrame.KeyFrames[1];
								doubleKeyFrame = doubleAnimationUsingKeyFrame.KeyFrames[2];
								item1 = doubleAnimationUsingKeyFrame.KeyFrames[3];
							}
							item.Value = num2;
							doubleKeyFrame.Value = num2;
							item1.Value = num3;
							item.InvalidateProperty(DoubleKeyFrame.ValueProperty);
							doubleKeyFrame.InvalidateProperty(DoubleKeyFrame.ValueProperty);
							item1.InvalidateProperty(DoubleKeyFrame.ValueProperty);
							doubleAnimationUsingKeyFrame.InvalidateProperty(Storyboard.TargetPropertyProperty);
							doubleAnimationUsingKeyFrame.InvalidateProperty(Storyboard.TargetNameProperty);
						}
						FrameworkElement templateChild = (FrameworkElement)base.GetTemplateChild("ContainingGrid");
						if (removeOldStoryboard && indeterminate.Storyboard != null)
						{
							indeterminate.Storyboard.Stop(templateChild);
							indeterminate.Storyboard.Remove(templateChild);
						}
						indeterminate.Storyboard = storyboard;
						if (indeterminate.Storyboard != null)
						{
							indeterminate.Storyboard.Begin(templateChild, true);
						}
					}
				}
				catch (Exception exception)
				{
				}
			}
		}

		private void SetEllipseDiameter(double width)
		{
			if (width <= 180)
			{
				this.EllipseDiameter = 4;
				return;
			}
			if (width <= 280)
			{
				this.EllipseDiameter = 5;
				return;
			}
			this.EllipseDiameter = 6;
		}

		private void SetEllipseOffset(double width)
		{
			if (width <= 180)
			{
				this.EllipseOffset = 4;
				return;
			}
			if (width <= 280)
			{
				this.EllipseOffset = 7;
				return;
			}
			this.EllipseOffset = 9;
		}

		private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
		{
			double actualWidth = base.ActualWidth;
			MetroProgressBar metroProgressBar = this;
			if (base.Visibility == System.Windows.Visibility.Visible && base.IsIndeterminate)
			{
				metroProgressBar.ResetStoryboard(actualWidth, true);
			}
		}

		private static void ToggleIndeterminate(MetroProgressBar bar, bool oldValue, bool newValue)
		{
			if (bar != null && newValue != oldValue)
			{
				VisualState indeterminate = bar.GetIndeterminate();
				FrameworkElement templateChild = bar.GetTemplateChild("ContainingGrid") as FrameworkElement;
				if (indeterminate != null && templateChild != null)
				{
					if (oldValue && indeterminate.Storyboard != null)
					{
						indeterminate.Storyboard.Stop(templateChild);
						indeterminate.Storyboard.Remove(templateChild);
					}
					if (newValue)
					{
						Action action = () => {
							bar.InvalidateMeasure();
							bar.InvalidateArrange();
							bar.ResetStoryboard(bar.ActualWidth, false);
						};
						bar.Dispatcher.BeginInvoke(DispatcherPriority.Background, action);
					}
				}
			}
		}

		private void VisibleChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (base.IsIndeterminate)
			{
				MetroProgressBar.ToggleIndeterminate(this, (bool)e.OldValue, (bool)e.NewValue);
			}
		}
	}
}