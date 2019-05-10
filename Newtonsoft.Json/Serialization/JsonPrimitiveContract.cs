using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	public class JsonPrimitiveContract : JsonContract
	{
		private readonly static Dictionary<Type, ReadType> ReadTypeMap;

		internal PrimitiveTypeCode TypeCode
		{
			get;
			set;
		}

		static JsonPrimitiveContract()
		{
			Class6.yDnXvgqzyB5jw();
			Dictionary<Type, ReadType> types = new Dictionary<Type, ReadType>();
			types[typeof(byte[])] = ReadType.ReadAsBytes;
			types[typeof(byte)] = ReadType.const_1;
			types[typeof(short)] = ReadType.const_1;
			types[typeof(int)] = ReadType.const_1;
			types[typeof(decimal)] = ReadType.ReadAsDecimal;
			types[typeof(bool)] = ReadType.ReadAsBoolean;
			types[typeof(string)] = ReadType.ReadAsString;
			types[typeof(DateTime)] = ReadType.ReadAsDateTime;
			types[typeof(DateTimeOffset)] = ReadType.ReadAsDateTimeOffset;
			types[typeof(float)] = ReadType.ReadAsDouble;
			types[typeof(double)] = ReadType.ReadAsDouble;
			types[typeof(long)] = ReadType.const_2;
			JsonPrimitiveContract.ReadTypeMap = types;
		}

		public JsonPrimitiveContract(Type underlyingType)
		{
			Class6.yDnXvgqzyB5jw();
			base(underlyingType);
			ReadType readType;
			this.ContractType = JsonContractType.Primitive;
			this.TypeCode = ConvertUtils.GetTypeCode(underlyingType);
			this.IsReadOnlyOrFixedSize = true;
			if (JsonPrimitiveContract.ReadTypeMap.TryGetValue(this.NonNullableUnderlyingType, out readType))
			{
				this.InternalReadType = readType;
			}
		}
	}
}