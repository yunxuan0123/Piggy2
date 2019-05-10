using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Serialization
{
	public class CamelCasePropertyNamesContractResolver : DefaultContractResolver
	{
		private readonly static object TypeContractCacheLock;

		private readonly static DefaultJsonNameTable NameTable;

		private static Dictionary<StructMultiKey<Type, Type>, JsonContract> _contractCache;

		static CamelCasePropertyNamesContractResolver()
		{
			Class6.yDnXvgqzyB5jw();
			CamelCasePropertyNamesContractResolver.TypeContractCacheLock = new object();
			CamelCasePropertyNamesContractResolver.NameTable = new DefaultJsonNameTable();
		}

		public CamelCasePropertyNamesContractResolver()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			base.NamingStrategy = new CamelCaseNamingStrategy()
			{
				ProcessDictionaryKeys = true,
				OverrideSpecifiedNames = true
			};
		}

		internal override DefaultJsonNameTable GetNameTable()
		{
			return CamelCasePropertyNamesContractResolver.NameTable;
		}

		public override JsonContract ResolveContract(Type type)
		{
			JsonContract jsonContract;
			Dictionary<StructMultiKey<Type, Type>, JsonContract> structMultiKeys;
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			StructMultiKey<Type, Type> structMultiKey = new StructMultiKey<Type, Type>(base.GetType(), type);
			Dictionary<StructMultiKey<Type, Type>, JsonContract> structMultiKeys1 = CamelCasePropertyNamesContractResolver._contractCache;
			if (structMultiKeys1 == null || !structMultiKeys1.TryGetValue(structMultiKey, out jsonContract))
			{
				jsonContract = this.CreateContract(type);
				lock (CamelCasePropertyNamesContractResolver.TypeContractCacheLock)
				{
					structMultiKeys1 = CamelCasePropertyNamesContractResolver._contractCache;
					structMultiKeys = (structMultiKeys1 != null ? new Dictionary<StructMultiKey<Type, Type>, JsonContract>(structMultiKeys1) : new Dictionary<StructMultiKey<Type, Type>, JsonContract>());
					structMultiKeys[structMultiKey] = jsonContract;
					CamelCasePropertyNamesContractResolver._contractCache = structMultiKeys;
				}
			}
			return jsonContract;
		}
	}
}