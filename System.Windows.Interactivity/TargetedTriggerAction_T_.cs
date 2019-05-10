using System;

namespace System.Windows.Interactivity
{
	public abstract class TargetedTriggerAction<T> : TargetedTriggerAction
	where T : class
	{
		protected new T Target
		{
			get
			{
				return (T)base.Target;
			}
		}

		protected TargetedTriggerAction()
		{
			Class6.yDnXvgqzyB5jw();
			base(typeof(T));
		}

		protected virtual void OnTargetChanged(T oldTarget, T newTarget)
		{
		}

		internal sealed override void OnTargetChangedImpl(object oldTarget, object newTarget)
		{
			base.OnTargetChangedImpl(oldTarget, newTarget);
			this.OnTargetChanged((T)(oldTarget as T), (T)(newTarget as T));
		}
	}
}