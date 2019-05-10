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
	public partial class InputDialog : BaseMetroDialog
	{
		public readonly static DependencyProperty MessageProperty;

		public readonly static DependencyProperty InputProperty;

		public readonly static DependencyProperty AffirmativeButtonTextProperty;

		public readonly static DependencyProperty NegativeButtonTextProperty;

		public string AffirmativeButtonText
		{
			get
			{
				return (string)base.GetValue(InputDialog.AffirmativeButtonTextProperty);
			}
			set
			{
				base.SetValue(InputDialog.AffirmativeButtonTextProperty, value);
			}
		}

		public string Input
		{
			get
			{
				return (string)base.GetValue(InputDialog.InputProperty);
			}
			set
			{
				base.SetValue(InputDialog.InputProperty, value);
			}
		}

		public string Message
		{
			get
			{
				return (string)base.GetValue(InputDialog.MessageProperty);
			}
			set
			{
				base.SetValue(InputDialog.MessageProperty, value);
			}
		}

		public string NegativeButtonText
		{
			get
			{
				return (string)base.GetValue(InputDialog.NegativeButtonTextProperty);
			}
			set
			{
				base.SetValue(InputDialog.NegativeButtonTextProperty, value);
			}
		}

		static InputDialog()
		{
			Class6.yDnXvgqzyB5jw();
			InputDialog.MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(InputDialog), new PropertyMetadata(null));
			InputDialog.InputProperty = DependencyProperty.Register("Input", typeof(string), typeof(InputDialog), new PropertyMetadata(null));
			InputDialog.AffirmativeButtonTextProperty = DependencyProperty.Register("AffirmativeButtonText", typeof(string), typeof(InputDialog), new PropertyMetadata("OK"));
			InputDialog.NegativeButtonTextProperty = DependencyProperty.Register("NegativeButtonText", typeof(string), typeof(InputDialog), new PropertyMetadata("Cancel"));
		}

		internal InputDialog(MetroWindow parentWindow)
		{
			Class6.yDnXvgqzyB5jw();
			this(parentWindow, null);
		}

		internal InputDialog(MetroWindow parentWindow, MetroDialogSettings settings)
		{
			Class6.yDnXvgqzyB5jw();
			base(parentWindow, settings);
			this.InitializeComponent();
		}

		protected override void OnLoaded()
		{
			this.AffirmativeButtonText = base.DialogSettings.AffirmativeButtonText;
			this.NegativeButtonText = base.DialogSettings.NegativeButtonText;
			if (base.DialogSettings.ColorScheme == MetroDialogColorScheme.Accented)
			{
				this.PART_NegativeButton.Style = base.FindResource("AccentedDialogHighlightedSquareButton") as System.Windows.Style;
				this.PART_TextBox.SetResourceReference(Control.ForegroundProperty, "BlackColorBrush");
				this.PART_TextBox.SetResourceReference(ControlsHelper.FocusBorderBrushProperty, "TextBoxFocusBorderBrush");
			}
		}

		internal Task<string> WaitForButtonPressAsync()
		{
			base.Dispatcher.BeginInvoke(new Action(() => {
				base.Focus();
				this.PART_TextBox.Focus();
			}), new object[0]);
			TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();
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
					taskCompletionSource.TrySetResult(this.Input);
				}
			};
			routedEventHandler = (object sender, RoutedEventArgs e) => {
				pARTTextBox();
				taskCompletionSource.TrySetResult(null);
				e.Handled = true;
			};
			routedEventHandler1 = (object sender, RoutedEventArgs e) => {
				pARTTextBox();
				taskCompletionSource.TrySetResult(this.Input);
				e.Handled = true;
			};
			this.PART_NegativeButton.KeyDown += key;
			this.PART_AffirmativeButton.KeyDown += keyEventHandler;
			this.PART_TextBox.KeyDown += keyEventHandler;
			base.KeyDown += key1;
			this.PART_NegativeButton.Click += routedEventHandler;
			this.PART_AffirmativeButton.Click += routedEventHandler1;
			return taskCompletionSource.Task;
		}
	}
}