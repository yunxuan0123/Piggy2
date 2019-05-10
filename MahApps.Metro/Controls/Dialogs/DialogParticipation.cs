using System;
using System.Collections.Generic;
using System.Windows;

namespace MahApps.Metro.Controls.Dialogs
{
	public static class DialogParticipation
	{
		private readonly static IDictionary<object, DependencyObject> ContextRegistrationIndex;

		public readonly static DependencyProperty RegisterProperty;

		static DialogParticipation()
		{
			Class6.yDnXvgqzyB5jw();
			DialogParticipation.ContextRegistrationIndex = new Dictionary<object, DependencyObject>();
			DialogParticipation.RegisterProperty = DependencyProperty.RegisterAttached("Register", typeof(object), typeof(DialogParticipation), new PropertyMetadata(null, new PropertyChangedCallback(DialogParticipation.RegisterPropertyChangedCallback)));
		}

		internal static DependencyObject GetAssociation(object context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			return DialogParticipation.ContextRegistrationIndex[context];
		}

		public static object GetRegister(DependencyObject element)
		{
			return element.GetValue(DialogParticipation.RegisterProperty);
		}

		internal static bool IsRegistered(object context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			return DialogParticipation.ContextRegistrationIndex.ContainsKey(context);
		}

		private static void RegisterPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			if (dependencyPropertyChangedEventArgs.OldValue != null)
			{
				DialogParticipation.ContextRegistrationIndex.Remove(dependencyPropertyChangedEventArgs.OldValue);
			}
			if (dependencyPropertyChangedEventArgs.NewValue != null)
			{
				DialogParticipation.ContextRegistrationIndex[dependencyPropertyChangedEventArgs.NewValue] = dependencyObject;
			}
		}

		public static void SetRegister(DependencyObject element, object context)
		{
			element.SetValue(DialogParticipation.RegisterProperty, context);
		}
	}
}