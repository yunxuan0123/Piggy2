using System;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class Tile : Button
	{
		public readonly static DependencyProperty TitleProperty;

		public readonly static DependencyProperty CountProperty;

		public readonly static DependencyProperty KeepDraggingProperty;

		public readonly static DependencyProperty TiltFactorProperty;

		public readonly static DependencyProperty TitleFontSizeProperty;

		public readonly static DependencyProperty CountFontSizeProperty;

		public string Count
		{
			get
			{
				return (string)base.GetValue(Tile.CountProperty);
			}
			set
			{
				base.SetValue(Tile.CountProperty, value);
			}
		}

		public int CountFontSize
		{
			get
			{
				return (int)base.GetValue(Tile.CountFontSizeProperty);
			}
			set
			{
				base.SetValue(Tile.CountFontSizeProperty, value);
			}
		}

		public bool KeepDragging
		{
			get
			{
				return (bool)base.GetValue(Tile.KeepDraggingProperty);
			}
			set
			{
				base.SetValue(Tile.KeepDraggingProperty, value);
			}
		}

		public int TiltFactor
		{
			get
			{
				return (int)base.GetValue(Tile.TiltFactorProperty);
			}
			set
			{
				base.SetValue(Tile.TiltFactorProperty, value);
			}
		}

		public string Title
		{
			get
			{
				return (string)base.GetValue(Tile.TitleProperty);
			}
			set
			{
				base.SetValue(Tile.TitleProperty, value);
			}
		}

		public int TitleFontSize
		{
			get
			{
				return (int)base.GetValue(Tile.TitleFontSizeProperty);
			}
			set
			{
				base.SetValue(Tile.TitleFontSizeProperty, value);
			}
		}

		static Tile()
		{
			Class6.yDnXvgqzyB5jw();
			Tile.TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Tile), new PropertyMetadata(null));
			Tile.CountProperty = DependencyProperty.Register("Count", typeof(string), typeof(Tile), new PropertyMetadata(null));
			Tile.KeepDraggingProperty = DependencyProperty.Register("KeepDragging", typeof(bool), typeof(Tile), new PropertyMetadata(true));
			Tile.TiltFactorProperty = DependencyProperty.Register("TiltFactor", typeof(int), typeof(Tile), new PropertyMetadata((object)5));
			Tile.TitleFontSizeProperty = DependencyProperty.Register("TitleFontSize", typeof(int), typeof(Tile), new PropertyMetadata((object)16));
			Tile.CountFontSizeProperty = DependencyProperty.Register("CountFontSize", typeof(int), typeof(Tile), new PropertyMetadata((object)28));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Tile), new FrameworkPropertyMetadata(typeof(Tile)));
		}

		public Tile()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}
	}
}