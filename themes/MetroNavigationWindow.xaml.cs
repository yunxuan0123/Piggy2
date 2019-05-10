using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace MahApps.Metro.Controls
{
	[ContentProperty("OverlayContent")]
	public partial class MetroNavigationWindow : MetroWindow
	{
		public readonly static DependencyProperty OverlayContentProperty;

		public readonly static DependencyProperty PageContentProperty;

		public IEnumerable BackStack
		{
			get
			{
				return this.PART_Frame.BackStack;
			}
		}

		public bool CanGoBack
		{
			get
			{
				return this.PART_Frame.CanGoBack;
			}
		}

		public bool CanGoForward
		{
			get
			{
				return this.PART_Frame.CanGoForward;
			}
		}

		public IEnumerable ForwardStack
		{
			get
			{
				return this.PART_Frame.ForwardStack;
			}
		}

		public System.Windows.Navigation.NavigationService NavigationService
		{
			get
			{
				return this.PART_Frame.NavigationService;
			}
		}

		public object OverlayContent
		{
			get
			{
				return base.GetValue(MetroNavigationWindow.OverlayContentProperty);
			}
			set
			{
				base.SetValue(MetroNavigationWindow.OverlayContentProperty, value);
			}
		}

		public object PageContent
		{
			get
			{
				return base.GetValue(MetroNavigationWindow.PageContentProperty);
			}
			private set
			{
				base.SetValue(MetroNavigationWindow.PageContentProperty, value);
			}
		}

		public Uri Source
		{
			get
			{
				return this.PART_Frame.Source;
			}
			set
			{
				this.PART_Frame.Source = value;
			}
		}

		Uri System.Windows.Markup.IUriContext.BaseUri
		{
			get;
			set;
		}

		static MetroNavigationWindow()
		{
			Class6.yDnXvgqzyB5jw();
			MetroNavigationWindow.OverlayContentProperty = DependencyProperty.Register("OverlayContent", typeof(object), typeof(MetroNavigationWindow));
			MetroNavigationWindow.PageContentProperty = DependencyProperty.Register("PageContent", typeof(object), typeof(MetroNavigationWindow));
		}

		public MetroNavigationWindow()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.InitializeComponent();
			base.Loaded += new RoutedEventHandler(this.MetroNavigationWindow_Loaded);
			base.Closing += new CancelEventHandler(this.MetroNavigationWindow_Closing);
		}

		private void MetroNavigationWindow_Loaded(object sender, RoutedEventArgs e)
		{
			this.PART_Frame.Navigated += new NavigatedEventHandler(this.PART_Frame_Navigated);
			this.PART_Frame.Navigating += new NavigatingCancelEventHandler(this.PART_Frame_Navigating);
			this.PART_Frame.NavigationFailed += new NavigationFailedEventHandler(this.PART_Frame_NavigationFailed);
			this.PART_Frame.NavigationProgress += new NavigationProgressEventHandler(this.PART_Frame_NavigationProgress);
			this.PART_Frame.NavigationStopped += new NavigationStoppedEventHandler(this.PART_Frame_NavigationStopped);
			this.PART_Frame.LoadCompleted += new LoadCompletedEventHandler(this.PART_Frame_LoadCompleted);
			this.PART_Frame.FragmentNavigation += new FragmentNavigationEventHandler(this.PART_Frame_FragmentNavigation);
			this.PART_BackButton.Click += new RoutedEventHandler(this.PART_BackButton_Click);
			this.PART_ForwardButton.Click += new RoutedEventHandler(this.PART_ForwardButton_Click);
		}

		public event FragmentNavigationEventHandler FragmentNavigation;

		public event LoadCompletedEventHandler LoadCompleted;

		public event NavigatedEventHandler Navigated;

		public event NavigatingCancelEventHandler Navigating;

		public event NavigationFailedEventHandler NavigationFailed;

		public event NavigationProgressEventHandler NavigationProgress;

		public event NavigationStoppedEventHandler NavigationStopped;
	}
}