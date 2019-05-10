using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	internal class DynamicReflectionDelegateFactory : ReflectionDelegateFactory
	{
		internal static DynamicReflectionDelegateFactory Instance
		{
			get;
		}

		static DynamicReflectionDelegateFactory()
		{
			Class6.yDnXvgqzyB5jw();
			DynamicReflectionDelegateFactory.Instance = new DynamicReflectionDelegateFactory();
		}

		public DynamicReflectionDelegateFactory()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public override Func<T> CreateDefaultConstructor<T>(Type type)
		{
			DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod(string.Concat("Create", type.FullName), typeof(T), ReflectionUtils.EmptyTypes, type);
			dynamicMethod.InitLocals = true;
			this.GenerateCreateDefaultConstructorIL(type, dynamicMethod.GetILGenerator(), typeof(T));
			return (Func<T>)dynamicMethod.CreateDelegate(typeof(Func<T>));
		}

		private static DynamicMethod CreateDynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner)
		{
			if (!owner.IsInterface())
			{
				return new DynamicMethod(name, returnType, parameterTypes, owner, true);
			}
			return new DynamicMethod(name, returnType, parameterTypes, owner.Module, true);
		}

		public override Func<T, object> CreateGet<T>(PropertyInfo propertyInfo)
		{
			DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod(string.Concat("Get", propertyInfo.Name), typeof(object), new Type[] { typeof(T) }, propertyInfo.DeclaringType);
			this.GenerateCreateGetPropertyIL(propertyInfo, dynamicMethod.GetILGenerator());
			return (Func<T, object>)dynamicMethod.CreateDelegate(typeof(Func<T, object>));
		}

		public override Func<T, object> CreateGet<T>(FieldInfo fieldInfo)
		{
			if (fieldInfo.IsLiteral)
			{
				object value = fieldInfo.GetValue(null);
				return (T o) => value;
			}
			DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod(string.Concat("Get", fieldInfo.Name), typeof(T), new Type[] { typeof(object) }, fieldInfo.DeclaringType);
			this.GenerateCreateGetFieldIL(fieldInfo, dynamicMethod.GetILGenerator());
			return (Func<T, object>)dynamicMethod.CreateDelegate(typeof(Func<T, object>));
		}

		public override MethodCall<T, object> CreateMethodCall<T>(MethodBase method)
		{
			DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod(method.ToString(), typeof(object), new Type[] { typeof(object), typeof(object[]) }, method.DeclaringType);
			this.GenerateCreateMethodCallIL(method, dynamicMethod.GetILGenerator(), 1);
			return (MethodCall<T, object>)dynamicMethod.CreateDelegate(typeof(MethodCall<T, object>));
		}

		public override ObjectConstructor<object> CreateParameterizedConstructor(MethodBase method)
		{
			DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod(method.ToString(), typeof(object), new Type[] { typeof(object[]) }, method.DeclaringType);
			this.GenerateCreateMethodCallIL(method, dynamicMethod.GetILGenerator(), 0);
			return (ObjectConstructor<object>)dynamicMethod.CreateDelegate(typeof(ObjectConstructor<object>));
		}

		public override Action<T, object> CreateSet<T>(FieldInfo fieldInfo)
		{
			DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod(string.Concat("Set", fieldInfo.Name), null, new Type[] { typeof(T), typeof(object) }, fieldInfo.DeclaringType);
			DynamicReflectionDelegateFactory.GenerateCreateSetFieldIL(fieldInfo, dynamicMethod.GetILGenerator());
			return (Action<T, object>)dynamicMethod.CreateDelegate(typeof(Action<T, object>));
		}

		public override Action<T, object> CreateSet<T>(PropertyInfo propertyInfo)
		{
			DynamicMethod dynamicMethod = DynamicReflectionDelegateFactory.CreateDynamicMethod(string.Concat("Set", propertyInfo.Name), null, new Type[] { typeof(T), typeof(object) }, propertyInfo.DeclaringType);
			DynamicReflectionDelegateFactory.GenerateCreateSetPropertyIL(propertyInfo, dynamicMethod.GetILGenerator());
			return (Action<T, object>)dynamicMethod.CreateDelegate(typeof(Action<T, object>));
		}

		private void GenerateCreateDefaultConstructorIL(Type type, ILGenerator generator, Type delegateType)
		{
			if (!type.IsValueType())
			{
				ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, ReflectionUtils.EmptyTypes, null);
				if (constructor == null)
				{
					throw new ArgumentException("Could not get constructor for {0}.".FormatWith(CultureInfo.InvariantCulture, type));
				}
				generator.Emit(OpCodes.Newobj, constructor);
			}
			else
			{
				generator.DeclareLocal(type);
				generator.Emit(OpCodes.Ldloc_0);
				if (type != delegateType)
				{
					generator.Emit(OpCodes.Box, type);
				}
			}
			generator.Return();
		}

		private void GenerateCreateGetFieldIL(FieldInfo fieldInfo, ILGenerator generator)
		{
			if (fieldInfo.IsStatic)
			{
				generator.Emit(OpCodes.Ldsfld, fieldInfo);
			}
			else
			{
				generator.PushInstance(fieldInfo.DeclaringType);
				generator.Emit(OpCodes.Ldfld, fieldInfo);
			}
			generator.BoxIfNeeded(fieldInfo.FieldType);
			generator.Return();
		}

		private void GenerateCreateGetPropertyIL(PropertyInfo propertyInfo, ILGenerator generator)
		{
			MethodInfo getMethod = propertyInfo.GetGetMethod(true);
			if (getMethod == null)
			{
				throw new ArgumentException("Property '{0}' does not have a getter.".FormatWith(CultureInfo.InvariantCulture, propertyInfo.Name));
			}
			if (!getMethod.IsStatic)
			{
				generator.PushInstance(propertyInfo.DeclaringType);
			}
			generator.CallMethod(getMethod);
			generator.BoxIfNeeded(propertyInfo.PropertyType);
			generator.Return();
		}

		private void GenerateCreateMethodCallIL(MethodBase method, ILGenerator generator, int argsIndex)
		{
			ParameterInfo[] parameters = method.GetParameters();
			Label label = generator.DefineLabel();
			generator.Emit(OpCodes.Ldarg, argsIndex);
			generator.Emit(OpCodes.Ldlen);
			generator.Emit(OpCodes.Ldc_I4, (int)parameters.Length);
			generator.Emit(OpCodes.Beq, label);
			generator.Emit(OpCodes.Newobj, typeof(TargetParameterCountException).GetConstructor(ReflectionUtils.EmptyTypes));
			generator.Emit(OpCodes.Throw);
			generator.MarkLabel(label);
			if (!method.IsConstructor && !method.IsStatic)
			{
				generator.PushInstance(method.DeclaringType);
			}
			LocalBuilder localBuilder = generator.DeclareLocal(typeof(IConvertible));
			LocalBuilder localBuilder1 = generator.DeclareLocal(typeof(object));
			for (int i = 0; i < (int)parameters.Length; i++)
			{
				ParameterInfo parameterInfo = parameters[i];
				Type parameterType = parameterInfo.ParameterType;
				if (parameterType.IsByRef)
				{
					parameterType = parameterType.GetElementType();
					LocalBuilder localBuilder2 = generator.DeclareLocal(parameterType);
					if (!parameterInfo.IsOut)
					{
						generator.PushArrayInstance(argsIndex, i);
						if (!parameterType.IsValueType())
						{
							generator.UnboxIfNeeded(parameterType);
							generator.Emit(OpCodes.Stloc_S, localBuilder2);
						}
						else
						{
							Label label1 = generator.DefineLabel();
							Label label2 = generator.DefineLabel();
							generator.Emit(OpCodes.Brtrue_S, label1);
							generator.Emit(OpCodes.Ldloca_S, localBuilder2);
							generator.Emit(OpCodes.Initobj, parameterType);
							generator.Emit(OpCodes.Br_S, label2);
							generator.MarkLabel(label1);
							generator.PushArrayInstance(argsIndex, i);
							generator.UnboxIfNeeded(parameterType);
							generator.Emit(OpCodes.Stloc_S, localBuilder2);
							generator.MarkLabel(label2);
						}
					}
					generator.Emit(OpCodes.Ldloca_S, localBuilder2);
				}
				else if (!parameterType.IsValueType())
				{
					generator.PushArrayInstance(argsIndex, i);
					generator.UnboxIfNeeded(parameterType);
				}
				else
				{
					generator.PushArrayInstance(argsIndex, i);
					generator.Emit(OpCodes.Stloc_S, localBuilder1);
					Label label3 = generator.DefineLabel();
					Label label4 = generator.DefineLabel();
					generator.Emit(OpCodes.Ldloc_S, localBuilder1);
					generator.Emit(OpCodes.Brtrue_S, label3);
					LocalBuilder localBuilder3 = generator.DeclareLocal(parameterType);
					generator.Emit(OpCodes.Ldloca_S, localBuilder3);
					generator.Emit(OpCodes.Initobj, parameterType);
					generator.Emit(OpCodes.Ldloc_S, localBuilder3);
					generator.Emit(OpCodes.Br_S, label4);
					generator.MarkLabel(label3);
					if (parameterType.IsPrimitive())
					{
						MethodInfo methodInfo = typeof(IConvertible).GetMethod(string.Concat("To", parameterType.Name), new Type[] { typeof(IFormatProvider) });
						if (methodInfo != null)
						{
							Label label5 = generator.DefineLabel();
							generator.Emit(OpCodes.Ldloc_S, localBuilder1);
							generator.Emit(OpCodes.Isinst, parameterType);
							generator.Emit(OpCodes.Brtrue_S, label5);
							generator.Emit(OpCodes.Ldloc_S, localBuilder1);
							generator.Emit(OpCodes.Isinst, typeof(IConvertible));
							generator.Emit(OpCodes.Stloc_S, localBuilder);
							generator.Emit(OpCodes.Ldloc_S, localBuilder);
							generator.Emit(OpCodes.Brfalse_S, label5);
							generator.Emit(OpCodes.Ldloc_S, localBuilder);
							generator.Emit(OpCodes.Ldnull);
							generator.Emit(OpCodes.Callvirt, methodInfo);
							generator.Emit(OpCodes.Br_S, label4);
							generator.MarkLabel(label5);
						}
					}
					generator.Emit(OpCodes.Ldloc_S, localBuilder1);
					generator.UnboxIfNeeded(parameterType);
					generator.MarkLabel(label4);
				}
			}
			if (!method.IsConstructor)
			{
				generator.CallMethod((MethodInfo)method);
			}
			else
			{
				generator.Emit(OpCodes.Newobj, (ConstructorInfo)method);
			}
			Type type = (method.IsConstructor ? method.DeclaringType : ((MethodInfo)method).ReturnType);
			if (type == typeof(void))
			{
				generator.Emit(OpCodes.Ldnull);
			}
			else
			{
				generator.BoxIfNeeded(type);
			}
			generator.Return();
		}

		internal static void GenerateCreateSetFieldIL(FieldInfo fieldInfo, ILGenerator generator)
		{
			if (!fieldInfo.IsStatic)
			{
				generator.PushInstance(fieldInfo.DeclaringType);
			}
			generator.Emit(OpCodes.Ldarg_1);
			generator.UnboxIfNeeded(fieldInfo.FieldType);
			if (fieldInfo.IsStatic)
			{
				generator.Emit(OpCodes.Stsfld, fieldInfo);
			}
			else
			{
				generator.Emit(OpCodes.Stfld, fieldInfo);
			}
			generator.Return();
		}

		internal static void GenerateCreateSetPropertyIL(PropertyInfo propertyInfo, ILGenerator generator)
		{
			MethodInfo setMethod = propertyInfo.GetSetMethod(true);
			if (!setMethod.IsStatic)
			{
				generator.PushInstance(propertyInfo.DeclaringType);
			}
			generator.Emit(OpCodes.Ldarg_1);
			generator.UnboxIfNeeded(propertyInfo.PropertyType);
			generator.CallMethod(setMethod);
			generator.Return();
		}
	}
}