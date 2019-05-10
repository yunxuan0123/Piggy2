using System;
using System.ComponentModel;

namespace Newtonsoft.Json.Linq
{
	public class GClass0 : PropertyDescriptor
	{
		public override Type ComponentType
		{
			get
			{
				return typeof(JObject);
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		protected override int NameHashCode
		{
			get
			{
				return base.NameHashCode;
			}
		}

		public override Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		public GClass0(string name)
		{
			Class6.yDnXvgqzyB5jw();
			base(name, null);
		}

		public override bool CanResetValue(object component)
		{
			return false;
		}

		private static JObject CastInstance(JObject instance)
		{
			return (JObject)instance;
		}

		public override object GetValue(object component)
		{
			JObject jObjects = component as JObject;
			if (jObjects == null)
			{
				return null;
			}
			return jObjects[this.Name];
		}

		public override void ResetValue(object component)
		{
		}

		public override void SetValue(object component, object value)
		{
			JObject jObjects = component as JObject;
			JObject jObjects1 = jObjects;
			if (jObjects != null)
			{
				object jValue = value as JToken;
				if (jValue == null)
				{
					jValue = new JValue(value);
				}
				JToken jTokens = (JToken)jValue;
				jObjects1[this.Name] = jTokens;
			}
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}
	}
}