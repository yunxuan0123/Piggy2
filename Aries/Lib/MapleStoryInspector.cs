using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Aries.Lib
{
	public class MapleStoryInspector
	{
		public Aries.Lib.OnMapleStoryWindowChange OnMapleStoryWindowChange;

		private static IPAddress ipa;

		private IPEndPoint ipe;

		private TcpClient tcpClient;

		public Action OnMapleStoryShutdown;

		public Aries.Lib.OnMapleStoryStartFail OnMapleStoryStartFail;

		public Aries.Lib.OnMapleStoryStartSuccess OnMapleStoryStartSuccess;

		public Aries.Lib.GetMapleMainPath GetMapleMainPath;

		public Aries.Lib.WarpMessage WarpMessage;

		public string MapleStoryExe;

		public string AccountName;

		public bool QuickPass;

		private Process MapleProcess;

		private CommunicationBase cb;

		private int bkd;

		private Thread MainInspectingThread;

		private Thread MainReceiveThread;

		private static string ip;

		private static string client_vers;

		private static string check_md5;

		static MapleStoryInspector()
		{
			Class6.yDnXvgqzyB5jw();
			MapleStoryInspector.ip = "daaep.com";
			MapleStoryInspector.client_vers = "xxx1001";
			MapleStoryInspector.check_md5 = "";
		}

		public MapleStoryInspector()
		{
			Class6.yDnXvgqzyB5jw();
			this.tcpClient = new TcpClient();
			this.QuickPass = true;
			base();
			MapleStoryInspector.ipa = Dns.GetHostAddresses(MapleStoryInspector.ip)[0];
			this.ipe = new IPEndPoint(MapleStoryInspector.ipa, 25070);
			this.tcpClient = new TcpClient();
			this.cb = new CommunicationBase();
		}

		public MapleStoryInspector(string filePath)
		{
			Class6.yDnXvgqzyB5jw();
			this.tcpClient = new TcpClient();
			this.QuickPass = true;
			base();
			MapleStoryInspector.ipa = Dns.GetHostAddresses(MapleStoryInspector.ip)[0];
			this.ipe = new IPEndPoint(MapleStoryInspector.ipa, 25070);
			this.tcpClient = new TcpClient();
			this.cb = new CommunicationBase();
			this.MapleStoryExe = filePath;
		}

		public static byte[] ConvertDoubleToByteArray(double d)
		{
			return BitConverter.GetBytes(d);
		}

		private Process findExistBadAss()
		{
			Process process = null;
			Process[] processesByName = Process.GetProcessesByName("MapleStory");
			for (int i = 0; i < (int)processesByName.Length; i++)
			{
				process = processesByName[i];
			}
			return process;
		}

		private IntPtr findExistBypassProccess()
		{
			return MapleStoryInspector.FindWindow(IntPtr.Zero, "HackShield Bypass");
		}

		private Process findExistMapleProccess()
		{
			Process process = null;
			Process[] processesByName = Process.GetProcessesByName("MapleStory");
			for (int i = 0; i < (int)processesByName.Length; i++)
			{
				process = processesByName[i];
			}
			return process;
		}

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		public static extern IntPtr FindWindow(IntPtr zeroOnly, string lpWindowName);

		private void HookMapleProcess()
		{
			this.MapleProcess.EnableRaisingEvents = true;
			this.MapleProcess.Exited += new EventHandler(this.ProcessExited);
			this.SendMessage("小喵谷主程式Hook完畢...");
			if (this.OnMapleStoryStartSuccess != null)
			{
				this.OnMapleStoryStartSuccess();
			}
			this.MainReceiveThread = new Thread(new ThreadStart(this.MaingReceiveWorking))
			{
				IsBackground = true
			};
			this.MainReceiveThread.Start();
			this.MainInspectingThread = new Thread(new ThreadStart(this.MaingInspectingWorking))
			{
				IsBackground = true
			};
			this.MainInspectingThread.Start();
		}

		public void Launch()
		{
			this.SendMessage("正在啟動小喵谷....");
			this.StartMaple();
			this.bkd = 0;
		}

		private void LaunchNewMaple()
		{
			if (!File.Exists(this.MapleStoryExe))
			{
				if (this.GetMapleMainPath == null)
				{
					this.SendErrorMessage("找不到小喵谷主程式，啟動失敗！");
					Aries.Lib.OnMapleStoryStartFail onMapleStoryStartFail = this.OnMapleStoryStartFail;
					if (onMapleStoryStartFail == null)
					{
						return;
					}
					onMapleStoryStartFail();
					return;
				}
				this.SendMessage("當前設置的小喵谷路徑錯誤，請選擇...");
				this.MapleStoryExe = this.GetMapleMainPath();
				if (this.MapleStoryExe == "" || this.MapleStoryExe == null)
				{
					this.SendMessage("用戶已取消！");
					Aries.Lib.OnMapleStoryStartFail onMapleStoryStartFail1 = this.OnMapleStoryStartFail;
					if (onMapleStoryStartFail1 == null)
					{
						return;
					}
					onMapleStoryStartFail1();
					return;
				}
			}
			try
			{
				bool flag = false;
				if (File.Exists("C:\\WINDOWS\\System32\\drivers\\etc\\hosts"))
				{
					StreamReader streamReader = new StreamReader("C:\\WINDOWS\\System32\\drivers\\etc\\hosts");
					while (true)
					{
						if (!streamReader.EndOfStream)
						{
							string str = streamReader.ReadLine();
							if (str.Contains("202.80.106.36"))
							{
								Console.WriteLine(str);
								flag = true;
								break;
							}
						}
						else
						{
							break;
						}
					}
					streamReader.Close();
					if (!flag)
					{
						StreamWriter streamWriter = new StreamWriter("C:\\WINDOWS\\System32\\drivers\\etc\\hosts");
						streamWriter.Write("202.80.106.36 tw.hackshield.gamania.com");
						streamWriter.Close();
					}
				}
			}
			catch (Exception exception)
			{
				if (File.Exists("\\error.log"))
				{
					this.SendErrorMessage("修改Hosts不成功,請手動修改！");
					Aries.Lib.OnMapleStoryStartFail onMapleStoryStartFail2 = this.OnMapleStoryStartFail;
					if (onMapleStoryStartFail2 != null)
					{
						onMapleStoryStartFail2();
					}
					else
					{
					}
					return;
				}
				else
				{
					this.SendErrorMessage("修改Hosts不成功,請手動修改！");
					Aries.Lib.OnMapleStoryStartFail onMapleStoryStartFail3 = this.OnMapleStoryStartFail;
					if (onMapleStoryStartFail3 != null)
					{
						onMapleStoryStartFail3();
					}
					else
					{
					}
					return;
				}
			}
			this.tcpClient = new TcpClient();
			if (!this.tcpClient.BeginConnect(this.ipe.Address, this.ipe.Port, null, null).AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2)))
			{
				this.SendErrorMessage("伺服器維修中或網路狀態有問題");
				Aries.Lib.OnMapleStoryStartFail onMapleStoryStartFail4 = this.OnMapleStoryStartFail;
				if (onMapleStoryStartFail4 == null)
				{
					return;
				}
				onMapleStoryStartFail4();
				return;
			}
			if (!this.tcpClient.Connected)
			{
				this.SendMessage("發送失敗!");
				Aries.Lib.OnMapleStoryStartFail onMapleStoryStartFail5 = this.OnMapleStoryStartFail;
				if (onMapleStoryStartFail5 == null)
				{
					return;
				}
				onMapleStoryStartFail5();
				return;
			}
			this.cb.SendMsg(this.AccountName, this.tcpClient, 7777);
			this.cb.ReceiveMsg(this.tcpClient);
			string str1 = this.MapleStoryExe.Substring(0, this.MapleStoryExe.Length - "MapleStory.Exe".Length);
			if (!File.Exists(string.Concat(str1, "Skill.wz")))
			{
				Console.WriteLine(str1);
				this.SendMessage("主程式資料夾檔案不全!");
				Aries.Lib.OnMapleStoryStartFail onMapleStoryStartFail6 = this.OnMapleStoryStartFail;
				if (onMapleStoryStartFail6 == null)
				{
					return;
				}
				onMapleStoryStartFail6();
				return;
			}
			try
			{
				byte[] numArray = new byte[16384];
				Stream stream = File.Open(string.Concat(str1, "Skill.wz"), FileMode.Open, FileAccess.Read, FileShare.Read);
				HashAlgorithm mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
				int num = 0;
				byte[] numArray1 = new byte[16384];
				while (true)
				{
					int num1 = stream.Read(numArray, 0, (int)numArray.Length);
					num = num1;
					if (num1 <= 0)
					{
						break;
					}
					mD5CryptoServiceProvider.TransformBlock(numArray, 0, num, numArray1, 0);
				}
				mD5CryptoServiceProvider.TransformFinalBlock(numArray, 0, 0);
				string str2 = BitConverter.ToString(mD5CryptoServiceProvider.Hash);
				mD5CryptoServiceProvider.Clear();
				stream.Close();
				str2 = str2.Replace("-", "");
				Console.WriteLine(str2);
				if (!this.tcpClient.Connected)
				{
					this.SendMessage("發送失敗!");
					Aries.Lib.OnMapleStoryStartFail onMapleStoryStartFail7 = this.OnMapleStoryStartFail;
					if (onMapleStoryStartFail7 != null)
					{
						onMapleStoryStartFail7();
					}
					else
					{
					}
				}
				else
				{
					this.cb.SendMsg(string.Concat(str2, "#", MapleStoryInspector.client_vers), this.tcpClient, 7778);
					if (this.cb.ReceiveMsg(this.tcpClient).Value[0] == '1')
					{
						MapleStoryInspector.check_md5 = str2;
						goto Label1;
					}
					else
					{
						this.SendMessage("遊戲檔案錯誤!");
						Aries.Lib.OnMapleStoryStartFail onMapleStoryStartFail8 = this.OnMapleStoryStartFail;
						if (onMapleStoryStartFail8 != null)
						{
							onMapleStoryStartFail8();
						}
						else
						{
						}
					}
				}
			}
			catch (FileNotFoundException fileNotFoundException)
			{
				this.SendMessage("權限不足,請以管理員模式開啟登入器!");
				Aries.Lib.OnMapleStoryStartFail onMapleStoryStartFail9 = this.OnMapleStoryStartFail;
				if (onMapleStoryStartFail9 != null)
				{
					onMapleStoryStartFail9();
				}
				else
				{
				}
			}
			return;
		Label1:
			this.MapleProcess = Process.Start(this.MapleStoryExe, string.Concat(MapleStoryInspector.ip, " 25090"));
			if (this.QuickPass)
			{
				DateTime now = DateTime.Now;
				IntPtr zero = IntPtr.Zero;
				while (zero == IntPtr.Zero && (DateTime.Now - now) <= TimeSpan.FromSeconds(15))
				{
					try
					{
						Thread.Sleep(50);
						zero = this.MapleProcess.MainWindowHandle;
					}
					catch (Exception exception1)
					{
						return;
					}
				}
				if (zero == IntPtr.Zero)
				{
					this.SendErrorMessage("檢測小喵谷程式超時...啟動失敗");
					Aries.Lib.OnMapleStoryStartFail onMapleStoryStartFail10 = this.OnMapleStoryStartFail;
					if (onMapleStoryStartFail10 != null)
					{
						onMapleStoryStartFail10();
					}
					else
					{
					}
				}
				this.MapleProcess.CloseMainWindow();
				this.SendMessage("已跳過Play畫面....");
			}
			this.HookMapleProcess();
		}

		private void MaingInspectingWorking()
		{
			bool flag = false;
			if (!this.MainInspectingThread.IsAlive)
			{
				return;
			}
			while (!flag)
			{
				Process process = this.findExistMapleProccess();
				if (process == null || process.HasExited || !(process.MainWindowHandle != IntPtr.Zero))
				{
					continue;
				}
				IntPtr intPtr = this.findExistBypassProccess();
				if (intPtr == IntPtr.Zero)
				{
					continue;
				}
				MapleStoryInspector.ShowWindow(intPtr, 0);
				MapleStoryInspector.SetWindowText(process.MainWindowHandle, "小喵谷");
				Thread.Sleep(5000);
				IntPtr intPtr1 = MapleStoryInspector.OpenProcess(2035711, false, process.Id);
				byte[] bytes = BitConverter.GetBytes(1999999);
				int num = 0;
				MapleStoryInspector.WriteProcessMemory((int)intPtr1, 11887312, bytes, (int)bytes.Length, ref num);
				flag = true;
				this.SendMessage("破攻完成");
				this.SendMessage("啟動成功！");
			}
		}

		private void MaingReceiveWorking()
		{
		Label1:
			while (this.MainReceiveThread.IsAlive)
			{
				KeyValuePair<short, string> keyValuePair = this.cb.ReceiveMsg(this.tcpClient);
				short key = keyValuePair.Key;
				if (key == 10)
				{
					Process process = this.findExistMapleProccess();
					int num = 0;
					try
					{
						num = int.Parse(keyValuePair.Value);
					}
					catch (FormatException formatException)
					{
						StringBuilder stringBuilder = new StringBuilder();
						char[] array = keyValuePair.Value.ToArray<char>();
						for (int i = 0; i < (int)array.Length; i++)
						{
							char chr = array[i];
							if (!char.IsNumber(chr))
							{
								break;
							}
							stringBuilder.Append(chr);
						}
						num = int.Parse(stringBuilder.ToString());
					}
					if (this.bkd == num)
					{
						continue;
					}
					IntPtr intPtr = MapleStoryInspector.OpenProcess(2035711, false, process.Id);
					byte[] bytes = BitConverter.GetBytes((double)(1999999 + num));
					int num1 = 0;
					MapleStoryInspector.WriteProcessMemory((int)intPtr, 11887312, bytes, (int)bytes.Length, ref num1);
					this.bkd = num;
					this.SendMessage(string.Concat("破攻完成 - 頂傷", 1999999 + num));
				}
				else
				{
					switch (key)
					{
						case 20:
						{
							Process.Start(keyValuePair.Value);
							this.SendMessage(string.Concat("您的贊助網址 : ", keyValuePair.Value));
							continue;
						}
						case 21:
						{
							this.SendMessage(string.Concat("GM幫忙開的贊助網址 : ", keyValuePair.Value));
							continue;
						}
						case 22:
						{
							string str = this.MapleStoryExe.Substring(0, this.MapleStoryExe.Length - "MapleStory.Exe".Length);
							if (!File.Exists(string.Concat(str, "Skill.wz")))
							{
								Console.WriteLine(str);
								this.SendMessage("主程式資料夾檔案不全!");
								Aries.Lib.OnMapleStoryStartFail onMapleStoryStartFail = this.OnMapleStoryStartFail;
								if (onMapleStoryStartFail == null)
								{
									return;
								}
								onMapleStoryStartFail();
								return;
							}
							try
							{
								byte[] numArray = new byte[16384];
								Stream stream = File.Open(string.Concat(str, "Skill.wz"), FileMode.Open, FileAccess.Read, FileShare.Read);
								HashAlgorithm mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
								int num2 = 0;
								byte[] numArray1 = new byte[16384];
								while (true)
								{
									int num3 = stream.Read(numArray, 0, (int)numArray.Length);
									num2 = num3;
									if (num3 <= 0)
									{
										break;
									}
									mD5CryptoServiceProvider.TransformBlock(numArray, 0, num2, numArray1, 0);
								}
								mD5CryptoServiceProvider.TransformFinalBlock(numArray, 0, 0);
								string str1 = BitConverter.ToString(mD5CryptoServiceProvider.Hash);
								mD5CryptoServiceProvider.Clear();
								stream.Close();
								if (str1.Replace("-", "") != MapleStoryInspector.check_md5)
								{
									this.Stop();
									this.SendMessage("檢測到您的遊戲檔案與伺服器不符合，可能為惡意修改WZ或系統錯誤。");
								}
								continue;
							}
							catch (FileNotFoundException fileNotFoundException)
							{
								this.SendMessage("權限不足,請以管理員模式開啟登入器!");
								continue;
							}
							break;
						}
						default:
						{
							if (key == 666)
							{
								break;
							}
							else
							{
								goto Label1;
							}
						}
					}
					this.SendMessage(keyValuePair.Value);
				}
			}
		}

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		private void ProcessExited(object sender, EventArgs e)
		{
			this.MainReceiveThread.Abort();
			this.SendMessage("小喵谷已退出...");
			this.SendMessage("連線已斷開!");
			this.tcpClient.Close();
			this.OnMapleStoryShutdown();
		}

		private void SendErrorMessage(string Msg)
		{
			Aries.Lib.WarpMessage warpMessage = this.WarpMessage;
			if (warpMessage == null)
			{
				return;
			}
			warpMessage(1, Msg);
		}

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		private void SendMessage(string Msg)
		{
			Aries.Lib.WarpMessage warpMessage = this.WarpMessage;
			if (warpMessage == null)
			{
				return;
			}
			warpMessage(0, Msg);
		}

		[DllImport("User32.dll", CharSet=CharSet.Ansi, ExactSpelling=false)]
		private static extern int SetWindowText(IntPtr hWnd, string lpString);

		[DllImport("User32.dll", CharSet=CharSet.Ansi, ExactSpelling=false)]
		private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

		private void StartMaple()
		{
			this.MapleProcess = this.findExistMapleProccess();
			if (this.MapleProcess != null)
			{
				this.HookMapleProcess();
				return;
			}
			(new Thread(new ThreadStart(this.LaunchNewMaple))
			{
				IsBackground = true
			}).Start();
		}

		public void Stop()
		{
			try
			{
				this.MainInspectingThread.Abort();
				this.MapleProcess = this.findExistMapleProccess();
				this.tcpClient.Close();
				if (this.MapleProcess != null)
				{
					this.MapleProcess.Kill();
				}
			}
			catch
			{
			}
		}

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false, SetLastError=true)]
		private static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);
	}
}