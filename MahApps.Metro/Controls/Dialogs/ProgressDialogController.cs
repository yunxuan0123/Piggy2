using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MahApps.Metro.Controls.Dialogs
{
	public class ProgressDialogController
	{
		private Func<Task> CloseCallback
		{
			get;
			set;
		}

		public bool IsCanceled
		{
			get;
			private set;
		}

		public bool IsOpen
		{
			get;
			private set;
		}

		public double Maximum
		{
			get
			{
				return this.InvokeFunc(() => this.WrappedDialog.Maximum);
			}
			set
			{
				this.InvokeAction(() => this.WrappedDialog.Maximum = value);
			}
		}

		public double Minimum
		{
			get
			{
				return this.InvokeFunc(() => this.WrappedDialog.Minimum);
			}
			set
			{
				this.InvokeAction(() => this.WrappedDialog.Minimum = value);
			}
		}

		private ProgressDialog WrappedDialog
		{
			get;
			set;
		}

		internal ProgressDialogController(ProgressDialog dialog, Func<Task> closeCallBack)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.WrappedDialog = dialog;
			this.CloseCallback = closeCallBack;
			this.IsOpen = dialog.IsVisible;
			this.InvokeAction(() => this.WrappedDialog.PART_NegativeButton.Click += new RoutedEventHandler(this.PART_NegativeButton_Click));
			CancellationToken cancellationToken = dialog.CancellationToken;
			cancellationToken.Register(() => this.PART_NegativeButton_Click(null, new RoutedEventArgs()));
		}

		public Task CloseAsync()
		{
			this.InvokeAction(() => {
				if (!this.WrappedDialog.IsVisible)
				{
					throw new InvalidOperationException("Dialog isn't visible to close");
				}
				this.WrappedDialog.Dispatcher.VerifyAccess();
				this.WrappedDialog.PART_NegativeButton.Click -= new RoutedEventHandler(this.PART_NegativeButton_Click);
			});
			return this.CloseCallback().ContinueWith((Task _) => this.InvokeAction(() => {
				this.IsOpen = false;
				EventHandler eventHandler = this.Closed;
				if (eventHandler != null)
				{
					eventHandler(this, EventArgs.Empty);
				}
			}));
		}

		private void InvokeAction(Action setValueAction)
		{
			if (this.WrappedDialog.Dispatcher.CheckAccess())
			{
				setValueAction();
				return;
			}
			this.WrappedDialog.Dispatcher.Invoke(setValueAction);
		}

		private double InvokeFunc(Func<double> getValueFunc)
		{
			if (this.WrappedDialog.Dispatcher.CheckAccess())
			{
				return getValueFunc();
			}
			return (double)this.WrappedDialog.Dispatcher.Invoke<double>(new Func<double>(getValueFunc.Invoke));
		}

		private void PART_NegativeButton_Click(object sender, RoutedEventArgs e)
		{
			this.InvokeAction(() => {
				this.IsCanceled = true;
				EventHandler eventHandler = this.Canceled;
				if (eventHandler != null)
				{
					eventHandler(this, EventArgs.Empty);
				}
				this.WrappedDialog.PART_NegativeButton.IsEnabled = false;
			});
		}

		public void SetCancelable(bool value)
		{
			this.InvokeAction(() => this.WrappedDialog.IsCancelable = value);
		}

		public void SetIndeterminate()
		{
			this.InvokeAction(() => this.WrappedDialog.SetIndeterminate());
		}

		public void SetMessage(string message)
		{
			this.InvokeAction(() => this.WrappedDialog.Message = message);
		}

		public void SetProgress(double value)
		{
			this.InvokeAction(() => {
				if (value < this.WrappedDialog.Minimum || value > this.WrappedDialog.Maximum)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.WrappedDialog.ProgressValue = value;
			});
		}

		public void SetTitle(string title)
		{
			this.InvokeAction(() => this.WrappedDialog.Title = title);
		}

		public event EventHandler Canceled;

		public event EventHandler Closed;
	}
}