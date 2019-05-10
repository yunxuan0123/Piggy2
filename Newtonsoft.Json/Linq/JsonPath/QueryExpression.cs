using Newtonsoft.Json.Linq;
using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal abstract class QueryExpression
	{
		public QueryOperator Operator
		{
			get;
			set;
		}

		protected QueryExpression()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public abstract bool IsMatch(JToken root, JToken t);
	}
}