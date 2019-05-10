using MahApps.Metro;
using MahApps.Metro.Controls;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;

namespace MahApps.Metro.Controls.Dialogs
{
	public partial class ProgressDialog : BaseMetroDialog
	{
		public readonly static DependencyProperty ProgressBarForegroundProperty;

		public readonly static DependencyProperty MessageProperty;

		public readonly static DependencyProperty IsCancelableProperty;

		public readonly static DependencyProperty NegativeButtonTextProperty;

		internal System.Threading.CancellationToken CancellationToken
		{
			get
			{
				return base.DialogSettings.CancellationToken;
			}
		}

		public bool IsCancelable
		{
			get
			{
				return (bool)base.GetValue(ProgressDialog.IsCancelableProperty);
			}
			set
			{
				base.SetValue(ProgressDialog.IsCancelableProperty, value);
			}
		}

		internal double Maximum
		{
			get
			{
				return this.PART_ProgressBar.Maximum;
			}
			set
			{
				this.PART_ProgressBar.Maximum = value;
			}
		}

		public string Message
		{
			get
			{
				return (string)base.GetValue(ProgressDialog.MessageProperty);
			}
			set
			{
				base.SetValue(ProgressDialog.MessageProperty, value);
			}
		}

		internal double Minimum
		{
			get
			{
				return this.PART_ProgressBar.Minimum;
			}
			set
			{
				this.PART_ProgressBar.Minimum = value;
			}
		}

		public string NegativeButtonText
		{
			get
			{
				return (string)base.GetValue(ProgressDialog.NegativeButtonTextProperty);
			}
			set
			{
				base.SetValue(ProgressDialog.NegativeButtonTextProperty, value);
			}
		}

		public Brush ProgressBarForeground
		{
			get
			{
				return (Brush)base.GetValue(ProgressDialog.ProgressBarForegroundProperty);
			}
			set
			{
				base.SetValue(ProgressDialog.ProgressBarForegroundProperty, value);
			}
		}

		internal double ProgressValue
		{
			get
			{
				return this.PART_ProgressBar.Value;
			}
			set
			{
				this.PART_ProgressBar.IsIndeterminate = false;
				this.PART_ProgressBar.Value = value;
				this.PART_ProgressBar.ApplyTemplate();
			}
		}

		static ProgressDialog()
		{
			Class6.yDnXvgqzyB5jw();
			ProgressDialog.ProgressBarForegroundProperty = DependencyProperty.Register("ProgressBarForeground", typeof(Brush), typeof(ProgressDialog), new PropertyMetadata(null));
			ProgressDialog.MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(ProgressDialog), new PropertyMetadata(null));
			ProgressDialog.IsCancelableProperty = DependencyProperty.Register("IsCancelable", typeof(bool), typeof(ProgressDialog), new PropertyMetadata(false, (DependencyObject s, DependencyPropertyChangedEventArgs e) => ((ProgressDialog)s).PART_NegativeButton.Visibility = ((bool)e.NewValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden)));
			ProgressDialog.NegativeButtonTextProperty = DependencyProperty.Register("NegativeButtonText", typeof(string), typeof(ProgressDialog), new PropertyMetadata("Cancel"));
		}

		internal ProgressDialog(MetroWindow parentWindow, MetroDialogSettings settings)
		{
			Class6.yDnXvgqzyB5jw();
			base(parentWindow, settings);
			this.InitializeComponent();
			if (parentWindow.MetroDialogOptions.ColorScheme != MetroDialogColorScheme.Theme)
			{
				this.ProgressBarForeground = Brushes.White;
			}
			else
			{
				try
				{
					this.ProgressBarForeground = ThemeManager.GetResourceFromAppStyle(parentWindow, "AccentColorBrush") as Brush;
				}
				catch (Exception exception)
				{
				}
			}
		}

		internal ProgressDialog(MetroWindow parentWindow)
		{
			Class6.yDnXvgqzyB5jw();
			this(parentWindow, null);
		}

		internal void SetIndeterminate()
		{
			this.PART_ProgressBar.IsIndeterminate = true;
		}
	}
}