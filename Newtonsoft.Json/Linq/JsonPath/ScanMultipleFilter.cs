using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class ScanMultipleFilter : PathFilter
	{
		public List<string> Names
		{
			get;
			set;
		}

		public ScanMultipleFilter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			return new ScanMultipleFilter.<ExecuteFilter>d__4(-2)
			{
				<>4__this = this,
				<>3__current = current
			};
		}
	}
}