using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public class Glow : Control
	{
		public readonly static DependencyProperty GlowBrushProperty;

		public readonly static DependencyProperty NonActiveGlowBrushProperty;

		public readonly static DependencyProperty IsGlowProperty;

		public readonly static DependencyProperty OrientationProperty;

		public readonly static DependencyProperty DirectionProperty;

		public GlowDirection Direction
		{
			get
			{
				return (GlowDirection)base.GetValue(Glow.DirectionProperty);
			}
			set
			{
				base.SetValue(Glow.DirectionProperty, value);
			}
		}

		public Brush GlowBrush
		{
			get
			{
				return (Brush)base.GetValue(Glow.GlowBrushProperty);
			}
			set
			{
				base.SetValue(Glow.GlowBrushProperty, value);
			}
		}

		public bool IsGlow
		{
			get
			{
				return (bool)base.GetValue(Glow.IsGlowProperty);
			}
			set
			{
				base.SetValue(Glow.IsGlowProperty, value);
			}
		}

		public Brush NonActiveGlowBrush
		{
			get
			{
				return (Brush)base.GetValue(Glow.NonActiveGlowBrushProperty);
			}
			set
			{
				base.SetValue(Glow.NonActiveGlowBrushProperty, value);
			}
		}

		public System.Windows.Controls.Orientation Orientation
		{
			get
			{
				return (System.Windows.Controls.Orientation)base.GetValue(Glow.OrientationProperty);
			}
			set
			{
				base.SetValue(Glow.OrientationProperty, value);
			}
		}

		static Glow()
		{
			Class6.yDnXvgqzyB5jw();
			Glow.GlowBrushProperty = DependencyProperty.Register("GlowBrush", typeof(Brush), typeof(Glow), new UIPropertyMetadata(Brushes.Transparent));
			Glow.NonActiveGlowBrushProperty = DependencyProperty.Register("NonActiveGlowBrush", typeof(Brush), typeof(Glow), new UIPropertyMetadata(Brushes.Transparent));
			Glow.IsGlowProperty = DependencyProperty.Register("IsGlow", typeof(bool), typeof(Glow), new UIPropertyMetadata(true));
			Glow.OrientationProperty = DependencyProperty.Register("Orientation", typeof(System.Windows.Controls.Orientation), typeof(Glow), new UIPropertyMetadata((object)System.Windows.Controls.Orientation.Vertical));
			Glow.DirectionProperty = DependencyProperty.Register("Direction", typeof(GlowDirection), typeof(Glow), new UIPropertyMetadata((object)GlowDirection.Top));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Glow), new FrameworkPropertyMetadata(typeof(Glow)));
		}

		public Glow()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}
	}
}