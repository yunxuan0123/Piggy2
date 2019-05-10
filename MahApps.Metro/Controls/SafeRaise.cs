using System;

namespace MahApps.Metro.Controls
{
	internal static class SafeRaise
	{
		public static void Raise(EventHandler eventToRaise, object sender)
		{
			if (eventToRaise != null)
			{
				eventToRaise(sender, EventArgs.Empty);
			}
		}

		public static void Raise(EventHandler<EventArgs> eventToRaise, object sender)
		{
			SafeRaise.Raise<EventArgs>(eventToRaise, sender, EventArgs.Empty);
		}

		public static void Raise<T>(EventHandler<T> eventToRaise, object sender, T args)
		where T : EventArgs
		{
			if (eventToRaise != null)
			{
				eventToRaise(sender, args);
			}
		}

		public static void Raise<T>(EventHandler<T> eventToRaise, object sender, SafeRaise.GetEventArgs<T> getEventArgs)
		where T : EventArgs
		{
			if (eventToRaise != null)
			{
				eventToRaise(sender, getEventArgs());
			}
		}

		public delegate T GetEventArgs<T>()
		where T : EventArgs;
	}
}