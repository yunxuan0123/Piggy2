using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace System.Windows.Interactivity
{
	[DefaultTrigger(typeof(ButtonBase), typeof(System.Windows.Interactivity.EventTrigger), "Click")]
	[DefaultTrigger(typeof(UIElement), typeof(System.Windows.Interactivity.EventTrigger), "MouseLeftButtonDown")]
	public abstract class TriggerAction : Animatable, IAttachedObject
	{
		private bool isHosted;

		private DependencyObject associatedObject;

		private Type associatedObjectTypeConstraint;

		public readonly static DependencyProperty IsEnabledProperty;

		protected DependencyObject AssociatedObject
		{
			get
			{
				base.ReadPreamble();
				return this.associatedObject;
			}
		}

		protected virtual Type AssociatedObjectTypeConstraint
		{
			get
			{
				base.ReadPreamble();
				return this.associatedObjectTypeConstraint;
			}
		}

		public bool IsEnabled
		{
			get
			{
				return (bool)base.GetValue(System.Windows.Interactivity.TriggerAction.IsEnabledProperty);
			}
			set
			{
				base.SetValue(System.Windows.Interactivity.TriggerAction.IsEnabledProperty, value);
			}
		}

		internal bool IsHosted
		{
			get
			{
				base.ReadPreamble();
				return this.isHosted;
			}
			set
			{
				base.WritePreamble();
				this.isHosted = value;
				base.WritePostscript();
			}
		}

		DependencyObject System.Windows.Interactivity.IAttachedObject.AssociatedObject
		{
			get
			{
				return this.AssociatedObject;
			}
		}

		static TriggerAction()
		{
			Class6.yDnXvgqzyB5jw();
			System.Windows.Interactivity.TriggerAction.IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(System.Windows.Interactivity.TriggerAction), new FrameworkPropertyMetadata(true));
		}

		internal TriggerAction(Type associatedObjectTypeConstraint)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
		}

		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != this.AssociatedObject)
			{
				if (this.AssociatedObject != null)
				{
					throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerActionMultipleTimesExceptionMessage);
				}
				if (dependencyObject != null && !this.AssociatedObjectTypeConstraint.IsAssignableFrom(dependencyObject.GetType()))
				{
					CultureInfo currentCulture = CultureInfo.CurrentCulture;
					string typeConstraintViolatedExceptionMessage = ExceptionStringTable.TypeConstraintViolatedExceptionMessage;
					object[] name = new object[] { base.GetType().Name, dependencyObject.GetType().Name, this.AssociatedObjectTypeConstraint.Name };
					throw new InvalidOperationException(string.Format(currentCulture, typeConstraintViolatedExceptionMessage, name));
				}
				base.WritePreamble();
				this.associatedObject = dependencyObject;
				base.WritePostscript();
				this.OnAttached();
			}
		}

		internal void CallInvoke(object parameter)
		{
			if (this.IsEnabled)
			{
				this.Invoke(parameter);
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
		}

		protected abstract void Invoke(object parameter);

		protected virtual void OnAttached()
		{
		}

		protected virtual void OnDetaching()
		{
		}
	}
}