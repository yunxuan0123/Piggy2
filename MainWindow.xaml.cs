using Aries.Lib;
using Aries.Model;
using MahApps.Metro.Controls;
using Microsoft.Win32;
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
using System.Windows.Threading;

namespace Aries
{
	public partial class MainWindow : MetroWindow
	{
		private BindingList<ServerConfig> serverConfigs;

		public MapleStoryInspector inspector;

		public PortForwardingService portService;

		private static Mutex appMutex;

		private bool isNetworkAdapterReady;

		private Thread MainDetectThread;

		public MainWindow()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			bool flag = false;
			MainWindow.appMutex = new Mutex(true, "Bearms", out flag);
			if (!flag)
			{
				MainWindow.appMutex.Close();
				MainWindow.appMutex = null;
				MessageBox.Show("小喵谷登入器已經開啟");
				base.Close();
				return;
			}
			if (!this.GoodChild())
			{
				MessageBox.Show("偵測到非法程式");
				base.Close();
				return;
			}
			this.MainDetectThread = new Thread(new ThreadStart(this.MaingBadDetecting))
			{
				IsBackground = true
			};
			this.MainDetectThread.Start();
			this.InitializeComponent();
			this.LoadServerConfigs();
			this.InitMapleInspector();
			this.InitPortForwarding();
			this.InitNetworkAdapter();
		}

		private void btnAbout_Click(object sender, RoutedEventArgs e)
		{
			(new AriesAbout()).ShowDialog(this);
		}

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{
			(new EditServerConfigWindow(this.cbServerConfig.SelectedItem as ServerConfig)).ShowDialog(this);
		}

		private void btnNew_Click(object sender, RoutedEventArgs e)
		{
			(new EditServerConfigWindow()).ShowDialog(this);
		}

		private void btnRemove_Click(object sender, RoutedEventArgs e)
		{
			ServerConfigService.RemoveFromMemory(this.cbServerConfig.SelectedItem as ServerConfig);
		}

		private void btnReset_Click(object sender, RoutedEventArgs e)
		{
			NetworkAdapterInstaller.ResetNetworkSettings();
			this.isNetworkAdapterReady = false;
		}

		private void btnStart_Click(object sender, RoutedEventArgs e)
		{
			this.SetStartBtn(false);
			ServerConfigService.LastId = (int)this.cbServerConfig.SelectedValue;
			if (this.isNetworkAdapterReady)
			{
				this.LaunchForwarding(this.isNetworkAdapterReady);
				return;
			}
			NetworkAdapterInstaller.CheckAndInstallAdapter(new Action<bool>(this.LaunchForwarding));
		}

		private void btnStop_Click(object sender, RoutedEventArgs e)
		{
			this.btnStop.IsEnabled = false;
			this.inspector.Stop();
			this.portService.Stop();
		}

		private void cbServerConfig_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
		}

		private void checkQuickPass_Checked(object sender, RoutedEventArgs e)
		{
			ServerConfigService.QuickPass = this.checkQuickPass.IsChecked.Value;
			this.inspector.QuickPass = ServerConfigService.QuickPass;
		}

		private void checkQuickPass_Unchecked(object sender, RoutedEventArgs e)
		{
			ServerConfigService.QuickPass = this.checkQuickPass.IsChecked.Value;
			this.inspector.QuickPass = ServerConfigService.QuickPass;
		}

		private static Process findExistMapleProccess()
		{
			Process process = null;
			Process[] processesByName = Process.GetProcessesByName("MapleStory");
			for (int i = 0; i < (int)processesByName.Length; i++)
			{
				process = processesByName[i];
			}
			return process;
		}

		public string GetMapleMainPath()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				Filter = "應用程式 (Mpalestory.exe)|MapleStory.exe"
			};
			bool? nullable = openFileDialog.ShowDialog();
			if ((nullable.GetValueOrDefault() ? !nullable.HasValue : true))
			{
				return null;
			}
			base.Dispatcher.Invoke(() => ((ServerConfig)this.cbServerConfig.SelectedItem).ExeLocation = openFileDialog.FileName);
			return openFileDialog.FileName;
		}

		private bool GoodChild()
		{
			Process[] processes = Process.GetProcesses();
			for (int i = 0; i < (int)processes.Length; i++)
			{
				Process process = processes[i];
				if (process.ProcessName.ToLower().Contains("ucheatengine") || process.ProcessName.ToLower().Contains("按鍵精靈") || process.ProcessName.ToLower().Contains("輔助") || process.ProcessName.ToLower().Contains("限速") || process.ProcessName.ToLower().Contains("netlimiter") || process.ProcessName.ToLower().Contains("nlclient") || process.ProcessName.ToLower().Contains("ollydbg"))
				{
					return false;
				}
			}
			return true;
		}

		private void InitMapleInspector()
		{
			this.inspector = new MapleStoryInspector();
			this.inspector.OnMapleStoryStartSuccess += new OnMapleStoryStartSuccess(this.OnMapleStoryStartSuccess);
			this.inspector.OnMapleStoryStartFail += new OnMapleStoryStartFail(this.OnMapleStoryStartFail);
			this.inspector.OnMapleStoryShutdown += new Action(this.OnMapleStoryShutdown);
			this.inspector.OnMapleStoryWindowChange += new OnMapleStoryWindowChange(this.OnMapleStoryWindowChange);
			this.inspector.WarpMessage += new WarpMessage(this.WarpMessage);
			this.inspector.GetMapleMainPath += new GetMapleMainPath(this.GetMapleMainPath);
			this.inspector.QuickPass = ServerConfigService.QuickPass;
		}

		private void InitNetworkAdapter()
		{
			NetworkAdapterInstaller.WarpMessage = new WarpMessage(this.WarpMessage);
			this.checkQuickPass.IsChecked = new bool?(ServerConfigService.QuickPass);
		}

		private void InitPortForwarding()
		{
			this.portService = new PortForwardingService();
			this.portService.WarpMessage += new WarpMessage(this.WarpMessage);
		}

		private void LauchMaple(bool success)
		{
			base.Dispatcher.Invoke(() => {
				if (!success)
				{
					this.btnStart.IsEnabled = true;
					this.btnStop.IsEnabled = false;
					return;
				}
				this.inspector.MapleStoryExe = (this.cbServerConfig.SelectedItem as ServerConfig).ExeLocation;
				this.inspector.AccountName = this.Text_account.Text;
				this.inspector.Launch();
			});
		}

		private void LaunchForwarding(bool success)
		{
			base.Dispatcher.Invoke(() => {
				if (!success)
				{
					this.SetStartBtn(false);
					return;
				}
				this.LauchMaple(success);
			});
		}

		private void LoadServerConfigs()
		{
			this.serverConfigs = ServerConfigService.LoadAll();
			this.cbServerConfig.DataContext = this.serverConfigs;
			this.cbServerConfig.SelectedValue = ServerConfigService.LastId;
		}

		private void MaingBadDetecting()
		{
			while (this.GoodChild())
			{
				Thread.Sleep(2000);
			}
			base.Dispatcher.Invoke(() => base.Close());
		}

		private void MetroWindow_Closed(object sender, EventArgs e)
		{
		}

		private void MetroWindow_Closing(object sender, CancelEventArgs e)
		{
			ServerConfigService.SaveAll();
			NetworkAdapterInstaller.CloseNetwork();
			if (MainWindow.appMutex != null)
			{
				MainWindow.appMutex.ReleaseMutex();
				MainWindow.appMutex.Close();
			}
			Process process = MainWindow.findExistMapleProccess();
			if (process != null)
			{
				process.Kill();
			}
		}

		public void OnMapleStoryShutdown()
		{
			base.Dispatcher.Invoke(() => {
				this.SetStartBtn(true);
				this.btnStop.IsEnabled = false;
			});
		}

		public void OnMapleStoryStartFail()
		{
			base.Dispatcher.Invoke(() => {
				this.SetStartBtn(true);
				this.btnStop.IsEnabled = false;
			});
		}

		public void OnMapleStoryStartSuccess()
		{
			base.Dispatcher.Invoke(() => {
				this.SetStartBtn(false);
				this.btnStop.IsEnabled = true;
			});
		}

		public void OnMapleStoryWindowChange(MapleStoryWindowType windowType)
		{
		}

		private void radio_Adapter_Checked(object sender, RoutedEventArgs e)
		{
			this.SetStartBtn(false);
			NetworkAdapterInstaller.ChangeMode(NetForwardMode.Adapter, (bool success) => base.Dispatcher.Invoke(() => {
				this.SetStartBtn(true);
				ServerConfigService.Mode = NetForwardMode.Adapter;
			}));
		}

		private void radio_Super_Checked(object sender, RoutedEventArgs e)
		{
			this.SetStartBtn(false);
			NetworkAdapterInstaller.ChangeMode(NetForwardMode.Route, (bool success) => base.Dispatcher.Invoke(() => {
				this.SetStartBtn(true);
				ServerConfigService.Mode = NetForwardMode.Route;
			}));
		}

		private void SetStartBtn(bool enable)
		{
			this.btnStart.IsEnabled = enable;
			this.Text_account.IsReadOnly = !enable;
		}

		public void WarpMessage(MessageType type, string message)
		{
			base.Dispatcher.Invoke(() => {
				TextBox u003cu003e4_this = this.tbLogs;
				u003cu003e4_this.Text = string.Concat(u003cu003e4_this.Text, (type == MessageType.Tips ? "[信息]" : "[錯誤]"), message, "\n");
				this.tbLogs.ScrollToEnd();
			});
		}
	}
}