using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
	public class DefaultSerializationBinder : SerializationBinder, GInterface3
	{
		internal readonly static DefaultSerializationBinder Instance;

		private readonly ThreadSafeStore<StructMultiKey<string, string>, Type> _typeCache;

		static DefaultSerializationBinder()
		{
			Class6.yDnXvgqzyB5jw();
			DefaultSerializationBinder.Instance = new DefaultSerializationBinder();
		}

		public DefaultSerializationBinder()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this._typeCache = new ThreadSafeStore<StructMultiKey<string, string>, Type>(new Func<StructMultiKey<string, string>, Type>(this.GetTypeFromTypeNameKey));
		}

		public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			assemblyName = serializedType.Assembly.FullName;
			typeName = serializedType.FullName;
		}

		public override Type BindToType(string assemblyName, string typeName)
		{
			return this.GetTypeByName(new StructMultiKey<string, string>(assemblyName, typeName));
		}

		private Type GetGenericTypeFromTypeName(string typeName, Assembly assembly)
		{
			Type type = null;
			int num = typeName.IndexOf('[');
			if (num >= 0)
			{
				Type type1 = assembly.GetType(typeName.Substring(0, num));
				if (type1 != null)
				{
					List<Type> types = new List<Type>();
					int num1 = 0;
					int num2 = 0;
					int length = typeName.Length - 1;
					for (int i = num + 1; i < length; i++)
					{
						char chr = typeName[i];
						if (chr == '[')
						{
							if (num1 == 0)
							{
								num2 = i + 1;
							}
							num1++;
						}
						else if (chr == ']')
						{
							num1--;
							if (num1 == 0)
							{
								StructMultiKey<string, string> structMultiKey = ReflectionUtils.SplitFullyQualifiedTypeName(typeName.Substring(num2, i - num2));
								types.Add(this.GetTypeByName(structMultiKey));
							}
						}
					}
					type = type1.MakeGenericType(types.ToArray());
				}
			}
			return type;
		}

		private Type GetTypeByName(StructMultiKey<string, string> typeNameKey)
		{
			return this._typeCache.Get(typeNameKey);
		}

		private Type GetTypeFromTypeNameKey(StructMultiKey<string, string> typeNameKey)
		{
			Type type;
			Exception exception;
			string value1 = typeNameKey.Value1;
			string value2 = typeNameKey.Value2;
			if (value1 == null)
			{
				return Type.GetType(value2);
			}
			Assembly assembly = Assembly.LoadWithPartialName(value1);
			if (assembly == null)
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				int num = 0;
				while (num < (int)assemblies.Length)
				{
					Assembly assembly1 = assemblies[num];
					if (assembly1.FullName == value1 || assembly1.GetName().Name == value1)
					{
						assembly = assembly1;
						if (assembly == null)
						{
							throw new JsonSerializationException("Could not load assembly '{0}'.".FormatWith(CultureInfo.InvariantCulture, value1));
						}
						type = assembly.GetType(value2);
						if (type == null)
						{
							if (value2.IndexOf('\u0060') >= 0)
							{
								try
								{
									type = this.GetGenericTypeFromTypeName(value2, assembly);
								}
								catch (Exception exception1)
								{
									exception = exception1;
									throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, value2, assembly.FullName), exception);
								}
							}
							if (type == null)
							{
								throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, value2, assembly.FullName));
							}
						}
						return type;
					}
					else
					{
						num++;
					}
				}
			}
			if (assembly == null)
			{
				throw new JsonSerializationException("Could not load assembly '{0}'.".FormatWith(CultureInfo.InvariantCulture, value1));
			}
			type = assembly.GetType(value2);
			if (type == null)
			{
				if (value2.IndexOf('\u0060') >= 0)
				{
					try
					{
						type = this.GetGenericTypeFromTypeName(value2, assembly);
					}
					catch (Exception exception1)
					{
						exception = exception1;
						throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, value2, assembly.FullName), exception);
					}
				}
				if (type == null)
				{
					throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, value2, assembly.FullName));
				}
			}
			return type;
		}
	}
}