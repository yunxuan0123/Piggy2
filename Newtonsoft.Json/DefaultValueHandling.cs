using System;

namespace Newtonsoft.Json
{
	[Flags]
	public enum DefaultValueHandling
	{
		Include,
		Ignore,
		Populate,
		IgnoreAndPopulate
	}
}