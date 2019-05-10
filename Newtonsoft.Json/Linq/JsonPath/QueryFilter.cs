using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class QueryFilter : PathFilter
	{
		public QueryExpression Expression
		{
			get;
			set;
		}

		public QueryFilter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			return new QueryFilter.<ExecuteFilter>d__4(-2)
			{
				<>4__this = this,
				<>3__root = root,
				<>3__current = current
			};
		}
	}
}