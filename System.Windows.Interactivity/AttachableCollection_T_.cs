using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows;

namespace System.Windows.Interactivity
{
	public abstract class AttachableCollection<T> : FreezableCollection<T>, IAttachedObject
	where T : IAttachedObject, DependencyObject
	{
		private Collection<T> snapshot;

		private DependencyObject associatedObject;

		protected DependencyObject AssociatedObject
		{
			get
			{
				base.ReadPreamble();
				return this.associatedObject;
			}
		}

		DependencyObject System.Windows.Interactivity.IAttachedObject.AssociatedObject
		{
			get
			{
				return this.AssociatedObject;
			}
		}

		internal AttachableCollection()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);
			this.snapshot = new Collection<T>();
		}

		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != this.AssociatedObject)
			{
				if (this.AssociatedObject != null)
				{
					throw new InvalidOperationException();
				}
				if (Interaction.ShouldRunInDesignMode || !(bool)base.GetValue(DesignerProperties.IsInDesignModeProperty))
				{
					base.WritePreamble();
					this.associatedObject = dependencyObject;
					base.WritePostscript();
				}
				this.OnAttached();
			}
		}

		public void Detach()
		{
			this.OnDetaching();
			base.WritePreamble();
			this.associatedObject = null;
			base.WritePostscript();
		}

		internal abstract void ItemAdded(T item);

		internal abstract void ItemRemoved(T item);

		protected abstract void OnAttached();

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
				{
					IEnumerator enumerator = e.NewItems.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							T current = (T)enumerator.Current;
							try
							{
								this.VerifyAdd(current);
								this.ItemAdded(current);
							}
							finally
							{
								this.snapshot.Insert(base.IndexOf(current), current);
							}
						}
						return;
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					break;
				}
				case NotifyCollectionChangedAction.Remove:
				{
					IEnumerator enumerator1 = e.OldItems.GetEnumerator();
					try
					{
						while (enumerator1.MoveNext())
						{
							T t = (T)enumerator1.Current;
							this.ItemRemoved(t);
							this.snapshot.Remove(t);
						}
						return;
					}
					finally
					{
						IDisposable disposable1 = enumerator1 as IDisposable;
						if (disposable1 != null)
						{
							disposable1.Dispose();
						}
					}
					break;
				}
				case NotifyCollectionChangedAction.Replace:
				{
					foreach (T oldItem in e.OldItems)
					{
						this.ItemRemoved(oldItem);
						this.snapshot.Remove(oldItem);
					}
					IEnumerator enumerator2 = e.NewItems.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							T current1 = (T)enumerator2.Current;
							try
							{
								this.VerifyAdd(current1);
								this.ItemAdded(current1);
							}
							finally
							{
								this.snapshot.Insert(base.IndexOf(current1), current1);
							}
						}
						return;
					}
					finally
					{
						IDisposable disposable2 = enumerator2 as IDisposable;
						if (disposable2 != null)
						{
							disposable2.Dispose();
						}
					}
					break;
				}
				case NotifyCollectionChangedAction.Move:
				{
					return;
				}
				case NotifyCollectionChangedAction.Reset:
				{
					foreach (T t1 in this.snapshot)
					{
						this.ItemRemoved(t1);
					}
					this.snapshot = new Collection<T>();
					FreezableCollection<T>.Enumerator enumerator3 = base.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							T current2 = enumerator3.Current;
							this.VerifyAdd(current2);
							this.ItemAdded(current2);
						}
						return;
					}
					finally
					{
						((IDisposable)enumerator3).Dispose();
					}
					break;
				}
				default:
				{
					return;
				}
			}
		}

		protected abstract void OnDetaching();

		private void VerifyAdd(T item)
		{
			if (this.snapshot.Contains(item))
			{
				CultureInfo currentCulture = CultureInfo.CurrentCulture;
				string duplicateItemInCollectionExceptionMessage = ExceptionStringTable.DuplicateItemInCollectionExceptionMessage;
				object[] name = new object[] { typeof(T).Name, base.GetType().Name };
				throw new InvalidOperationException(string.Format(currentCulture, duplicateItemInCollectionExceptionMessage, name));
			}
		}

		[Conditional("DEBUG")]
		private void VerifySnapshotIntegrity()
		{
			if (base.Count == this.snapshot.Count)
			{
				for (int i = 0; i < base.Count; i++)
				{
					if ((object)base[i] != (object)this.snapshot[i])
					{
						return;
					}
				}
			}
		}
	}
}