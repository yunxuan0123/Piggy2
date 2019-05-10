using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class ContentControlEx : ContentControl
	{
		public readonly static DependencyProperty ContentCharacterCasingProperty;

		public readonly static DependencyProperty RecognizesAccessKeyProperty;

		public CharacterCasing ContentCharacterCasing
		{
			get
			{
				return (CharacterCasing)base.GetValue(ContentControlEx.ContentCharacterCasingProperty);
			}
			set
			{
				base.SetValue(ContentControlEx.ContentCharacterCasingProperty, value);
			}
		}

		public bool RecognizesAccessKey
		{
			get
			{
				return (bool)base.GetValue(ContentControlEx.RecognizesAccessKeyProperty);
			}
			set
			{
				base.SetValue(ContentControlEx.RecognizesAccessKeyProperty, value);
			}
		}

		static ContentControlEx()
		{
			Class6.yDnXvgqzyB5jw();
			ContentControlEx.ContentCharacterCasingProperty = DependencyProperty.Register("ContentCharacterCasing", typeof(CharacterCasing), typeof(ContentControlEx), new FrameworkPropertyMetadata((object)CharacterCasing.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits), (object value) => {
				if (CharacterCasing.Normal > (CharacterCasing)value)
				{
					return false;
				}
				return (CharacterCasing)value <= CharacterCasing.Upper;
			});
			ContentControlEx.RecognizesAccessKeyProperty = DependencyProperty.Register("RecognizesAccessKey", typeof(bool), typeof(ContentControlEx), new FrameworkPropertyMetadata(false));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentControlEx), new FrameworkPropertyMetadata(typeof(ContentControlEx)));
		}

		public ContentControlEx()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}
	}
}