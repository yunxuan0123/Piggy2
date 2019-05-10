using System;

namespace Newtonsoft.Json
{
	[Flags]
	public enum PreserveReferencesHandling
	{
		None,
		Objects,
		Arrays,
		All
	}
}