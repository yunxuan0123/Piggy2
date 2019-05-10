using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	internal static class DynamicUtils
	{
		public static IEnumerable<string> GetDynamicMemberNames(this IDynamicMetaObjectProvider dynamicProvider)
		{
			return dynamicProvider.GetMetaObject(Expression.Constant(dynamicProvider)).GetDynamicMemberNames();
		}

		internal static class BinderWrapper
		{
			public const string CSharpAssemblyName = "Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

			private static object _getCSharpArgumentInfoArray;

			private static object _setCSharpArgumentInfoArray;

			private static MethodCall<object, object> _getMemberCall;

			private static MethodCall<object, object> _setMemberCall;

			private static bool _init;

			private static void CreateMemberCalls()
			{
				Type type = Type.GetType("Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
				Type type1 = Type.GetType("Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
				Type type2 = Type.GetType("Microsoft.CSharp.RuntimeBinder.Binder, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
				Type type3 = typeof(IEnumerable<>).MakeGenericType(new Type[] { type });
				MethodInfo method = type2.GetMethod("GetMember", new Type[] { type1, typeof(string), typeof(Type), type3 });
				DynamicUtils.BinderWrapper._getMemberCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
				MethodInfo methodInfo = type2.GetMethod("SetMember", new Type[] { type1, typeof(string), typeof(Type), type3 });
				DynamicUtils.BinderWrapper._setMemberCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodInfo);
			}

			private static object CreateSharpArgumentInfoArray(params int[] values)
			{
				Type type = Type.GetType("Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
				Type type1 = Type.GetType("Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
				Array arrays = Array.CreateInstance(type, (int)values.Length);
				for (int i = 0; i < (int)values.Length; i++)
				{
					arrays.SetValue(type.GetMethod("Create", new Type[] { type1, typeof(string) }).Invoke(null, new object[] { 0, null }), i);
				}
				return arrays;
			}

			public static CallSiteBinder GetMember(string name, Type context)
			{
				DynamicUtils.BinderWrapper.Init();
				return (CallSiteBinder)DynamicUtils.BinderWrapper._getMemberCall(null, new object[] { 0, name, context, DynamicUtils.BinderWrapper._getCSharpArgumentInfoArray });
			}

			private static void Init()
			{
				if (!DynamicUtils.BinderWrapper._init)
				{
					if (Type.GetType("Microsoft.CSharp.RuntimeBinder.Binder, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false) == null)
					{
						throw new InvalidOperationException("Could not resolve type '{0}'. You may need to add a reference to Microsoft.CSharp.dll to work with dynamic types.".FormatWith(CultureInfo.InvariantCulture, "Microsoft.CSharp.RuntimeBinder.Binder, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"));
					}
					DynamicUtils.BinderWrapper._getCSharpArgumentInfoArray = DynamicUtils.BinderWrapper.CreateSharpArgumentInfoArray(new int[1]);
					DynamicUtils.BinderWrapper._setCSharpArgumentInfoArray = DynamicUtils.BinderWrapper.CreateSharpArgumentInfoArray(new int[] { 0, 3 });
					DynamicUtils.BinderWrapper.CreateMemberCalls();
					DynamicUtils.BinderWrapper._init = true;
				}
			}

			public static CallSiteBinder SetMember(string name, Type context)
			{
				DynamicUtils.BinderWrapper.Init();
				return (CallSiteBinder)DynamicUtils.BinderWrapper._setMemberCall(null, new object[] { 0, name, context, DynamicUtils.BinderWrapper._setCSharpArgumentInfoArray });
			}
		}
	}
}