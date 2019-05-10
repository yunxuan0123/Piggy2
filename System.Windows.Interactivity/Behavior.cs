using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;

namespace System.Windows.Interactivity
{
	public abstract class Behavior : Animatable, IAttachedObject
	{
		private Type associatedType;

		private DependencyObject associatedObject;

		protected DependencyObject AssociatedObject
		{
			get
			{
				base.ReadPreamble();
				return this.associatedObject;
			}
		}

		protected Type AssociatedType
		{
			get
			{
				base.ReadPreamble();
				return this.associatedType;
			}
		}

		DependencyObject System.Windows.Interactivity.IAttachedObject.AssociatedObject
		{
			get
			{
				return this.AssociatedObject;
			}
		}

		internal Behavior(Type associatedType)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.associatedType = associatedType;
		}

		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != this.AssociatedObject)
			{
				if (this.AssociatedObject != null)
				{
					throw new InvalidOperationException(ExceptionStringTable.CannotHostBehaviorMultipleTimesExceptionMessage);
				}
				if (dependencyObject != null && !this.AssociatedType.IsAssignableFrom(dependencyObject.GetType()))
				{
					CultureInfo currentCulture = CultureInfo.CurrentCulture;
					string typeConstraintViolatedExceptionMessage = ExceptionStringTable.TypeConstraintViolatedExceptionMessage;
					object[] name = new object[] { base.GetType().Name, dependencyObject.GetType().Name, this.AssociatedType.Name };
					throw new InvalidOperationException(string.Format(currentCulture, typeConstraintViolatedExceptionMessage, name));
				}
				base.WritePreamble();
				this.associatedObject = dependencyObject;
				base.WritePostscript();
				this.OnAssociatedObjectChanged();
				this.OnAttached();
			}
		}

		protected override Freezable CreateInstanceCore()
		{
			return (Freezable)Activator.CreateInstance(base.GetType());
		}

		public void Detach()
		{
			this.OnDetaching();
			base.WritePreamble();
			this.associatedObject = null;
			base.WritePostscript();
			this.OnAssociatedObjectChanged();
		}

		private void OnAssociatedObjectChanged()
		{
			if (this.AssociatedObjectChanged != null)
			{
				this.AssociatedObjectChanged(this, new EventArgs());
			}
		}

		protected virtual void OnAttached()
		{
		}

		protected virtual void OnDetaching()
		{
		}

		internal event EventHandler AssociatedObjectChanged;
	}
}