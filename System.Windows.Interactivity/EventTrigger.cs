using System;
using System.Windows;

namespace System.Windows.Interactivity
{
	public class EventTrigger : EventTriggerBase<object>
	{
		public readonly static DependencyProperty EventNameProperty;

		public string EventName
		{
			get
			{
				return (string)base.GetValue(System.Windows.Interactivity.EventTrigger.EventNameProperty);
			}
			set
			{
				base.SetValue(System.Windows.Interactivity.EventTrigger.EventNameProperty, value);
			}
		}

		static EventTrigger()
		{
			Class6.yDnXvgqzyB5jw();
			System.Windows.Interactivity.EventTrigger.EventNameProperty = DependencyProperty.Register("EventName", typeof(string), typeof(System.Windows.Interactivity.EventTrigger), new FrameworkPropertyMetadata("Loaded", new PropertyChangedCallback(System.Windows.Interactivity.EventTrigger.OnEventNameChanged)));
		}

		public EventTrigger()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public EventTrigger(string eventName)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.EventName = eventName;
		}

		protected override string GetEventName()
		{
			return this.EventName;
		}

		private static void OnEventNameChanged(System.Windows.Interactivity.EventTrigger sender, DependencyPropertyChangedEventArgs args)
		{
			((System.Windows.Interactivity.EventTrigger)sender).OnEventNameChanged((string)args.OldValue, (string)args.NewValue);
		}
	}
}