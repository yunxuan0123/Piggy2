using System;
using System.Windows;

namespace System.Windows.Interactivity
{
	public abstract class Behavior<T> : Behavior
	where T : DependencyObject
	{
		protected new T AssociatedObject
		{
			get
			{
				return (T)base.AssociatedObject;
			}
		}

		protected Behavior()
		{
			Class6.yDnXvgqzyB5jw();
			base(typeof(T));
		}
	}
}