using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace System.Windows.Interactivity
{
	public static class Interaction
	{
		private readonly static DependencyProperty TriggersProperty;

		private readonly static DependencyProperty BehaviorsProperty;

		internal static bool ShouldRunInDesignMode
		{
			get;
			set;
		}

		static Interaction()
		{
			Class6.yDnXvgqzyB5jw();
			Interaction.TriggersProperty = DependencyProperty.RegisterAttached("ShadowTriggers", typeof(System.Windows.Interactivity.TriggerCollection), typeof(Interaction), new FrameworkPropertyMetadata(new PropertyChangedCallback(Interaction.OnTriggersChanged)));
			Interaction.BehaviorsProperty = DependencyProperty.RegisterAttached("ShadowBehaviors", typeof(BehaviorCollection), typeof(Interaction), new FrameworkPropertyMetadata(new PropertyChangedCallback(Interaction.OnBehaviorsChanged)));
		}

		public static BehaviorCollection GetBehaviors(DependencyObject obj)
		{
			BehaviorCollection value = (BehaviorCollection)obj.GetValue(Interaction.BehaviorsProperty);
			if (value == null)
			{
				value = new BehaviorCollection();
				obj.SetValue(Interaction.BehaviorsProperty, value);
			}
			return value;
		}

		public static System.Windows.Interactivity.TriggerCollection GetTriggers(DependencyObject obj)
		{
			System.Windows.Interactivity.TriggerCollection value = (System.Windows.Interactivity.TriggerCollection)obj.GetValue(Interaction.TriggersProperty);
			if (value == null)
			{
				value = new System.Windows.Interactivity.TriggerCollection();
				obj.SetValue(Interaction.TriggersProperty, value);
			}
			return value;
		}

		internal static bool IsElementLoaded(FrameworkElement element)
		{
			return element.IsLoaded;
		}

		private static void OnBehaviorsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			BehaviorCollection oldValue = (BehaviorCollection)args.OldValue;
			BehaviorCollection newValue = (BehaviorCollection)args.NewValue;
			if (oldValue != newValue)
			{
				if (oldValue != null && ((IAttachedObject)oldValue).AssociatedObject != null)
				{
					oldValue.Detach();
				}
				if (newValue != null && obj != null)
				{
					if (((IAttachedObject)newValue).AssociatedObject != null)
					{
						throw new InvalidOperationException(ExceptionStringTable.CannotHostBehaviorCollectionMultipleTimesExceptionMessage);
					}
					newValue.Attach(obj);
				}
			}
		}

		private static void OnTriggersChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			System.Windows.Interactivity.TriggerCollection oldValue = args.OldValue as System.Windows.Interactivity.TriggerCollection;
			System.Windows.Interactivity.TriggerCollection newValue = args.NewValue as System.Windows.Interactivity.TriggerCollection;
			if (oldValue != newValue)
			{
				if (oldValue != null && ((IAttachedObject)oldValue).AssociatedObject != null)
				{
					oldValue.Detach();
				}
				if (newValue != null && obj != null)
				{
					if (((IAttachedObject)newValue).AssociatedObject != null)
					{
						throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerCollectionMultipleTimesExceptionMessage);
					}
					newValue.Attach(obj);
				}
			}
		}
	}
}