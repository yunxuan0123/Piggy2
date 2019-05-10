using System;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
	internal class SerializationBinderAdapter : GInterface3
	{
		public readonly System.Runtime.Serialization.SerializationBinder SerializationBinder;

		public SerializationBinderAdapter(System.Runtime.Serialization.SerializationBinder serializationBinder)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.SerializationBinder = serializationBinder;
		}

		public void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			this.SerializationBinder.BindToName(serializedType, out assemblyName, out typeName);
		}

		public Type BindToType(string assemblyName, string typeName)
		{
			return this.SerializationBinder.BindToType(assemblyName, typeName);
		}
	}
}