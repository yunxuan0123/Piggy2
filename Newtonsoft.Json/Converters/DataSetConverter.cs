using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Data;

namespace Newtonsoft.Json.Converters
{
	public class DataSetConverter : JsonConverter
	{
		public DataSetConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override bool CanConvert(Type valueType)
		{
			return typeof(DataSet).IsAssignableFrom(valueType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			DataSet dataSet = (objectType == typeof(DataSet) ? new DataSet() : (DataSet)Activator.CreateInstance(objectType));
			DataTableConverter dataTableConverter = new DataTableConverter();
			reader.ReadAndAssert();
			while (reader.TokenType == JsonToken.PropertyName)
			{
				DataTable item = dataSet.Tables[(string)reader.Value];
				bool flag = item != null;
				item = (DataTable)dataTableConverter.ReadJson(reader, typeof(DataTable), item, serializer);
				if (!flag)
				{
					dataSet.Tables.Add(item);
				}
				reader.ReadAndAssert();
			}
			return dataSet;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			DataSet dataSet = (DataSet)value;
			DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
			DataTableConverter dataTableConverter = new DataTableConverter();
			writer.WriteStartObject();
			foreach (DataTable table in dataSet.Tables)
			{
				writer.WritePropertyName((contractResolver != null ? contractResolver.GetResolvedPropertyName(table.TableName) : table.TableName));
				dataTableConverter.WriteJson(writer, table, serializer);
			}
			writer.WriteEndObject();
		}
	}
}