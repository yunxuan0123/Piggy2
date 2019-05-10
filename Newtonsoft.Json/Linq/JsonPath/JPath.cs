using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class JPath
	{
		private readonly static char[] FloatCharacters;

		private readonly string _expression;

		private int _currentIndex;

		public List<PathFilter> Filters
		{
			get;
		}

		static JPath()
		{
			Class6.yDnXvgqzyB5jw();
			JPath.FloatCharacters = new char[] { '.', 'E', 'e' };
		}

		public JPath(string expression)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			ValidationUtils.ArgumentNotNull(expression, "expression");
			this._expression = expression;
			this.Filters = new List<PathFilter>();
			this.ParseMain();
		}

		private static PathFilter CreatePathFilter(string member, bool scan)
		{
			if (!scan)
			{
				return new FieldFilter()
				{
					Name = member
				};
			}
			return new ScanFilter()
			{
				Name = member
			};
		}

		private JsonException CreateUnexpectedCharacterException()
		{
			char chr = this._expression[this._currentIndex];
			return new JsonException(string.Concat("Unexpected character while parsing path query: ", chr.ToString()));
		}

		private void EatWhitespace()
		{
			while (this._currentIndex < this._expression.Length && this._expression[this._currentIndex] == ' ')
			{
				this._currentIndex++;
			}
		}

		private void EnsureLength(string message)
		{
			if (this._currentIndex >= this._expression.Length)
			{
				throw new JsonException(message);
			}
		}

		internal IEnumerable<JToken> Evaluate(JToken root, JToken t, bool errorWhenNoMatch)
		{
			return JPath.Evaluate(this.Filters, root, t, errorWhenNoMatch);
		}

		internal static IEnumerable<JToken> Evaluate(List<PathFilter> filters, JToken root, JToken t, bool errorWhenNoMatch)
		{
			IEnumerable<JToken> jTokens = new JToken[] { t };
			foreach (PathFilter filter in filters)
			{
				jTokens = filter.ExecuteFilter(root, jTokens, errorWhenNoMatch);
			}
			return jTokens;
		}

		private bool Match(string s)
		{
			int num = this._currentIndex;
			string str = s;
			for (int i = 0; i < str.Length; i++)
			{
				char chr = str[i];
				if (num >= this._expression.Length || this._expression[num] != chr)
				{
					return false;
				}
				num++;
			}
			this._currentIndex = num;
			return true;
		}

		private PathFilter ParseArrayIndexer(char indexerCloseChar)
		{
			int? nullable;
			int num = this._currentIndex;
			int? nullable1 = null;
			List<int> nums = null;
			int num1 = 0;
			int? nullable2 = null;
			int? nullable3 = null;
			int? nullable4 = null;
			while (this._currentIndex < this._expression.Length)
			{
				char chr = this._expression[this._currentIndex];
				if (chr != ' ')
				{
					if (chr == indexerCloseChar)
					{
						nullable = nullable1;
						int num2 = (nullable.HasValue ? nullable.GetValueOrDefault() : this._currentIndex) - num;
						if (nums != null)
						{
							if (num2 == 0)
							{
								throw new JsonException("Array index expected.");
							}
							int num3 = Convert.ToInt32(this._expression.Substring(num, num2), CultureInfo.InvariantCulture);
							nums.Add(num3);
							return new ArrayMultipleIndexFilter()
							{
								Indexes = nums
							};
						}
						if (num1 <= 0)
						{
							if (num2 == 0)
							{
								throw new JsonException("Array index expected.");
							}
							int num4 = Convert.ToInt32(this._expression.Substring(num, num2), CultureInfo.InvariantCulture);
							return new ArrayIndexFilter()
							{
								Index = new int?(num4)
							};
						}
						if (num2 > 0)
						{
							int num5 = Convert.ToInt32(this._expression.Substring(num, num2), CultureInfo.InvariantCulture);
							if (num1 != 1)
							{
								nullable4 = new int?(num5);
							}
							else
							{
								nullable3 = new int?(num5);
							}
						}
						return new ArraySliceFilter()
						{
							Start = nullable2,
							End = nullable3,
							Step = nullable4
						};
					}
					if (chr != ',')
					{
						if (chr == '*')
						{
							this._currentIndex++;
							this.EnsureLength("Path ended with open indexer.");
							this.EatWhitespace();
							if (this._expression[this._currentIndex] != indexerCloseChar)
							{
								throw new JsonException(string.Concat("Unexpected character while parsing path indexer: ", chr.ToString()));
							}
							return new ArrayIndexFilter();
						}
						if (chr != ':')
						{
							if (!char.IsDigit(chr) && chr != '-')
							{
								throw new JsonException(string.Concat("Unexpected character while parsing path indexer: ", chr.ToString()));
							}
							if (nullable1.HasValue)
							{
								throw new JsonException(string.Concat("Unexpected character while parsing path indexer: ", chr.ToString()));
							}
							this._currentIndex++;
						}
						else
						{
							nullable = nullable1;
							int num6 = (nullable.HasValue ? nullable.GetValueOrDefault() : this._currentIndex) - num;
							if (num6 > 0)
							{
								int num7 = Convert.ToInt32(this._expression.Substring(num, num6), CultureInfo.InvariantCulture);
								if (num1 == 0)
								{
									nullable2 = new int?(num7);
								}
								else if (num1 != 1)
								{
									nullable4 = new int?(num7);
								}
								else
								{
									nullable3 = new int?(num7);
								}
							}
							num1++;
							this._currentIndex++;
							this.EatWhitespace();
							num = this._currentIndex;
							nullable1 = null;
						}
					}
					else
					{
						nullable = nullable1;
						int num8 = (nullable.HasValue ? nullable.GetValueOrDefault() : this._currentIndex) - num;
						if (num8 == 0)
						{
							throw new JsonException("Array index expected.");
						}
						if (nums == null)
						{
							nums = new List<int>();
						}
						string str = this._expression.Substring(num, num8);
						nums.Add(Convert.ToInt32(str, CultureInfo.InvariantCulture));
						this._currentIndex++;
						this.EatWhitespace();
						num = this._currentIndex;
						nullable1 = null;
					}
				}
				else
				{
					nullable1 = new int?(this._currentIndex);
					this.EatWhitespace();
				}
			}
			throw new JsonException("Path ended with open indexer.");
		}

		private QueryExpression ParseExpression()
		{
			QueryOperator queryOperator;
			QueryExpression queryExpression = null;
			CompositeExpression compositeExpression = null;
			while (this._currentIndex < this._expression.Length)
			{
				object obj = this.ParseSide();
				object obj1 = null;
				if (this._expression[this._currentIndex] != ')' && this._expression[this._currentIndex] != '|')
				{
					if (this._expression[this._currentIndex] == '&')
					{
						goto Label2;
					}
					queryOperator = this.ParseOperator();
					obj1 = this.ParseSide();
					goto Label0;
				}
			Label2:
				queryOperator = QueryOperator.Exists;
			Label0:
				BooleanQueryExpression booleanQueryExpression = new BooleanQueryExpression()
				{
					Left = obj,
					Operator = queryOperator,
					Right = obj1
				};
				BooleanQueryExpression booleanQueryExpression1 = booleanQueryExpression;
				if (this._expression[this._currentIndex] == ')')
				{
					if (compositeExpression == null)
					{
						return booleanQueryExpression1;
					}
					compositeExpression.Expressions.Add(booleanQueryExpression1);
					return queryExpression;
				}
				if (this._expression[this._currentIndex] == '&')
				{
					if (!this.Match("&&"))
					{
						throw this.CreateUnexpectedCharacterException();
					}
					if (compositeExpression == null || compositeExpression.Operator != QueryOperator.And)
					{
						CompositeExpression compositeExpression1 = new CompositeExpression()
						{
							Operator = QueryOperator.And
						};
						if (compositeExpression != null)
						{
							compositeExpression.Expressions.Add(compositeExpression1);
						}
						compositeExpression = compositeExpression1;
						if (queryExpression == null)
						{
							queryExpression = compositeExpression;
						}
					}
					compositeExpression.Expressions.Add(booleanQueryExpression1);
				}
				if (this._expression[this._currentIndex] != '|')
				{
					continue;
				}
				if (!this.Match("||"))
				{
					throw this.CreateUnexpectedCharacterException();
				}
				if (compositeExpression == null || compositeExpression.Operator != QueryOperator.Or)
				{
					CompositeExpression compositeExpression2 = new CompositeExpression()
					{
						Operator = QueryOperator.Or
					};
					if (compositeExpression != null)
					{
						compositeExpression.Expressions.Add(compositeExpression2);
					}
					compositeExpression = compositeExpression2;
					if (queryExpression == null)
					{
						queryExpression = compositeExpression;
					}
				}
				compositeExpression.Expressions.Add(booleanQueryExpression1);
			}
			throw new JsonException("Path ended with open query.");
		}

		private PathFilter ParseIndexer(char indexerOpenChar, bool scan)
		{
			this._currentIndex++;
			char chr = (indexerOpenChar == '[' ? ']' : ')');
			this.EnsureLength("Path ended with open indexer.");
			this.EatWhitespace();
			if (this._expression[this._currentIndex] == '\'')
			{
				return this.ParseQuotedField(chr, scan);
			}
			if (this._expression[this._currentIndex] != '?')
			{
				return this.ParseArrayIndexer(chr);
			}
			return this.ParseQuery(chr, scan);
		}

		private void ParseMain()
		{
			int num = this._currentIndex;
			this.EatWhitespace();
			if (this._expression.Length == this._currentIndex)
			{
				return;
			}
			if (this._expression[this._currentIndex] == '$')
			{
				if (this._expression.Length == 1)
				{
					return;
				}
				char chr = this._expression[this._currentIndex + 1];
				if (chr == '.' || chr == '[')
				{
					this._currentIndex++;
					num = this._currentIndex;
				}
			}
			if (!this.ParsePath(this.Filters, num, false))
			{
				int num1 = this._currentIndex;
				this.EatWhitespace();
				if (this._currentIndex < this._expression.Length)
				{
					char chr1 = this._expression[num1];
					throw new JsonException(string.Concat("Unexpected character while parsing path: ", chr1.ToString()));
				}
			}
		}

		private QueryOperator ParseOperator()
		{
			if (this._currentIndex + 1 >= this._expression.Length)
			{
				throw new JsonException("Path ended with open query.");
			}
			if (this.Match("==="))
			{
				return QueryOperator.StrictEquals;
			}
			if (this.Match("=="))
			{
				return QueryOperator.Equals;
			}
			if (this.Match("=~"))
			{
				return QueryOperator.RegexEquals;
			}
			if (this.Match("!=="))
			{
				return QueryOperator.StrictNotEquals;
			}
			if (this.Match("!=") || this.Match("<>"))
			{
				return QueryOperator.NotEquals;
			}
			if (this.Match("<="))
			{
				return QueryOperator.LessThanOrEquals;
			}
			if (this.Match("<"))
			{
				return QueryOperator.LessThan;
			}
			if (this.Match(">="))
			{
				return QueryOperator.GreaterThanOrEquals;
			}
			if (!this.Match(">"))
			{
				throw new JsonException("Could not read query operator.");
			}
			return QueryOperator.GreaterThan;
		}

		private bool ParsePath(List<PathFilter> filters, int currentPartStartIndex, bool query)
		{
			char chr;
			bool flag = false;
			bool flag1 = false;
			bool flag2 = false;
			bool flag3 = false;
		Label3:
			while (this._currentIndex < this._expression.Length)
			{
				if (flag3)
				{
					break;
				}
				chr = this._expression[this._currentIndex];
				if (chr <= ')')
				{
					if (chr == ' ')
					{
						if (this._currentIndex >= this._expression.Length)
						{
							continue;
						}
						flag3 = true;
						continue;
					}
					else
					{
						if (chr == '(')
						{
							goto Label2;
						}
						if (chr == ')')
						{
							goto Label0;
						}
						goto Label1;
					}
				}
				else if (chr == '.')
				{
					if (this._currentIndex > currentPartStartIndex)
					{
						string str = this._expression.Substring(currentPartStartIndex, this._currentIndex - currentPartStartIndex);
						if (str == "*")
						{
							str = null;
						}
						filters.Add(JPath.CreatePathFilter(str, flag));
						flag = false;
					}
					if (this._currentIndex + 1 < this._expression.Length && this._expression[this._currentIndex + 1] == '.')
					{
						flag = true;
						this._currentIndex++;
					}
					this._currentIndex++;
					currentPartStartIndex = this._currentIndex;
					flag1 = false;
					flag2 = true;
					continue;
				}
				else
				{
					if (chr == '[')
					{
						goto Label2;
					}
					if (chr == ']')
					{
						goto Label0;
					}
					else
					{
						goto Label1;
					}
				}
			Label2:
				if (this._currentIndex > currentPartStartIndex)
				{
					string str1 = this._expression.Substring(currentPartStartIndex, this._currentIndex - currentPartStartIndex);
					if (str1 == "*")
					{
						str1 = null;
					}
					filters.Add(JPath.CreatePathFilter(str1, flag));
					flag = false;
				}
				filters.Add(this.ParseIndexer(chr, flag));
				this._currentIndex++;
				currentPartStartIndex = this._currentIndex;
				flag1 = true;
				flag2 = false;
			}
			bool length = this._currentIndex == this._expression.Length;
			if (this._currentIndex > currentPartStartIndex)
			{
				string str2 = this._expression.Substring(currentPartStartIndex, this._currentIndex - currentPartStartIndex).TrimEnd(new char[0]);
				if (str2 == "*")
				{
					str2 = null;
				}
				filters.Add(JPath.CreatePathFilter(str2, flag));
			}
			else if (flag2 && length | query)
			{
				throw new JsonException("Unexpected end while parsing path.");
			}
			return length;
		Label0:
			flag3 = true;
			goto Label3;
		Label1:
			if (!query || chr != '=' && chr != '<' && chr != '!' && chr != '>' && chr != '|' && chr != '&')
			{
				if (flag1)
				{
					throw new JsonException(string.Concat("Unexpected character following indexer: ", chr.ToString()));
				}
				this._currentIndex++;
				goto Label3;
			}
			else
			{
				flag3 = true;
				goto Label3;
			}
		}

		private PathFilter ParseQuery(char indexerCloseChar, bool scan)
		{
			char chr;
			this._currentIndex++;
			this.EnsureLength("Path ended with open indexer.");
			if (this._expression[this._currentIndex] != '(')
			{
				chr = this._expression[this._currentIndex];
				throw new JsonException(string.Concat("Unexpected character while parsing path indexer: ", chr.ToString()));
			}
			this._currentIndex++;
			QueryExpression queryExpression = this.ParseExpression();
			this._currentIndex++;
			this.EnsureLength("Path ended with open indexer.");
			this.EatWhitespace();
			if (this._expression[this._currentIndex] != indexerCloseChar)
			{
				chr = this._expression[this._currentIndex];
				throw new JsonException(string.Concat("Unexpected character while parsing path indexer: ", chr.ToString()));
			}
			if (!scan)
			{
				return new QueryFilter()
				{
					Expression = queryExpression
				};
			}
			return new QueryScanFilter()
			{
				Expression = queryExpression
			};
		}

		private PathFilter ParseQuotedField(char indexerCloseChar, bool scan)
		{
			List<string> strs = null;
			while (this._currentIndex < this._expression.Length)
			{
				string str = this.ReadQuotedString();
				this.EatWhitespace();
				this.EnsureLength("Path ended with open indexer.");
				if (this._expression[this._currentIndex] == indexerCloseChar)
				{
					if (strs == null)
					{
						return JPath.CreatePathFilter(str, scan);
					}
					strs.Add(str);
					if (!scan)
					{
						return new FieldMultipleFilter()
						{
							Names = strs
						};
					}
					return new ScanMultipleFilter()
					{
						Names = strs
					};
				}
				if (this._expression[this._currentIndex] != ',')
				{
					char chr = this._expression[this._currentIndex];
					throw new JsonException(string.Concat("Unexpected character while parsing path indexer: ", chr.ToString()));
				}
				this._currentIndex++;
				this.EatWhitespace();
				if (strs == null)
				{
					strs = new List<string>();
				}
				strs.Add(str);
			}
			throw new JsonException("Path ended with open indexer.");
		}

		private object ParseSide()
		{
			List<PathFilter> pathFilters;
			object obj;
			this.EatWhitespace();
			if (this.TryParseExpression(out pathFilters))
			{
				this.EatWhitespace();
				this.EnsureLength("Path ended with open query.");
				return pathFilters;
			}
			if (!this.TryParseValue(out obj))
			{
				throw this.CreateUnexpectedCharacterException();
			}
			this.EatWhitespace();
			this.EnsureLength("Path ended with open query.");
			return new JValue(obj);
		}

		private string ReadQuotedString()
		{
			char chr;
			char chr1;
			StringBuilder stringBuilder = new StringBuilder();
			this._currentIndex++;
			while (true)
			{
				if (this._currentIndex >= this._expression.Length)
				{
					throw new JsonException("Path ended with an open string.");
				}
				chr = this._expression[this._currentIndex];
				if (chr != '\\' || this._currentIndex + 1 >= this._expression.Length)
				{
					if (chr == '\'')
					{
						this._currentIndex++;
						return stringBuilder.ToString();
					}
					this._currentIndex++;
					stringBuilder.Append(chr);
				}
				else
				{
					this._currentIndex++;
					chr = this._expression[this._currentIndex];
					if (chr <= '\\')
					{
						if (chr <= '\'')
						{
							if (chr != '\"' && chr != '\'')
							{
								break;
							}
						}
						else if (chr != '/' && chr != '\\')
						{
							break;
						}
						chr1 = chr;
					}
					else if (chr <= 'f')
					{
						if (chr == 'b')
						{
							chr1 = '\b';
						}
						else if (chr == 'f')
						{
							chr1 = '\f';
						}
						else
						{
							break;
						}
					}
					else if (chr == 'n')
					{
						chr1 = '\n';
					}
					else if (chr == 'r')
					{
						chr1 = '\r';
					}
					else if (chr == 't')
					{
						chr1 = '\t';
					}
					else
					{
						break;
					}
					stringBuilder.Append(chr1);
					this._currentIndex++;
				}
			}
			throw new JsonException(string.Concat("Unknown escape character: \\", chr.ToString()));
		}

		private string ReadRegexString()
		{
			int num = this._currentIndex;
			this._currentIndex++;
			while (this._currentIndex < this._expression.Length)
			{
				char chr = this._expression[this._currentIndex];
				if (chr != '\\' || this._currentIndex + 1 >= this._expression.Length)
				{
					if (chr == '/')
					{
						this._currentIndex++;
						while (this._currentIndex < this._expression.Length)
						{
							chr = this._expression[this._currentIndex];
							if (!char.IsLetter(chr))
							{
								break;
							}
							this._currentIndex++;
						}
						return this._expression.Substring(num, this._currentIndex - num);
					}
					this._currentIndex++;
				}
				else
				{
					this._currentIndex += 2;
				}
			}
			throw new JsonException("Path ended with an open regex.");
		}

		private bool TryParseExpression(out List<PathFilter> expressionPath)
		{
			if (this._expression[this._currentIndex] != '$')
			{
				if (this._expression[this._currentIndex] != '@')
				{
					expressionPath = null;
					return false;
				}
				expressionPath = new List<PathFilter>();
			}
			else
			{
				expressionPath = new List<PathFilter>()
				{
					RootFilter.Instance
				};
			}
			this._currentIndex++;
			if (this.ParsePath(expressionPath, this._currentIndex, true))
			{
				throw new JsonException("Path ended with open query.");
			}
			return true;
		}

		private bool TryParseValue(out object value)
		{
			double num;
			long num1;
			char chr = this._expression[this._currentIndex];
			if (chr == '\'')
			{
				value = this.ReadQuotedString();
				return true;
			}
			if (!char.IsDigit(chr))
			{
				if (chr == '-')
				{
					goto Label0;
				}
				if (chr == 't')
				{
					if (this.Match("true"))
					{
						value = true;
						return true;
					}
				}
				else if (chr == 'f')
				{
					if (this.Match("false"))
					{
						value = false;
						return true;
					}
				}
				else if (chr == 'n')
				{
					if (this.Match("null"))
					{
						value = null;
						return true;
					}
				}
				else if (chr == '/')
				{
					value = this.ReadRegexString();
					return true;
				}
			}
			else
			{
				goto Label0;
			}
			value = null;
			return false;
		Label0:
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(chr);
			this._currentIndex++;
			while (true)
			{
				if (this._currentIndex >= this._expression.Length)
				{
					value = null;
					return false;
				}
				chr = this._expression[this._currentIndex];
				if (chr == ' ')
				{
					break;
				}
				if (chr != ')')
				{
					stringBuilder.Append(chr);
					this._currentIndex++;
				}
				else
				{
					break;
				}
			}
			string str = stringBuilder.ToString();
			if (str.IndexOfAny(JPath.FloatCharacters) != -1)
			{
				bool flag = double.TryParse(str, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.Integer | NumberStyles.Float, CultureInfo.InvariantCulture, out num);
				value = num;
				return flag;
			}
			bool flag1 = long.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out num1);
			value = num1;
			return flag1;
		}
	}
}