using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class WindowCommands : ItemsControl, INotifyPropertyChanged
	{
		public readonly static DependencyProperty ThemeProperty;

		public readonly static DependencyProperty LightTemplateProperty;

		public readonly static DependencyProperty DarkTemplateProperty;

		public readonly static DependencyProperty ShowSeparatorsProperty;

		public readonly static DependencyProperty ShowLastSeparatorProperty;

		public readonly static DependencyProperty SeparatorHeightProperty;

		private Window _parentWindow;

		public ControlTemplate DarkTemplate
		{
			get
			{
				return (ControlTemplate)base.GetValue(WindowCommands.DarkTemplateProperty);
			}
			set
			{
				base.SetValue(WindowCommands.DarkTemplateProperty, value);
			}
		}

		public ControlTemplate LightTemplate
		{
			get
			{
				return (ControlTemplate)base.GetValue(WindowCommands.LightTemplateProperty);
			}
			set
			{
				base.SetValue(WindowCommands.LightTemplateProperty, value);
			}
		}

		public Window ParentWindow
		{
			get
			{
				return this._parentWindow;
			}
			set
			{
				if (object.Equals(this._parentWindow, value))
				{
					return;
				}
				this._parentWindow = value;
				this.RaisePropertyChanged("ParentWindow");
			}
		}

		public int SeparatorHeight
		{
			get
			{
				return (int)base.GetValue(WindowCommands.SeparatorHeightProperty);
			}
			set
			{
				base.SetValue(WindowCommands.SeparatorHeightProperty, value);
			}
		}

		public bool ShowLastSeparator
		{
			get
			{
				return (bool)base.GetValue(WindowCommands.ShowLastSeparatorProperty);
			}
			set
			{
				base.SetValue(WindowCommands.ShowLastSeparatorProperty, value);
			}
		}

		public bool ShowSeparators
		{
			get
			{
				return (bool)base.GetValue(WindowCommands.ShowSeparatorsProperty);
			}
			set
			{
				base.SetValue(WindowCommands.ShowSeparatorsProperty, value);
			}
		}

		public MahApps.Metro.Controls.Theme Theme
		{
			get
			{
				return (MahApps.Metro.Controls.Theme)base.GetValue(WindowCommands.ThemeProperty);
			}
			set
			{
				base.SetValue(WindowCommands.ThemeProperty, value);
			}
		}

		static WindowCommands()
		{
			Class6.yDnXvgqzyB5jw();
			WindowCommands.ThemeProperty = DependencyProperty.Register("Theme", typeof(MahApps.Metro.Controls.Theme), typeof(WindowCommands), new PropertyMetadata((object)MahApps.Metro.Controls.Theme.Light, new PropertyChangedCallback(WindowCommands.OnThemeChanged)));
			WindowCommands.LightTemplateProperty = DependencyProperty.Register("LightTemplate", typeof(ControlTemplate), typeof(WindowCommands), new PropertyMetadata(null));
			WindowCommands.DarkTemplateProperty = DependencyProperty.Register("DarkTemplate", typeof(ControlTemplate), typeof(WindowCommands), new PropertyMetadata(null));
			WindowCommands.ShowSeparatorsProperty = DependencyProperty.Register("ShowSeparators", typeof(bool), typeof(WindowCommands), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(WindowCommands.OnShowSeparatorsChanged)));
			WindowCommands.ShowLastSeparatorProperty = DependencyProperty.Register("ShowLastSeparator", typeof(bool), typeof(WindowCommands), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(WindowCommands.OnShowLastSeparatorChanged)));
			WindowCommands.SeparatorHeightProperty = DependencyProperty.Register("SeparatorHeight", typeof(int), typeof(WindowCommands), new FrameworkPropertyMetadata((object)15, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCommands), new FrameworkPropertyMetadata(typeof(WindowCommands)));
		}

		public WindowCommands()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			base.Loaded += new RoutedEventHandler(this.WindowCommands_Loaded);
		}

		private void AttachVisibilityHandler(WindowCommandsItem container, UIElement item)
		{
			if (container != null)
			{
				if (item == null)
				{
					container.Visibility = System.Windows.Visibility.Collapsed;
					return;
				}
				container.Visibility = item.Visibility;
				PropertyChangeNotifier propertyChangeNotifier = new PropertyChangeNotifier(item, UIElement.VisibilityProperty);
				propertyChangeNotifier.ValueChanged += new EventHandler(this.VisibilityPropertyChanged);
				container.VisibilityPropertyChangeNotifier = propertyChangeNotifier;
			}
		}

		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			this.DetachVisibilityHandler(element as WindowCommandsItem);
			this.ResetSeparators(false);
		}

		private void DetachVisibilityHandler(WindowCommandsItem container)
		{
			if (container != null)
			{
				container.VisibilityPropertyChangeNotifier = null;
			}
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new WindowCommandsItem();
		}

		private WindowCommandsItem GetWindowCommandsItem(object item)
		{
			WindowCommandsItem windowCommandsItem = item as WindowCommandsItem;
			if (windowCommandsItem != null)
			{
				return windowCommandsItem;
			}
			return (WindowCommandsItem)base.ItemContainerGenerator.ContainerFromItem(item);
		}

		private IEnumerable<WindowCommandsItem> GetWindowCommandsItems()
		{
			return 
				from object item in base.Items
				select this.GetWindowCommandsItem(item) into i
				where i != null
				select i;
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is WindowCommandsItem;
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			this.ResetSeparators(true);
		}

		private static void OnShowLastSeparatorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == e.OldValue)
			{
				return;
			}
			((WindowCommands)d).ResetSeparators(false);
		}

		private static void OnShowSeparatorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == e.OldValue)
			{
				return;
			}
			((WindowCommands)d).ResetSeparators(true);
		}

		private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == e.OldValue)
			{
				return;
			}
			WindowCommands windowCommand = (WindowCommands)d;
			if ((MahApps.Metro.Controls.Theme)e.NewValue == MahApps.Metro.Controls.Theme.Light)
			{
				if (windowCommand.LightTemplate != null)
				{
					windowCommand.SetValue(Control.TemplateProperty, windowCommand.LightTemplate);
					return;
				}
			}
			else if ((MahApps.Metro.Controls.Theme)e.NewValue == MahApps.Metro.Controls.Theme.Dark && windowCommand.DarkTemplate != null)
			{
				windowCommand.SetValue(Control.TemplateProperty, windowCommand.DarkTemplate);
			}
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			this.AttachVisibilityHandler(element as WindowCommandsItem, item as UIElement);
			this.ResetSeparators(true);
		}

		protected virtual void RaisePropertyChanged(string propertyName = null)
		{
			PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
			if (propertyChangedEventHandler != null)
			{
				propertyChangedEventHandler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private void ResetSeparators(bool reset = true)
		{
			if (base.Items.Count == 0)
			{
				return;
			}
			List<WindowCommandsItem> list = this.GetWindowCommandsItems().ToList<WindowCommandsItem>();
			if (reset)
			{
				foreach (WindowCommandsItem showSeparators in list)
				{
					showSeparators.IsSeparatorVisible = this.ShowSeparators;
				}
			}
			WindowCommandsItem windowCommandsItem = list.LastOrDefault<WindowCommandsItem>((WindowCommandsItem i) => i.IsVisible);
			if (windowCommandsItem != null)
			{
				windowCommandsItem.IsSeparatorVisible = (!this.ShowSeparators ? false : this.ShowLastSeparator);
			}
		}

		private void VisibilityPropertyChanged(object sender, EventArgs e)
		{
			UIElement uIElement = sender as UIElement;
			if (uIElement != null)
			{
				WindowCommandsItem windowCommandsItem = this.GetWindowCommandsItem(uIElement);
				if (windowCommandsItem != null)
				{
					windowCommandsItem.Visibility = uIElement.Visibility;
					this.ResetSeparators(true);
				}
			}
		}

		private void WindowCommands_Loaded(object sender, RoutedEventArgs e)
		{
			base.Loaded -= new RoutedEventHandler(this.WindowCommands_Loaded);
			if (this.ParentWindow == null)
			{
				this.ParentWindow = this.TryFindParent<Window>();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}