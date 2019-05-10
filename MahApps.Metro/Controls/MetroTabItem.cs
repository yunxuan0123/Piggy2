using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	public class MetroTabItem : TabItem
	{
		internal Button closeButton;

		internal Thickness newButtonMargin;

		internal ContentPresenter contentSite;

		private bool closeButtonClickUnloaded;

		public readonly static DependencyProperty CloseButtonEnabledProperty;

		public readonly static DependencyProperty CloseTabCommandProperty;

		public readonly static DependencyProperty CloseTabCommandParameterProperty;

		public bool CloseButtonEnabled
		{
			get
			{
				return (bool)base.GetValue(MetroTabItem.CloseButtonEnabledProperty);
			}
			set
			{
				base.SetValue(MetroTabItem.CloseButtonEnabledProperty, value);
			}
		}

		public ICommand CloseTabCommand
		{
			get
			{
				return (ICommand)base.GetValue(MetroTabItem.CloseTabCommandProperty);
			}
			set
			{
				base.SetValue(MetroTabItem.CloseTabCommandProperty, value);
			}
		}

		public object CloseTabCommandParameter
		{
			get
			{
				return base.GetValue(MetroTabItem.CloseTabCommandParameterProperty);
			}
			set
			{
				base.SetValue(MetroTabItem.CloseTabCommandParameterProperty, value);
			}
		}

		static MetroTabItem()
		{
			Class6.yDnXvgqzyB5jw();
			MetroTabItem.CloseButtonEnabledProperty = DependencyProperty.Register("CloseButtonEnabled", typeof(bool), typeof(MetroTabItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(MetroTabItem.OnCloseButtonEnabledPropertyChangedCallback)));
			MetroTabItem.CloseTabCommandProperty = DependencyProperty.Register("CloseTabCommand", typeof(ICommand), typeof(MetroTabItem));
			MetroTabItem.CloseTabCommandParameterProperty = DependencyProperty.Register("CloseTabCommandParameter", typeof(object), typeof(MetroTabItem), new PropertyMetadata(null));
		}

		public MetroTabItem()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			base.DefaultStyleKey = typeof(MetroTabItem);
			base.Unloaded += new RoutedEventHandler(this.MetroTabItem_Unloaded);
			base.Loaded += new RoutedEventHandler(this.MetroTabItem_Loaded);
		}

		private void AdjustCloseButton()
		{
			this.closeButton = this.closeButton ?? base.GetTemplateChild("PART_CloseButton") as Button;
			if (this.closeButton != null)
			{
				this.closeButton.Margin = this.newButtonMargin;
				this.closeButton.Click -= new RoutedEventHandler(this.closeButton_Click);
				this.closeButton.Click += new RoutedEventHandler(this.closeButton_Click);
			}
		}

		private void closeButton_Click(object sender, RoutedEventArgs e)
		{
			ICommand closeTabCommand = this.CloseTabCommand;
			object closeTabCommandParameter = this.CloseTabCommandParameter;
			if (closeTabCommandParameter == null)
			{
				closeTabCommandParameter = this;
			}
			object obj = closeTabCommandParameter;
			if (closeTabCommand != null)
			{
				if (closeTabCommand.CanExecute(obj))
				{
					closeTabCommand.Execute(obj);
				}
				this.CloseTabCommand = null;
				this.CloseTabCommandParameter = null;
			}
			BaseMetroTabControl baseMetroTabControl = this.TryFindParent<BaseMetroTabControl>();
			if (baseMetroTabControl == null)
			{
				throw new InvalidOperationException();
			}
			object obj1 = baseMetroTabControl.ItemContainerGenerator.ItemFromContainer(this);
			baseMetroTabControl.InternalCloseTabCommand.Execute(new Tuple<object, MetroTabItem>((obj1 == DependencyProperty.UnsetValue ? base.Content : obj1), this));
		}

		private void MetroTabItem_Loaded(object sender, RoutedEventArgs e)
		{
			if (this.closeButton != null && this.closeButtonClickUnloaded)
			{
				this.closeButton.Click += new RoutedEventHandler(this.closeButton_Click);
				this.closeButtonClickUnloaded = false;
			}
		}

		private void MetroTabItem_Unloaded(object sender, RoutedEventArgs e)
		{
			base.Unloaded -= new RoutedEventHandler(this.MetroTabItem_Unloaded);
			if (this.closeButton != null)
			{
				this.closeButton.Click -= new RoutedEventHandler(this.closeButton_Click);
			}
			this.closeButtonClickUnloaded = true;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.AdjustCloseButton();
			this.contentSite = base.GetTemplateChild("ContentSite") as ContentPresenter;
		}

		private static void OnCloseButtonEnabledPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			MetroTabItem metroTabItem = dependencyObject as MetroTabItem;
			if (metroTabItem != null)
			{
				metroTabItem.AdjustCloseButton();
			}
		}
	}
}