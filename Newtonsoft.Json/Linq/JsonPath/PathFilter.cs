using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal abstract class PathFilter
	{
		protected PathFilter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public abstract IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch);

		protected static JToken GetNextScanValue(JToken originalParent, JToken container, JToken value)
		{
			if (container == null || !container.HasValues)
			{
				while (value != null && value != originalParent)
				{
					if (value == value.Parent.Last)
					{
						value = value.Parent;
					}
					else
					{
						break;
					}
				}
				if (value != null)
				{
					if (value == originalParent)
					{
						return null;
					}
					value = value.Next;
					return value;
				}
				return null;
			}
			else
			{
				value = container.First;
			}
			return value;
		}

		protected static JToken GetTokenIndex(JToken t, bool errorWhenNoMatch, int index)
		{
			JArray jArrays = t as JArray;
			JArray jArrays1 = jArrays;
			if (jArrays != null)
			{
				if (jArrays1.Count > index)
				{
					return jArrays1[index];
				}
				if (errorWhenNoMatch)
				{
					throw new JsonException("Index {0} outside the bounds of JArray.".FormatWith(CultureInfo.InvariantCulture, index));
				}
				return null;
			}
			JConstructor jConstructor = t as JConstructor;
			JConstructor jConstructor1 = jConstructor;
			if (jConstructor == null)
			{
				if (errorWhenNoMatch)
				{
					throw new JsonException("Index {0} not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, index, t.GetType().Name));
				}
				return null;
			}
			if (jConstructor1.Count > index)
			{
				return jConstructor1[index];
			}
			if (errorWhenNoMatch)
			{
				throw new JsonException("Index {0} outside the bounds of JConstructor.".FormatWith(CultureInfo.InvariantCulture, index));
			}
			return null;
		}
	}
}