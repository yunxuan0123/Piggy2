using System;
using System.Windows;

namespace System.Windows.Interactivity
{
	public class TriggerActionCollection : AttachableCollection<System.Windows.Interactivity.TriggerAction>
	{
		internal TriggerActionCollection()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		protected override Freezable CreateInstanceCore()
		{
			return new System.Windows.Interactivity.TriggerActionCollection();
		}

		internal override void ItemAdded(System.Windows.Interactivity.TriggerAction item)
		{
			if (item.IsHosted)
			{
				throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerActionMultipleTimesExceptionMessage);
			}
			if (base.AssociatedObject != null)
			{
				item.Attach(base.AssociatedObject);
			}
			item.IsHosted = true;
		}

		internal override void ItemRemoved(System.Windows.Interactivity.TriggerAction item)
		{
			if (((IAttachedObject)item).AssociatedObject != null)
			{
				item.Detach();
			}
			item.IsHosted = false;
		}

		protected override void OnAttached()
		{
			foreach (System.Windows.Interactivity.TriggerAction triggerAction in this)
			{
				triggerAction.Attach(base.AssociatedObject);
			}
		}

		protected override void OnDetaching()
		{
			foreach (System.Windows.Interactivity.TriggerAction triggerAction in this)
			{
				triggerAction.Detach();
			}
		}
	}
}