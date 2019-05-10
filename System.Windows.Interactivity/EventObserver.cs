using System;
using System.Reflection;

namespace System.Windows.Interactivity
{
	public sealed class EventObserver : IDisposable
	{
		private EventInfo eventInfo;

		private object target;

		private Delegate handler;

		public EventObserver(EventInfo eventInfo, object target, Delegate handler)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			if (eventInfo == null)
			{
				throw new ArgumentNullException("eventInfo");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			this.eventInfo = eventInfo;
			this.target = target;
			this.handler = handler;
			this.eventInfo.AddEventHandler(this.target, handler);
		}

		public void Dispose()
		{
			this.eventInfo.RemoveEventHandler(this.target, this.handler);
		}
	}
}