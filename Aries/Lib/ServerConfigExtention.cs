using Aries.Model;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Aries.Lib
{
	public static class ServerConfigExtention
	{
		public static string ToJsonString(this ServerConfig sc)
		{
			return JsonHelper.SerializeObject(sc);
		}

		public static void UpdateData(this ServerConfig sc, ServerConfig other)
		{
			PropertyInfo[] properties = sc.GetType().GetProperties();
			for (int i = 0; i < (int)properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				propertyInfo.SetValue(sc, propertyInfo.GetValue(other));
			}
		}
	}
}