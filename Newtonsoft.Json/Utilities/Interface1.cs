using System;
using System.Collections;

namespace Newtonsoft.Json.Utilities
{
	internal interface Interface1 : IEnumerable, IList, ICollection
	{
		object UnderlyingCollection
		{
			get;
		}
	}
}