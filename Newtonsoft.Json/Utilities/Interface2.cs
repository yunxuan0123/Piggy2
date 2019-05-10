using System;
using System.Collections;

namespace Newtonsoft.Json.Utilities
{
	internal interface Interface2 : IEnumerable, ICollection, IDictionary
	{
		object UnderlyingDictionary
		{
			get;
		}
	}
}