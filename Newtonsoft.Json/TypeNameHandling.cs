using System;

namespace Newtonsoft.Json
{
	[Flags]
	public enum TypeNameHandling
	{
		None,
		Objects,
		Arrays,
		All,
		Auto
	}
}