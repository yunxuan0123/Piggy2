using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class BooleanQueryExpression : QueryExpression
	{
		public object Left
		{
			get;
			set;
		}

		public object Right
		{
			get;
			set;
		}

		public BooleanQueryExpression()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		internal static bool EqualsWithStrictMatch(JValue value, JValue queryValue)
		{
			if (value.Type == JTokenType.Integer && queryValue.Type == JTokenType.Float || value.Type == JTokenType.Float && queryValue.Type == JTokenType.Integer)
			{
				return JValue.Compare(value.Type, value.Value, queryValue.Value) == 0;
			}
			if (value.Type != queryValue.Type)
			{
				return false;
			}
			return value.Equals(queryValue);
		}

		internal static bool EqualsWithStringCoercion(JValue value, JValue queryValue)
		{
			string str;
			if (value.Equals(queryValue))
			{
				return true;
			}
			if (value.Type == JTokenType.Integer && queryValue.Type == JTokenType.Float || value.Type == JTokenType.Float && queryValue.Type == JTokenType.Integer)
			{
				return JValue.Compare(value.Type, value.Value, queryValue.Value) == 0;
			}
			if (queryValue.Type != JTokenType.String)
			{
				return false;
			}
			string str1 = (string)queryValue.Value;
			switch (value.Type)
			{
				case JTokenType.Date:
				{
					using (StringWriter stringWriter = StringUtils.CreateStringWriter(64))
					{
						object obj = value.Value;
						object obj1 = obj;
						if (!(obj is DateTimeOffset))
						{
							DateTimeUtils.WriteDateTimeString(stringWriter, (DateTime)value.Value, DateFormatHandling.IsoDateFormat, null, CultureInfo.InvariantCulture);
						}
						else
						{
							DateTimeOffset dateTimeOffset = (DateTimeOffset)obj1;
							DateTimeUtils.WriteDateTimeOffsetString(stringWriter, dateTimeOffset, DateFormatHandling.IsoDateFormat, null, CultureInfo.InvariantCulture);
						}
						str = stringWriter.ToString();
						break;
					}
					break;
				}
				case JTokenType.Raw:
				{
					return false;
				}
				case JTokenType.Bytes:
				{
					str = Convert.ToBase64String((byte[])value.Value);
					break;
				}
				case JTokenType.Guid:
				case JTokenType.TimeSpan:
				{
					str = value.Value.ToString();
					break;
				}
				case JTokenType.Uri:
				{
					str = ((Uri)value.Value).OriginalString;
					break;
				}
				default:
				{
					return false;
				}
			}
			return string.Equals(str, str1, StringComparison.Ordinal);
		}

		private IEnumerable<JToken> GetResult(JToken root, JToken t, object o)
		{
			JToken jTokens = o as JToken;
			JToken jTokens1 = jTokens;
			if (jTokens != null)
			{
				return new JToken[] { jTokens1 };
			}
			List<PathFilter> pathFilters = o as List<PathFilter>;
			List<PathFilter> pathFilters1 = pathFilters;
			if (pathFilters == null)
			{
				return CollectionUtils.ArrayEmpty<JToken>();
			}
			return JPath.Evaluate(pathFilters1, root, t, false);
		}

		public override bool IsMatch(JToken root, JToken t)
		{
			bool flag;
			if (base.Operator == QueryOperator.Exists)
			{
				return this.GetResult(root, t, this.Left).Any<JToken>();
			}
			using (IEnumerator<JToken> enumerator = this.GetResult(root, t, this.Left).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					IEnumerable<JToken> result = this.GetResult(root, t, this.Right);
					object list = result as ICollection<JToken>;
					if (list == null)
					{
						list = result.ToList<JToken>();
					}
					ICollection<JToken> jTokens = (ICollection<JToken>)list;
					do
					{
						JToken current = enumerator.Current;
						using (IEnumerator<JToken> enumerator1 = jTokens.GetEnumerator())
						{
							while (enumerator1.MoveNext())
							{
								if (!this.MatchTokens(current, enumerator1.Current))
								{
									continue;
								}
								flag = true;
								return flag;
							}
						}
					}
					while (enumerator.MoveNext());
				}
				return false;
			}
			return flag;
		}

		private bool MatchTokens(JToken leftResult, JToken rightResult)
		{
			JValue jValue = leftResult as JValue;
			JValue jValue1 = jValue;
			if (jValue != null)
			{
				JValue jValue2 = rightResult as JValue;
				JValue jValue3 = jValue2;
				if (jValue2 == null)
				{
					goto Label0;
				}
				switch (base.Operator)
				{
					case QueryOperator.Equals:
					{
						if (!BooleanQueryExpression.EqualsWithStringCoercion(jValue1, jValue3))
						{
							break;
						}
						return true;
					}
					case QueryOperator.NotEquals:
					{
						if (BooleanQueryExpression.EqualsWithStringCoercion(jValue1, jValue3))
						{
							break;
						}
						return true;
					}
					case QueryOperator.Exists:
					{
						return true;
					}
					case QueryOperator.LessThan:
					{
						if (jValue1.CompareTo(jValue3) >= 0)
						{
							break;
						}
						return true;
					}
					case QueryOperator.LessThanOrEquals:
					{
						if (jValue1.CompareTo(jValue3) > 0)
						{
							break;
						}
						return true;
					}
					case QueryOperator.GreaterThan:
					{
						if (jValue1.CompareTo(jValue3) <= 0)
						{
							break;
						}
						return true;
					}
					case QueryOperator.GreaterThanOrEquals:
					{
						if (jValue1.CompareTo(jValue3) < 0)
						{
							break;
						}
						return true;
					}
					case QueryOperator.RegexEquals:
					{
						if (!BooleanQueryExpression.RegexEquals(jValue1, jValue3))
						{
							break;
						}
						return true;
					}
					case QueryOperator.StrictEquals:
					{
						if (!BooleanQueryExpression.EqualsWithStrictMatch(jValue1, jValue3))
						{
							break;
						}
						return true;
					}
					case QueryOperator.StrictNotEquals:
					{
						if (BooleanQueryExpression.EqualsWithStrictMatch(jValue1, jValue3))
						{
							break;
						}
						return true;
					}
				}
			}
			else
			{
				goto Label0;
			}
			return false;
		Label0:
			if ((int)base.Operator - (int)QueryOperator.NotEquals <= (int)QueryOperator.Equals)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private static bool RegexEquals(JValue input, JValue pattern)
		{
			if (input.Type == JTokenType.String)
			{
				if (pattern.Type == JTokenType.String)
				{
					string value = (string)pattern.Value;
					int num = value.LastIndexOf('/');
					string str = value.Substring(1, num - 1);
					string str1 = value.Substring(num + 1);
					return Regex.IsMatch((string)input.Value, str, MiscellaneousUtils.GetRegexOptions(str1));
				}
			}
			return false;
		}
	}
}