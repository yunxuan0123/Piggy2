using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;

namespace System.Windows.Interactivity
{
	public abstract class TargetedTriggerAction : System.Windows.Interactivity.TriggerAction
	{
		private Type targetTypeConstraint;

		private bool isTargetChangedRegistered;

		private NameResolver targetResolver;

		public readonly static DependencyProperty TargetObjectProperty;

		public readonly static DependencyProperty TargetNameProperty;

		protected sealed override Type AssociatedObjectTypeConstraint
		{
			get
			{
				AttributeCollection attributes = TypeDescriptor.GetAttributes(base.GetType());
				TypeConstraintAttribute item = attributes[typeof(TypeConstraintAttribute)] as TypeConstraintAttribute;
				if (item != null)
				{
					return item.Constraint;
				}
				return typeof(DependencyObject);
			}
		}

		private bool IsTargetChangedRegistered
		{
			get
			{
				return this.isTargetChangedRegistered;
			}
			set
			{
				this.isTargetChangedRegistered = value;
			}
		}

		private bool IsTargetNameSet
		{
			get
			{
				if (!string.IsNullOrEmpty(this.TargetName))
				{
					return true;
				}
				return base.ReadLocalValue(TargetedTriggerAction.TargetNameProperty) != DependencyProperty.UnsetValue;
			}
		}

		protected object Target
		{
			get
			{
				object associatedObject = base.AssociatedObject;
				if (this.TargetObject != null)
				{
					associatedObject = this.TargetObject;
				}
				else if (this.IsTargetNameSet)
				{
					associatedObject = this.TargetResolver.Object;
				}
				if (associatedObject != null && !this.TargetTypeConstraint.IsAssignableFrom(associatedObject.GetType()))
				{
					CultureInfo currentCulture = CultureInfo.CurrentCulture;
					string retargetedTypeConstraintViolatedExceptionMessage = ExceptionStringTable.RetargetedTypeConstraintViolatedExceptionMessage;
					object[] name = new object[] { base.GetType().Name, associatedObject.GetType(), this.TargetTypeConstraint, "Target" };
					throw new InvalidOperationException(string.Format(currentCulture, retargetedTypeConstraintViolatedExceptionMessage, name));
				}
				return associatedObject;
			}
		}

		public string TargetName
		{
			get
			{
				return (string)base.GetValue(TargetedTriggerAction.TargetNameProperty);
			}
			set
			{
				base.SetValue(TargetedTriggerAction.TargetNameProperty, value);
			}
		}

		public object TargetObject
		{
			get
			{
				return base.GetValue(TargetedTriggerAction.TargetObjectProperty);
			}
			set
			{
				base.SetValue(TargetedTriggerAction.TargetObjectProperty, value);
			}
		}

		private NameResolver TargetResolver
		{
			get
			{
				return this.targetResolver;
			}
		}

		protected Type TargetTypeConstraint
		{
			get
			{
				base.ReadPreamble();
				return this.targetTypeConstraint;
			}
		}

		static TargetedTriggerAction()
		{
			Class6.yDnXvgqzyB5jw();
			TargetedTriggerAction.TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(object), typeof(TargetedTriggerAction), new FrameworkPropertyMetadata(new PropertyChangedCallback(TargetedTriggerAction.OnTargetObjectChanged)));
			TargetedTriggerAction.TargetNameProperty = DependencyProperty.Register("TargetName", typeof(string), typeof(TargetedTriggerAction), new FrameworkPropertyMetadata(new PropertyChangedCallback(TargetedTriggerAction.OnTargetNameChanged)));
		}

		internal TargetedTriggerAction(Type targetTypeConstraint)
		{
			Class6.yDnXvgqzyB5jw();
			base(typeof(DependencyObject));
			this.targetTypeConstraint = targetTypeConstraint;
			this.targetResolver = new NameResolver();
			this.RegisterTargetChanged();
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			DependencyObject associatedObject = base.AssociatedObject;
			Behavior behavior = associatedObject as Behavior;
			this.RegisterTargetChanged();
			if (behavior != null)
			{
				associatedObject = ((IAttachedObject)behavior).AssociatedObject;
				behavior.AssociatedObjectChanged += new EventHandler(this.OnBehaviorHostChanged);
			}
			this.TargetResolver.NameScopeReferenceElement = associatedObject as FrameworkElement;
		}

		private void OnBehaviorHostChanged(object sender, EventArgs e)
		{
			this.TargetResolver.NameScopeReferenceElement = ((IAttachedObject)sender).AssociatedObject as FrameworkElement;
		}

		protected override void OnDetaching()
		{
			Behavior associatedObject = base.AssociatedObject as Behavior;
			base.OnDetaching();
			this.OnTargetChangedImpl(this.TargetResolver.Object, null);
			this.UnregisterTargetChanged();
			if (associatedObject != null)
			{
				associatedObject.AssociatedObjectChanged -= new EventHandler(this.OnBehaviorHostChanged);
			}
			this.TargetResolver.NameScopeReferenceElement = null;
		}

		private void OnTargetChanged(object sender, NameResolvedEventArgs e)
		{
			if (base.AssociatedObject != null)
			{
				this.OnTargetChangedImpl(e.OldObject, e.NewObject);
			}
		}

		internal virtual void OnTargetChangedImpl(object oldTarget, object newTarget)
		{
		}

		private static void OnTargetNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			((TargetedTriggerAction)obj).TargetResolver.Name = (string)args.NewValue;
		}

		private static void OnTargetObjectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			TargetedTriggerAction targetedTriggerAction = (TargetedTriggerAction)obj;
			targetedTriggerAction.OnTargetChanged(obj, new NameResolvedEventArgs(args.OldValue, args.NewValue));
		}

		private void RegisterTargetChanged()
		{
			if (!this.IsTargetChangedRegistered)
			{
				this.TargetResolver.ResolvedElementChanged += new EventHandler<NameResolvedEventArgs>(this.OnTargetChanged);
				this.IsTargetChangedRegistered = true;
			}
		}

		private void UnregisterTargetChanged()
		{
			if (this.IsTargetChangedRegistered)
			{
				this.TargetResolver.ResolvedElementChanged -= new EventHandler<NameResolvedEventArgs>(this.OnTargetChanged);
				this.IsTargetChangedRegistered = false;
			}
		}
	}
}