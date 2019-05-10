using Aries.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Aries.Lib
{
	public static class ServerConfigService
	{
		public static Aries.Lib.WarpMessage WarpMessage;

		public readonly static string ARIESDIR;

		public readonly static string FILE;

		private static BindingList<ServerConfig> serverConfigs;

		public static int LastId
		{
			get;
			set;
		}

		public static NetForwardMode Mode
		{
			get;
			set;
		}

		public static bool QuickPass
		{
			get;
			set;
		}

		static ServerConfigService()
		{
			Class6.yDnXvgqzyB5jw();
			ServerConfigService.ARIESDIR = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "\\小喵谷\\");
			ServerConfigService.FILE = string.Concat(ServerConfigService.ARIESDIR, "kitten.json");
		}

		public static BindingList<ServerConfig> LoadAll()
		{
			if (ServerConfigService.serverConfigs == null)
			{
				var variable = new { mode = 0, lastId = 0, quickPass = true, configs = new ServerConfig[0] };
				variable = JsonHelper.DeserializeAnonymousType(ServerConfigService.LoadFile(), variable);
				ServerConfigService.LastId = variable.lastId;
				ServerConfigService.Mode = NetForwardMode.Adapter;
				ServerConfigService.QuickPass = variable.quickPass;
				ServerConfigService.serverConfigs = new BindingList<ServerConfig>((
					from c in (IEnumerable<ServerConfig>)variable.configs
					select c).ToList<ServerConfig>());
			}
			return ServerConfigService.serverConfigs;
		}

		private static string LoadDefault()
		{
			return JsonHelper.SerializeObject(new { configs = new ServerConfig[] { new ServerConfig()
			{
				ID = 1,
				ServerName = "小喵谷"
			} }, lastId = 1, mode = 0, quickPass = true });
		}

		private static string LoadFile()
		{
			string end;
			try
			{
				using (StreamReader streamReader = new StreamReader(ServerConfigService.FILE))
				{
					end = streamReader.ReadToEnd();
				}
			}
			catch
			{
				end = ServerConfigService.LoadDefault();
			}
			return end;
		}

		public static bool RemoveFromMemory(ServerConfig serverConfig)
		{
			return ServerConfigService.serverConfigs.Remove(serverConfig);
		}

		public static void SaveAll()
		{
			if (!Directory.Exists(ServerConfigService.ARIESDIR))
			{
				Directory.CreateDirectory(ServerConfigService.ARIESDIR);
			}
			using (StreamWriter streamWriter = new StreamWriter(ServerConfigService.FILE))
			{
				streamWriter.Write(JsonHelper.SerializeObject(new { configs = ServerConfigService.serverConfigs, lastId = ServerConfigService.<LastId>k__BackingField, mode = ServerConfigService.<Mode>k__BackingField, quickPass = ServerConfigService.<QuickPass>k__BackingField }));
			}
		}

		public static void SaveOrUpdateInMemory(ServerConfig serverConfig)
		{
			if (serverConfig.ID != 0)
			{
				(
					from sc in ServerConfigService.serverConfigs
					where sc.ID == serverConfig.ID
					select sc).First<ServerConfig>().UpdateData(serverConfig);
				return;
			}
			IEnumerable<int> d = 
				from sc in ServerConfigService.serverConfigs
				select sc.ID;
			serverConfig.ID = d.Max() + 1;
			ServerConfigService.serverConfigs.Add(serverConfig);
		}
	}
}