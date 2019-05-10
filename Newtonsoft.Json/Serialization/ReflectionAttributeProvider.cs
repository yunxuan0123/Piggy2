using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
	public class ReflectionAttributeProvider : GInterface1
	{
		private readonly ICustomAttributeProvider _attributeProvider;

		public ReflectionAttributeProvider(object attributeProvider)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
			this._attributeProvider = attributeProvider;
		}

		public IList<Attribute> GetAttributes(bool inherit)
		{
			return ReflectionUtils.GetAttributes(this._attributeProvider, null, inherit);
		}

		public IList<Attribute> GetAttributes(Type attributeType, bool inherit)
		{
			return ReflectionUtils.GetAttributes(this._attributeProvider, attributeType, inherit);
		}
	}
}