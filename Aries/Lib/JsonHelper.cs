using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Aries.Lib
{
	public class JsonHelper
	{
		public JsonHelper()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
		{
			return JsonConvert.DeserializeAnonymousType<T>(json, anonymousTypeObject);
		}

		public static List<T> DeserializeJsonToList<T>(string json)
		where T : class
		{
			JsonSerializer jsonSerializer = new JsonSerializer();
			StringReader stringReader = new StringReader(json);
			return jsonSerializer.Deserialize(new JsonTextReader(stringReader), typeof(List<T>)) as List<T>;
		}

		public static T DeserializeJsonToObject<T>(string json)
		where T : class
		{
			JsonSerializer jsonSerializer = new JsonSerializer();
			StringReader stringReader = new StringReader(json);
			return (T)(jsonSerializer.Deserialize(new JsonTextReader(stringReader), typeof(T)) as T);
		}

		public static string SerializeObject(object o)
		{
			return JsonConvert.SerializeObject(o);
		}
	}
}