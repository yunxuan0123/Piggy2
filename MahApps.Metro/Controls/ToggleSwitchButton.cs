using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name="PART_BackgroundTranslate", Type=typeof(TranslateTransform))]
	[TemplatePart(Name="PART_DraggingThumb", Type=typeof(Thumb))]
	[TemplatePart(Name="PART_SwitchTrack", Type=typeof(Grid))]
	[TemplatePart(Name="PART_ThumbIndicator", Type=typeof(Shape))]
	[TemplatePart(Name="PART_ThumbTranslate", Type=typeof(TranslateTransform))]
	public class ToggleSwitchButton : ToggleButton
	{
		private const string PART_DraggingThumb = "PART_DraggingThumb";

		private const string PART_SwitchTrack = "PART_SwitchTrack";

		private const string PART_ThumbIndicator = "PART_ThumbIndicator";

		private TranslateTransform _BackgroundTranslate;

		private Thumb _DraggingThumb;

		private Grid _SwitchTrack;

		private Shape _ThumbIndicator;

		private TranslateTransform _ThumbTranslate;

		[Obsolete("This property will be deleted in the next release. You should use OnSwitchBrush and OffSwitchBrush to change the switch's brushes.")]
		public readonly static DependencyProperty SwitchForegroundProperty;

		public readonly static DependencyProperty OnSwitchBrushProperty;

		public readonly static DependencyProperty OffSwitchBrushProperty;

		public readonly static DependencyProperty ThumbIndicatorBrushProperty;

		public readonly static DependencyProperty ThumbIndicatorDisabledBrushProperty;

		public readonly static DependencyProperty ThumbIndicatorWidthProperty;

		private DoubleAnimation _thumbAnimation;

		private double? _lastDragPosition;

		private bool _isDragging;

		public Brush OffSwitchBrush
		{
			get
			{
				return (Brush)base.GetValue(ToggleSwitchButton.OffSwitchBrushProperty);
			}
			set
			{
				base.SetValue(ToggleSwitchButton.OffSwitchBrushProperty, value);
			}
		}

		public Brush OnSwitchBrush
		{
			get
			{
				return (Brush)base.GetValue(ToggleSwitchButton.OnSwitchBrushProperty);
			}
			set
			{
				base.SetValue(ToggleSwitchButton.OnSwitchBrushProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release. You should use OnSwitchBrush and OffSwitchBrush to change the switch's brushes.")]
		public Brush SwitchForeground
		{
			get
			{
				return (Brush)base.GetValue(ToggleSwitchButton.SwitchForegroundProperty);
			}
			set
			{
				base.SetValue(ToggleSwitchButton.SwitchForegroundProperty, value);
			}
		}

		public Brush ThumbIndicatorBrush
		{
			get
			{
				return (Brush)base.GetValue(ToggleSwitchButton.ThumbIndicatorBrushProperty);
			}
			set
			{
				base.SetValue(ToggleSwitchButton.ThumbIndicatorBrushProperty, value);
			}
		}

		public Brush ThumbIndicatorDisabledBrush
		{
			get
			{
				return (Brush)base.GetValue(ToggleSwitchButton.ThumbIndicatorDisabledBrushProperty);
			}
			set
			{
				base.SetValue(ToggleSwitchButton.ThumbIndicatorDisabledBrushProperty, value);
			}
		}

		public double ThumbIndicatorWidth
		{
			get
			{
				return (double)base.GetValue(ToggleSwitchButton.ThumbIndicatorWidthProperty);
			}
			set
			{
				base.SetValue(ToggleSwitchButton.ThumbIndicatorWidthProperty, value);
			}
		}

		static ToggleSwitchButton()
		{
			Class6.yDnXvgqzyB5jw();
			ToggleSwitchButton.SwitchForegroundProperty = DependencyProperty.Register("SwitchForeground", typeof(Brush), typeof(ToggleSwitchButton), new PropertyMetadata(null, (DependencyObject o, DependencyPropertyChangedEventArgs e) => ((ToggleSwitchButton)o).OnSwitchBrush = e.NewValue as Brush));
			ToggleSwitchButton.OnSwitchBrushProperty = DependencyProperty.Register("OnSwitchBrush", typeof(Brush), typeof(ToggleSwitchButton), null);
			ToggleSwitchButton.OffSwitchBrushProperty = DependencyProperty.Register("OffSwitchBrush", typeof(Brush), typeof(ToggleSwitchButton), null);
			ToggleSwitchButton.ThumbIndicatorBrushProperty = DependencyProperty.Register("ThumbIndicatorBrush", typeof(Brush), typeof(ToggleSwitchButton), null);
			ToggleSwitchButton.ThumbIndicatorDisabledBrushProperty = DependencyProperty.Register("ThumbIndicatorDisabledBrush", typeof(Brush), typeof(ToggleSwitchButton), null);
			ToggleSwitchButton.ThumbIndicatorWidthProperty = DependencyProperty.Register("ThumbIndicatorWidth", typeof(double), typeof(ToggleSwitchButton), new PropertyMetadata((object)13));
		}

		public ToggleSwitchButton()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			base.DefaultStyleKey = typeof(ToggleSwitchButton);
			base.Checked += new RoutedEventHandler(this.ToggleSwitchButton_Checked);
			base.Unchecked += new RoutedEventHandler(this.ToggleSwitchButton_Checked);
		}

		private void _DraggingThumb_DragCompleted(object sender, DragCompletedEventArgs e)
		{
			this._lastDragPosition = null;
			if (!this._isDragging)
			{
				this.OnToggle();
				return;
			}
			if (this._ThumbTranslate != null && this._SwitchTrack != null)
			{
				if (!base.IsChecked.GetValueOrDefault() && this._ThumbTranslate.X + 6.5 >= this._SwitchTrack.ActualWidth / 2)
				{
					this.OnToggle();
					return;
				}
				if (base.IsChecked.GetValueOrDefault() && this._ThumbTranslate.X + 6.5 <= this._SwitchTrack.ActualWidth / 2)
				{
					this.OnToggle();
					return;
				}
				this.UpdateThumb();
			}
		}

		private void _DraggingThumb_DragDelta(object sender, DragDeltaEventArgs e)
		{
			if (this._lastDragPosition.HasValue)
			{
				if (Math.Abs(e.HorizontalChange) > 3)
				{
					this._isDragging = true;
				}
				if (this._SwitchTrack != null && this._ThumbIndicator != null)
				{
					double value = this._lastDragPosition.Value;
					TranslateTransform translateTransform = this._ThumbTranslate;
					double actualWidth = base.ActualWidth;
					double left = this._SwitchTrack.Margin.Left;
					Thickness margin = this._SwitchTrack.Margin;
					translateTransform.X = Math.Min(actualWidth - (left + margin.Right + this._ThumbIndicator.ActualWidth), Math.Max(0, value + e.HorizontalChange));
				}
			}
		}

		private void _DraggingThumb_DragStarted(object sender, DragStartedEventArgs e)
		{
			double right;
			if (this._ThumbTranslate != null)
			{
				this._ThumbTranslate.BeginAnimation(TranslateTransform.XProperty, null);
				if (base.IsChecked.GetValueOrDefault())
				{
					double actualWidth = base.ActualWidth;
					double left = this._SwitchTrack.Margin.Left;
					Thickness margin = this._SwitchTrack.Margin;
					right = actualWidth - (left + margin.Right + this._ThumbIndicator.ActualWidth);
				}
				else
				{
					right = 0;
				}
				this._ThumbTranslate.X = right;
				this._thumbAnimation = null;
			}
			this._lastDragPosition = new double?(this._ThumbTranslate.X);
			this._isDragging = false;
		}

		private void _SwitchTrack_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			double right;
			if (this._ThumbTranslate != null && this._SwitchTrack != null && this._ThumbIndicator != null)
			{
				if (base.IsChecked.GetValueOrDefault())
				{
					double actualWidth = base.ActualWidth;
					double left = this._SwitchTrack.Margin.Left;
					Thickness margin = this._SwitchTrack.Margin;
					right = actualWidth - (left + margin.Right + this._ThumbIndicator.ActualWidth);
				}
				else
				{
					right = 0;
				}
				this._ThumbTranslate.X = right;
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._BackgroundTranslate = base.GetTemplateChild("PART_BackgroundTranslate") as TranslateTransform;
			this._DraggingThumb = base.GetTemplateChild("PART_DraggingThumb") as Thumb;
			this._SwitchTrack = base.GetTemplateChild("PART_SwitchTrack") as Grid;
			this._ThumbIndicator = base.GetTemplateChild("PART_ThumbIndicator") as Shape;
			this._ThumbTranslate = base.GetTemplateChild("PART_ThumbTranslate") as TranslateTransform;
			if (this._ThumbIndicator != null && this._BackgroundTranslate != null)
			{
				Binding binding = new Binding("X")
				{
					Source = this._ThumbTranslate
				};
				BindingOperations.SetBinding(this._BackgroundTranslate, TranslateTransform.XProperty, binding);
			}
			if (this._DraggingThumb != null && this._SwitchTrack != null && this._ThumbIndicator != null && this._ThumbTranslate != null)
			{
				this._DraggingThumb.DragStarted += new DragStartedEventHandler(this._DraggingThumb_DragStarted);
				this._DraggingThumb.DragDelta += new DragDeltaEventHandler(this._DraggingThumb_DragDelta);
				this._DraggingThumb.DragCompleted += new DragCompletedEventHandler(this._DraggingThumb_DragCompleted);
				this._SwitchTrack.SizeChanged += new SizeChangedEventHandler(this._SwitchTrack_SizeChanged);
			}
		}

		protected override void OnToggle()
		{
			bool? isChecked = base.IsChecked;
			base.IsChecked = new bool?((isChecked.GetValueOrDefault() ? !isChecked.HasValue : true));
			this.UpdateThumb();
		}

		private void ToggleSwitchButton_Checked(object sender, RoutedEventArgs e)
		{
			this.UpdateThumb();
		}

		private void UpdateThumb()
		{
			double right;
			if (this._ThumbTranslate != null && this._SwitchTrack != null && this._ThumbIndicator != null)
			{
				if (base.IsChecked.GetValueOrDefault())
				{
					double actualWidth = base.ActualWidth;
					double left = this._SwitchTrack.Margin.Left;
					Thickness margin = this._SwitchTrack.Margin;
					right = actualWidth - (left + margin.Right + this._ThumbIndicator.ActualWidth);
				}
				else
				{
					right = 0;
				}
				double num = right;
				this._thumbAnimation = new DoubleAnimation()
				{
					To = new double?(num),
					Duration = TimeSpan.FromMilliseconds(500),
					EasingFunction = new ExponentialEase()
					{
						Exponent = 9
					},
					FillBehavior = FillBehavior.Stop
				};
				AnimationTimeline animationTimeline = this._thumbAnimation;
				this._thumbAnimation.Completed += new EventHandler((object sender, EventArgs e) => {
					if (this._thumbAnimation != null && animationTimeline == this._thumbAnimation)
					{
						this._ThumbTranslate.X = num;
						this._thumbAnimation = null;
					}
				});
				this._ThumbTranslate.BeginAnimation(TranslateTransform.XProperty, this._thumbAnimation);
			}
		}
	}
}