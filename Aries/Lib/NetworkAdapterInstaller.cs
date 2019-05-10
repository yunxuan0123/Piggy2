using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Aries.Lib
{
	public static class NetworkAdapterInstaller
	{
		public readonly static string X86FILE;

		public readonly static string X64FILE;

		public readonly static string X86MD5;

		public readonly static string X64MD5;

		public readonly static string ARIESDIR;

		public readonly static string OUTFILE;

		public static Aries.Lib.WarpMessage WarpMessage;

		public static NetForwardMode InstallMode;

		static NetworkAdapterInstaller()
		{
			Class6.yDnXvgqzyB5jw();
			NetworkAdapterInstaller.X86FILE = "devcon_x86";
			NetworkAdapterInstaller.X64FILE = "devcon_x64";
			NetworkAdapterInstaller.X86MD5 = "7EB69E1F3BC96DE3E79299BA96890C80";
			NetworkAdapterInstaller.X64MD5 = "48E5B0185208D7B0DF5D29EB9A0BA24C";
			NetworkAdapterInstaller.ARIESDIR = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "\\小喵谷\\");
			NetworkAdapterInstaller.OUTFILE = string.Concat(NetworkAdapterInstaller.ARIESDIR, "devcon.exe");
			NetworkAdapterInstaller.InstallMode = NetForwardMode.Route;
		}

		public static async void ChangeMode(NetForwardMode mode, Action<bool> callback)
		{
			await Task.Run(() => {
				NetworkAdapterInstaller.InstallMode = mode;
				if (mode != NetForwardMode.Adapter)
				{
					callback(NetworkAdapterInstaller.DisableLoopAdapters());
					return;
				}
				NetworkAdapterInstaller.SendMessage("正在启用虚拟网卡模式..");
				callback(NetworkAdapterInstaller.DisableRedirection());
			});
		}

		public static async void CheckAndInstallAdapter(Action<bool> callback)
		{
			await Task.Run(() => {
				if (NetworkAdapterInstaller.InstallMode != NetForwardMode.Adapter)
				{
					callback(NetworkAdapterInstaller.OpenSuperMode());
					return;
				}
				callback((!NetworkAdapterInstaller.ReleaseDevcon() ? false : NetworkAdapterInstaller.CheckAndOpenAdapter()));
			});
		}

		private static bool CheckAndOpenAdapter()
		{
			NetworkAdapterInstaller.SendMessage("开始检查虚拟网卡..");
			List<ManagementBaseObject> managementBaseObjects = NetworkAdapterInstaller.QueryInstalledLoopbackAdapters();
			if (managementBaseObjects.Count <= 0)
			{
				NetworkAdapterInstaller.SendMessage("未找到已安装的虚拟网卡...开始安装..");
				if (!NetworkAdapterInstaller.InstallAdapter())
				{
					return false;
				}
				NetworkAdapterInstaller.SendMessage("开始设置网卡信息");
				return NetworkAdapterInstaller.SetAdapter();
			}
			if (managementBaseObjects.Count == 1)
			{
				if (NetworkAdapterInstaller.EnableAdapter(managementBaseObjects[0].Properties["PNPDeviceID"].Value.ToString()))
				{
					NetworkAdapterInstaller.SendMessage("开始设置网卡信息");
					return NetworkAdapterInstaller.SetAdapter();
				}
				NetworkAdapterInstaller.SendErrorMessage("网卡启动失败！");
				return false;
			}
			NetworkAdapterInstaller.SendMessage("找到多个虚拟网卡，处理中");
			for (int i = 0; i < managementBaseObjects.Count; i++)
			{
				if (i != 0)
				{
					NetworkAdapterInstaller.DeleteAdapter(managementBaseObjects[i].Properties["PNPDeviceID"].Value.ToString());
				}
			}
			NetworkAdapterInstaller.SendMessage("开始设置网卡信息");
			return NetworkAdapterInstaller.SetAdapter();
		}

		public static async void CloseNetwork()
		{
			await Task.Run(() => {
				try
				{
					NetworkAdapterInstaller.DisableRedirection();
					NetworkAdapterInstaller.DisableLoopAdapters();
				}
				catch
				{
				}
			});
		}

		private static void DEBUGWriteToConsole(Process p)
		{
		}

		private static bool DeleteAdapter(string string_0)
		{
			List<ManagementBaseObject> managementBaseObjects = NetworkAdapterInstaller.QueryAdapterByPnp(string_0);
			if (managementBaseObjects.Count <= 0)
			{
				NetworkAdapterInstaller.SendErrorMessage(string.Format("未找到网卡{0},删除失败", string_0));
				return false;
			}
			NetworkAdapterInstaller.RunCmd(string.Format("devcon.exe /r remove @{0}", string_0), new Action<Process>(NetworkAdapterInstaller.DEBUGWriteToConsole));
			if (NetworkAdapterInstaller.QueryAdapterByPnp(string_0).Count > 0)
			{
				NetworkAdapterInstaller.SendErrorMessage(string.Format("网卡{0}删除失败！", managementBaseObjects[0]["Caption"]));
				return false;
			}
			NetworkAdapterInstaller.SendMessage(string.Format("网卡{0}删除成功！", managementBaseObjects[0]["Caption"]));
			return true;
		}

		private static bool DeleteAllLoopAdapters()
		{
			NetworkAdapterInstaller.SendMessage("开始删除虚拟网卡");
			List<ManagementBaseObject> managementBaseObjects = NetworkAdapterInstaller.QuerySysinfo("Win32_NetworkAdapter", "Caption like '%Loopback Adapter%'");
			if (managementBaseObjects.Count <= 0)
			{
				NetworkAdapterInstaller.SendErrorMessage("没有可删除的虚拟网卡");
				return true;
			}
			foreach (ManagementBaseObject managementBaseObject in managementBaseObjects)
			{
				NetworkAdapterInstaller.SendMessage(string.Format("找到虚拟网卡{0}", managementBaseObject["Name"]));
				NetworkAdapterInstaller.RunCmd(string.Format("devcon.exe /r remove @{0}", managementBaseObject["PNPDeviceID"]), new Action<Process>(NetworkAdapterInstaller.DEBUGWriteToConsole));
			}
			if (NetworkAdapterInstaller.QuerySysinfo("Win32_NetworkAdapter", "Caption like '%Loopback Adapter%'").Count == 0)
			{
				NetworkAdapterInstaller.SendMessage("删除成功！");
				return false;
			}
			NetworkAdapterInstaller.SendMessage("删除失败！");
			return false;
		}

		private static bool DisableLoopAdapters()
		{
			return true;
		}

		private static bool DisableRedirection()
		{
			NetworkAdapterInstaller.SendMessage("开始取消重定向..");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("netsh int ip delete addr 1 221.231.130.70 \n");
			stringBuilder.Append("route delete 221.231.130.70");
			NetworkAdapterInstaller.RunCmd(stringBuilder.ToString(), new Action<Process>(NetworkAdapterInstaller.DEBUGWriteToConsole));
			NetworkAdapterInstaller.SendMessage("重定向已取消");
			return true;
		}

		private static bool EnableAdapter(string string_0)
		{
			List<ManagementBaseObject> managementBaseObjects = NetworkAdapterInstaller.QueryAdapterByPnp(string_0);
			if (managementBaseObjects.Count <= 0)
			{
				NetworkAdapterInstaller.SendErrorMessage(string.Format("未找到网卡{0},网卡启动失败", string_0));
				return false;
			}
			NetworkAdapterInstaller.SendMessage(string.Format("找到虚拟网卡{0}", managementBaseObjects[0]["Caption"]));
			NetworkAdapterInstaller.RunCmd(string.Format("devcon.exe /r enable @{0}", string_0), new Action<Process>(NetworkAdapterInstaller.DEBUGWriteToConsole));
			return true;
		}

		private static bool InstallAdapter()
		{
			NetworkAdapterInstaller.RunCmd("devcon.exe install %windir%/inf/netloop.inf *msloop", new Action<Process>(NetworkAdapterInstaller.DEBUGWriteToConsole));
			List<ManagementBaseObject> managementBaseObjects = NetworkAdapterInstaller.QueryInstalledLoopbackAdapters();
			if (managementBaseObjects.Count <= 0)
			{
				NetworkAdapterInstaller.SendErrorMessage("虚拟网卡安装失败！");
				return false;
			}
			NetworkAdapterInstaller.SendMessage(string.Format("虚拟网卡安装成功：{0}", managementBaseObjects[0]["Caption"]));
			return true;
		}

		private static bool OpenSuperMode()
		{
			return true;
		}

		private static bool Ping()
		{
			int num = 0;
			while (!NetworkAdapterInstaller.PingTest())
			{
				num++;
			}
			if (num >= 5)
			{
				return NetworkAdapterInstaller.PingTest();
			}
			return true;
		}

		private static bool PingTest()
		{
			Ping ping = new Ping();
			PingOptions pingOption = new PingOptions()
			{
				DontFragment = true
			};
			if (ping.Send("221.231.130.70", 1000, Encoding.ASCII.GetBytes("Test Data!"), pingOption).Status == IPStatus.Success)
			{
				return true;
			}
			return false;
		}

		private static List<ManagementBaseObject> QueryAdapterByPnp(string string_0)
		{
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(string.Format("select * from Win32_NetworkAdapter where PNPDeviceID = '{0}'", string_0.Replace("\\", "\\\\")));
			List<ManagementBaseObject> managementBaseObjects = new List<ManagementBaseObject>();
			foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
			{
				managementBaseObjects.Add(managementBaseObject);
			}
			return managementBaseObjects;
		}

		private static List<ManagementBaseObject> QueryInstalledLoopbackAdapters()
		{
			return NetworkAdapterInstaller.QuerySysinfo("Win32_NetworkAdapter", "Caption like '%Loopback Adapter%'");
		}

		private static List<ManagementBaseObject> QuerySysinfo(string Table, string whereClause)
		{
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(string.Format("select * from {0} where {1}", Table, whereClause));
			List<ManagementBaseObject> managementBaseObjects = new List<ManagementBaseObject>();
			foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
			{
				managementBaseObjects.Add(managementBaseObject);
			}
			return managementBaseObjects;
		}

		private static bool ReleaseDevcon()
		{
			bool flag;
			Directory.CreateDirectory(NetworkAdapterInstaller.ARIESDIR);
			!Environment.Is64BitOperatingSystem;
			if (!FileUtil.FileMD5Validation((Environment.Is64BitOperatingSystem ? NetworkAdapterInstaller.X64MD5 : NetworkAdapterInstaller.X86MD5), NetworkAdapterInstaller.OUTFILE))
			{
				byte[] numArray = (Environment.Is64BitOperatingSystem ? Resource.devcon_x64 : Resource.devcon_x86);
				try
				{
					using (FileStream fileStream = File.Create(NetworkAdapterInstaller.OUTFILE))
					{
						using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
						{
							binaryWriter.Write(numArray);
						}
					}
					return true;
				}
				catch (Exception exception)
				{
					NetworkAdapterInstaller.SendErrorMessage("无法解压文件！");
					flag = false;
				}
				return flag;
			}
			return true;
		}

		public static async void ResetNetworkSettings()
		{
			await Task.Run(() => {
				NetworkAdapterInstaller.SendMessage("正在重置网络..,");
				NetworkAdapterInstaller.SendMessage("正在清空重定向设置...");
				int[] numArray = new int[] { 8484, 8600, 7575, 7576, 7577, 7578, 7579, 7580, 7581, 7582, 7583, 7584 };
				StringBuilder stringBuilder = new StringBuilder();
				int[] numArray1 = numArray;
				for (int i = 0; i < (int)numArray1.Length; i++)
				{
					int num = numArray1[i];
					stringBuilder.Append(string.Format("netsh interface portproxy delete v4tov4 {0} 221.231.130.70 \n", num));
					stringBuilder.Append(string.Format("netsh interface portproxy delete v4tov4 {0} 127.0.0.1 \n", num));
				}
				stringBuilder.Append("netsh int ip delete addr 1 221.231.130.70 \n");
				stringBuilder.Append("route delete 221.231.130.70");
				NetworkAdapterInstaller.RunCmd(stringBuilder.ToString(), null);
				NetworkAdapterInstaller.SendMessage("重定向设置已清除!");
				NetworkAdapterInstaller.DeleteAllLoopAdapters();
				NetworkAdapterInstaller.SendMessage("网络配置重置成功！");
			});
		}

		private static void RunCmd(string cmd, Action<Process> action)
		{
			Process process = new Process();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.CreateNoWindow = true;
			process.Start();
			process.StandardInput.AutoFlush = true;
			StreamWriter standardInput = process.StandardInput;
			string aRIESDIR = NetworkAdapterInstaller.ARIESDIR;
			char chr = NetworkAdapterInstaller.ARIESDIR[0];
			standardInput.WriteLine(string.Format("cd {0}\n{1}", aRIESDIR, string.Concat(chr.ToString(), ":")));
			process.StandardInput.WriteLine(string.Concat(cmd, "&exit"));
			if (action != null)
			{
				action(process);
			}
			process.WaitForExit();
			process.Close();
		}

		private static void SendErrorMessage(string Msg)
		{
			Aries.Lib.WarpMessage warpMessage = NetworkAdapterInstaller.WarpMessage;
			if (warpMessage == null)
			{
				return;
			}
			warpMessage(1, Msg);
		}

		private static void SendMessage(string Msg)
		{
			Aries.Lib.WarpMessage warpMessage = NetworkAdapterInstaller.WarpMessage;
			if (warpMessage == null)
			{
				return;
			}
			warpMessage(0, Msg);
		}

		private static bool SetAdapter()
		{
			bool flag;
			ManagementBaseObject methodParameters = null;
			using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = (new ManagementObjectSearcher("select * from Win32_NetworkAdapterConfiguration where IPEnabled = 1 and Caption like '%Loopback Adapter%'")).Get().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ManagementObject current = (ManagementObject)enumerator.Current;
					if (!(bool)current["IPEnabled"])
					{
						continue;
					}
					methodParameters = current.GetMethodParameters("EnableStatic");
					methodParameters["IPAddress"] = new string[] { "221.231.130.70" };
					methodParameters["SubnetMask"] = new string[] { "255.255.255.0" };
					current.InvokeMethod("EnableStatic", methodParameters, null);
					NetworkAdapterInstaller.SendMessage("网卡信息设置成功！");
					flag = true;
					return flag;
				}
				NetworkAdapterInstaller.SendErrorMessage("未找到可设置的虚拟网卡！");
				return false;
			}
			return flag;
		}
	}
}