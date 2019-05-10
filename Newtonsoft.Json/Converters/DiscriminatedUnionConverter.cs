using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	public class DiscriminatedUnionConverter : JsonConverter
	{
		private readonly static ThreadSafeStore<Type, DiscriminatedUnionConverter.Union> UnionCache;

		private readonly static ThreadSafeStore<Type, Type> UnionTypeLookupCache;

		static DiscriminatedUnionConverter()
		{
			Class6.yDnXvgqzyB5jw();
			DiscriminatedUnionConverter.UnionCache = new ThreadSafeStore<Type, DiscriminatedUnionConverter.Union>(new Func<Type, DiscriminatedUnionConverter.Union>(DiscriminatedUnionConverter.CreateUnion));
			DiscriminatedUnionConverter.UnionTypeLookupCache = new ThreadSafeStore<Type, Type>(new Func<Type, Type>(DiscriminatedUnionConverter.CreateUnionTypeLookup));
		}

		public DiscriminatedUnionConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override bool CanConvert(Type objectType)
		{
			if (typeof(IEnumerable).IsAssignableFrom(objectType))
			{
				return false;
			}
			bool flag = false;
			object[] customAttributes = objectType.GetCustomAttributes(true);
			int num = 0;
			while (true)
			{
				if (num < (int)customAttributes.Length)
				{
					Type type = customAttributes[num].GetType();
					if (type.FullName == "Microsoft.FSharp.Core.CompilationMappingAttribute")
					{
						FSharpUtils.EnsureInitialized(type.Assembly());
						flag = true;
						break;
					}
					else
					{
						num++;
					}
				}
				else
				{
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			return (bool)FSharpUtils.IsUnion(null, new object[] { objectType, null });
		}

		private static DiscriminatedUnionConverter.Union CreateUnion(Type t)
		{
			DiscriminatedUnionConverter.Union union = new DiscriminatedUnionConverter.Union()
			{
				TagReader = (FSharpFunction)FSharpUtils.PreComputeUnionTagReader(null, new object[] { t, null }),
				Cases = new List<DiscriminatedUnionConverter.UnionCase>()
			};
			object[] getUnionCases = (object[])FSharpUtils.GetUnionCases(null, new object[] { t, null });
			for (int i = 0; i < (int)getUnionCases.Length; i++)
			{
				object obj = getUnionCases[i];
				DiscriminatedUnionConverter.UnionCase unionCase = new DiscriminatedUnionConverter.UnionCase()
				{
					Tag = (int)FSharpUtils.GetUnionCaseInfoTag(obj),
					Name = (string)FSharpUtils.GetUnionCaseInfoName(obj),
					Fields = (PropertyInfo[])FSharpUtils.GetUnionCaseInfoFields(obj, new object[0]),
					FieldReader = (FSharpFunction)FSharpUtils.PreComputeUnionReader(null, new object[] { obj, null }),
					Constructor = (FSharpFunction)FSharpUtils.PreComputeUnionConstructor(null, new object[] { obj, null })
				};
				union.Cases.Add(unionCase);
			}
			return union;
		}

		private static Type CreateUnionTypeLookup(Type t)
		{
			object obj = ((object[])FSharpUtils.GetUnionCases(null, new object[] { t, null })).First<object>();
			return (Type)FSharpUtils.GetUnionCaseInfoDeclaringType(obj);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Func<DiscriminatedUnionConverter.UnionCase, bool> func = null;
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			DiscriminatedUnionConverter.UnionCase unionCase = null;
			string str = null;
			JArray jArrays = null;
			reader.ReadAndAssert();
			while (reader.TokenType == JsonToken.PropertyName)
			{
				string str1 = reader.Value.ToString();
				if (!string.Equals(str1, "Case", StringComparison.OrdinalIgnoreCase))
				{
					if (!string.Equals(str1, "Fields", StringComparison.OrdinalIgnoreCase))
					{
						throw JsonSerializationException.Create(reader, "Unexpected property '{0}' found when reading union.".FormatWith(CultureInfo.InvariantCulture, str1));
					}
					reader.ReadAndAssert();
					if (reader.TokenType != JsonToken.StartArray)
					{
						throw JsonSerializationException.Create(reader, "Union fields must been an array.");
					}
					jArrays = (JArray)JToken.ReadFrom(reader);
				}
				else
				{
					reader.ReadAndAssert();
					DiscriminatedUnionConverter.Union union = DiscriminatedUnionConverter.UnionCache.Get(objectType);
					str = reader.Value.ToString();
					List<DiscriminatedUnionConverter.UnionCase> cases = union.Cases;
					Func<DiscriminatedUnionConverter.UnionCase, bool> func1 = func;
					if (func1 == null)
					{
						Func<DiscriminatedUnionConverter.UnionCase, bool> name = (DiscriminatedUnionConverter.UnionCase c) => c.Name == str;
						Func<DiscriminatedUnionConverter.UnionCase, bool> func2 = name;
						func = name;
						func1 = func2;
					}
					unionCase = cases.SingleOrDefault<DiscriminatedUnionConverter.UnionCase>(func1);
					if (unionCase == null)
					{
						throw JsonSerializationException.Create(reader, "No union type found with the name '{0}'.".FormatWith(CultureInfo.InvariantCulture, str));
					}
				}
				reader.ReadAndAssert();
			}
			if (unionCase == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union name found.".FormatWith(CultureInfo.InvariantCulture, "Case"));
			}
			object[] obj = new object[(int)unionCase.Fields.Length];
			if (unionCase.Fields.Length != 0 && jArrays == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union fields found.".FormatWith(CultureInfo.InvariantCulture, "Fields"));
			}
			if (jArrays != null)
			{
				if ((int)unionCase.Fields.Length != jArrays.Count)
				{
					throw JsonSerializationException.Create(reader, "The number of field values does not match the number of properties defined by union '{0}'.".FormatWith(CultureInfo.InvariantCulture, str));
				}
				for (int i = 0; i < jArrays.Count; i++)
				{
					JToken item = jArrays[i];
					PropertyInfo fields = unionCase.Fields[i];
					obj[i] = item.ToObject(fields.PropertyType, serializer);
				}
			}
			object[] objArray = new object[] { obj };
			return unionCase.Constructor.Invoke(objArray);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
			Type type = DiscriminatedUnionConverter.UnionTypeLookupCache.Get(value.GetType());
			DiscriminatedUnionConverter.Union union = DiscriminatedUnionConverter.UnionCache.Get(type);
			int num = (int)union.TagReader.Invoke(new object[] { value });
			DiscriminatedUnionConverter.UnionCase unionCase = union.Cases.Single<DiscriminatedUnionConverter.UnionCase>((DiscriminatedUnionConverter.UnionCase c) => c.Tag == num);
			writer.WriteStartObject();
			writer.WritePropertyName((contractResolver != null ? contractResolver.GetResolvedPropertyName("Case") : "Case"));
			writer.WriteValue(unionCase.Name);
			if (unionCase.Fields != null && unionCase.Fields.Length != 0)
			{
				object[] objArray = (object[])unionCase.FieldReader.Invoke(new object[] { value });
				writer.WritePropertyName((contractResolver != null ? contractResolver.GetResolvedPropertyName("Fields") : "Fields"));
				writer.WriteStartArray();
				object[] objArray1 = objArray;
				for (int i = 0; i < (int)objArray1.Length; i++)
				{
					serializer.Serialize(writer, objArray1[i]);
				}
				writer.WriteEndArray();
			}
			writer.WriteEndObject();
		}

		internal class Union
		{
			public List<DiscriminatedUnionConverter.UnionCase> Cases;

			public FSharpFunction TagReader
			{
				get;
				set;
			}

			public Union()
			{
				Class6.yDnXvgqzyB5jw();
				base();
			}
		}

		internal class UnionCase
		{
			public int Tag;

			public string Name;

			public PropertyInfo[] Fields;

			public FSharpFunction FieldReader;

			public FSharpFunction Constructor;

			public UnionCase()
			{
				Class6.yDnXvgqzyB5jw();
				base();
			}
		}
	}
}