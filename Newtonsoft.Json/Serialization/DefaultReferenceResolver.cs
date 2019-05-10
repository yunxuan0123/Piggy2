using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;

namespace Newtonsoft.Json.Serialization
{
	internal class DefaultReferenceResolver : GInterface2
	{
		private int _referenceCount;

		public DefaultReferenceResolver()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public void AddReference(object context, string reference, object value)
		{
			this.GetMappings(context).Set(reference, value);
		}

		private BidirectionalDictionary<string, object> GetMappings(object context)
		{
			JsonSerializerInternalBase jsonSerializerInternalBase = context as JsonSerializerInternalBase;
			JsonSerializerInternalBase internalSerializer = jsonSerializerInternalBase;
			if (jsonSerializerInternalBase == null)
			{
				JsonSerializerProxy jsonSerializerProxy = context as JsonSerializerProxy;
				JsonSerializerProxy jsonSerializerProxy1 = jsonSerializerProxy;
				if (jsonSerializerProxy == null)
				{
					throw new JsonException("The DefaultReferenceResolver can only be used internally.");
				}
				internalSerializer = jsonSerializerProxy1.GetInternalSerializer();
			}
			return internalSerializer.DefaultReferenceMappings;
		}

		public string GetReference(object context, object value)
		{
			string str;
			BidirectionalDictionary<string, object> mappings = this.GetMappings(context);
			if (!mappings.TryGetBySecond(value, out str))
			{
				this._referenceCount++;
				str = this._referenceCount.ToString(CultureInfo.InvariantCulture);
				mappings.Set(str, value);
			}
			return str;
		}

		public bool IsReferenced(object context, object value)
		{
			string str;
			return this.GetMappings(context).TryGetBySecond(value, out str);
		}

		public object ResolveReference(object context, string reference)
		{
			object obj;
			this.GetMappings(context).TryGetByFirst(reference, out obj);
			return obj;
		}
	}
}