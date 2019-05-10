using System;

namespace Newtonsoft.Json.Serialization
{
	public interface IValueProvider
	{
		object GetValue(object target);

		void SetValue(object target, object value);
	}
}