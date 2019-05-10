using Newtonsoft.Json;
using System;

namespace Newtonsoft.Json.Utilities
{
	internal static class JsonTokenUtils
	{
		internal static bool IsEndToken(JsonToken token)
		{
			if ((int)token - (int)JsonToken.EndObject <= (int)JsonToken.StartArray)
			{
				return true;
			}
			return false;
		}

		internal static bool IsPrimitiveToken(JsonToken token)
		{
			if ((int)token - (int)JsonToken.Integer > (int)JsonToken.Comment && (int)token - (int)JsonToken.Date > (int)JsonToken.StartObject)
			{
				return false;
			}
			return true;
		}

		internal static bool IsStartToken(JsonToken token)
		{
			if ((int)token - (int)JsonToken.StartObject <= (int)JsonToken.StartArray)
			{
				return true;
			}
			return false;
		}
	}
}