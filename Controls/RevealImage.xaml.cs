using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Controls
{
	public partial class RevealImage : UserControl
	{
		public readonly static DependencyProperty TextProperty;

		public readonly static DependencyProperty ImageProperty;

		public ImageSource Image
		{
			get
			{
				return (ImageSource)base.GetValue(RevealImage.ImageProperty);
			}
			set
			{
				base.SetValue(RevealImage.ImageProperty, value);
			}
		}

		public string Text
		{
			get
			{
				return (string)base.GetValue(RevealImage.TextProperty);
			}
			set
			{
				base.SetValue(RevealImage.TextProperty, value);
			}
		}

		static RevealImage()
		{
			Class6.yDnXvgqzyB5jw();
			RevealImage.TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(RevealImage), new UIPropertyMetadata(""));
			RevealImage.ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(RevealImage), new UIPropertyMetadata(null));
		}

		public RevealImage()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.InitializeComponent();
		}

		private void GridMouseEnter(object sender, MouseEventArgs e)
		{
			RevealImage.TypewriteTextblock(this.Text.ToUpper(), this.textBlock, TimeSpan.FromSeconds(0.25));
		}

		private static void TypewriteTextblock(string textToAnimate, TextBlock txt, TimeSpan timeSpan)
		{
			Storyboard storyboard = new Storyboard()
			{
				FillBehavior = FillBehavior.HoldEnd
			};
			StringAnimationUsingKeyFrames stringAnimationUsingKeyFrame = new StringAnimationUsingKeyFrames()
			{
				Duration = new Duration(timeSpan)
			};
			string empty = string.Empty;
			string str = textToAnimate;
			for (int i = 0; i < str.Length; i++)
			{
				char chr = str[i];
				DiscreteStringKeyFrame discreteStringKeyFrame = new DiscreteStringKeyFrame()
				{
					KeyTime = KeyTime.Paced
				};
				empty = string.Concat(empty, chr.ToString());
				discreteStringKeyFrame.Value = empty;
				stringAnimationUsingKeyFrame.KeyFrames.Add(discreteStringKeyFrame);
			}
			Storyboard.SetTargetName(stringAnimationUsingKeyFrame, txt.Name);
			Storyboard.SetTargetProperty(stringAnimationUsingKeyFrame, new PropertyPath(TextBlock.TextProperty));
			storyboard.Children.Add(stringAnimationUsingKeyFrame);
			storyboard.Begin(txt);
		}
	}
}