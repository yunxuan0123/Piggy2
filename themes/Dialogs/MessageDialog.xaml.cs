using MahApps.Metro.Controls;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace MahApps.Metro.Controls.Dialogs
{
	public partial class MessageDialog : BaseMetroDialog
	{
		public readonly static DependencyProperty MessageProperty;

		public readonly static DependencyProperty AffirmativeButtonTextProperty;

		public readonly static DependencyProperty NegativeButtonTextProperty;

		public readonly static DependencyProperty FirstAuxiliaryButtonTextProperty;

		public readonly static DependencyProperty SecondAuxiliaryButtonTextProperty;

		public readonly static DependencyProperty ButtonStyleProperty;

		public string AffirmativeButtonText
		{
			get
			{
				return (string)base.GetValue(MessageDialog.AffirmativeButtonTextProperty);
			}
			set
			{
				base.SetValue(MessageDialog.AffirmativeButtonTextProperty, value);
			}
		}

		public MessageDialogStyle ButtonStyle
		{
			get
			{
				return (MessageDialogStyle)base.GetValue(MessageDialog.ButtonStyleProperty);
			}
			set
			{
				base.SetValue(MessageDialog.ButtonStyleProperty, value);
			}
		}

		public string FirstAuxiliaryButtonText
		{
			get
			{
				return (string)base.GetValue(MessageDialog.FirstAuxiliaryButtonTextProperty);
			}
			set
			{
				base.SetValue(MessageDialog.FirstAuxiliaryButtonTextProperty, value);
			}
		}

		public string Message
		{
			get
			{
				return (string)base.GetValue(MessageDialog.MessageProperty);
			}
			set
			{
				base.SetValue(MessageDialog.MessageProperty, value);
			}
		}

		public string NegativeButtonText
		{
			get
			{
				return (string)base.GetValue(MessageDialog.NegativeButtonTextProperty);
			}
			set
			{
				base.SetValue(MessageDialog.NegativeButtonTextProperty, value);
			}
		}

		public string SecondAuxiliaryButtonText
		{
			get
			{
				return (string)base.GetValue(MessageDialog.SecondAuxiliaryButtonTextProperty);
			}
			set
			{
				base.SetValue(MessageDialog.SecondAuxiliaryButtonTextProperty, value);
			}
		}

		static MessageDialog()
		{
			Class6.yDnXvgqzyB5jw();
			MessageDialog.MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(MessageDialog), new PropertyMetadata(null));
			MessageDialog.AffirmativeButtonTextProperty = DependencyProperty.Register("AffirmativeButtonText", typeof(string), typeof(MessageDialog), new PropertyMetadata("OK"));
			MessageDialog.NegativeButtonTextProperty = DependencyProperty.Register("NegativeButtonText", typeof(string), typeof(MessageDialog), new PropertyMetadata("Cancel"));
			MessageDialog.FirstAuxiliaryButtonTextProperty = DependencyProperty.Register("FirstAuxiliaryButtonText", typeof(string), typeof(MessageDialog), new PropertyMetadata("Cancel"));
			MessageDialog.SecondAuxiliaryButtonTextProperty = DependencyProperty.Register("SecondAuxiliaryButtonText", typeof(string), typeof(MessageDialog), new PropertyMetadata("Cancel"));
			MessageDialog.ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(MessageDialogStyle), typeof(MessageDialog), new PropertyMetadata((object)MessageDialogStyle.Affirmative, (DependencyObject s, DependencyPropertyChangedEventArgs e) => MessageDialog.SetButtonState((MessageDialog)s)));
		}

		internal MessageDialog(MetroWindow parentWindow)
		{
			Class6.yDnXvgqzyB5jw();
			this(parentWindow, null);
		}

		internal MessageDialog(MetroWindow parentWindow, MetroDialogSettings settings)
		{
			Class6.yDnXvgqzyB5jw();
			base(parentWindow, settings);
			this.InitializeComponent();
			this.PART_MessageScrollViewer.Height = base.DialogSettings.MaximumBodyHeight;
		}

		private bool IsApplicable(MessageDialogResult value)
		{
			switch (value)
			{
				case MessageDialogResult.Negative:
				{
					return this.PART_NegativeButton.IsVisible;
				}
				case MessageDialogResult.Affirmative:
				{
					return this.PART_AffirmativeButton.IsVisible;
				}
				case MessageDialogResult.FirstAuxiliary:
				{
					return this.PART_FirstAuxiliaryButton.IsVisible;
				}
				case MessageDialogResult.SecondAuxiliary:
				{
					return this.PART_SecondAuxiliaryButton.IsVisible;
				}
				default:
				{
					return false;
				}
			}
		}

		private void OnKeyCopyExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			Clipboard.SetDataObject(this.Message);
		}

		protected override void OnLoaded()
		{
			MessageDialog.SetButtonState(this);
		}

		private static void SetButtonState(MessageDialog md)
		{
			if (md.PART_AffirmativeButton == null)
			{
				return;
			}
			switch (md.ButtonStyle)
			{
				case MessageDialogStyle.Affirmative:
				{
					md.PART_AffirmativeButton.Visibility = System.Windows.Visibility.Visible;
					md.PART_NegativeButton.Visibility = System.Windows.Visibility.Collapsed;
					md.PART_FirstAuxiliaryButton.Visibility = System.Windows.Visibility.Collapsed;
					md.PART_SecondAuxiliaryButton.Visibility = System.Windows.Visibility.Collapsed;
					break;
				}
				case MessageDialogStyle.AffirmativeAndNegative:
				case MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary:
				case MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary:
				{
					md.PART_AffirmativeButton.Visibility = System.Windows.Visibility.Visible;
					md.PART_NegativeButton.Visibility = System.Windows.Visibility.Visible;
					if (md.ButtonStyle == MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary || md.ButtonStyle == MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary)
					{
						md.PART_FirstAuxiliaryButton.Visibility = System.Windows.Visibility.Visible;
					}
					if (md.ButtonStyle != MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary)
					{
						break;
					}
					md.PART_SecondAuxiliaryButton.Visibility = System.Windows.Visibility.Visible;
					break;
				}
			}
			md.AffirmativeButtonText = md.DialogSettings.AffirmativeButtonText;
			md.NegativeButtonText = md.DialogSettings.NegativeButtonText;
			md.FirstAuxiliaryButtonText = md.DialogSettings.FirstAuxiliaryButtonText;
			md.SecondAuxiliaryButtonText = md.DialogSettings.SecondAuxiliaryButtonText;
			if (md.DialogSettings.ColorScheme == MetroDialogColorScheme.Accented)
			{
				md.PART_NegativeButton.Style = md.FindResource("AccentedDialogHighlightedSquareButton") as System.Windows.Style;
				md.PART_FirstAuxiliaryButton.Style = md.FindResource("AccentedDialogHighlightedSquareButton") as System.Windows.Style;
				md.PART_SecondAuxiliaryButton.Style = md.FindResource("AccentedDialogHighlightedSquareButton") as System.Windows.Style;
			}
		}

		internal Task<MessageDialogResult> WaitForButtonPressAsync()
		{
			base.Dispatcher.BeginInvoke(new Action(() => {
				bool flag;
				base.Focus();
				MessageDialogResult defaultButtonFocus = base.DialogSettings.DefaultButtonFocus;
				if (this.IsApplicable(defaultButtonFocus))
				{
					switch (defaultButtonFocus)
					{
						case MessageDialogResult.Negative:
						{
							flag = this.PART_NegativeButton.Focus();
							return;
						}
						case MessageDialogResult.Affirmative:
						{
							break;
						}
						case MessageDialogResult.FirstAuxiliary:
						{
							this.PART_FirstAuxiliaryButton.Focus();
							return;
						}
						case MessageDialogResult.SecondAuxiliary:
						{
							this.PART_SecondAuxiliaryButton.Focus();
							return;
						}
						default:
						{
							return;
						}
					}
				}
				else
				{
					if (this.ButtonStyle != MessageDialogStyle.Affirmative)
					{
						defaultButtonFocus = MessageDialogResult.Negative;
						flag = this.PART_NegativeButton.Focus();
						return;
					}
					defaultButtonFocus = MessageDialogResult.Affirmative;
				}
				this.PART_AffirmativeButton.Focus();
			}), new object[0]);
			TaskCompletionSource<MessageDialogResult> taskCompletionSource = new TaskCompletionSource<MessageDialogResult>();
			RoutedEventHandler routedEventHandler = null;
			KeyEventHandler key = null;
			RoutedEventHandler routedEventHandler1 = null;
			KeyEventHandler keyEventHandler = null;
			RoutedEventHandler routedEventHandler2 = null;
			KeyEventHandler key1 = null;
			RoutedEventHandler routedEventHandler3 = null;
			KeyEventHandler keyEventHandler1 = null;
			KeyEventHandler key2 = null;
			Action pARTNegativeButton = null;
			CancellationToken cancellationToken = base.DialogSettings.CancellationToken;
			CancellationTokenRegistration cancellationTokenRegistration = cancellationToken.Register(() => {
				pARTNegativeButton();
				taskCompletionSource.TrySetResult((this.ButtonStyle == MessageDialogStyle.Affirmative ? MessageDialogResult.Affirmative : MessageDialogResult.Negative));
			});
			pARTNegativeButton = () => {
				this.PART_NegativeButton.Click -= routedEventHandler;
				this.PART_AffirmativeButton.Click -= routedEventHandler1;
				this.PART_FirstAuxiliaryButton.Click -= routedEventHandler2;
				this.PART_SecondAuxiliaryButton.Click -= routedEventHandler3;
				this.PART_NegativeButton.KeyDown -= key;
				this.PART_AffirmativeButton.KeyDown -= keyEventHandler;
				this.PART_FirstAuxiliaryButton.KeyDown -= key1;
				this.PART_SecondAuxiliaryButton.KeyDown -= keyEventHandler1;
				this.KeyDown -= key2;
				cancellationTokenRegistration.Dispose();
			};
			key = (object sender, KeyEventArgs e) => {
				if (e.Key == Key.Return)
				{
					pARTNegativeButton();
					taskCompletionSource.TrySetResult(MessageDialogResult.Negative);
				}
			};
			keyEventHandler = (object sender, KeyEventArgs e) => {
				if (e.Key == Key.Return)
				{
					pARTNegativeButton();
					taskCompletionSource.TrySetResult(MessageDialogResult.Affirmative);
				}
			};
			key1 = (object sender, KeyEventArgs e) => {
				if (e.Key == Key.Return)
				{
					pARTNegativeButton();
					taskCompletionSource.TrySetResult(MessageDialogResult.FirstAuxiliary);
				}
			};
			keyEventHandler1 = (object sender, KeyEventArgs e) => {
				if (e.Key == Key.Return)
				{
					pARTNegativeButton();
					taskCompletionSource.TrySetResult(MessageDialogResult.SecondAuxiliary);
				}
			};
			routedEventHandler = (object sender, RoutedEventArgs e) => {
				pARTNegativeButton();
				taskCompletionSource.TrySetResult(MessageDialogResult.Negative);
				e.Handled = true;
			};
			routedEventHandler1 = (object sender, RoutedEventArgs e) => {
				pARTNegativeButton();
				taskCompletionSource.TrySetResult(MessageDialogResult.Affirmative);
				e.Handled = true;
			};
			routedEventHandler2 = (object sender, RoutedEventArgs e) => {
				pARTNegativeButton();
				taskCompletionSource.TrySetResult(MessageDialogResult.FirstAuxiliary);
				e.Handled = true;
			};
			routedEventHandler3 = (object sender, RoutedEventArgs e) => {
				pARTNegativeButton();
				taskCompletionSource.TrySetResult(MessageDialogResult.SecondAuxiliary);
				e.Handled = true;
			};
			key2 = (object sender, KeyEventArgs e) => {
				if (e.Key != Key.Escape)
				{
					if (e.Key == Key.Return)
					{
						pARTNegativeButton();
						taskCompletionSource.TrySetResult(MessageDialogResult.Affirmative);
					}
					return;
				}
				pARTNegativeButton();
				taskCompletionSource.TrySetResult((this.ButtonStyle == MessageDialogStyle.Affirmative ? MessageDialogResult.Affirmative : MessageDialogResult.Negative));
			};
			this.PART_NegativeButton.KeyDown += key;
			this.PART_AffirmativeButton.KeyDown += keyEventHandler;
			this.PART_FirstAuxiliaryButton.KeyDown += key1;
			this.PART_SecondAuxiliaryButton.KeyDown += keyEventHandler1;
			this.PART_NegativeButton.Click += routedEventHandler;
			this.PART_AffirmativeButton.Click += routedEventHandler1;
			this.PART_FirstAuxiliaryButton.Click += routedEventHandler2;
			this.PART_SecondAuxiliaryButton.Click += routedEventHandler3;
			base.KeyDown += key2;
			return taskCompletionSource.Task;
		}
	}
}