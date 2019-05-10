using System;

namespace System.Windows.Interactivity
{
	public abstract class EventTriggerBase<T> : EventTriggerBase
	where T : class
	{
		public new T Source
		{
			get
			{
				return (T)base.Source;
			}
		}

		protected EventTriggerBase()
		{
			Class6.yDnXvgqzyB5jw();
			base(typeof(T));
		}

		protected virtual void OnSourceChanged(T oldSource, T newSource)
		{
		}

		internal sealed override void OnSourceChangedImpl(object oldSource, object newSource)
		{
			base.OnSourceChangedImpl(oldSource, newSource);
			this.OnSourceChanged((T)(oldSource as T), (T)(newSource as T));
		}
	}
}