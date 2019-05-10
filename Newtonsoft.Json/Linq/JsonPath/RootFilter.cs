using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class RootFilter : PathFilter
	{
		public readonly static RootFilter Instance;

		static RootFilter()
		{
			Class6.yDnXvgqzyB5jw();
			RootFilter.Instance = new RootFilter();
		}

		private RootFilter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			return new JToken[] { root };
		}
	}
}