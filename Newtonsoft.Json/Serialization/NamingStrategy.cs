using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	public abstract class NamingStrategy
	{
		public bool OverrideSpecifiedNames
		{
			get;
			set;
		}

		public bool ProcessDictionaryKeys
		{
			get;
			set;
		}

		public bool ProcessExtensionDataNames
		{
			get;
			set;
		}

		protected NamingStrategy()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public virtual string GetDictionaryKey(string key)
		{
			if (!this.ProcessDictionaryKeys)
			{
				return key;
			}
			return this.ResolvePropertyName(key);
		}

		public virtual string GetExtensionDataName(string name)
		{
			if (!this.ProcessExtensionDataNames)
			{
				return name;
			}
			return this.ResolvePropertyName(name);
		}

		public virtual string GetPropertyName(string name, bool hasSpecifiedName)
		{
			if (hasSpecifiedName && !this.OverrideSpecifiedNames)
			{
				return name;
			}
			return this.ResolvePropertyName(name);
		}

		protected abstract string ResolvePropertyName(string name);
	}
}