using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	internal static class FSharpUtils
	{
		private readonly static object Lock;

		private static bool _initialized;

		private static MethodInfo _ofSeq;

		private static Type _mapType;

		public const string FSharpSetTypeName = "FSharpSet`1";

		public const string FSharpListTypeName = "FSharpList`1";

		public const string FSharpMapTypeName = "FSharpMap`2";

		public static Assembly FSharpCoreAssembly
		{
			get;
			private set;
		}

		public static Func<object, object> GetUnionCaseInfoDeclaringType
		{
			get;
			private set;
		}

		public static MethodCall<object, object> GetUnionCaseInfoFields
		{
			get;
			private set;
		}

		public static Func<object, object> GetUnionCaseInfoName
		{
			get;
			private set;
		}

		public static Func<object, object> GetUnionCaseInfoTag
		{
			get;
			private set;
		}

		public static MethodCall<object, object> GetUnionCases
		{
			get;
			private set;
		}

		public static MethodCall<object, object> IsUnion
		{
			get;
			private set;
		}

		public static MethodCall<object, object> PreComputeUnionConstructor
		{
			get;
			private set;
		}

		public static MethodCall<object, object> PreComputeUnionReader
		{
			get;
			private set;
		}

		public static MethodCall<object, object> PreComputeUnionTagReader
		{
			get;
			private set;
		}

		static FSharpUtils()
		{
			Class6.yDnXvgqzyB5jw();
			FSharpUtils.Lock = new object();
		}

		public static ObjectConstructor<object> BuildMapCreator<TKey, TValue>()
		{
			ConstructorInfo constructor = FSharpUtils._mapType.MakeGenericType(new Type[] { typeof(TKey), typeof(TValue) }).GetConstructor(new Type[] { typeof(IEnumerable<Tuple<TKey, TValue>>) });
			ObjectConstructor<object> objectConstructor = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor);
			return (object[] args) => {
				IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs = (IEnumerable<KeyValuePair<TKey, TValue>>)args[0];
				Func<KeyValuePair<TKey, TValue>, Tuple<TKey, TValue>> u003cu003e9_521 = FSharpUtils.<>c__52<TKey, TValue>.<>9__52_1;
				if (u003cu003e9_521 == null)
				{
					u003cu003e9_521 = (KeyValuePair<TKey, TValue> kv) => new Tuple<TKey, TValue>(kv.Key, kv.Value);
					FSharpUtils.<>c__52<TKey, TValue>.<>9__52_1 = u003cu003e9_521;
				}
				IEnumerable<Tuple<TKey, TValue>> tuples = keyValuePairs.Select<KeyValuePair<TKey, TValue>, Tuple<TKey, TValue>>(u003cu003e9_521);
				return objectConstructor(new object[] { tuples });
			};
		}

		private static MethodCall<object, object> CreateFSharpFuncCall(Type type, string methodName)
		{
			MethodInfo methodWithNonPublicFallback = FSharpUtils.GetMethodWithNonPublicFallback(type, methodName, BindingFlags.Static | BindingFlags.Public);
			MethodInfo method = methodWithNonPublicFallback.ReturnType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
			MethodCall<object, object> methodCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback);
			MethodCall<object, object> methodCall1 = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return (object target, object[] args) => new FSharpFunction(methodCall(target, args), methodCall1);
		}

		public static ObjectConstructor<object> CreateMap(Type keyType, Type valueType)
		{
			return (ObjectConstructor<object>)typeof(FSharpUtils).GetMethod("BuildMapCreator").MakeGenericMethod(new Type[] { keyType, valueType }).Invoke(null, null);
		}

		public static ObjectConstructor<object> CreateSeq(Type t)
		{
			MethodInfo methodInfo = FSharpUtils._ofSeq.MakeGenericMethod(new Type[] { t });
			return JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(methodInfo);
		}

		public static void EnsureInitialized(Assembly fsharpCoreAssembly)
		{
			if (!FSharpUtils._initialized)
			{
				lock (FSharpUtils.Lock)
				{
					if (!FSharpUtils._initialized)
					{
						FSharpUtils.FSharpCoreAssembly = fsharpCoreAssembly;
						Type type = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpType");
						MethodInfo methodWithNonPublicFallback = FSharpUtils.GetMethodWithNonPublicFallback(type, "IsUnion", BindingFlags.Static | BindingFlags.Public);
						FSharpUtils.IsUnion = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback);
						MethodInfo methodInfo = FSharpUtils.GetMethodWithNonPublicFallback(type, "GetUnionCases", BindingFlags.Static | BindingFlags.Public);
						FSharpUtils.GetUnionCases = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodInfo);
						Type type1 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpValue");
						FSharpUtils.PreComputeUnionTagReader = FSharpUtils.CreateFSharpFuncCall(type1, "PreComputeUnionTagReader");
						FSharpUtils.PreComputeUnionReader = FSharpUtils.CreateFSharpFuncCall(type1, "PreComputeUnionReader");
						FSharpUtils.PreComputeUnionConstructor = FSharpUtils.CreateFSharpFuncCall(type1, "PreComputeUnionConstructor");
						Type type2 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.UnionCaseInfo");
						FSharpUtils.GetUnionCaseInfoName = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type2.GetProperty("Name"));
						FSharpUtils.GetUnionCaseInfoTag = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type2.GetProperty("Tag"));
						FSharpUtils.GetUnionCaseInfoDeclaringType = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type2.GetProperty("DeclaringType"));
						FSharpUtils.GetUnionCaseInfoFields = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(type2.GetMethod("GetFields"));
						FSharpUtils._ofSeq = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.ListModule").GetMethod("OfSeq");
						FSharpUtils._mapType = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.FSharpMap`2");
						Thread.MemoryBarrier();
						FSharpUtils._initialized = true;
					}
				}
			}
		}

		private static MethodInfo GetMethodWithNonPublicFallback(Type type, string methodName, BindingFlags bindingFlags)
		{
			MethodInfo method = type.GetMethod(methodName, bindingFlags);
			if (method == null && (bindingFlags & BindingFlags.NonPublic) != BindingFlags.NonPublic)
			{
				method = type.GetMethod(methodName, bindingFlags | BindingFlags.NonPublic);
			}
			return method;
		}
	}
}