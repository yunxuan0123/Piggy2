using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Serialization
{
	public class CamelCaseNamingStrategy : NamingStrategy
	{
		public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames)
		{
			Class6.yDnXvgqzyB5jw();
			this(processDictionaryKeys, overrideSpecifiedNames);
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		public CamelCaseNamingStrategy()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToCamelCase(name);
		}
	}
}