using MahApps.Metro;
using MahApps.Metro.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MahApps.Metro.Controls.Dialogs
{
	public abstract class BaseMetroDialog : ContentControl
	{
		public readonly static DependencyProperty TitleProperty;

		public readonly static DependencyProperty DialogTopProperty;

		public readonly static DependencyProperty DialogBottomProperty;

		public object DialogBottom
		{
			get
			{
				return base.GetValue(BaseMetroDialog.DialogBottomProperty);
			}
			set
			{
				base.SetValue(BaseMetroDialog.DialogBottomProperty, value);
			}
		}

		public MetroDialogSettings DialogSettings
		{
			get;
			private set;
		}

		public object DialogTop
		{
			get
			{
				return base.GetValue(BaseMetroDialog.DialogTopProperty);
			}
			set
			{
				base.SetValue(BaseMetroDialog.DialogTopProperty, value);
			}
		}

		protected internal MetroWindow OwningWindow
		{
			get;
			internal set;
		}

		protected internal Window ParentDialogWindow
		{
			get;
			internal set;
		}

		internal SizeChangedEventHandler SizeChangedHandler
		{
			get;
			set;
		}

		public string Title
		{
			get
			{
				return (string)base.GetValue(BaseMetroDialog.TitleProperty);
			}
			set
			{
				base.SetValue(BaseMetroDialog.TitleProperty, value);
			}
		}

		static BaseMetroDialog()
		{
			Class6.yDnXvgqzyB5jw();
			BaseMetroDialog.TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(BaseMetroDialog), new PropertyMetadata(null));
			BaseMetroDialog.DialogTopProperty = DependencyProperty.Register("DialogTop", typeof(object), typeof(BaseMetroDialog), new PropertyMetadata(null));
			BaseMetroDialog.DialogBottomProperty = DependencyProperty.Register("DialogBottom", typeof(object), typeof(BaseMetroDialog), new PropertyMetadata(null));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseMetroDialog), new FrameworkPropertyMetadata(typeof(BaseMetroDialog)));
		}

		protected BaseMetroDialog(MetroWindow owningWindow, MetroDialogSettings settings)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.DialogSettings = settings ?? owningWindow.MetroDialogOptions;
			this.OwningWindow = owningWindow;
			this.Initialize();
		}

		protected BaseMetroDialog()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.DialogSettings = new MetroDialogSettings();
			this.Initialize();
		}

		public Task _WaitForCloseAsync()
		{
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			if (!this.DialogSettings.AnimateHide)
			{
				base.Opacity = 0;
				taskCompletionSource.TrySetResult(null);
			}
			else
			{
				Storyboard item = base.Resources["DialogCloseStoryboard"] as Storyboard;
				if (item == null)
				{
					throw new InvalidOperationException("Unable to find the dialog closing storyboard. Did you forget to add BaseMetroDialog.xaml to your merged dictionaries?");
				}
				EventHandler eventHandler = null;
				eventHandler = (object sender, EventArgs e) => {
					item.Completed -= eventHandler;
					taskCompletionSource.TrySetResult(null);
				};
				item = item.Clone();
				item.Completed += eventHandler;
				item.Begin(this);
			}
			return taskCompletionSource.Task;
		}

		private void BaseMetroDialog_Unloaded(object sender, RoutedEventArgs e)
		{
			ThemeManager.IsThemeChanged -= new EventHandler<OnThemeChangedEventArgs>(this.ThemeManager_IsThemeChanged);
			base.Unloaded -= new RoutedEventHandler(this.BaseMetroDialog_Unloaded);
		}

		private static Tuple<AppTheme, Accent> DetectTheme(BaseMetroDialog dialog)
		{
			Tuple<AppTheme, Accent> tuple;
			Tuple<AppTheme, Accent> tuple1;
			if (dialog == null)
			{
				return null;
			}
			MetroWindow metroWindow = dialog.TryFindParent<MetroWindow>();
			if (metroWindow != null)
			{
				tuple = ThemeManager.DetectAppStyle(metroWindow);
			}
			else
			{
				tuple = null;
			}
			Tuple<AppTheme, Accent> tuple2 = tuple;
			if (tuple2 != null && tuple2.Item2 != null)
			{
				return tuple2;
			}
			if (Application.Current != null)
			{
				MetroWindow mainWindow = Application.Current.MainWindow as MetroWindow;
				if (mainWindow != null)
				{
					tuple1 = ThemeManager.DetectAppStyle(mainWindow);
				}
				else
				{
					tuple1 = null;
				}
				tuple2 = tuple1;
				if (tuple2 != null && tuple2.Item2 != null)
				{
					return tuple2;
				}
				tuple2 = ThemeManager.DetectAppStyle(Application.Current);
				if (tuple2 != null && tuple2.Item2 != null)
				{
					return tuple2;
				}
			}
			return null;
		}

		private void HandleTheme()
		{
			if (this.DialogSettings != null)
			{
				Tuple<AppTheme, Accent> tuple = BaseMetroDialog.DetectTheme(this);
				if (DesignerProperties.GetIsInDesignMode(this) || tuple == null)
				{
					return;
				}
				AppTheme item1 = tuple.Item1;
				Accent item2 = tuple.Item2;
				switch (this.DialogSettings.ColorScheme)
				{
					case MetroDialogColorScheme.Theme:
					{
						ThemeManager.ChangeAppStyle(base.Resources, item2, item1);
						DependencyProperty backgroundProperty = Control.BackgroundProperty;
						Window owningWindow = this.OwningWindow;
						if (owningWindow == null)
						{
							owningWindow = Application.Current.MainWindow;
						}
						base.SetValue(backgroundProperty, ThemeManager.GetResourceFromAppStyle(owningWindow, "WhiteColorBrush"));
						DependencyProperty foregroundProperty = Control.ForegroundProperty;
						Window mainWindow = this.OwningWindow;
						if (mainWindow == null)
						{
							mainWindow = Application.Current.MainWindow;
						}
						base.SetValue(foregroundProperty, ThemeManager.GetResourceFromAppStyle(mainWindow, "BlackBrush"));
						break;
					}
					case MetroDialogColorScheme.Accented:
					{
						ThemeManager.ChangeAppStyle(base.Resources, item2, item1);
						DependencyProperty dependencyProperty = Control.BackgroundProperty;
						Window window = this.OwningWindow;
						if (window == null)
						{
							window = Application.Current.MainWindow;
						}
						base.SetValue(dependencyProperty, ThemeManager.GetResourceFromAppStyle(window, "HighlightBrush"));
						DependencyProperty foregroundProperty1 = Control.ForegroundProperty;
						Window owningWindow1 = this.OwningWindow;
						if (owningWindow1 == null)
						{
							owningWindow1 = Application.Current.MainWindow;
						}
						base.SetValue(foregroundProperty1, ThemeManager.GetResourceFromAppStyle(owningWindow1, "IdealForegroundColorBrush"));
						break;
					}
					case MetroDialogColorScheme.Inverted:
					{
						AppTheme inverseAppTheme = ThemeManager.GetInverseAppTheme(item1);
						if (inverseAppTheme == null)
						{
							throw new InvalidOperationException("The inverse dialog theme only works if the window theme abides the naming convention. See ThemeManager.GetInverseAppTheme for more infos");
						}
						ThemeManager.ChangeAppStyle(base.Resources, item2, inverseAppTheme);
						DependencyProperty backgroundProperty1 = Control.BackgroundProperty;
						Window mainWindow1 = this.OwningWindow;
						if (mainWindow1 == null)
						{
							mainWindow1 = Application.Current.MainWindow;
						}
						base.SetValue(backgroundProperty1, ThemeManager.GetResourceFromAppStyle(mainWindow1, "BlackColorBrush"));
						DependencyProperty dependencyProperty1 = Control.ForegroundProperty;
						Window window1 = this.OwningWindow;
						if (window1 == null)
						{
							window1 = Application.Current.MainWindow;
						}
						base.SetValue(dependencyProperty1, ThemeManager.GetResourceFromAppStyle(window1, "WhiteColorBrush"));
						break;
					}
				}
			}
			if (this.ParentDialogWindow != null)
			{
				this.ParentDialogWindow.SetValue(Control.BackgroundProperty, base.Background);
				Window owningWindow2 = this.OwningWindow;
				if (owningWindow2 == null)
				{
					owningWindow2 = Application.Current.MainWindow;
				}
				object resourceFromAppStyle = ThemeManager.GetResourceFromAppStyle(owningWindow2, "AccentColorBrush");
				if (resourceFromAppStyle != null)
				{
					this.ParentDialogWindow.SetValue(MetroWindow.GlowBrushProperty, resourceFromAppStyle);
				}
			}
		}

		private void Initialize()
		{
			if (this.DialogSettings != null && !this.DialogSettings.SuppressDefaultResources)
			{
				base.Resources.MergedDictionaries.Add(new ResourceDictionary()
				{
					Source = new Uri("pack://application:,,,/小喵谷登入器;component/Styles/Controls.xaml")
				});
			}
			base.Resources.MergedDictionaries.Add(new ResourceDictionary()
			{
				Source = new Uri("pack://application:,,,/小喵谷登入器;component/Styles/Fonts.xaml")
			});
			base.Resources.MergedDictionaries.Add(new ResourceDictionary()
			{
				Source = new Uri("pack://application:,,,/小喵谷登入器;component/Styles/Colors.xaml")
			});
			base.Resources.MergedDictionaries.Add(new ResourceDictionary()
			{
				Source = new Uri("pack://application:,,,/小喵谷登入器;component/Themes/Dialogs/BaseMetroDialog.xaml")
			});
			if (this.DialogSettings != null && this.DialogSettings.CustomResourceDictionary != null)
			{
				base.Resources.MergedDictionaries.Add(this.DialogSettings.CustomResourceDictionary);
			}
			base.Loaded += new RoutedEventHandler((object sender, RoutedEventArgs e) => {
				this.OnLoaded();
				this.HandleTheme();
			});
			ThemeManager.IsThemeChanged += new EventHandler<OnThemeChangedEventArgs>(this.ThemeManager_IsThemeChanged);
			base.Unloaded += new RoutedEventHandler(this.BaseMetroDialog_Unloaded);
		}

		protected internal virtual void OnClose()
		{
			if (this.ParentDialogWindow != null)
			{
				this.ParentDialogWindow.Close();
			}
		}

		protected virtual void OnLoaded()
		{
		}

		protected internal virtual bool OnRequestClose()
		{
			return true;
		}

		protected internal virtual void OnShown()
		{
		}

		public Task RequestCloseAsync()
		{
			if (this.OnRequestClose())
			{
				if (this.ParentDialogWindow == null)
				{
					return this.OwningWindow.HideMetroDialogAsync(this, null);
				}
				return this._WaitForCloseAsync().ContinueWith((Task x) => this.ParentDialogWindow.Dispatcher.Invoke(() => this.ParentDialogWindow.Close()));
			}
			return Task.Factory.StartNew(() => {
			});
		}

		private void ThemeManager_IsThemeChanged(object sender, OnThemeChangedEventArgs e)
		{
			this.HandleTheme();
		}

		public Task WaitForLoadAsync()
		{
			base.Dispatcher.VerifyAccess();
			if (base.IsLoaded)
			{
				return new Task(() => {
				});
			}
			if (!this.DialogSettings.AnimateShow)
			{
				base.Opacity = 1;
			}
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			RoutedEventHandler routedEventHandler = null;
			routedEventHandler = (object sender, RoutedEventArgs e) => {
				this.Loaded -= routedEventHandler;
				base.Focus();
				taskCompletionSource.TrySetResult(null);
			};
			base.Loaded += routedEventHandler;
			return taskCompletionSource.Task;
		}

		public Task WaitUntilUnloadedAsync()
		{
			TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
			base.Unloaded += new RoutedEventHandler((object sender, RoutedEventArgs e) => taskCompletionSource.TrySetResult(null));
			return taskCompletionSource.Task;
		}
	}
}