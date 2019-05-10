using System;

namespace Newtonsoft.Json.Utilities
{
	internal class EnumInfo
	{
		public readonly bool IsFlags;

		public readonly ulong[] Values;

		public readonly string[] Names;

		public readonly string[] ResolvedNames;

		public EnumInfo(bool isFlags, ulong[] values, string[] names, string[] resolvedNames)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.IsFlags = isFlags;
			this.Values = values;
			this.Names = names;
			this.ResolvedNames = resolvedNames;
		}
	}
}