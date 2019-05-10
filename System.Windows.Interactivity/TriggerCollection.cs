using System;
using System.Windows;

namespace System.Windows.Interactivity
{
	public sealed class TriggerCollection : AttachableCollection<System.Windows.Interactivity.TriggerBase>
	{
		internal TriggerCollection()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		protected override Freezable CreateInstanceCore()
		{
			return new System.Windows.Interactivity.TriggerCollection();
		}

		internal override void ItemAdded(System.Windows.Interactivity.TriggerBase item)
		{
			if (base.AssociatedObject != null)
			{
				item.Attach(base.AssociatedObject);
			}
		}

		internal override void ItemRemoved(System.Windows.Interactivity.TriggerBase item)
		{
			if (((IAttachedObject)item).AssociatedObject != null)
			{
				item.Detach();
			}
		}

		protected override void OnAttached()
		{
			foreach (System.Windows.Interactivity.TriggerBase triggerBase in this)
			{
				triggerBase.Attach(base.AssociatedObject);
			}
		}

		protected override void OnDetaching()
		{
			foreach (System.Windows.Interactivity.TriggerBase triggerBase in this)
			{
				triggerBase.Detach();
			}
		}
	}
}