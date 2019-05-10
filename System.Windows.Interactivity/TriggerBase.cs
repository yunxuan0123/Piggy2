using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace System.Windows.Interactivity
{
	[ContentProperty("Actions")]
	public abstract class TriggerBase : Animatable, IAttachedObject
	{
		private DependencyObject associatedObject;

		private Type associatedObjectTypeConstraint;

		private readonly static DependencyPropertyKey ActionsPropertyKey;

		public readonly static DependencyProperty ActionsProperty;

		public System.Windows.Interactivity.TriggerActionCollection Actions
		{
			get
			{
				return (System.Windows.Interactivity.TriggerActionCollection)base.GetValue(System.Windows.Interactivity.TriggerBase.ActionsProperty);
			}
		}

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

		DependencyObject System.Windows.Interactivity.IAttachedObject.AssociatedObject
		{
			get
			{
				return this.AssociatedObject;
			}
		}

		static TriggerBase()
		{
			Class6.yDnXvgqzyB5jw();
			System.Windows.Interactivity.TriggerBase.ActionsPropertyKey = DependencyProperty.RegisterReadOnly("Actions", typeof(System.Windows.Interactivity.TriggerActionCollection), typeof(System.Windows.Interactivity.TriggerBase), new FrameworkPropertyMetadata());
			System.Windows.Interactivity.TriggerBase.ActionsProperty = System.Windows.Interactivity.TriggerBase.ActionsPropertyKey.DependencyProperty;
		}

		internal TriggerBase(Type associatedObjectTypeConstraint)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
			System.Windows.Interactivity.TriggerActionCollection triggerActionCollection = new System.Windows.Interactivity.TriggerActionCollection();
			base.SetValue(System.Windows.Interactivity.TriggerBase.ActionsPropertyKey, triggerActionCollection);
		}

		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != this.AssociatedObject)
			{
				if (this.AssociatedObject != null)
				{
					throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerMultipleTimesExceptionMessage);
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
				this.Actions.Attach(dependencyObject);
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
			this.Actions.Detach();
		}

		protected void InvokeActions(object parameter)
		{
			if (this.PreviewInvoke != null)
			{
				PreviewInvokeEventArgs previewInvokeEventArg = new PreviewInvokeEventArgs();
				this.PreviewInvoke(this, previewInvokeEventArg);
				if (previewInvokeEventArg.Cancelling)
				{
					return;
				}
			}
			foreach (System.Windows.Interactivity.TriggerAction action in this.Actions)
			{
				action.CallInvoke(parameter);
			}
		}

		protected virtual void OnAttached()
		{
		}

		protected virtual void OnDetaching()
		{
		}

		public event EventHandler<PreviewInvokeEventArgs> PreviewInvoke;
	}
}