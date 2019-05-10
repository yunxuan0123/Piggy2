using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class ArraySliceFilter : PathFilter
	{
		public int? End
		{
			get;
			set;
		}

		public int? Start
		{
			get;
			set;
		}

		public int? Step
		{
			get;
			set;
		}

		public ArraySliceFilter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			return new ArraySliceFilter.<ExecuteFilter>d__12(-2)
			{
				<>4__this = this,
				<>3__current = current,
				<>3__errorWhenNoMatch = errorWhenNoMatch
			};
		}

		private bool IsValid(int index, int stopIndex, bool positiveStep)
		{
			if (positiveStep)
			{
				return index < stopIndex;
			}
			return index > stopIndex;
		}
	}
}