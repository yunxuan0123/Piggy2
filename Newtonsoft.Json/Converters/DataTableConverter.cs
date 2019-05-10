using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Newtonsoft.Json.Converters
{
	public class DataTableConverter : JsonConverter
	{
		public DataTableConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override bool CanConvert(Type valueType)
		{
			return typeof(DataTable).IsAssignableFrom(valueType);
		}

		private static void CreateRow(JsonReader reader, DataTable dt, JsonSerializer serializer)
		{
			object value;
			DataRow dataRow = dt.NewRow();
			reader.ReadAndAssert();
			while (reader.TokenType == JsonToken.PropertyName)
			{
				string str = (string)reader.Value;
				reader.ReadAndAssert();
				DataColumn item = dt.Columns[str];
				if (item == null)
				{
					item = new DataColumn(str, DataTableConverter.GetColumnDataType(reader));
					dt.Columns.Add(item);
				}
				if (item.DataType == typeof(DataTable))
				{
					if (reader.TokenType == JsonToken.StartArray)
					{
						reader.ReadAndAssert();
					}
					DataTable dataTable = new DataTable();
					while (reader.TokenType != JsonToken.EndArray)
					{
						DataTableConverter.CreateRow(reader, dataTable, serializer);
						reader.ReadAndAssert();
					}
					dataRow[str] = dataTable;
				}
				else if (!item.DataType.IsArray || !(item.DataType != typeof(byte[])))
				{
					if (reader.Value != null)
					{
						value = serializer.Deserialize(reader, item.DataType);
						if (value == null)
						{
							value = DBNull.Value;
						}
					}
					else
					{
						value = DBNull.Value;
					}
					dataRow[str] = value;
				}
				else
				{
					if (reader.TokenType == JsonToken.StartArray)
					{
						reader.ReadAndAssert();
					}
					List<object> objs = new List<object>();
					while (reader.TokenType != JsonToken.EndArray)
					{
						objs.Add(reader.Value);
						reader.ReadAndAssert();
					}
					Array arrays = Array.CreateInstance(item.DataType.GetElementType(), objs.Count);
					((ICollection)objs).CopyTo(arrays, 0);
					dataRow[str] = arrays;
				}
				reader.ReadAndAssert();
			}
			dataRow.EndEdit();
			dt.Rows.Add(dataRow);
		}

		private static Type GetColumnDataType(JsonReader reader)
		{
			JsonToken tokenType = reader.TokenType;
			switch (tokenType)
			{
				case JsonToken.StartArray:
				{
					reader.ReadAndAssert();
					if (reader.TokenType == JsonToken.StartObject)
					{
						return typeof(DataTable);
					}
					return DataTableConverter.GetColumnDataType(reader).MakeArrayType();
				}
				case JsonToken.StartConstructor:
				case JsonToken.PropertyName:
				case JsonToken.Comment:
				case JsonToken.Raw:
				case JsonToken.EndObject:
				case JsonToken.EndConstructor:
				{
					throw JsonSerializationException.Create(reader, "Unexpected JSON token when reading DataTable: {0}".FormatWith(CultureInfo.InvariantCulture, tokenType));
				}
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Date:
				case JsonToken.Bytes:
				{
					return reader.ValueType;
				}
				case JsonToken.Null:
				case JsonToken.Undefined:
				case JsonToken.EndArray:
				{
					return typeof(string);
				}
				default:
				{
					throw JsonSerializationException.Create(reader, "Unexpected JSON token when reading DataTable: {0}".FormatWith(CultureInfo.InvariantCulture, tokenType));
				}
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			DataTable dataTable = existingValue as DataTable;
			DataTable value = dataTable;
			if (dataTable == null)
			{
				value = (objectType == typeof(DataTable) ? new DataTable() : (DataTable)Activator.CreateInstance(objectType));
			}
			if (reader.TokenType == JsonToken.PropertyName)
			{
				value.TableName = (string)reader.Value;
				reader.ReadAndAssert();
				if (reader.TokenType == JsonToken.Null)
				{
					return value;
				}
			}
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw JsonSerializationException.Create(reader, "Unexpected JSON token when reading DataTable. Expected StartArray, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			reader.ReadAndAssert();
			while (reader.TokenType != JsonToken.EndArray)
			{
				DataTableConverter.CreateRow(reader, value, serializer);
				reader.ReadAndAssert();
			}
			return value;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			DataTable dataTable = (DataTable)value;
			DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
			writer.WriteStartArray();
			foreach (DataRow row in dataTable.Rows)
			{
				writer.WriteStartObject();
				foreach (DataColumn column in row.Table.Columns)
				{
					object item = row[column];
					if (serializer.NullValueHandling == NullValueHandling.Ignore && (item == null || item == DBNull.Value))
					{
						continue;
					}
					writer.WritePropertyName((contractResolver != null ? contractResolver.GetResolvedPropertyName(column.ColumnName) : column.ColumnName));
					serializer.Serialize(writer, item);
				}
				writer.WriteEndObject();
			}
			writer.WriteEndArray();
		}
	}
}