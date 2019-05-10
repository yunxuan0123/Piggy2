using Aries.Lib;
using Aries.Model;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Aries
{
	public partial class EditServerConfigWindow : MetroWindow
	{
		private ServerConfig serverConfig;

		public EditServerConfigWindow(ServerConfig serverConfig)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			base.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
			this.InitializeComponent();
			this.serverConfig = (serverConfig != null ? serverConfig.Clone() : null) as ServerConfig ?? new ServerConfig();
			base.DataContext = this.serverConfig;
		}

		public EditServerConfigWindow()
		{
			Class6.yDnXvgqzyB5jw();
			this(new ServerConfig());
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			ServerConfigService.SaveOrUpdateInMemory(this.serverConfig);
			base.Close();
		}

		private void btnSelect_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				Filter = "應用程式 (Mpalestory.exe)|Maplestory.exe"
			};
			bool? nullable = openFileDialog.ShowDialog(this);
			if ((nullable.HasValue ? nullable.GetValueOrDefault() : false))
			{
				this.serverConfig.ExeLocation = openFileDialog.FileName;
			}
		}

		public bool? ShowDialog(Window window)
		{
			base.Owner = window;
			return base.ShowDialog();
		}
	}
}