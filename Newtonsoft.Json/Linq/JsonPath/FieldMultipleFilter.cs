using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class FieldMultipleFilter : PathFilter
	{
		public List<string> Names
		{
			get;
			set;
		}

		public FieldMultipleFilter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			return new FieldMultipleFilter.<ExecuteFilter>d__4(-2)
			{
				<>4__this = this,
				<>3__current = current,
				<>3__errorWhenNoMatch = errorWhenNoMatch
			};
		}
	}
}