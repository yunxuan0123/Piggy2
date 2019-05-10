using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
	internal static class ReflectionUtils
	{
		public readonly static Type[] EmptyTypes;

		static ReflectionUtils()
		{
			Class6.yDnXvgqzyB5jw();
			ReflectionUtils.EmptyTypes = Type.EmptyTypes;
		}

		public static bool CanReadMemberValue(MemberInfo member, bool nonPublic)
		{
			MemberTypes memberType = member.MemberType();
			if (memberType == MemberTypes.Field)
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				if (nonPublic)
				{
					return true;
				}
				if (fieldInfo.IsPublic)
				{
					return true;
				}
				return false;
			}
			if (memberType != MemberTypes.Property)
			{
				return false;
			}
			PropertyInfo propertyInfo = (PropertyInfo)member;
			if (!propertyInfo.CanRead)
			{
				return false;
			}
			if (nonPublic)
			{
				return true;
			}
			return propertyInfo.GetGetMethod(nonPublic) != null;
		}

		public static bool CanSetMemberValue(MemberInfo member, bool nonPublic, bool canSetReadOnly)
		{
			MemberTypes memberType = member.MemberType();
			if (memberType != MemberTypes.Field)
			{
				if (memberType != MemberTypes.Property)
				{
					return false;
				}
				PropertyInfo propertyInfo = (PropertyInfo)member;
				if (!propertyInfo.CanWrite)
				{
					return false;
				}
				if (nonPublic)
				{
					return true;
				}
				return propertyInfo.GetSetMethod(nonPublic) != null;
			}
			FieldInfo fieldInfo = (FieldInfo)member;
			if (fieldInfo.IsLiteral)
			{
				return false;
			}
			if (fieldInfo.IsInitOnly && !canSetReadOnly)
			{
				return false;
			}
			if (nonPublic)
			{
				return true;
			}
			if (fieldInfo.IsPublic)
			{
				return true;
			}
			return false;
		}

		public static Type EnsureNotByRefType(Type t)
		{
			if (!t.IsByRef || !t.HasElementType)
			{
				return t;
			}
			return t.GetElementType();
		}

		public static Type EnsureNotNullableType(Type t)
		{
			if (!ReflectionUtils.IsNullableType(t))
			{
				return t;
			}
			return Nullable.GetUnderlyingType(t);
		}

		private static int? GetAssemblyDelimiterIndex(string fullyQualifiedTypeName)
		{
			int num = 0;
			for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
			{
				char chr = fullyQualifiedTypeName[i];
				if (chr == ',')
				{
					if (num == 0)
					{
						return new int?(i);
					}
				}
				else if (chr == '[')
				{
					num++;
				}
				else if (chr == ']')
				{
					num--;
				}
			}
			return null;
		}

		public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider)
		where T : Attribute
		{
			return ReflectionUtils.GetAttribute<T>(attributeProvider, true);
		}

		public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider, bool inherit)
		where T : Attribute
		{
			T[] attributes = ReflectionUtils.GetAttributes<T>(attributeProvider, inherit);
			if (attributes != null)
			{
				return attributes.FirstOrDefault<T>();
			}
			return default(T);
		}

		public static T[] GetAttributes<T>(ICustomAttributeProvider attributeProvider, bool inherit)
		where T : Attribute
		{
			Attribute[] attributes = ReflectionUtils.GetAttributes(attributeProvider, typeof(T), inherit);
			T[] tArray = attributes as T[];
			T[] tArray1 = tArray;
			if (tArray != null)
			{
				return tArray1;
			}
			return attributes.Cast<T>().ToArray<T>();
		}

		public static Attribute[] GetAttributes(ICustomAttributeProvider attributeProvider, Type attributeType, bool inherit)
		{
			ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
			object obj = attributeProvider;
			if (obj != null)
			{
				Type type = obj as Type;
				Type type1 = type;
				if (type != null)
				{
					Type type2 = type1;
					return ((attributeType != null ? type2.GetCustomAttributes(attributeType, inherit) : type2.GetCustomAttributes(inherit))).Cast<Attribute>().ToArray<Attribute>();
				}
				Assembly assembly = obj as Assembly;
				Assembly assembly1 = assembly;
				if (assembly != null)
				{
					Assembly assembly2 = assembly1;
					if (attributeType == null)
					{
						return Attribute.GetCustomAttributes(assembly2);
					}
					return Attribute.GetCustomAttributes(assembly2, attributeType);
				}
				MemberInfo memberInfo = obj as MemberInfo;
				MemberInfo memberInfo1 = memberInfo;
				if (memberInfo != null)
				{
					MemberInfo memberInfo2 = memberInfo1;
					if (attributeType == null)
					{
						return Attribute.GetCustomAttributes(memberInfo2, inherit);
					}
					return Attribute.GetCustomAttributes(memberInfo2, attributeType, inherit);
				}
				Module module = obj as Module;
				Module module1 = module;
				if (module != null)
				{
					Module module2 = module1;
					if (attributeType == null)
					{
						return Attribute.GetCustomAttributes(module2, inherit);
					}
					return Attribute.GetCustomAttributes(module2, attributeType, inherit);
				}
				ParameterInfo parameterInfo = obj as ParameterInfo;
				ParameterInfo parameterInfo1 = parameterInfo;
				if (parameterInfo != null)
				{
					ParameterInfo parameterInfo2 = parameterInfo1;
					if (attributeType == null)
					{
						return Attribute.GetCustomAttributes(parameterInfo2, inherit);
					}
					return Attribute.GetCustomAttributes(parameterInfo2, attributeType, inherit);
				}
			}
			ICustomAttributeProvider customAttributeProvider = (ICustomAttributeProvider)attributeProvider;
			return (Attribute[])((attributeType != null ? customAttributeProvider.GetCustomAttributes(attributeType, inherit) : customAttributeProvider.GetCustomAttributes(inherit)));
		}

		public static MethodInfo GetBaseDefinition(this PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			MethodInfo getMethod = propertyInfo.GetGetMethod(true);
			if (getMethod != null)
			{
				return getMethod.GetBaseDefinition();
			}
			MethodInfo setMethod = propertyInfo.GetSetMethod(true);
			if (setMethod != null)
			{
				return setMethod.GetBaseDefinition();
			}
			return null;
		}

		private static void GetChildPrivateFields(IList<MemberInfo> initialFields, Type targetType, BindingFlags bindingAttr)
		{
			if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
			{
				BindingFlags bindingFlag = bindingAttr.RemoveFlag(BindingFlags.Public);
				while (true)
				{
					Type type = targetType.BaseType();
					targetType = type;
					if (type == null)
					{
						break;
					}
					IEnumerable<FieldInfo> fields = 
						from f in (IEnumerable<FieldInfo>)targetType.GetFields(bindingFlag)
						where f.IsPrivate
						select f;
					initialFields.AddRange<MemberInfo>(fields);
				}
			}
		}

		private static void GetChildPrivateProperties(IList<PropertyInfo> initialProperties, Type targetType, BindingFlags bindingAttr)
		{
			object obj;
		Label0:
			Type type = targetType.BaseType();
			targetType = type;
			if (type == null)
			{
				return;
			}
			PropertyInfo[] properties = targetType.GetProperties(bindingAttr);
			for (int i = 0; i < (int)properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				if (propertyInfo.IsVirtual())
				{
					MethodInfo methodInfo = propertyInfo.GetBaseDefinition();
					if (methodInfo != null)
					{
						obj = methodInfo.DeclaringType;
						if (obj != null)
						{
							goto Label1;
						}
					}
					else
					{
						obj = null;
					}
					obj = propertyInfo.DeclaringType;
				Label1:
					Type type1 = (Type)obj;
					if (initialProperties.IndexOf<PropertyInfo>((PropertyInfo p) => {
						object declaringType;
						if (!(p.Name == propertyInfo.Name) || !p.IsVirtual())
						{
							return false;
						}
						MethodInfo baseDefinition = p.GetBaseDefinition();
						if (baseDefinition != null)
						{
							declaringType = baseDefinition.DeclaringType;
							if (declaringType != null)
							{
								return ((Type)declaringType).IsAssignableFrom(type1);
							}
						}
						else
						{
							declaringType = null;
						}
						declaringType = p.DeclaringType;
						return ((Type)declaringType).IsAssignableFrom(type1);
					}) == -1)
					{
						initialProperties.Add(propertyInfo);
					}
				}
				else if (!ReflectionUtils.IsPublic(propertyInfo))
				{
					int num = initialProperties.IndexOf<PropertyInfo>((PropertyInfo p) => p.Name == propertyInfo.Name);
					if (num == -1)
					{
						initialProperties.Add(propertyInfo);
					}
					else if (!ReflectionUtils.IsPublic(initialProperties[num]))
					{
						initialProperties[num] = propertyInfo;
					}
				}
				else if (initialProperties.IndexOf<PropertyInfo>((PropertyInfo p) => {
					if (p.Name != propertyInfo.Name)
					{
						return false;
					}
					return p.DeclaringType == propertyInfo.DeclaringType;
				}) == -1)
				{
					initialProperties.Add(propertyInfo);
				}
			}
			goto Label0;
		}

		public static Type GetCollectionItemType(Type type)
		{
			Type type1;
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsArray)
			{
				return type.GetElementType();
			}
			if (ReflectionUtils.ImplementsGenericDefinition(type, typeof(IEnumerable<>), out type1))
			{
				if (type1.IsGenericTypeDefinition())
				{
					throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, type));
				}
				return type1.GetGenericArguments()[0];
			}
			if (!typeof(IEnumerable).IsAssignableFrom(type))
			{
				throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, type));
			}
			return null;
		}

		public static ConstructorInfo GetDefaultConstructor(Type t)
		{
			return ReflectionUtils.GetDefaultConstructor(t, false);
		}

		public static ConstructorInfo GetDefaultConstructor(Type t, bool nonPublic)
		{
			BindingFlags bindingFlag = BindingFlags.Instance | BindingFlags.Public;
			if (nonPublic)
			{
				bindingFlag |= BindingFlags.NonPublic;
			}
			return ((IEnumerable<ConstructorInfo>)t.GetConstructors(bindingFlag)).SingleOrDefault<ConstructorInfo>((ConstructorInfo c) => !c.GetParameters().Any<ParameterInfo>());
		}

		public static object GetDefaultValue(Type type)
		{
			if (!type.IsValueType())
			{
				return null;
			}
			PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(type);
			switch (typeCode)
			{
				case PrimitiveTypeCode.Char:
				case PrimitiveTypeCode.SByte:
				case PrimitiveTypeCode.Int16:
				case PrimitiveTypeCode.UInt16:
				case PrimitiveTypeCode.Int32:
				case PrimitiveTypeCode.Byte:
				case PrimitiveTypeCode.UInt32:
				{
					return 0;
				}
				case PrimitiveTypeCode.CharNullable:
				case PrimitiveTypeCode.BooleanNullable:
				case PrimitiveTypeCode.SByteNullable:
				case PrimitiveTypeCode.Int16Nullable:
				case PrimitiveTypeCode.UInt16Nullable:
				case PrimitiveTypeCode.Int32Nullable:
				case PrimitiveTypeCode.ByteNullable:
				case PrimitiveTypeCode.UInt32Nullable:
				case PrimitiveTypeCode.Int64Nullable:
				case PrimitiveTypeCode.UInt64Nullable:
				case PrimitiveTypeCode.SingleNullable:
				case PrimitiveTypeCode.DoubleNullable:
				case PrimitiveTypeCode.DateTimeNullable:
				case PrimitiveTypeCode.DateTimeOffsetNullable:
				case PrimitiveTypeCode.DecimalNullable:
				{
					if (ReflectionUtils.IsNullable(type))
					{
						return null;
					}
					return Activator.CreateInstance(type);
				}
				case PrimitiveTypeCode.Boolean:
				{
					return false;
				}
				case PrimitiveTypeCode.Int64:
				case PrimitiveTypeCode.UInt64:
				{
					return 0L;
				}
				case PrimitiveTypeCode.Single:
				{
					return 0f;
				}
				case PrimitiveTypeCode.Double:
				{
					return 0;
				}
				case PrimitiveTypeCode.DateTime:
				{
					return new DateTime();
				}
				case PrimitiveTypeCode.DateTimeOffset:
				{
					return new DateTimeOffset();
				}
				case PrimitiveTypeCode.Decimal:
				{
					return decimal.Zero;
				}
				case PrimitiveTypeCode.Guid:
				{
					return new Guid();
				}
				default:
				{
					if (typeCode == PrimitiveTypeCode.BigInteger)
					{
						return new BigInteger();
					}
					if (ReflectionUtils.IsNullable(type))
					{
						return null;
					}
					return Activator.CreateInstance(type);
				}
			}
		}

		public static void GetDictionaryKeyValueTypes(Type dictionaryType, out Type keyType, out Type valueType)
		{
			Type type;
			ValidationUtils.ArgumentNotNull(dictionaryType, "dictionaryType");
			if (!ReflectionUtils.ImplementsGenericDefinition(dictionaryType, typeof(IDictionary<,>), out type))
			{
				if (!typeof(IDictionary).IsAssignableFrom(dictionaryType))
				{
					throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, dictionaryType));
				}
				keyType = null;
				valueType = null;
				return;
			}
			if (type.IsGenericTypeDefinition())
			{
				throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, dictionaryType));
			}
			Type[] genericArguments = type.GetGenericArguments();
			keyType = genericArguments[0];
			valueType = genericArguments[1];
		}

		public static IEnumerable<FieldInfo> GetFields(Type targetType, BindingFlags bindingAttr)
		{
			ValidationUtils.ArgumentNotNull(targetType, "targetType");
			List<MemberInfo> memberInfos = new List<MemberInfo>(targetType.GetFields(bindingAttr));
			ReflectionUtils.GetChildPrivateFields(memberInfos, targetType, bindingAttr);
			return memberInfos.Cast<FieldInfo>();
		}

		public static List<MemberInfo> GetFieldsAndProperties(Type type, BindingFlags bindingAttr)
		{
			List<MemberInfo> memberInfos = new List<MemberInfo>();
			memberInfos.AddRange(ReflectionUtils.GetFields(type, bindingAttr));
			memberInfos.AddRange(ReflectionUtils.GetProperties(type, bindingAttr));
			List<MemberInfo> memberInfos1 = new List<MemberInfo>(memberInfos.Count);
			foreach (IGrouping<string, MemberInfo> strs in 
				from m in memberInfos
				group m by m.Name)
			{
				if (strs.Count<MemberInfo>() != 1)
				{
					List<MemberInfo> memberInfos2 = new List<MemberInfo>();
					foreach (MemberInfo memberInfo in strs)
					{
						if (memberInfos2.Count != 0)
						{
							if (ReflectionUtils.IsOverridenGenericMember(memberInfo, bindingAttr) && !(memberInfo.Name == "Item") || memberInfos2.Any<MemberInfo>((MemberInfo m) => m.DeclaringType == memberInfo.DeclaringType))
							{
								continue;
							}
							memberInfos2.Add(memberInfo);
						}
						else
						{
							memberInfos2.Add(memberInfo);
						}
					}
					memberInfos1.AddRange(memberInfos2);
				}
				else
				{
					memberInfos1.Add(strs.First<MemberInfo>());
				}
			}
			return memberInfos1;
		}

		private static string GetFullyQualifiedTypeName(Type t, GInterface3 binder)
		{
			string str;
			string str1;
			if (binder == null)
			{
				return t.AssemblyQualifiedName;
			}
			binder.BindToName(t, out str, out str1);
			return string.Concat(str1, (str == null ? "" : string.Concat(", ", str)));
		}

		public static MemberInfo GetMemberInfoFromType(Type targetType, MemberInfo memberInfo)
		{
			if (memberInfo.MemberType() != MemberTypes.Property)
			{
				return targetType.GetMember(memberInfo.Name, memberInfo.MemberType(), BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SingleOrDefault<MemberInfo>();
			}
			PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
			Type[] array = (
				from p in (IEnumerable<ParameterInfo>)propertyInfo.GetIndexParameters()
				select p.ParameterType).ToArray<Type>();
			return targetType.GetProperty(propertyInfo.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, propertyInfo.PropertyType, array, null);
		}

		public static Type GetMemberUnderlyingType(MemberInfo member)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			MemberTypes memberType = member.MemberType();
			if (memberType > MemberTypes.Field)
			{
				if (memberType == MemberTypes.Method)
				{
					return ((MethodInfo)member).ReturnType;
				}
				if (memberType == MemberTypes.Property)
				{
					return ((PropertyInfo)member).PropertyType;
				}
			}
			else
			{
				if (memberType == MemberTypes.Event)
				{
					return ((EventInfo)member).EventHandlerType;
				}
				if (memberType == MemberTypes.Field)
				{
					return ((FieldInfo)member).FieldType;
				}
			}
			throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo, EventInfo or MethodInfo", "member");
		}

		public static object GetMemberValue(MemberInfo member, object target)
		{
			object value;
			ValidationUtils.ArgumentNotNull(member, "member");
			ValidationUtils.ArgumentNotNull(target, "target");
			MemberTypes memberType = member.MemberType();
			if (memberType == MemberTypes.Field)
			{
				return ((FieldInfo)member).GetValue(target);
			}
			if (memberType != MemberTypes.Property)
			{
				throw new ArgumentException("MemberInfo '{0}' is not of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, member.Name), "member");
			}
			try
			{
				value = ((PropertyInfo)member).GetValue(target, null);
			}
			catch (TargetParameterCountException targetParameterCountException1)
			{
				TargetParameterCountException targetParameterCountException = targetParameterCountException1;
				throw new ArgumentException("MemberInfo '{0}' has index parameters".FormatWith(CultureInfo.InvariantCulture, member.Name), targetParameterCountException);
			}
			return value;
		}

		public static Type GetObjectType(object v)
		{
			if (v == null)
			{
				return null;
			}
			return v.GetType();
		}

		public static IEnumerable<PropertyInfo> GetProperties(Type targetType, BindingFlags bindingAttr)
		{
			ValidationUtils.ArgumentNotNull(targetType, "targetType");
			List<PropertyInfo> propertyInfos = new List<PropertyInfo>(targetType.GetProperties(bindingAttr));
			if (targetType.IsInterface())
			{
				Type[] interfaces = targetType.GetInterfaces();
				for (int i = 0; i < (int)interfaces.Length; i++)
				{
					propertyInfos.AddRange(interfaces[i].GetProperties(bindingAttr));
				}
			}
			ReflectionUtils.GetChildPrivateProperties(propertyInfos, targetType, bindingAttr);
			for (int j = 0; j < propertyInfos.Count; j++)
			{
				PropertyInfo item = propertyInfos[j];
				if (item.DeclaringType != targetType)
				{
					PropertyInfo memberInfoFromType = (PropertyInfo)ReflectionUtils.GetMemberInfoFromType(item.DeclaringType, item);
					propertyInfos[j] = memberInfoFromType;
				}
			}
			return propertyInfos;
		}

		public static string GetTypeName(Type t, TypeNameAssemblyFormatHandling assemblyFormat, GInterface3 binder)
		{
			string fullyQualifiedTypeName = ReflectionUtils.GetFullyQualifiedTypeName(t, binder);
			if (assemblyFormat == TypeNameAssemblyFormatHandling.Simple)
			{
				return ReflectionUtils.RemoveAssemblyDetails(fullyQualifiedTypeName);
			}
			if (assemblyFormat != TypeNameAssemblyFormatHandling.Full)
			{
				throw new ArgumentOutOfRangeException();
			}
			return fullyQualifiedTypeName;
		}

		public static bool HasDefaultConstructor(Type t, bool nonPublic)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			if (t.IsValueType())
			{
				return true;
			}
			return ReflectionUtils.GetDefaultConstructor(t, nonPublic) != null;
		}

		public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition)
		{
			Type type1;
			return ReflectionUtils.ImplementsGenericDefinition(type, genericInterfaceDefinition, out type1);
		}

		public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition, out Type implementingType)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(genericInterfaceDefinition, "genericInterfaceDefinition");
			if (!genericInterfaceDefinition.IsInterface() || !genericInterfaceDefinition.IsGenericTypeDefinition())
			{
				throw new ArgumentNullException("'{0}' is not a generic interface definition.".FormatWith(CultureInfo.InvariantCulture, genericInterfaceDefinition));
			}
			if (type.IsInterface() && type.IsGenericType() && genericInterfaceDefinition == type.GetGenericTypeDefinition())
			{
				implementingType = type;
				return true;
			}
			Type[] interfaces = type.GetInterfaces();
			for (int i = 0; i < (int)interfaces.Length; i++)
			{
				Type type1 = interfaces[i];
				if (type1.IsGenericType() && genericInterfaceDefinition == type1.GetGenericTypeDefinition())
				{
					implementingType = type1;
					return true;
				}
			}
			implementingType = null;
			return false;
		}

		public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition)
		{
			Type type1;
			return ReflectionUtils.InheritsGenericDefinition(type, genericClassDefinition, out type1);
		}

		public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition, out Type implementingType)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(genericClassDefinition, "genericClassDefinition");
			if (!genericClassDefinition.IsClass() || !genericClassDefinition.IsGenericTypeDefinition())
			{
				throw new ArgumentNullException("'{0}' is not a generic class definition.".FormatWith(CultureInfo.InvariantCulture, genericClassDefinition));
			}
			return ReflectionUtils.InheritsGenericDefinitionInternal(type, genericClassDefinition, out implementingType);
		}

		private static bool InheritsGenericDefinitionInternal(Type currentType, Type genericClassDefinition, out Type implementingType)
		{
			do
			{
				if (currentType.IsGenericType())
				{
					if (genericClassDefinition == currentType.GetGenericTypeDefinition())
					{
						implementingType = currentType;
						return true;
					}
				}
				currentType = currentType.BaseType();
			}
			while (currentType != null);
			implementingType = null;
			return false;
		}

		public static bool IsByRefLikeType(Type type)
		{
			if (!type.IsValueType())
			{
				return false;
			}
			Attribute[] attributes = ReflectionUtils.GetAttributes(type, null, false);
			for (int i = 0; i < (int)attributes.Length; i++)
			{
				if (string.Equals(attributes[i].GetType().FullName, "System.Runtime.CompilerServices.IsByRefLikeAttribute", StringComparison.Ordinal))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsGenericDefinition(Type type, Type genericInterfaceDefinition)
		{
			if (!type.IsGenericType())
			{
				return false;
			}
			return type.GetGenericTypeDefinition() == genericInterfaceDefinition;
		}

		public static bool IsIndexedProperty(PropertyInfo property)
		{
			ValidationUtils.ArgumentNotNull(property, "property");
			return property.GetIndexParameters().Length != 0;
		}

		public static bool IsMethodOverridden(Type currentType, Type methodDeclaringType, string method)
		{
			return currentType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Any<MethodInfo>((MethodInfo info) => {
				if (!(info.Name == method) || !(info.DeclaringType != methodDeclaringType))
				{
					return false;
				}
				return info.GetBaseDefinition().DeclaringType == methodDeclaringType;
			});
		}

		public static bool IsNullable(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			if (!t.IsValueType())
			{
				return true;
			}
			return ReflectionUtils.IsNullableType(t);
		}

		public static bool IsNullableType(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			if (!t.IsGenericType())
			{
				return false;
			}
			return t.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		private static bool IsOverridenGenericMember(MemberInfo memberInfo, BindingFlags bindingAttr)
		{
			if (memberInfo.MemberType() != MemberTypes.Property)
			{
				return false;
			}
			PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
			if (!propertyInfo.IsVirtual())
			{
				return false;
			}
			Type declaringType = propertyInfo.DeclaringType;
			if (!declaringType.IsGenericType())
			{
				return false;
			}
			Type genericTypeDefinition = declaringType.GetGenericTypeDefinition();
			if (genericTypeDefinition == null)
			{
				return false;
			}
			MemberInfo[] member = genericTypeDefinition.GetMember(propertyInfo.Name, bindingAttr);
			if (member.Length == 0)
			{
				return false;
			}
			if (!ReflectionUtils.GetMemberUnderlyingType(member[0]).IsGenericParameter)
			{
				return false;
			}
			return true;
		}

		public static bool IsPublic(PropertyInfo property)
		{
			if (property.GetGetMethod() != null && property.GetGetMethod().IsPublic)
			{
				return true;
			}
			if (property.GetSetMethod() != null && property.GetSetMethod().IsPublic)
			{
				return true;
			}
			return false;
		}

		public static bool IsVirtual(this PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			MethodInfo getMethod = propertyInfo.GetGetMethod(true);
			if (getMethod != null && getMethod.IsVirtual)
			{
				return true;
			}
			getMethod = propertyInfo.GetSetMethod(true);
			if (getMethod != null && getMethod.IsVirtual)
			{
				return true;
			}
			return false;
		}

		private static string RemoveAssemblyDetails(string fullyQualifiedTypeName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			bool flag1 = false;
			for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
			{
				char chr = fullyQualifiedTypeName[i];
				if (chr == ',')
				{
					if (flag)
					{
						flag1 = true;
					}
					else
					{
						flag = true;
						stringBuilder.Append(chr);
					}
				}
				else if (chr == '[')
				{
					flag = false;
					flag1 = false;
					stringBuilder.Append(chr);
				}
				else if (chr == ']')
				{
					flag = false;
					flag1 = false;
					stringBuilder.Append(chr);
				}
				else if (!flag1)
				{
					stringBuilder.Append(chr);
				}
			}
			return stringBuilder.ToString();
		}

		public static BindingFlags RemoveFlag(this BindingFlags bindingAttr, BindingFlags flag)
		{
			if ((bindingAttr & flag) != flag)
			{
				return bindingAttr;
			}
			return bindingAttr ^ flag;
		}

		public static void SetMemberValue(MemberInfo member, object target, object value)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			ValidationUtils.ArgumentNotNull(target, "target");
			MemberTypes memberType = member.MemberType();
			if (memberType == MemberTypes.Field)
			{
				((FieldInfo)member).SetValue(target, value);
				return;
			}
			if (memberType != MemberTypes.Property)
			{
				throw new ArgumentException("MemberInfo '{0}' must be of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, member.Name), "member");
			}
			((PropertyInfo)member).SetValue(target, value, null);
		}

		public static StructMultiKey<string, string> SplitFullyQualifiedTypeName(string fullyQualifiedTypeName)
		{
			string str;
			string str1;
			int? assemblyDelimiterIndex = ReflectionUtils.GetAssemblyDelimiterIndex(fullyQualifiedTypeName);
			if (!assemblyDelimiterIndex.HasValue)
			{
				str = fullyQualifiedTypeName;
				str1 = null;
			}
			else
			{
				str = fullyQualifiedTypeName.Trim(0, assemblyDelimiterIndex.GetValueOrDefault());
				str1 = fullyQualifiedTypeName.Trim(assemblyDelimiterIndex.GetValueOrDefault() + 1, fullyQualifiedTypeName.Length - assemblyDelimiterIndex.GetValueOrDefault() - 1);
			}
			return new StructMultiKey<string, string>(str1, str);
		}
	}
}