using System;
using System.Windows;

namespace System.Windows.Interactivity
{
	public abstract class TriggerAction<T> : System.Windows.Interactivity.TriggerAction
	where T : DependencyObject
	{
		protected new T AssociatedObject
		{
			get
			{
				return (T)base.AssociatedObject;
			}
		}

		protected sealed override Type AssociatedObjectTypeConstraint
		{
			get
			{
				return base.AssociatedObjectTypeConstraint;
			}
		}

		protected TriggerAction()
		{
			Class6.yDnXvgqzyB5jw();
			base(typeof(T));
		}
	}
}