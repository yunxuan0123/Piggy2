using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name="PART_Headers", Type=typeof(ListView))]
	[TemplatePart(Name="PART_Mediator", Type=typeof(ScrollViewerOffsetMediator))]
	[TemplatePart(Name="PART_Scroll", Type=typeof(ScrollViewer))]
	public class Pivot : ItemsControl
	{
		private ScrollViewer scroller;

		private ListView headers;

		private PivotItem selectedItem;

		private ScrollViewerOffsetMediator mediator;

		internal int internalIndex;

		public readonly static RoutedEvent SelectionChangedEvent;

		public readonly static DependencyProperty HeaderProperty;

		public readonly static DependencyProperty HeaderTemplateProperty;

		public readonly static DependencyProperty SelectedIndexProperty;

		public string Header
		{
			get
			{
				return (string)base.GetValue(Pivot.HeaderProperty);
			}
			set
			{
				base.SetValue(Pivot.HeaderProperty, value);
			}
		}

		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(Pivot.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(Pivot.HeaderTemplateProperty, value);
			}
		}

		public int SelectedIndex
		{
			get
			{
				return (int)base.GetValue(Pivot.SelectedIndexProperty);
			}
			set
			{
				base.SetValue(Pivot.SelectedIndexProperty, value);
			}
		}

		static Pivot()
		{
			Class6.yDnXvgqzyB5jw();
			Pivot.SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Pivot));
			Pivot.HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(Pivot), new PropertyMetadata(null));
			Pivot.HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(Pivot));
			Pivot.SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Pivot), new FrameworkPropertyMetadata((object)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Pivot.SelectedItemChanged)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Pivot), new FrameworkPropertyMetadata(typeof(Pivot)));
		}

		public Pivot()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public void GoToItem(PivotItem item)
		{
			if (item != null)
			{
				if (item != this.selectedItem)
				{
					double actualWidth = 0;
					int num = 0;
					while (true)
					{
						if (num >= base.Items.Count)
						{
							break;
						}
						else if (base.Items[num] != item)
						{
							actualWidth += ((PivotItem)base.Items[num]).ActualWidth;
							num++;
						}
						else
						{
							this.internalIndex = num;
							break;
						}
					}
					this.mediator.HorizontalOffset = this.scroller.HorizontalOffset;
					Storyboard storyboard = this.mediator.Resources["Storyboard1"] as Storyboard;
					((EasingDoubleKeyFrame)this.mediator.FindName("edkf")).Value = actualWidth;
					storyboard.Completed -= new EventHandler(this.sb_Completed);
					storyboard.Completed += new EventHandler(this.sb_Completed);
					storyboard.Begin();
					base.RaiseEvent(new RoutedEventArgs(Pivot.SelectionChangedEvent));
					return;
				}
			}
		}

		private void headers_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.GoToItem((PivotItem)this.headers.SelectedItem);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.scroller = (ScrollViewer)base.GetTemplateChild("PART_Scroll");
			this.headers = (ListView)base.GetTemplateChild("PART_Headers");
			this.mediator = base.GetTemplateChild("PART_Mediator") as ScrollViewerOffsetMediator;
			if (this.scroller != null)
			{
				this.scroller.ScrollChanged += new ScrollChangedEventHandler(this.scroller_ScrollChanged);
				this.scroller.PreviewMouseWheel += new MouseWheelEventHandler(this.scroller_MouseWheel);
			}
			if (this.headers != null)
			{
				this.headers.SelectionChanged += new SelectionChangedEventHandler(this.headers_SelectionChanged);
			}
		}

		private void sb_Completed(object sender, EventArgs e)
		{
			this.SelectedIndex = this.internalIndex;
		}

		private void scroller_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			this.scroller.ScrollToHorizontalOffset(this.scroller.HorizontalOffset + (double)(-e.Delta));
		}

		private void scroller_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			double num = 0;
			int num1 = 0;
			while (true)
			{
				if (num1 < base.Items.Count)
				{
					PivotItem item = (PivotItem)base.Items[num1];
					double actualWidth = item.ActualWidth;
					if (e.HorizontalOffset <= num + actualWidth - 1)
					{
						this.selectedItem = item;
						if (this.headers.SelectedItem == this.selectedItem)
						{
							break;
						}
						this.headers.SelectedItem = this.selectedItem;
						this.internalIndex = num1;
						this.SelectedIndex = num1;
						base.RaiseEvent(new RoutedEventArgs(Pivot.SelectionChangedEvent));
						return;
					}
					else
					{
						num += actualWidth;
						num1++;
					}
				}
				else
				{
					break;
				}
			}
		}

		private static void SelectedItemChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
			{
				Pivot pivot = (Pivot)dependencyObject;
				int newValue = (int)e.NewValue;
				if (pivot.internalIndex != pivot.SelectedIndex && newValue >= 0 && newValue < pivot.Items.Count)
				{
					PivotItem item = (PivotItem)pivot.Items[newValue];
					pivot.headers.SelectedItem = item;
					pivot.GoToItem(item);
				}
			}
		}

		public event RoutedEventHandler SelectionChanged
		{
			add
			{
				base.AddHandler(Pivot.SelectionChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Pivot.SelectionChangedEvent, value);
			}
		}
	}
}