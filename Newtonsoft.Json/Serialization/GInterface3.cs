using System;

namespace Newtonsoft.Json.Serialization
{
	public interface GInterface3
	{
		void BindToName(Type serializedType, out string assemblyName, out string typeName);

		Type BindToType(string assemblyName, string typeName);
	}
}