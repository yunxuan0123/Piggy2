using System;
using System.Windows;

namespace System.Windows.Interactivity
{
	public sealed class BehaviorCollection : AttachableCollection<Behavior>
	{
		internal BehaviorCollection()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		protected override Freezable CreateInstanceCore()
		{
			return new BehaviorCollection();
		}

		internal override void ItemAdded(Behavior item)
		{
			if (base.AssociatedObject != null)
			{
				item.Attach(base.AssociatedObject);
			}
		}

		internal override void ItemRemoved(Behavior item)
		{
			if (((IAttachedObject)item).AssociatedObject != null)
			{
				item.Detach();
			}
		}

		protected override void OnAttached()
		{
			foreach (Behavior behavior in this)
			{
				behavior.Attach(base.AssociatedObject);
			}
		}

		protected override void OnDetaching()
		{
			foreach (Behavior behavior in this)
			{
				behavior.Detach();
			}
		}
	}
}