using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple=false)]
	public class JsonExtensionDataAttribute : Attribute
	{
		public bool ReadData
		{
			get;
			set;
		}

		public bool WriteData
		{
			get;
			set;
		}

		public JsonExtensionDataAttribute()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.WriteData = true;
			this.ReadData = true;
		}
	}
}