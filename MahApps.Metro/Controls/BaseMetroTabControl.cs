using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	public abstract class BaseMetroTabControl : TabControl
	{
		public readonly static DependencyProperty TabStripMarginProperty;

		public readonly static DependencyProperty CloseTabCommandProperty;

		private readonly static DependencyProperty InternalCloseTabCommandProperty;

		public ICommand CloseTabCommand
		{
			get
			{
				return (ICommand)base.GetValue(BaseMetroTabControl.CloseTabCommandProperty);
			}
			set
			{
				base.SetValue(BaseMetroTabControl.CloseTabCommandProperty, value);
			}
		}

		internal ICommand InternalCloseTabCommand
		{
			get
			{
				return (ICommand)base.GetValue(BaseMetroTabControl.InternalCloseTabCommandProperty);
			}
			set
			{
				base.SetValue(BaseMetroTabControl.InternalCloseTabCommandProperty, value);
			}
		}

		public Thickness TabStripMargin
		{
			get
			{
				return (Thickness)base.GetValue(BaseMetroTabControl.TabStripMarginProperty);
			}
			set
			{
				base.SetValue(BaseMetroTabControl.TabStripMarginProperty, value);
			}
		}

		static BaseMetroTabControl()
		{
			Class6.yDnXvgqzyB5jw();
			BaseMetroTabControl.TabStripMarginProperty = DependencyProperty.Register("TabStripMargin", typeof(Thickness), typeof(BaseMetroTabControl), new PropertyMetadata((object)(new Thickness(0))));
			BaseMetroTabControl.CloseTabCommandProperty = DependencyProperty.Register("CloseTabCommand", typeof(ICommand), typeof(BaseMetroTabControl), new PropertyMetadata(null));
			BaseMetroTabControl.InternalCloseTabCommandProperty = DependencyProperty.Register("InternalCloseTabCommand", typeof(ICommand), typeof(BaseMetroTabControl), new PropertyMetadata(null));
		}

		public BaseMetroTabControl()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.InternalCloseTabCommand = new BaseMetroTabControl.DefaultCloseTabCommand(this);
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new MetroTabItem();
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TabItem;
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			if (element != item)
			{
				element.SetCurrentValue(FrameworkElement.DataContextProperty, item);
			}
			base.PrepareContainerForItemOverride(element, item);
		}

		internal bool RaiseTabItemClosingEvent(MetroTabItem closingItem)
		{
			if (this.TabItemClosingEvent != null)
			{
				Delegate[] invocationList = this.TabItemClosingEvent.GetInvocationList();
				for (int i = 0; i < (int)invocationList.Length; i++)
				{
					BaseMetroTabControl.TabItemClosingEventHandler tabItemClosingEventHandler = (BaseMetroTabControl.TabItemClosingEventHandler)invocationList[i];
					BaseMetroTabControl.TabItemClosingEventArgs tabItemClosingEventArg = new BaseMetroTabControl.TabItemClosingEventArgs(closingItem);
					tabItemClosingEventHandler(this, tabItemClosingEventArg);
					if (tabItemClosingEventArg.Cancel)
					{
						return true;
					}
				}
			}
			return false;
		}

		public event BaseMetroTabControl.TabItemClosingEventHandler TabItemClosingEvent;

		internal class DefaultCloseTabCommand : ICommand
		{
			private readonly BaseMetroTabControl owner;

			internal DefaultCloseTabCommand(BaseMetroTabControl Owner)
			{
				Class6.yDnXvgqzyB5jw();
				base();
				this.owner = Owner;
			}

			public bool CanExecute(object parameter)
			{
				return true;
			}

			public void Execute(object parameter)
			{
				Func<object, bool> func = null;
				if (parameter != null)
				{
					Tuple<object, MetroTabItem> tuple = (Tuple<object, MetroTabItem>)parameter;
					if (this.owner.CloseTabCommand != null)
					{
						this.owner.CloseTabCommand.Execute(null);
						return;
					}
					if (tuple.Item2 != null)
					{
						MetroTabItem item2 = tuple.Item2;
						if (this.owner.RaiseTabItemClosingEvent(item2))
						{
							return;
						}
						if (this.owner.ItemsSource == null)
						{
							this.owner.Items.Remove(item2);
							return;
						}
						IList itemsSource = this.owner.ItemsSource as IList;
						if (itemsSource == null)
						{
							return;
						}
						IEnumerable<object> objs = this.owner.ItemsSource.Cast<object>();
						Func<object, bool> func1 = func;
						if (func1 == null)
						{
							Func<object, bool> func2 = (object item) => {
								if (item2 == item)
								{
									return true;
								}
								return item2.DataContext == item;
							};
							Func<object, bool> func3 = func2;
							func = func2;
							func1 = func3;
						}
						using (IEnumerator<object> enumerator = objs.Where<object>(func1).GetEnumerator())
						{
							if (enumerator.MoveNext())
							{
								itemsSource.Remove(enumerator.Current);
							}
						}
					}
				}
			}

			public event EventHandler CanExecuteChanged;
		}

		public class TabItemClosingEventArgs : CancelEventArgs
		{
			public MetroTabItem ClosingTabItem
			{
				get;
				private set;
			}

			internal TabItemClosingEventArgs(MetroTabItem item)
			{
				Class6.yDnXvgqzyB5jw();
				base();
				this.ClosingTabItem = item;
			}
		}

		public delegate void TabItemClosingEventHandler(object sender, BaseMetroTabControl.TabItemClosingEventArgs e);
	}
}