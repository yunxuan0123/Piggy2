using System;
using System.ComponentModel.DataAnnotations;

namespace Aries.Model
{
	public class ServerConfig : ObservableObject, ICloneable
	{
		private int id;

		private string serverName;

		private string exeLocation;

		[Required]
		[StringLength(255)]
		public string ExeLocation
		{
			get
			{
				return this.exeLocation;
			}
			set
			{
				this.exeLocation = value;
				this.RaisePropertyChanged("ExeLocation");
			}
		}

		public int ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
				this.RaisePropertyChanged("ID");
			}
		}

		[Required]
		[StringLength(50)]
		public string ServerName
		{
			get
			{
				return this.serverName;
			}
			set
			{
				this.serverName = value;
				this.RaisePropertyChanged("ServerName");
			}
		}

		public ServerConfig()
		{
			Class6.yDnXvgqzyB5jw();
			this.serverName = "小喵谷";
			this.exeLocation = "MapleStory.exe";
			base();
		}

		public object Clone()
		{
			return new ServerConfig()
			{
				ID = this.ID,
				ServerName = this.ServerName,
				ExeLocation = this.ExeLocation
			};
		}
	}
}