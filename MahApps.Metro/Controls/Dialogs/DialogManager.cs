using MahApps.Metro.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MahApps.Metro.Controls.Dialogs
{
	public static class DialogManager
	{
		private static void AddDialog(this MetroWindow window, BaseMetroDialog dialog)
		{
			UIElement uIElement = window.metroActiveDialogContainer.Children.Cast<UIElement>().SingleOrDefault<UIElement>();
			if (uIElement != null)
			{
				window.metroActiveDialogContainer.Children.Remove(uIElement);
				window.metroInactiveDialogContainer.Children.Add(uIElement);
			}
			window.metroActiveDialogContainer.Children.Add(dialog);
		}

		public static Task<TDialog> GetCurrentDialogAsync<TDialog>(this MetroWindow window)
		where TDialog : BaseMetroDialog
		{
			window.Dispatcher.VerifyAccess();
			TaskCompletionSource<TDialog> taskCompletionSource = new TaskCompletionSource<TDialog>();
			window.Dispatcher.Invoke(() => {
				TDialog tDialog = window.metroActiveDialogContainer.Children.OfType<TDialog>().LastOrDefault<TDialog>();
				taskCompletionSource.TrySetResult(tDialog);
			});
			return taskCompletionSource.Task;
		}

		private static Task HandleOverlayOnHide(MetroDialogSettings settings, MetroWindow window)
		{
			if (window.metroActiveDialogContainer.Children.OfType<BaseMetroDialog>().Any<BaseMetroDialog>())
			{
				TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
				taskCompletionSource.SetResult(null);
				return taskCompletionSource.Task;
			}
			if (settings == null || settings.AnimateHide)
			{
				return window.HideOverlayAsync();
			}
			return Task.Factory.StartNew(() => window.Dispatcher.Invoke(new Action(window.HideOverlay)));
		}

		private static Task HandleOverlayOnShow(MetroDialogSettings settings, MetroWindow window)
		{
			if (window.metroActiveDialogContainer.Children.OfType<BaseMetroDialog>().Any<BaseMetroDialog>())
			{
				TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
				taskCompletionSource.SetResult(null);
				return taskCompletionSource.Task;
			}
			if (settings == null || settings.AnimateShow)
			{
				return window.ShowOverlayAsync();
			}
			return Task.Factory.StartNew(() => window.Dispatcher.Invoke(new Action(window.ShowOverlay)));
		}

		public static Task HideMetroDialogAsync(this MetroWindow window, BaseMetroDialog dialog, MetroDialogSettings settings = null)
		{
			Action action1 = null;
			Func<Task> func2 = null;
			window.Dispatcher.VerifyAccess();
			if (!window.metroActiveDialogContainer.Children.Contains(dialog) && !window.metroInactiveDialogContainer.Children.Contains(dialog))
			{
				throw new InvalidOperationException("The provided dialog is not visible in the specified window.");
			}
			window.SizeChanged -= dialog.SizeChangedHandler;
			dialog.OnClose();
			Task task = window.Dispatcher.Invoke<Task>(new Func<Task>(dialog._WaitForCloseAsync));
			return task.ContinueWith<Task>((Task a) => {
				if (DialogManager.DialogClosed != null)
				{
					Dispatcher dispatcher = window.Dispatcher;
					Action u003cu003e9_1 = action1;
					if (u003cu003e9_1 == null)
					{
						Action dialogClosed = () => DialogManager.DialogClosed(window, new DialogStateChangedEventArgs());
						Action action = dialogClosed;
						action1 = dialogClosed;
						u003cu003e9_1 = action;
					}
					dispatcher.BeginInvoke(u003cu003e9_1, new object[0]);
				}
				Dispatcher dispatcher1 = window.Dispatcher;
				Func<Task> u003cu003e9_2 = func2;
				if (u003cu003e9_2 == null)
				{
					Func<Task> func = () => {
						window.RemoveDialog(dialog);
						return DialogManager.HandleOverlayOnHide(settings, window);
					};
					Func<Task> func1 = func;
					func2 = func;
					u003cu003e9_2 = func1;
				}
				return dispatcher1.Invoke<Task>(u003cu003e9_2);
			}).Unwrap();
		}

		private static void RemoveDialog(this MetroWindow window, BaseMetroDialog dialog)
		{
			if (!window.metroActiveDialogContainer.Children.Contains(dialog))
			{
				window.metroInactiveDialogContainer.Children.Remove(dialog);
			}
			else
			{
				window.metroActiveDialogContainer.Children.Remove(dialog);
				UIElement uIElement = window.metroInactiveDialogContainer.Children.Cast<UIElement>().LastOrDefault<UIElement>();
				if (uIElement != null)
				{
					window.metroInactiveDialogContainer.Children.Remove(uIElement);
					window.metroActiveDialogContainer.Children.Add(uIElement);
					return;
				}
			}
		}

		private static SizeChangedEventHandler SetupAndOpenDialog(MetroWindow window, BaseMetroDialog dialog)
		{
			dialog.SetValue(Panel.ZIndexProperty, (int)window.overlayBox.GetValue(Panel.ZIndexProperty) + 1);
			dialog.MinHeight = window.ActualHeight / 4;
			dialog.MaxHeight = window.ActualHeight;
			SizeChangedEventHandler minHeight = (object sender, SizeChangedEventArgs e) => {
				dialog.MinHeight = window.ActualHeight / 4;
				dialog.MaxHeight = window.ActualHeight;
			};
			window.SizeChanged += minHeight;
			window.AddDialog(dialog);
			dialog.OnShown();
			return minHeight;
		}

		private static Window SetupExternalDialogWindow(BaseMetroDialog dialog)
		{
			MetroWindow metroWindow = new MetroWindow()
			{
				ShowInTaskbar = false,
				ShowActivated = true,
				Topmost = true,
				ResizeMode = ResizeMode.NoResize,
				WindowStyle = WindowStyle.None,
				WindowStartupLocation = WindowStartupLocation.CenterScreen,
				ShowTitleBar = false,
				ShowCloseButton = false,
				WindowTransitionsEnabled = false
			};
			try
			{
				metroWindow.Resources.MergedDictionaries.Add(new ResourceDictionary()
				{
					Source = new Uri("pack://application:,,,/小喵谷登入器;component/Styles/Controls.xaml")
				});
				metroWindow.Resources.MergedDictionaries.Add(new ResourceDictionary()
				{
					Source = new Uri("pack://application:,,,/小喵谷登入器;component/Styles/Fonts.xaml")
				});
				metroWindow.Resources.MergedDictionaries.Add(new ResourceDictionary()
				{
					Source = new Uri("pack://application:,,,/小喵谷登入器;component/Styles/Colors.xaml")
				});
				metroWindow.SetResourceReference(MetroWindow.GlowBrushProperty, "AccentColorBrush");
			}
			catch (Exception exception)
			{
			}
			metroWindow.Width = SystemParameters.PrimaryScreenWidth;
			metroWindow.MinHeight = SystemParameters.PrimaryScreenHeight / 4;
			metroWindow.SizeToContent = SizeToContent.Height;
			dialog.ParentDialogWindow = metroWindow;
			metroWindow.Content = dialog;
			EventHandler parentDialogWindow = null;
			parentDialogWindow = (object sender, EventArgs e) => {
				metroWindow.Closed -= parentDialogWindow;
				dialog.ParentDialogWindow = null;
				metroWindow.Content = null;
			};
			metroWindow.Closed += parentDialogWindow;
			return metroWindow;
		}

		public static BaseMetroDialog ShowDialogExternally(this BaseMetroDialog dialog)
		{
			Window window = DialogManager.SetupExternalDialogWindow(dialog);
			dialog.OnShown();
			window.Show();
			return dialog;
		}

		public static Task<string> ShowInputAsync(this MetroWindow window, string title, string message, MetroDialogSettings settings = null)
		{
			Action action2 = null;
			Func<Task<string>> func7 = null;
			window.Dispatcher.VerifyAccess();
			return DialogManager.HandleOverlayOnShow(settings, window).ContinueWith<Task<string>>((Task z) => {
				Dispatcher dispatcher1 = window.Dispatcher;
				Func<Task<string>> u003cu003e9_1 = func7;
				if (u003cu003e9_1 == null)
				{
					Func<Task<string>> func3 = () => {
						Func<Task<string>, Task<Task<string>>> func2 = null;
						if (settings == null)
						{
							settings = window.MetroDialogOptions;
						}
						InputDialog inputDialog = new InputDialog(window, settings)
						{
							Title = title,
							Message = message,
							Input = settings.DefaultText,
							SizeChangedHandler = DialogManager.SetupAndOpenDialog(window, inputDialog)
						};
						return inputDialog.WaitForLoadAsync().ContinueWith<Task<Task<string>>>((Task x) => {
							if (DialogManager.DialogOpened != null)
							{
								Dispatcher dispatcher = window.Dispatcher;
								Action u003cu003e9_3 = action2;
								if (u003cu003e9_3 == null)
								{
									Action action = () => DialogManager.DialogOpened(window, new DialogStateChangedEventArgs());
									Action action1 = action;
									action2 = action;
									u003cu003e9_3 = action1;
								}
								dispatcher.BeginInvoke(u003cu003e9_3, new object[0]);
							}
							Task<string> task = inputDialog.WaitForButtonPressAsync();
							Func<Task<string>, Task<Task<string>>> u003cu003e9_4 = func2;
							if (u003cu003e9_4 == null)
							{
								Func<Task<string>, Task<Task<string>>> func = (Task<string> y) => {
									Func<Task, Task<string>> func4 = null;
									this.dialog.OnClose();
									if (DialogManager.DialogClosed != null)
									{
										Dispatcher dispatcher1 = this.CS$<>8__locals1.window.Dispatcher;
										Action u003cu003e9_5 = this.CS$<>8__locals1.<>9__5;
										if (u003cu003e9_5 == null)
										{
											DialogManager.<>c__DisplayClass1_0 cSu0024u003cu003e8_locals1 = this.CS$<>8__locals1;
											Action action = new Action(this.CS$<>8__locals1.<ShowInputAsync>b__5);
											Action action1 = action;
											cSu0024u003cu003e8_locals1.<>9__5 = action;
											u003cu003e9_5 = action1;
										}
										dispatcher1.BeginInvoke(u003cu003e9_5, new object[0]);
									}
									Dispatcher dispatcher2 = this.CS$<>8__locals1.window.Dispatcher;
									Func<Task> u003cu003e9_6 = this.<>9__6;
									if (u003cu003e9_6 == null)
									{
										Func<Task> func5 = () => this.dialog._WaitForCloseAsync();
										Func<Task> func6 = func5;
										this.<>9__6 = func5;
										u003cu003e9_6 = func6;
									}
									return dispatcher2.Invoke<Task>(u003cu003e9_6).ContinueWith<Task<string>>((Task a) => {
										Dispatcher dispatcher = this.CS$<>8__locals1.window.Dispatcher;
										Func<Task> u003cu003e9_8 = this.<>9__8;
										if (u003cu003e9_8 == null)
										{
											DialogManager.<>c__DisplayClass1_1 cSu0024u003cu003e8_locals2 = this;
											Func<Task> func = () => {
												this.CS$<>8__locals1.window.SizeChanged -= this.sizeHandler;
												this.CS$<>8__locals1.window.RemoveDialog(this.dialog);
												return DialogManager.HandleOverlayOnHide(this.CS$<>8__locals1.settings, this.CS$<>8__locals1.window);
											};
											Func<Task> func1 = func;
											cSu0024u003cu003e8_locals2.<>9__8 = func;
											u003cu003e9_8 = func1;
										}
										Task task = dispatcher.Invoke<Task>(u003cu003e9_8);
										Func<Task, Task<string>> u003cu003e9_9 = func4;
										if (u003cu003e9_9 == null)
										{
											Func<Task, Task<string>> func2 = new Func<Task, Task<string>>(this.<ShowInputAsync>b__9);
											Func<Task, Task<string>> func3 = func2;
											func4 = func2;
											u003cu003e9_9 = func3;
										}
										return task.ContinueWith<Task<string>>(u003cu003e9_9).Unwrap<string>();
									});
								};
								Func<Task<string>, Task<Task<string>>> func1 = func;
								func2 = func;
								u003cu003e9_4 = func1;
							}
							return task.ContinueWith<Task<Task<string>>>(u003cu003e9_4).Unwrap<Task<string>>();
						}).Unwrap<Task<string>>().Unwrap<string>();
					};
					Func<Task<string>> func4 = func3;
					func7 = func3;
					u003cu003e9_1 = func4;
				}
				return dispatcher1.Invoke<Task<string>>(u003cu003e9_1);
			}).Unwrap<string>();
		}

		public static Task<LoginDialogData> ShowLoginAsync(this MetroWindow window, string title, string message, LoginDialogSettings settings = null)
		{
			Action action2 = null;
			Func<Task<LoginDialogData>> func7 = null;
			window.Dispatcher.VerifyAccess();
			return DialogManager.HandleOverlayOnShow(settings, window).ContinueWith<Task<LoginDialogData>>((Task z) => {
				Dispatcher dispatcher1 = window.Dispatcher;
				Func<Task<LoginDialogData>> u003cu003e9_1 = func7;
				if (u003cu003e9_1 == null)
				{
					Func<Task<LoginDialogData>> func3 = () => {
						Func<Task<LoginDialogData>, Task<Task<LoginDialogData>>> func2 = null;
						if (settings == null)
						{
							settings = new LoginDialogSettings();
						}
						LoginDialog loginDialog = new LoginDialog(window, settings)
						{
							Title = title,
							Message = message,
							SizeChangedHandler = DialogManager.SetupAndOpenDialog(window, loginDialog)
						};
						return loginDialog.WaitForLoadAsync().ContinueWith<Task<Task<LoginDialogData>>>((Task x) => {
							if (DialogManager.DialogOpened != null)
							{
								Dispatcher dispatcher = window.Dispatcher;
								Action u003cu003e9_3 = action2;
								if (u003cu003e9_3 == null)
								{
									Action action = () => DialogManager.DialogOpened(window, new DialogStateChangedEventArgs());
									Action action1 = action;
									action2 = action;
									u003cu003e9_3 = action1;
								}
								dispatcher.BeginInvoke(u003cu003e9_3, new object[0]);
							}
							Task<LoginDialogData> task = loginDialog.WaitForButtonPressAsync();
							Func<Task<LoginDialogData>, Task<Task<LoginDialogData>>> u003cu003e9_4 = func2;
							if (u003cu003e9_4 == null)
							{
								Func<Task<LoginDialogData>, Task<Task<LoginDialogData>>> func = (Task<LoginDialogData> y) => {
									Func<Task, Task<LoginDialogData>> func4 = null;
									this.dialog.OnClose();
									if (DialogManager.DialogClosed != null)
									{
										Dispatcher dispatcher1 = this.CS$<>8__locals1.window.Dispatcher;
										Action u003cu003e9_5 = this.CS$<>8__locals1.<>9__5;
										if (u003cu003e9_5 == null)
										{
											DialogManager.<>c__DisplayClass0_0 cSu0024u003cu003e8_locals1 = this.CS$<>8__locals1;
											Action action = new Action(this.CS$<>8__locals1.<ShowLoginAsync>b__5);
											Action action1 = action;
											cSu0024u003cu003e8_locals1.<>9__5 = action;
											u003cu003e9_5 = action1;
										}
										dispatcher1.BeginInvoke(u003cu003e9_5, new object[0]);
									}
									Dispatcher dispatcher2 = this.CS$<>8__locals1.window.Dispatcher;
									Func<Task> u003cu003e9_6 = this.<>9__6;
									if (u003cu003e9_6 == null)
									{
										Func<Task> func5 = () => this.dialog._WaitForCloseAsync();
										Func<Task> func6 = func5;
										this.<>9__6 = func5;
										u003cu003e9_6 = func6;
									}
									return dispatcher2.Invoke<Task>(u003cu003e9_6).ContinueWith<Task<LoginDialogData>>((Task a) => {
										Dispatcher dispatcher = this.CS$<>8__locals1.window.Dispatcher;
										Func<Task> u003cu003e9_8 = this.<>9__8;
										if (u003cu003e9_8 == null)
										{
											DialogManager.<>c__DisplayClass0_1 cSu0024u003cu003e8_locals2 = this;
											Func<Task> func = () => {
												this.CS$<>8__locals1.window.SizeChanged -= this.sizeHandler;
												this.CS$<>8__locals1.window.RemoveDialog(this.dialog);
												return DialogManager.HandleOverlayOnHide(this.CS$<>8__locals1.settings, this.CS$<>8__locals1.window);
											};
											Func<Task> func1 = func;
											cSu0024u003cu003e8_locals2.<>9__8 = func;
											u003cu003e9_8 = func1;
										}
										Task task = dispatcher.Invoke<Task>(u003cu003e9_8);
										Func<Task, Task<LoginDialogData>> u003cu003e9_9 = func4;
										if (u003cu003e9_9 == null)
										{
											Func<Task, Task<LoginDialogData>> func2 = new Func<Task, Task<LoginDialogData>>(this.<ShowLoginAsync>b__9);
											Func<Task, Task<LoginDialogData>> func3 = func2;
											func4 = func2;
											u003cu003e9_9 = func3;
										}
										return task.ContinueWith<Task<LoginDialogData>>(u003cu003e9_9).Unwrap<LoginDialogData>();
									});
								};
								Func<Task<LoginDialogData>, Task<Task<LoginDialogData>>> func1 = func;
								func2 = func;
								u003cu003e9_4 = func1;
							}
							return task.ContinueWith<Task<Task<LoginDialogData>>>(u003cu003e9_4).Unwrap<Task<LoginDialogData>>();
						}).Unwrap<Task<LoginDialogData>>().Unwrap<LoginDialogData>();
					};
					Func<Task<LoginDialogData>> func4 = func3;
					func7 = func3;
					u003cu003e9_1 = func4;
				}
				return dispatcher1.Invoke<Task<LoginDialogData>>(u003cu003e9_1);
			}).Unwrap<LoginDialogData>();
		}

		public static Task<MessageDialogResult> ShowMessageAsync(this MetroWindow window, string title, string message, MessageDialogStyle style = 0, MetroDialogSettings settings = null)
		{
			Action action2 = null;
			Func<Task<MessageDialogResult>> func7 = null;
			window.Dispatcher.VerifyAccess();
			return DialogManager.HandleOverlayOnShow(settings, window).ContinueWith<Task<MessageDialogResult>>((Task z) => {
				Dispatcher dispatcher1 = window.Dispatcher;
				Func<Task<MessageDialogResult>> u003cu003e9_1 = func7;
				if (u003cu003e9_1 == null)
				{
					Func<Task<MessageDialogResult>> func3 = () => {
						Func<Task<MessageDialogResult>, Task<Task<MessageDialogResult>>> func2 = null;
						if (settings == null)
						{
							settings = window.MetroDialogOptions;
						}
						MessageDialog messageDialog = new MessageDialog(window, settings)
						{
							Message = message,
							Title = title,
							ButtonStyle = style,
							SizeChangedHandler = DialogManager.SetupAndOpenDialog(window, messageDialog)
						};
						return messageDialog.WaitForLoadAsync().ContinueWith<Task<Task<MessageDialogResult>>>((Task x) => {
							if (DialogManager.DialogOpened != null)
							{
								Dispatcher dispatcher = window.Dispatcher;
								Action u003cu003e9_3 = action2;
								if (u003cu003e9_3 == null)
								{
									Action action = () => DialogManager.DialogOpened(window, new DialogStateChangedEventArgs());
									Action action1 = action;
									action2 = action;
									u003cu003e9_3 = action1;
								}
								dispatcher.BeginInvoke(u003cu003e9_3, new object[0]);
							}
							Task<MessageDialogResult> task = messageDialog.WaitForButtonPressAsync();
							Func<Task<MessageDialogResult>, Task<Task<MessageDialogResult>>> u003cu003e9_4 = func2;
							if (u003cu003e9_4 == null)
							{
								Func<Task<MessageDialogResult>, Task<Task<MessageDialogResult>>> func = (Task<MessageDialogResult> y) => {
									Func<Task, Task<MessageDialogResult>> func4 = null;
									this.dialog.OnClose();
									if (DialogManager.DialogClosed != null)
									{
										Dispatcher dispatcher1 = this.CS$<>8__locals1.window.Dispatcher;
										Action u003cu003e9_5 = this.CS$<>8__locals1.<>9__5;
										if (u003cu003e9_5 == null)
										{
											DialogManager.<>c__DisplayClass2_0 cSu0024u003cu003e8_locals1 = this.CS$<>8__locals1;
											Action action = new Action(this.CS$<>8__locals1.<ShowMessageAsync>b__5);
											Action action1 = action;
											cSu0024u003cu003e8_locals1.<>9__5 = action;
											u003cu003e9_5 = action1;
										}
										dispatcher1.BeginInvoke(u003cu003e9_5, new object[0]);
									}
									Dispatcher dispatcher2 = this.CS$<>8__locals1.window.Dispatcher;
									Func<Task> u003cu003e9_6 = this.<>9__6;
									if (u003cu003e9_6 == null)
									{
										Func<Task> func5 = () => this.dialog._WaitForCloseAsync();
										Func<Task> func6 = func5;
										this.<>9__6 = func5;
										u003cu003e9_6 = func6;
									}
									return dispatcher2.Invoke<Task>(u003cu003e9_6).ContinueWith<Task<MessageDialogResult>>((Task a) => {
										Dispatcher dispatcher = this.CS$<>8__locals1.window.Dispatcher;
										Func<Task> u003cu003e9_8 = this.<>9__8;
										if (u003cu003e9_8 == null)
										{
											DialogManager.<>c__DisplayClass2_1 cSu0024u003cu003e8_locals2 = this;
											Func<Task> func = () => {
												this.CS$<>8__locals1.window.SizeChanged -= this.sizeHandler;
												this.CS$<>8__locals1.window.RemoveDialog(this.dialog);
												return DialogManager.HandleOverlayOnHide(this.CS$<>8__locals1.settings, this.CS$<>8__locals1.window);
											};
											Func<Task> func1 = func;
											cSu0024u003cu003e8_locals2.<>9__8 = func;
											u003cu003e9_8 = func1;
										}
										Task task = dispatcher.Invoke<Task>(u003cu003e9_8);
										Func<Task, Task<MessageDialogResult>> u003cu003e9_9 = func4;
										if (u003cu003e9_9 == null)
										{
											Func<Task, Task<MessageDialogResult>> func2 = new Func<Task, Task<MessageDialogResult>>(this.<ShowMessageAsync>b__9);
											Func<Task, Task<MessageDialogResult>> func3 = func2;
											func4 = func2;
											u003cu003e9_9 = func3;
										}
										return task.ContinueWith<Task<MessageDialogResult>>(u003cu003e9_9).Unwrap<MessageDialogResult>();
									});
								};
								Func<Task<MessageDialogResult>, Task<Task<MessageDialogResult>>> func1 = func;
								func2 = func;
								u003cu003e9_4 = func1;
							}
							return task.ContinueWith<Task<Task<MessageDialogResult>>>(u003cu003e9_4).Unwrap<Task<MessageDialogResult>>();
						}).Unwrap<Task<MessageDialogResult>>().Unwrap<MessageDialogResult>();
					};
					Func<Task<MessageDialogResult>> func4 = func3;
					func7 = func3;
					u003cu003e9_1 = func4;
				}
				return dispatcher1.Invoke<Task<MessageDialogResult>>(u003cu003e9_1);
			}).Unwrap<MessageDialogResult>();
		}

		public static Task ShowMetroDialogAsync(this MetroWindow window, BaseMetroDialog dialog, MetroDialogSettings settings = null)
		{
			Action action2 = null;
			Action<Task> action3 = null;
			Func<Task> func2 = null;
			window.Dispatcher.VerifyAccess();
			if (window.metroActiveDialogContainer.Children.Contains(dialog) || window.metroInactiveDialogContainer.Children.Contains(dialog))
			{
				throw new InvalidOperationException("The provided dialog is already visible in the specified window.");
			}
			return DialogManager.HandleOverlayOnShow(settings, window).ContinueWith((Task z) => {
				Dispatcher dispatcher = dialog.Dispatcher;
				Action u003cu003e9_1 = action2;
				if (u003cu003e9_1 == null)
				{
					Action action = () => {
						SizeChangedEventHandler sizeChangedEventHandler = DialogManager.SetupAndOpenDialog(window, dialog);
						dialog.SizeChangedHandler = sizeChangedEventHandler;
					};
					Action action1 = action;
					action2 = action;
					u003cu003e9_1 = action1;
				}
				dispatcher.Invoke(u003cu003e9_1);
			}).ContinueWith<Task>((Task y) => {
				Dispatcher dispatcher = dialog.Dispatcher;
				Func<Task> u003cu003e9_3 = func2;
				if (u003cu003e9_3 == null)
				{
					Func<Task> func = () => {
						Task task = dialog.WaitForLoadAsync();
						Action<Task> u003cu003e9_4 = action3;
						if (u003cu003e9_4 == null)
						{
							Action<Task> action = (Task x) => {
								dialog.OnShown();
								if (DialogManager.DialogOpened != null)
								{
									DialogManager.DialogOpened(window, new DialogStateChangedEventArgs());
								}
							};
							Action<Task> action1 = action;
							action3 = action;
							u003cu003e9_4 = action1;
						}
						return task.ContinueWith(u003cu003e9_4);
					};
					Func<Task> func1 = func;
					func2 = func;
					u003cu003e9_3 = func1;
				}
				return dispatcher.Invoke<Task>(u003cu003e9_3);
			});
		}

		public static BaseMetroDialog ShowModalDialogExternally(this BaseMetroDialog dialog)
		{
			Window window = DialogManager.SetupExternalDialogWindow(dialog);
			dialog.OnShown();
			window.ShowDialog();
			return dialog;
		}

		public static Task<ProgressDialogController> ShowProgressAsync(this MetroWindow window, string title, string message, bool isCancelable = false, MetroDialogSettings settings = null)
		{
			Action action2 = null;
			Func<Task<ProgressDialogController>> func5 = null;
			window.Dispatcher.VerifyAccess();
			return DialogManager.HandleOverlayOnShow(settings, window).ContinueWith<Task<ProgressDialogController>>((Task z) => {
				Dispatcher dispatcher1 = window.Dispatcher;
				Func<Task<ProgressDialogController>> u003cu003e9_1 = func5;
				if (u003cu003e9_1 == null)
				{
					Func<Task<ProgressDialogController>> func3 = () => {
						Func<Task> func2 = null;
						ProgressDialog negativeButtonText = new ProgressDialog(window)
						{
							Message = message,
							Title = title,
							IsCancelable = isCancelable
						};
						if (settings == null)
						{
							settings = window.MetroDialogOptions;
						}
						negativeButtonText.NegativeButtonText = settings.NegativeButtonText;
						negativeButtonText.SizeChangedHandler = DialogManager.SetupAndOpenDialog(window, negativeButtonText);
						return negativeButtonText.WaitForLoadAsync().ContinueWith<ProgressDialogController>((Task x) => {
							if (DialogManager.DialogOpened != null)
							{
								Dispatcher dispatcher = window.Dispatcher;
								Action u003cu003e9_3 = action2;
								if (u003cu003e9_3 == null)
								{
									Action action = () => DialogManager.DialogOpened(window, new DialogStateChangedEventArgs());
									Action action1 = action;
									action2 = action;
									u003cu003e9_3 = action1;
								}
								dispatcher.BeginInvoke(u003cu003e9_3, new object[0]);
							}
							ProgressDialog progressDialog = negativeButtonText;
							Func<Task> u003cu003e9_4 = func2;
							if (u003cu003e9_4 == null)
							{
								Func<Task> func = () => {
									this.dialog.OnClose();
									if (DialogManager.DialogClosed != null)
									{
										Dispatcher dispatcher1 = this.CS$<>8__locals1.window.Dispatcher;
										Action u003cu003e9_5 = this.CS$<>8__locals1.<>9__5;
										if (u003cu003e9_5 == null)
										{
											DialogManager.<>c__DisplayClass3_0 variable = this.CS$<>8__locals1;
											Action action = new Action(this.CS$<>8__locals1.<ShowProgressAsync>b__5);
											Action action1 = action;
											variable.<>9__5 = action;
											u003cu003e9_5 = action1;
										}
										dispatcher1.BeginInvoke(u003cu003e9_5, new object[0]);
									}
									Dispatcher dispatcher2 = this.CS$<>8__locals1.window.Dispatcher;
									Func<Task> u003cu003e9_6 = this.<>9__6;
									if (u003cu003e9_6 == null)
									{
										Func<Task> func1 = () => this.dialog._WaitForCloseAsync();
										Func<Task> func2 = func1;
										this.<>9__6 = func1;
										u003cu003e9_6 = func2;
									}
									Task task = dispatcher2.Invoke<Task>(u003cu003e9_6);
									Func<Task, Task> u003cu003e9_7 = this.<>9__7;
									if (u003cu003e9_7 == null)
									{
										Func<Task, Task> func3 = (Task a) => {
											Dispatcher dispatcher = this.CS$<>8__locals1.window.Dispatcher;
											Func<Task> u003cu003e9_8 = this.<>9__8;
											if (u003cu003e9_8 == null)
											{
												Func<Task> cSu0024u003cu003e8_locals1 = () => {
													this.CS$<>8__locals1.window.SizeChanged -= this.sizeHandler;
													this.CS$<>8__locals1.window.RemoveDialog(this.dialog);
													return DialogManager.HandleOverlayOnHide(this.CS$<>8__locals1.settings, this.CS$<>8__locals1.window);
												};
												Func<Task> func = cSu0024u003cu003e8_locals1;
												this.<>9__8 = cSu0024u003cu003e8_locals1;
												u003cu003e9_8 = func;
											}
											return dispatcher.Invoke<Task>(u003cu003e9_8);
										};
										Func<Task, Task> func4 = func3;
										this.<>9__7 = func3;
										u003cu003e9_7 = func4;
									}
									return task.ContinueWith<Task>(u003cu003e9_7).Unwrap();
								};
								Func<Task> func1 = func;
								func2 = func;
								u003cu003e9_4 = func1;
							}
							return new ProgressDialogController(progressDialog, u003cu003e9_4);
						});
					};
					Func<Task<ProgressDialogController>> func4 = func3;
					func5 = func3;
					u003cu003e9_1 = func4;
				}
				return dispatcher1.Invoke<Task<ProgressDialogController>>(u003cu003e9_1);
			}).Unwrap<ProgressDialogController>();
		}

		public static event EventHandler<DialogStateChangedEventArgs> DialogClosed;

		public static event EventHandler<DialogStateChangedEventArgs> DialogOpened;
	}
}