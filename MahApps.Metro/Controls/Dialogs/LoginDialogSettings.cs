using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MahApps.Metro.Controls.Dialogs
{
	public class LoginDialogSettings : MetroDialogSettings
	{
		public bool EnablePasswordPreview
		{
			get;
			set;
		}

		public string InitialPassword
		{
			get;
			set;
		}

		public string InitialUsername
		{
			get;
			set;
		}

		public Visibility NegativeButtonVisibility
		{
			get;
			set;
		}

		public string PasswordWatermark
		{
			get;
			set;
		}

		public bool ShouldHideUsername
		{
			get;
			set;
		}

		public string UsernameWatermark
		{
			get;
			set;
		}

		public LoginDialogSettings()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.UsernameWatermark = "Username...";
			this.PasswordWatermark = "Password...";
			this.NegativeButtonVisibility = Visibility.Collapsed;
			this.ShouldHideUsername = false;
			base.AffirmativeButtonText = "Login";
			this.EnablePasswordPreview = false;
		}
	}
}