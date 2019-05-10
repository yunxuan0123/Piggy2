using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace MahApps.Metro.Controls.Dialogs
{
	public class MetroDialogSettings
	{
		public string AffirmativeButtonText
		{
			get;
			set;
		}

		public bool AnimateHide
		{
			get;
			set;
		}

		public bool AnimateShow
		{
			get;
			set;
		}

		public System.Threading.CancellationToken CancellationToken
		{
			get;
			set;
		}

		public MetroDialogColorScheme ColorScheme
		{
			get;
			set;
		}

		public ResourceDictionary CustomResourceDictionary
		{
			get;
			set;
		}

		public MessageDialogResult DefaultButtonFocus
		{
			get;
			set;
		}

		public string DefaultText
		{
			get;
			set;
		}

		public string FirstAuxiliaryButtonText
		{
			get;
			set;
		}

		public double MaximumBodyHeight
		{
			get;
			set;
		}

		public string NegativeButtonText
		{
			get;
			set;
		}

		public string SecondAuxiliaryButtonText
		{
			get;
			set;
		}

		public bool SuppressDefaultResources
		{
			get;
			set;
		}

		public MetroDialogSettings()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.AffirmativeButtonText = "OK";
			this.NegativeButtonText = "Cancel";
			this.ColorScheme = MetroDialogColorScheme.Theme;
			this.AnimateHide = true;
			this.AnimateShow = true;
			this.MaximumBodyHeight = double.NaN;
			this.DefaultText = "";
			this.DefaultButtonFocus = MessageDialogResult.Negative;
			this.CancellationToken = System.Threading.CancellationToken.None;
		}
	}
}