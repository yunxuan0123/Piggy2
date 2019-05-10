using MahApps.Metro.Controls;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace Aries
{
	public partial class AriesAbout : MetroWindow
	{
		public string AssemblyCompany
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute)customAttributes[0]).Company;
			}
		}

		public string AssemblyCopyright
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
			}
		}

		public string AssemblyDescription
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyDescriptionAttribute)customAttributes[0]).Description;
			}
		}

		public string AssemblyProduct
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (customAttributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute)customAttributes[0]).Product;
			}
		}

		public string AssemblyTitle
		{
			get
			{
				object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (customAttributes.Length != 0)
				{
					AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)customAttributes[0];
					if (assemblyTitleAttribute.Title != "")
					{
						return assemblyTitleAttribute.Title;
					}
				}
				return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public AriesAbout()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.InitializeComponent();
			base.Title = string.Format("關於 {0}", this.AssemblyTitle);
			this.nameVersion.Content = string.Format("{0}v{1}", this.AssemblyDescription, this.AssemblyVersion);
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}

		public bool? ShowDialog(Window window)
		{
			base.Owner = window;
			return base.ShowDialog();
		}
	}
}