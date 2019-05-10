using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Serialization
{
	public interface GInterface1
	{
		IList<Attribute> GetAttributes(bool inherit);

		IList<Attribute> GetAttributes(Type attributeType, bool inherit);
	}
}