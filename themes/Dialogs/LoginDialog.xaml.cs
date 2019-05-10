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
	public partial class LoginDialog : BaseMetroDialog
	{
		public readonly static DependencyProperty MessageProperty;

		public readonly static DependencyProperty UsernameProperty;

		public readonly static DependencyProperty UsernameWatermarkProperty;

		public readonly static DependencyProperty PasswordProperty;

		public readonly static DependencyProperty PasswordWatermarkProperty;

		public readonly static DependencyProperty AffirmativeButtonTextProperty;

		public readonly static DependencyProperty NegativeButtonTextProperty;

		public readonly static DependencyProperty NegativeButtonButtonVisibilityProperty;

		public readonly static DependencyProperty ShouldHideUsernameProperty;

		public string AffirmativeButtonText
		{
			get
			{
				return (string)base.GetValue(LoginDialog.AffirmativeButtonTextProperty);
			}
			set
			{
				base.SetValue(LoginDialog.AffirmativeButtonTextProperty, value);
			}
		}

		public string Message
		{
			get
			{
				return (string)base.GetValue(LoginDialog.MessageProperty);
			}
			set
			{
				base.SetValue(LoginDialog.MessageProperty, value);
			}
		}

		public System.Windows.Visibility NegativeButtonButtonVisibility
		{
			get
			{
				return (System.Windows.Visibility)base.GetValue(LoginDialog.NegativeButtonButtonVisibilityProperty);
			}
			set
			{
				base.SetValue(LoginDialog.NegativeButtonButtonVisibilityProperty, value);
			}
		}

		public string NegativeButtonText
		{
			get
			{
				return (string)base.GetValue(LoginDialog.NegativeButtonTextProperty);
			}
			set
			{
				base.SetValue(LoginDialog.NegativeButtonTextProperty, value);
			}
		}

		public string Password
		{
			get
			{
				return (string)base.GetValue(LoginDialog.PasswordProperty);
			}
			set
			{
				base.SetValue(LoginDialog.PasswordProperty, value);
			}
		}

		public string PasswordWatermark
		{
			get
			{
				return (string)base.GetValue(LoginDialog.PasswordWatermarkProperty);
			}
			set
			{
				base.SetValue(LoginDialog.PasswordWatermarkProperty, value);
			}
		}

		public bool ShouldHideUsername
		{
			get
			{
				return (bool)base.GetValue(LoginDialog.ShouldHideUsernameProperty);
			}
			set
			{
				base.SetValue(LoginDialog.ShouldHideUsernameProperty, value);
			}
		}

		public string Username
		{
			get
			{
				return (string)base.GetValue(LoginDialog.UsernameProperty);
			}
			set
			{
				base.SetValue(LoginDialog.UsernameProperty, value);
			}
		}

		public string UsernameWatermark
		{
			get
			{
				return (string)base.GetValue(LoginDialog.UsernameWatermarkProperty);
			}
			set
			{
				base.SetValue(LoginDialog.UsernameWatermarkProperty, value);
			}
		}

		static LoginDialog()
		{
			Class6.yDnXvgqzyB5jw();
			LoginDialog.MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(LoginDialog), new PropertyMetadata(null));
			LoginDialog.UsernameProperty = DependencyProperty.Register("Username", typeof(string), typeof(LoginDialog), new PropertyMetadata(null));
			LoginDialog.UsernameWatermarkProperty = DependencyProperty.Register("UsernameWatermark", typeof(string), typeof(LoginDialog), new PropertyMetadata(null));
			LoginDialog.PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(LoginDialog), new PropertyMetadata(null));
			LoginDialog.PasswordWatermarkProperty = DependencyProperty.Register("PasswordWatermark", typeof(string), typeof(LoginDialog), new PropertyMetadata(null));
			LoginDialog.AffirmativeButtonTextProperty = DependencyProperty.Register("AffirmativeButtonText", typeof(string), typeof(LoginDialog), new PropertyMetadata("OK"));
			LoginDialog.NegativeButtonTextProperty = DependencyProperty.Register("NegativeButtonText", typeof(string), typeof(LoginDialog), new PropertyMetadata("Cancel"));
			LoginDialog.NegativeButtonButtonVisibilityProperty = DependencyProperty.Register("NegativeButtonButtonVisibility", typeof(System.Windows.Visibility), typeof(LoginDialog), new PropertyMetadata((object)System.Windows.Visibility.Collapsed));
			LoginDialog.ShouldHideUsernameProperty = DependencyProperty.Register("ShouldHideUsername", typeof(bool), typeof(LoginDialog), new PropertyMetadata(false));
		}

		internal LoginDialog(MetroWindow parentWindow)
		{
			Class6.yDnXvgqzyB5jw();
			this(parentWindow, null);
		}

		internal LoginDialog(MetroWindow parentWindow, LoginDialogSettings settings)
		{
			Class6.yDnXvgqzyB5jw();
			base(parentWindow, settings);
			this.InitializeComponent();
			this.Username = settings.InitialUsername;
			this.Password = settings.InitialPassword;
			this.UsernameWatermark = settings.UsernameWatermark;
			this.PasswordWatermark = settings.PasswordWatermark;
			this.NegativeButtonButtonVisibility = settings.NegativeButtonVisibility;
			this.ShouldHideUsername = settings.ShouldHideUsername;
		}

		protected override void OnLoaded()
		{
			LoginDialogSettings dialogSettings = base.DialogSettings as LoginDialogSettings;
			if (dialogSettings != null && dialogSettings.EnablePasswordPreview)
			{
				System.Windows.Style style = base.FindResource("Win8MetroPasswordBox") as System.Windows.Style;
				if (style != null)
				{
					this.PART_TextBox2.Style = style;
				}
			}
			this.AffirmativeButtonText = base.DialogSettings.AffirmativeButtonText;
			this.NegativeButtonText = base.DialogSettings.NegativeButtonText;
			if (base.DialogSettings.ColorScheme == MetroDialogColorScheme.Accented)
			{
				this.PART_NegativeButton.Style = base.FindResource("AccentedDialogHighlightedSquareButton") as System.Windows.Style;
				this.PART_TextBox.SetResourceReference(Control.ForegroundProperty, "BlackColorBrush");
				this.PART_TextBox2.SetResourceReference(Control.ForegroundProperty, "BlackColorBrush");
			}
		}

		internal Task<LoginDialogData> WaitForButtonPressAsync()
		{
			base.Dispatcher.BeginInvoke(new Action(() => {
				base.Focus();
				if (string.IsNullOrEmpty(this.PART_TextBox.Text) && !this.ShouldHideUsername)
				{
					this.PART_TextBox.Focus();
					return;
				}
				this.PART_TextBox2.Focus();
			}), new object[0]);
			TaskCompletionSource<LoginDialogData> taskCompletionSource = new TaskCompletionSource<LoginDialogData>();
			RoutedEventHandler routedEventHandler = null;
			KeyEventHandler key = null;
			RoutedEventHandler routedEventHandler1 = null;
			KeyEventHandler keyEventHandler = null;
			KeyEventHandler key1 = null;
			Action pARTTextBox = null;
			CancellationToken cancellationToken = base.DialogSettings.CancellationToken;
			CancellationTokenRegistration cancellationTokenRegistration = cancellationToken.Register(() => {
				pARTTextBox();
				taskCompletionSource.TrySetResult(null);
			});
			pARTTextBox = () => {
				this.PART_TextBox.KeyDown -= keyEventHandler;
				this.PART_TextBox2.KeyDown -= keyEventHandler;
				this.KeyDown -= key1;
				this.PART_NegativeButton.Click -= routedEventHandler;
				this.PART_AffirmativeButton.Click -= routedEventHandler1;
				this.PART_NegativeButton.KeyDown -= key;
				this.PART_AffirmativeButton.KeyDown -= keyEventHandler;
				cancellationTokenRegistration.Dispose();
			};
			key1 = (object sender, KeyEventArgs e) => {
				if (e.Key == Key.Escape)
				{
					pARTTextBox();
					taskCompletionSource.TrySetResult(null);
				}
			};
			key = (object sender, KeyEventArgs e) => {
				if (e.Key == Key.Return)
				{
					pARTTextBox();
					taskCompletionSource.TrySetResult(null);
				}
			};
			keyEventHandler = (object sender, KeyEventArgs e) => {
				if (e.Key == Key.Return)
				{
					pARTTextBox();
					taskCompletionSource.TrySetResult(new LoginDialogData()
					{
						Username = this.Username,
						Password = this.PART_TextBox2.Password
					});
				}
			};
			routedEventHandler = (object sender, RoutedEventArgs e) => {
				pARTTextBox();
				taskCompletionSource.TrySetResult(null);
				e.Handled = true;
			};
			routedEventHandler1 = (object sender, RoutedEventArgs e) => {
				pARTTextBox();
				taskCompletionSource.TrySetResult(new LoginDialogData()
				{
					Username = this.Username,
					Password = this.PART_TextBox2.Password
				});
				e.Handled = true;
			};
			this.PART_NegativeButton.KeyDown += key;
			this.PART_AffirmativeButton.KeyDown += keyEventHandler;
			this.PART_TextBox.KeyDown += keyEventHandler;
			this.PART_TextBox2.KeyDown += keyEventHandler;
			base.KeyDown += key1;
			this.PART_NegativeButton.Click += routedEventHandler;
			this.PART_AffirmativeButton.Click += routedEventHandler1;
			return taskCompletionSource.Task;
		}
	}
}