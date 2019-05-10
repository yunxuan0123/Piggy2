using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name="PART_BackButton", Type=typeof(Button))]
	[TemplatePart(Name="PART_BannerGrid", Type=typeof(Grid))]
	[TemplatePart(Name="PART_BannerLabel", Type=typeof(Label))]
	[TemplatePart(Name="PART_DownButton", Type=typeof(Button))]
	[TemplatePart(Name="PART_ForwardButton", Type=typeof(Button))]
	[TemplatePart(Name="PART_Presenter", Type=typeof(TransitioningContentControl))]
	[TemplatePart(Name="PART_UpButton", Type=typeof(Button))]
	public class FlipView : Selector
	{
		private const string PART_Presenter = "PART_Presenter";

		private const string PART_BackButton = "PART_BackButton";

		private const string PART_ForwardButton = "PART_ForwardButton";

		private const string PART_UpButton = "PART_UpButton";

		private const string PART_DownButton = "PART_DownButton";

		private const string PART_BannerGrid = "PART_BannerGrid";

		private const string PART_BannerLabel = "PART_BannerLabel";

		private TransitioningContentControl presenter;

		private Button backButton;

		private Button forwardButton;

		private Button upButton;

		private Button downButton;

		private Grid bannerGrid;

		private Label bannerLabel;

		private Storyboard showBannerStoryboard;

		private Storyboard hideBannerStoryboard;

		private Storyboard hideControlStoryboard;

		private Storyboard showControlStoryboard;

		private EventHandler hideControlStoryboardCompletedHandler;

		private bool loaded;

		private bool controlsVisibilityOverride;

		public readonly static DependencyProperty UpTransitionProperty;

		public readonly static DependencyProperty DownTransitionProperty;

		public readonly static DependencyProperty LeftTransitionProperty;

		public readonly static DependencyProperty RightTransitionProperty;

		public readonly static DependencyProperty MouseOverGlowEnabledProperty;

		public readonly static DependencyProperty OrientationProperty;

		public readonly static DependencyProperty IsBannerEnabledProperty;

		public readonly static DependencyProperty BannerTextProperty;

		public string BannerText
		{
			get
			{
				return (string)base.GetValue(FlipView.BannerTextProperty);
			}
			set
			{
				base.SetValue(FlipView.BannerTextProperty, value);
			}
		}

		public TransitionType DownTransition
		{
			get
			{
				return (TransitionType)base.GetValue(FlipView.DownTransitionProperty);
			}
			set
			{
				base.SetValue(FlipView.DownTransitionProperty, value);
			}
		}

		public bool IsBannerEnabled
		{
			get
			{
				return (bool)base.GetValue(FlipView.IsBannerEnabledProperty);
			}
			set
			{
				base.SetValue(FlipView.IsBannerEnabledProperty, value);
			}
		}

		public TransitionType LeftTransition
		{
			get
			{
				return (TransitionType)base.GetValue(FlipView.LeftTransitionProperty);
			}
			set
			{
				base.SetValue(FlipView.LeftTransitionProperty, value);
			}
		}

		public bool MouseOverGlowEnabled
		{
			get
			{
				return (bool)base.GetValue(FlipView.MouseOverGlowEnabledProperty);
			}
			set
			{
				base.SetValue(FlipView.MouseOverGlowEnabledProperty, value);
			}
		}

		public System.Windows.Controls.Orientation Orientation
		{
			get
			{
				return (System.Windows.Controls.Orientation)base.GetValue(FlipView.OrientationProperty);
			}
			set
			{
				base.SetValue(FlipView.OrientationProperty, value);
			}
		}

		public TransitionType RightTransition
		{
			get
			{
				return (TransitionType)base.GetValue(FlipView.RightTransitionProperty);
			}
			set
			{
				base.SetValue(FlipView.RightTransitionProperty, value);
			}
		}

		public TransitionType UpTransition
		{
			get
			{
				return (TransitionType)base.GetValue(FlipView.UpTransitionProperty);
			}
			set
			{
				base.SetValue(FlipView.UpTransitionProperty, value);
			}
		}

		static FlipView()
		{
			Class6.yDnXvgqzyB5jw();
			FlipView.UpTransitionProperty = DependencyProperty.Register("UpTransition", typeof(TransitionType), typeof(FlipView), new PropertyMetadata((object)TransitionType.Up));
			FlipView.DownTransitionProperty = DependencyProperty.Register("DownTransition", typeof(TransitionType), typeof(FlipView), new PropertyMetadata((object)TransitionType.Down));
			FlipView.LeftTransitionProperty = DependencyProperty.Register("LeftTransition", typeof(TransitionType), typeof(FlipView), new PropertyMetadata((object)TransitionType.LeftReplace));
			FlipView.RightTransitionProperty = DependencyProperty.Register("RightTransition", typeof(TransitionType), typeof(FlipView), new PropertyMetadata((object)TransitionType.RightReplace));
			FlipView.MouseOverGlowEnabledProperty = DependencyProperty.Register("MouseOverGlowEnabled", typeof(bool), typeof(FlipView), new PropertyMetadata(true));
			FlipView.OrientationProperty = DependencyProperty.Register("Orientation", typeof(System.Windows.Controls.Orientation), typeof(FlipView), new PropertyMetadata((object)System.Windows.Controls.Orientation.Horizontal));
			FlipView.IsBannerEnabledProperty = DependencyProperty.Register("IsBannerEnabled", typeof(bool), typeof(FlipView), new UIPropertyMetadata(true, new PropertyChangedCallback(FlipView.OnIsBannerEnabledPropertyChangedCallback)));
			FlipView.BannerTextProperty = DependencyProperty.Register("BannerText", typeof(string), typeof(FlipView), new FrameworkPropertyMetadata("Banner", FrameworkPropertyMetadataOptions.AffectsRender, (DependencyObject d, DependencyPropertyChangedEventArgs e) => FlipView.ExecuteWhenLoaded((FlipView)d, () => ((FlipView)d).ChangeBannerText((string)e.NewValue))));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(typeof(FlipView)));
		}

		public FlipView()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			base.Unloaded += new RoutedEventHandler(this.FlipView_Unloaded);
			base.Loaded += new RoutedEventHandler(this.FlipView_Loaded);
			base.MouseLeftButtonDown += new MouseButtonEventHandler(this.FlipView_MouseLeftButtonDown);
		}

		private void backButton_Click(object sender, RoutedEventArgs e)
		{
			this.GoBack();
		}

		private void ChangeBannerText(string value = null)
		{
			if (!this.IsBannerEnabled)
			{
				FlipView.ExecuteWhenLoaded(this, () => this.bannerLabel.Content = value ?? this.BannerText);
				return;
			}
			string str = value ?? this.BannerText;
			if (str == null)
			{
				return;
			}
			if (this.hideControlStoryboardCompletedHandler != null)
			{
				this.hideControlStoryboard.Completed -= this.hideControlStoryboardCompletedHandler;
			}
			this.hideControlStoryboardCompletedHandler = (object sender, EventArgs e) => {
				try
				{
					this.hideControlStoryboard.Completed -= this.hideControlStoryboardCompletedHandler;
					this.bannerLabel.Content = str;
					this.bannerLabel.BeginStoryboard(this.showControlStoryboard, HandoffBehavior.SnapshotAndReplace);
				}
				catch (Exception exception)
				{
				}
			};
			this.hideControlStoryboard.Completed += this.hideControlStoryboardCompletedHandler;
			this.bannerLabel.BeginStoryboard(this.hideControlStoryboard, HandoffBehavior.SnapshotAndReplace);
		}

		private void DetectControlButtonsStatus()
		{
			if (this.controlsVisibilityOverride)
			{
				return;
			}
			if (this.backButton == null || this.forwardButton == null)
			{
				return;
			}
			this.backButton.Visibility = System.Windows.Visibility.Hidden;
			this.forwardButton.Visibility = System.Windows.Visibility.Hidden;
			this.upButton.Visibility = System.Windows.Visibility.Hidden;
			this.downButton.Visibility = System.Windows.Visibility.Hidden;
			if (base.Items.Count <= 0)
			{
				this.backButton.Visibility = System.Windows.Visibility.Hidden;
				this.forwardButton.Visibility = System.Windows.Visibility.Hidden;
				this.upButton.Visibility = System.Windows.Visibility.Hidden;
				this.downButton.Visibility = System.Windows.Visibility.Hidden;
				return;
			}
			if (this.Orientation == System.Windows.Controls.Orientation.Horizontal)
			{
				this.backButton.Visibility = (base.SelectedIndex == 0 ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible);
				this.forwardButton.Visibility = (base.SelectedIndex == base.Items.Count - 1 ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible);
				return;
			}
			this.upButton.Visibility = (base.SelectedIndex == 0 ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible);
			this.downButton.Visibility = (base.SelectedIndex == base.Items.Count - 1 ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible);
		}

		private void downButton_Click(object sender, RoutedEventArgs e)
		{
			this.GoForward();
		}

		private static void ExecuteWhenLoaded(FlipView flipview, Action body)
		{
			if (flipview.IsLoaded)
			{
				System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke(body);
				return;
			}
			RoutedEventHandler cSu0024u003cu003e8_locals1 = null;
			cSu0024u003cu003e8_locals1 = (object sender, RoutedEventArgs e) => {
				this.CS$<>8__locals1.flipview.Loaded -= this.handler;
				System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke(this.CS$<>8__locals1.body);
			};
			flipview.Loaded += cSu0024u003cu003e8_locals1;
		}

		private void FlipView_Loaded(object sender, RoutedEventArgs e)
		{
			if (this.backButton == null || this.forwardButton == null)
			{
				base.ApplyTemplate();
			}
			if (this.loaded)
			{
				return;
			}
			this.backButton.Click += new RoutedEventHandler(this.backButton_Click);
			this.forwardButton.Click += new RoutedEventHandler(this.forwardButton_Click);
			this.upButton.Click += new RoutedEventHandler(this.upButton_Click);
			this.downButton.Click += new RoutedEventHandler(this.downButton_Click);
			base.SelectionChanged += new SelectionChangedEventHandler(this.FlipView_SelectionChanged);
			base.PreviewKeyDown += new KeyEventHandler(this.FlipView_PreviewKeyDown);
			base.SelectedIndex = 0;
			this.DetectControlButtonsStatus();
			this.ShowBanner();
			this.loaded = true;
		}

		private void FlipView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			base.Focus();
		}

		private void FlipView_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			Key key = e.Key;
			if (key == Key.Left)
			{
				this.GoBack();
				e.Handled = true;
			}
			else if (key == Key.Right)
			{
				this.GoForward();
				e.Handled = true;
			}
			if (e.Handled)
			{
				base.Focus();
			}
		}

		private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.DetectControlButtonsStatus();
		}

		private void FlipView_Unloaded(object sender, RoutedEventArgs e)
		{
			base.Unloaded -= new RoutedEventHandler(this.FlipView_Unloaded);
			base.MouseLeftButtonDown -= new MouseButtonEventHandler(this.FlipView_MouseLeftButtonDown);
			base.SelectionChanged -= new SelectionChangedEventHandler(this.FlipView_SelectionChanged);
			base.PreviewKeyDown -= new KeyEventHandler(this.FlipView_PreviewKeyDown);
			this.backButton.Click -= new RoutedEventHandler(this.backButton_Click);
			this.forwardButton.Click -= new RoutedEventHandler(this.forwardButton_Click);
			this.upButton.Click -= new RoutedEventHandler(this.upButton_Click);
			this.downButton.Click -= new RoutedEventHandler(this.downButton_Click);
			if (this.hideControlStoryboardCompletedHandler != null)
			{
				this.hideControlStoryboard.Completed -= this.hideControlStoryboardCompletedHandler;
			}
			this.loaded = false;
		}

		private void forwardButton_Click(object sender, RoutedEventArgs e)
		{
			this.GoForward();
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new FlipViewItem()
			{
				HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
			};
		}

		public void GoBack()
		{
			if (base.SelectedIndex > 0)
			{
				this.presenter.Transition = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? this.RightTransition : this.UpTransition);
				base.SelectedIndex = base.SelectedIndex - 1;
			}
		}

		public void GoForward()
		{
			if (base.SelectedIndex < base.Items.Count - 1)
			{
				this.presenter.Transition = (this.Orientation == System.Windows.Controls.Orientation.Horizontal ? this.LeftTransition : this.DownTransition);
				base.SelectedIndex = base.SelectedIndex + 1;
			}
		}

		private void HideBanner()
		{
			if (base.ActualHeight > 0)
			{
				this.bannerLabel.BeginStoryboard(this.hideControlStoryboard);
				this.bannerGrid.BeginStoryboard(this.hideBannerStoryboard);
			}
		}

		public void HideControlButtons()
		{
			this.controlsVisibilityOverride = true;
			FlipView.ExecuteWhenLoaded(this, () => {
				this.backButton.Visibility = System.Windows.Visibility.Hidden;
				this.forwardButton.Visibility = System.Windows.Visibility.Hidden;
			});
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is FlipViewItem;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.showBannerStoryboard = ((Storyboard)base.Template.Resources["ShowBannerStoryboard"]).Clone();
			this.hideBannerStoryboard = ((Storyboard)base.Template.Resources["HideBannerStoryboard"]).Clone();
			this.showControlStoryboard = ((Storyboard)base.Template.Resources["ShowControlStoryboard"]).Clone();
			this.hideControlStoryboard = ((Storyboard)base.Template.Resources["HideControlStoryboard"]).Clone();
			this.presenter = base.GetTemplateChild("PART_Presenter") as TransitioningContentControl;
			this.backButton = base.GetTemplateChild("PART_BackButton") as Button;
			this.forwardButton = base.GetTemplateChild("PART_ForwardButton") as Button;
			this.upButton = base.GetTemplateChild("PART_UpButton") as Button;
			this.downButton = base.GetTemplateChild("PART_DownButton") as Button;
			this.bannerGrid = base.GetTemplateChild("PART_BannerGrid") as Grid;
			this.bannerLabel = base.GetTemplateChild("PART_BannerLabel") as Label;
			this.bannerLabel.Opacity = (this.IsBannerEnabled ? 1 : 0);
		}

		private static void OnIsBannerEnabledPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FlipView flipView = (FlipView)d;
			if (!flipView.IsLoaded)
			{
				FlipView.ExecuteWhenLoaded(flipView, () => {
					flipView.ApplyTemplate();
					if (!(bool)e.NewValue)
					{
						flipView.HideBanner();
						return;
					}
					flipView.ChangeBannerText(flipView.BannerText);
					flipView.ShowBanner();
				});
				return;
			}
			if (!(bool)e.NewValue)
			{
				flipView.HideBanner();
				return;
			}
			flipView.ChangeBannerText(flipView.BannerText);
			flipView.ShowBanner();
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			this.DetectControlButtonsStatus();
		}

		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			base.OnItemsSourceChanged(oldValue, newValue);
			base.SelectedIndex = 0;
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			if (element != item)
			{
				element.SetCurrentValue(FrameworkElement.DataContextProperty, item);
			}
			base.PrepareContainerForItemOverride(element, item);
		}

		private void ShowBanner()
		{
			if (this.IsBannerEnabled)
			{
				this.bannerGrid.BeginStoryboard(this.showBannerStoryboard);
			}
		}

		public void ShowControlButtons()
		{
			this.controlsVisibilityOverride = false;
			FlipView.ExecuteWhenLoaded(this, () => {
				this.backButton.Visibility = System.Windows.Visibility.Visible;
				this.forwardButton.Visibility = System.Windows.Visibility.Visible;
			});
		}

		private void upButton_Click(object sender, RoutedEventArgs e)
		{
			this.GoBack();
		}
	}
}