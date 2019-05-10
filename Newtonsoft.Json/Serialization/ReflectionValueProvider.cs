using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
	public class ReflectionValueProvider : IValueProvider
	{
		private readonly MemberInfo _memberInfo;

		public ReflectionValueProvider(MemberInfo memberInfo)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			ValidationUtils.ArgumentNotNull(memberInfo, "memberInfo");
			this._memberInfo = memberInfo;
		}

		public object GetValue(object target)
		{
			object memberValue;
			try
			{
				PropertyInfo propertyInfo = this._memberInfo as PropertyInfo;
				PropertyInfo propertyInfo1 = propertyInfo;
				if (propertyInfo != null && propertyInfo1.PropertyType.IsByRef)
				{
					throw new InvalidOperationException("Could not create getter for {0}. ByRef return values are not supported.".FormatWith(CultureInfo.InvariantCulture, propertyInfo1));
				}
				memberValue = ReflectionUtils.GetMemberValue(this._memberInfo, target);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, this._memberInfo.Name, target.GetType()), exception);
			}
			return memberValue;
		}

		public void SetValue(object target, object value)
		{
			try
			{
				ReflectionUtils.SetMemberValue(this._memberInfo, target, value);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, this._memberInfo.Name, target.GetType()), exception);
			}
		}
	}
}