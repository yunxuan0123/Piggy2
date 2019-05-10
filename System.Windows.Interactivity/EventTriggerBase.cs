using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace System.Windows.Interactivity
{
	public abstract class EventTriggerBase : System.Windows.Interactivity.TriggerBase
	{
		private Type sourceTypeConstraint;

		private bool isSourceChangedRegistered;

		private NameResolver sourceNameResolver;

		private MethodInfo eventHandlerMethodInfo;

		public readonly static DependencyProperty SourceObjectProperty;

		public readonly static DependencyProperty SourceNameProperty;

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

		private bool IsLoadedRegistered
		{
			get;
			set;
		}

		private bool IsSourceChangedRegistered
		{
			get
			{
				return this.isSourceChangedRegistered;
			}
			set
			{
				this.isSourceChangedRegistered = value;
			}
		}

		private bool IsSourceNameSet
		{
			get
			{
				if (!string.IsNullOrEmpty(this.SourceName))
				{
					return true;
				}
				return base.ReadLocalValue(EventTriggerBase.SourceNameProperty) != DependencyProperty.UnsetValue;
			}
		}

		public object Source
		{
			get
			{
				object associatedObject = base.AssociatedObject;
				if (this.SourceObject != null)
				{
					associatedObject = this.SourceObject;
				}
				else if (this.IsSourceNameSet)
				{
					associatedObject = this.SourceNameResolver.Object;
					if (associatedObject != null && !this.SourceTypeConstraint.IsAssignableFrom(associatedObject.GetType()))
					{
						CultureInfo currentCulture = CultureInfo.CurrentCulture;
						string retargetedTypeConstraintViolatedExceptionMessage = ExceptionStringTable.RetargetedTypeConstraintViolatedExceptionMessage;
						object[] name = new object[] { base.GetType().Name, associatedObject.GetType(), this.SourceTypeConstraint, "Source" };
						throw new InvalidOperationException(string.Format(currentCulture, retargetedTypeConstraintViolatedExceptionMessage, name));
					}
				}
				return associatedObject;
			}
		}

		public string SourceName
		{
			get
			{
				return (string)base.GetValue(EventTriggerBase.SourceNameProperty);
			}
			set
			{
				base.SetValue(EventTriggerBase.SourceNameProperty, value);
			}
		}

		private NameResolver SourceNameResolver
		{
			get
			{
				return this.sourceNameResolver;
			}
		}

		public object SourceObject
		{
			get
			{
				return base.GetValue(EventTriggerBase.SourceObjectProperty);
			}
			set
			{
				base.SetValue(EventTriggerBase.SourceObjectProperty, value);
			}
		}

		protected Type SourceTypeConstraint
		{
			get
			{
				return this.sourceTypeConstraint;
			}
		}

		static EventTriggerBase()
		{
			Class6.yDnXvgqzyB5jw();
			EventTriggerBase.SourceObjectProperty = DependencyProperty.Register("SourceObject", typeof(object), typeof(EventTriggerBase), new PropertyMetadata(new PropertyChangedCallback(EventTriggerBase.OnSourceObjectChanged)));
			EventTriggerBase.SourceNameProperty = DependencyProperty.Register("SourceName", typeof(string), typeof(EventTriggerBase), new PropertyMetadata(new PropertyChangedCallback(EventTriggerBase.OnSourceNameChanged)));
		}

		internal EventTriggerBase(Type sourceTypeConstraint)
		{
			Class6.yDnXvgqzyB5jw();
			base(typeof(DependencyObject));
			this.sourceTypeConstraint = sourceTypeConstraint;
			this.sourceNameResolver = new NameResolver();
			this.RegisterSourceChanged();
		}

		protected abstract string GetEventName();

		private static bool IsValidEvent(EventInfo eventInfo)
		{
			Type eventHandlerType = eventInfo.EventHandlerType;
			if (!typeof(Delegate).IsAssignableFrom(eventInfo.EventHandlerType))
			{
				return false;
			}
			ParameterInfo[] parameters = eventHandlerType.GetMethod("Invoke").GetParameters();
			if ((int)parameters.Length != 2 || !typeof(object).IsAssignableFrom(parameters[0].ParameterType))
			{
				return false;
			}
			return typeof(EventArgs).IsAssignableFrom(parameters[1].ParameterType);
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			DependencyObject associatedObject = base.AssociatedObject;
			Behavior behavior = associatedObject as Behavior;
			FrameworkElement frameworkElement = associatedObject as FrameworkElement;
			this.RegisterSourceChanged();
			if (behavior != null)
			{
				associatedObject = ((IAttachedObject)behavior).AssociatedObject;
				behavior.AssociatedObjectChanged += new EventHandler(this.OnBehaviorHostChanged);
			}
			else if (this.SourceObject != null || frameworkElement == null)
			{
				try
				{
					this.OnSourceChanged(null, this.Source);
				}
				catch (InvalidOperationException invalidOperationException)
				{
				}
			}
			else
			{
				this.SourceNameResolver.NameScopeReferenceElement = frameworkElement;
			}
			if (string.Compare(this.GetEventName(), "Loaded", StringComparison.Ordinal) == 0 && frameworkElement != null && !Interaction.IsElementLoaded(frameworkElement))
			{
				this.RegisterLoaded(frameworkElement);
			}
		}

		private void OnBehaviorHostChanged(object sender, EventArgs e)
		{
			this.SourceNameResolver.NameScopeReferenceElement = ((IAttachedObject)sender).AssociatedObject as FrameworkElement;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			Behavior associatedObject = base.AssociatedObject as Behavior;
			FrameworkElement frameworkElement = base.AssociatedObject as FrameworkElement;
			try
			{
				this.OnSourceChanged(this.Source, null);
			}
			catch (InvalidOperationException invalidOperationException)
			{
			}
			this.UnregisterSourceChanged();
			if (associatedObject != null)
			{
				associatedObject.AssociatedObjectChanged -= new EventHandler(this.OnBehaviorHostChanged);
			}
			this.SourceNameResolver.NameScopeReferenceElement = null;
			if (string.Compare(this.GetEventName(), "Loaded", StringComparison.Ordinal) == 0 && frameworkElement != null)
			{
				this.UnregisterLoaded(frameworkElement);
			}
		}

		protected virtual void OnEvent(EventArgs eventArgs)
		{
			base.InvokeActions(eventArgs);
		}

		private void OnEventImpl(object sender, EventArgs e)
		{
			this.OnEvent(e);
		}

		internal void OnEventNameChanged(string oldEventName, string newEventName)
		{
			if (base.AssociatedObject != null)
			{
				FrameworkElement source = this.Source as FrameworkElement;
				if (source != null && string.Compare(oldEventName, "Loaded", StringComparison.Ordinal) == 0)
				{
					this.UnregisterLoaded(source);
				}
				else if (!string.IsNullOrEmpty(oldEventName))
				{
					this.UnregisterEvent(this.Source, oldEventName);
				}
				if (source != null && string.Compare(newEventName, "Loaded", StringComparison.Ordinal) == 0)
				{
					this.RegisterLoaded(source);
					return;
				}
				if (!string.IsNullOrEmpty(newEventName))
				{
					this.RegisterEvent(this.Source, newEventName);
				}
			}
		}

		private void OnSourceChanged(object oldSource, object newSource)
		{
			if (base.AssociatedObject != null)
			{
				this.OnSourceChangedImpl(oldSource, newSource);
			}
		}

		internal virtual void OnSourceChangedImpl(object oldSource, object newSource)
		{
			if (string.IsNullOrEmpty(this.GetEventName()))
			{
				return;
			}
			if (string.Compare(this.GetEventName(), "Loaded", StringComparison.Ordinal) != 0)
			{
				if (oldSource != null && this.SourceTypeConstraint.IsAssignableFrom(oldSource.GetType()))
				{
					this.UnregisterEvent(oldSource, this.GetEventName());
				}
				if (newSource != null && this.SourceTypeConstraint.IsAssignableFrom(newSource.GetType()))
				{
					this.RegisterEvent(newSource, this.GetEventName());
				}
			}
		}

		private static void OnSourceNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			((EventTriggerBase)obj).SourceNameResolver.Name = (string)args.NewValue;
		}

		private void OnSourceNameResolverElementChanged(object sender, NameResolvedEventArgs e)
		{
			if (this.SourceObject == null)
			{
				this.OnSourceChanged(e.OldObject, e.NewObject);
			}
		}

		private static void OnSourceObjectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EventTriggerBase eventTriggerBase = (EventTriggerBase)obj;
			object obj1 = eventTriggerBase.SourceNameResolver.Object;
			if (args.NewValue == null)
			{
				eventTriggerBase.OnSourceChanged(args.OldValue, obj1);
				return;
			}
			if (args.OldValue == null && obj1 != null)
			{
				eventTriggerBase.UnregisterEvent(obj1, eventTriggerBase.GetEventName());
			}
			eventTriggerBase.OnSourceChanged(args.OldValue, args.NewValue);
		}

		private void RegisterEvent(object obj, string eventName)
		{
			EventInfo @event = obj.GetType().GetEvent(eventName);
			if (@event == null)
			{
				if (this.SourceObject != null)
				{
					CultureInfo currentCulture = CultureInfo.CurrentCulture;
					string eventTriggerCannotFindEventNameExceptionMessage = ExceptionStringTable.EventTriggerCannotFindEventNameExceptionMessage;
					object[] objArray = new object[] { eventName, obj.GetType().Name };
					throw new ArgumentException(string.Format(currentCulture, eventTriggerCannotFindEventNameExceptionMessage, objArray));
				}
				return;
			}
			if (EventTriggerBase.IsValidEvent(@event))
			{
				this.eventHandlerMethodInfo = typeof(EventTriggerBase).GetMethod("OnEventImpl", BindingFlags.Instance | BindingFlags.NonPublic);
				@event.AddEventHandler(obj, Delegate.CreateDelegate(@event.EventHandlerType, this, this.eventHandlerMethodInfo));
				return;
			}
			if (this.SourceObject != null)
			{
				CultureInfo cultureInfo = CultureInfo.CurrentCulture;
				string eventTriggerBaseInvalidEventExceptionMessage = ExceptionStringTable.EventTriggerBaseInvalidEventExceptionMessage;
				object[] objArray1 = new object[] { eventName, obj.GetType().Name };
				throw new ArgumentException(string.Format(cultureInfo, eventTriggerBaseInvalidEventExceptionMessage, objArray1));
			}
		}

		private void RegisterLoaded(FrameworkElement associatedElement)
		{
			if (!this.IsLoadedRegistered && associatedElement != null)
			{
				associatedElement.Loaded += new RoutedEventHandler(this.OnEventImpl);
				this.IsLoadedRegistered = true;
			}
		}

		private void RegisterSourceChanged()
		{
			if (!this.IsSourceChangedRegistered)
			{
				this.SourceNameResolver.ResolvedElementChanged += new EventHandler<NameResolvedEventArgs>(this.OnSourceNameResolverElementChanged);
				this.IsSourceChangedRegistered = true;
			}
		}

		private void UnregisterEvent(object obj, string eventName)
		{
			if (string.Compare(eventName, "Loaded", StringComparison.Ordinal) != 0)
			{
				this.UnregisterEventImpl(obj, eventName);
			}
			else
			{
				FrameworkElement frameworkElement = obj as FrameworkElement;
				if (frameworkElement != null)
				{
					this.UnregisterLoaded(frameworkElement);
					return;
				}
			}
		}

		private void UnregisterEventImpl(object obj, string eventName)
		{
			Type type = obj.GetType();
			if (this.eventHandlerMethodInfo == null)
			{
				return;
			}
			EventInfo @event = type.GetEvent(eventName);
			@event.RemoveEventHandler(obj, Delegate.CreateDelegate(@event.EventHandlerType, this, this.eventHandlerMethodInfo));
			this.eventHandlerMethodInfo = null;
		}

		private void UnregisterLoaded(FrameworkElement associatedElement)
		{
			if (this.IsLoadedRegistered && associatedElement != null)
			{
				associatedElement.Loaded -= new RoutedEventHandler(this.OnEventImpl);
				this.IsLoadedRegistered = false;
			}
		}

		private void UnregisterSourceChanged()
		{
			if (this.IsSourceChangedRegistered)
			{
				this.SourceNameResolver.ResolvedElementChanged -= new EventHandler<NameResolvedEventArgs>(this.OnSourceNameResolverElementChanged);
				this.IsSourceChangedRegistered = false;
			}
		}
	}
}