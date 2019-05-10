using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
	internal abstract class ReflectionDelegateFactory
	{
		protected ReflectionDelegateFactory()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public abstract Func<T> CreateDefaultConstructor<T>(Type type);

		public Func<T, object> CreateGet<T>(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			PropertyInfo propertyInfo1 = propertyInfo;
			if (propertyInfo != null)
			{
				if (propertyInfo1.PropertyType.IsByRef)
				{
					throw new InvalidOperationException("Could not create getter for {0}. ByRef return values are not supported.".FormatWith(CultureInfo.InvariantCulture, propertyInfo1));
				}
				return this.CreateGet<T>(propertyInfo1);
			}
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			FieldInfo fieldInfo1 = fieldInfo;
			if (fieldInfo == null)
			{
				throw new Exception("Could not create getter for {0}.".FormatWith(CultureInfo.InvariantCulture, memberInfo));
			}
			return this.CreateGet<T>(fieldInfo1);
		}

		public abstract Func<T, object> CreateGet<T>(PropertyInfo propertyInfo);

		public abstract Func<T, object> CreateGet<T>(FieldInfo fieldInfo);

		public abstract MethodCall<T, object> CreateMethodCall<T>(MethodBase method);

		public abstract ObjectConstructor<object> CreateParameterizedConstructor(MethodBase method);

		public Action<T, object> CreateSet<T>(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			PropertyInfo propertyInfo1 = propertyInfo;
			if (propertyInfo != null)
			{
				return this.CreateSet<T>(propertyInfo1);
			}
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			FieldInfo fieldInfo1 = fieldInfo;
			if (fieldInfo == null)
			{
				throw new Exception("Could not create setter for {0}.".FormatWith(CultureInfo.InvariantCulture, memberInfo));
			}
			return this.CreateSet<T>(fieldInfo1);
		}

		public abstract Action<T, object> CreateSet<T>(FieldInfo fieldInfo);

		public abstract Action<T, object> CreateSet<T>(PropertyInfo propertyInfo);
	}
}