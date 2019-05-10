using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MahApps.Metro.Controls
{
	public class MultiFrameImage : Image
	{
		private readonly List<BitmapSource> _frames;

		static MultiFrameImage()
		{
			Class6.yDnXvgqzyB5jw();
			Image.SourceProperty.OverrideMetadata(typeof(MultiFrameImage), new FrameworkPropertyMetadata(new PropertyChangedCallback(MultiFrameImage.OnSourceChanged)));
		}

		public MultiFrameImage()
		{
			Class6.yDnXvgqzyB5jw();
			this._frames = new List<BitmapSource>();
			base();
		}

		protected override void OnRender(DrawingContext dc)
		{
			if (this._frames.Count == 0)
			{
				base.OnRender(dc);
				return;
			}
			double width = base.RenderSize.Width;
			Size renderSize = base.RenderSize;
			double num = Math.Max(width, renderSize.Height);
			BitmapSource bitmapSource = this._frames.FirstOrDefault<BitmapSource>((BitmapSource f) => {
				if (f.Width < num)
				{
					return false;
				}
				return f.Height >= num;
			}) ?? this._frames.Last<BitmapSource>();
			double width1 = base.RenderSize.Width;
			renderSize = base.RenderSize;
			dc.DrawImage(bitmapSource, new Rect(0, 0, width1, renderSize.Height));
		}

		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((MultiFrameImage)d).UpdateFrameList();
		}

		private void UpdateFrameList()
		{
			this._frames.Clear();
			BitmapFrame source = base.Source as BitmapFrame;
			if (source == null)
			{
				return;
			}
			BitmapDecoder decoder = source.Decoder;
			if (decoder == null || decoder.Frames.Count == 0)
			{
				return;
			}
			this._frames.AddRange(
				from f in decoder.Frames
				group f by f.PixelWidth * f.PixelHeight into g
				orderby g.Key
				select (
					from f in g
					orderby f.Format.BitsPerPixel descending
					select f).First<BitmapFrame>());
		}
	}
}