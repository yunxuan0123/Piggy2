using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	public class JsonISerializableContract : JsonContainerContract
	{
		public ObjectConstructor<object> ISerializableCreator
		{
			get;
			set;
		}

		public JsonISerializableContract(Type underlyingType)
		{
			Class6.yDnXvgqzyB5jw();
			base(underlyingType);
			this.ContractType = JsonContractType.Serializable;
		}
	}
}