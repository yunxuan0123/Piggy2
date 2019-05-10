using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace Newtonsoft.Json.Serialization
{
	internal static class JsonTypeReflector
	{
		private static bool? _dynamicCodeGeneration;

		private static bool? _fullyTrusted;

		public const string IdPropertyName = "$id";

		public const string RefPropertyName = "$ref";

		public const string TypePropertyName = "$type";

		public const string ValuePropertyName = "$value";

		public const string ArrayValuesPropertyName = "$values";

		public const string ShouldSerializePrefix = "ShouldSerialize";

		public const string SpecifiedPostfix = "Specified";

		public const string ConcurrentDictionaryTypeName = "System.Collections.Concurrent.ConcurrentDictionary`2";

		private readonly static ThreadSafeStore<Type, Func<object[], object>> CreatorCache;

		private readonly static ThreadSafeStore<Type, Type> AssociatedMetadataTypesCache;

		private static ReflectionObject _metadataTypeAttributeReflectionObject;

		public static bool DynamicCodeGeneration
		{
			[SecuritySafeCritical]
			get
			{
				if (!JsonTypeReflector._dynamicCodeGeneration.HasValue)
				{
					try
					{
						(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess)).Demand();
						(new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess)).Demand();
						(new SecurityPermission(SecurityPermissionFlag.SkipVerification)).Demand();
						(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode)).Demand();
						(new SecurityPermission(PermissionState.Unrestricted)).Demand();
						JsonTypeReflector._dynamicCodeGeneration = new bool?(true);
					}
					catch (Exception exception)
					{
						JsonTypeReflector._dynamicCodeGeneration = new bool?(false);
					}
				}
				return JsonTypeReflector._dynamicCodeGeneration.GetValueOrDefault();
			}
		}

		public static bool FullyTrusted
		{
			get
			{
				if (!JsonTypeReflector._fullyTrusted.HasValue)
				{
					AppDomain currentDomain = AppDomain.CurrentDomain;
					JsonTypeReflector._fullyTrusted = new bool?((!currentDomain.IsHomogenous ? false : currentDomain.IsFullyTrusted));
				}
				return JsonTypeReflector._fullyTrusted.GetValueOrDefault();
			}
		}

		public static Newtonsoft.Json.Utilities.ReflectionDelegateFactory ReflectionDelegateFactory
		{
			get
			{
				if (JsonTypeReflector.DynamicCodeGeneration)
				{
					return DynamicReflectionDelegateFactory.Instance;
				}
				return LateBoundReflectionDelegateFactory.Instance;
			}
		}

		static JsonTypeReflector()
		{
			Class6.yDnXvgqzyB5jw();
			JsonTypeReflector.CreatorCache = new ThreadSafeStore<Type, Func<object[], object>>(new Func<Type, Func<object[], object>>(JsonTypeReflector.GetCreator));
			JsonTypeReflector.AssociatedMetadataTypesCache = new ThreadSafeStore<Type, Type>(new Func<Type, Type>(JsonTypeReflector.GetAssociateMetadataTypeFromAttribute));
		}

		public static bool CanTypeDescriptorConvertString(Type type, out TypeConverter typeConverter)
		{
			typeConverter = TypeDescriptor.GetConverter(type);
			if (typeConverter != null)
			{
				Type type1 = typeConverter.GetType();
				if (!string.Equals(type1.FullName, "System.ComponentModel.ComponentConverter", StringComparison.Ordinal) && !string.Equals(type1.FullName, "System.ComponentModel.ReferenceConverter", StringComparison.Ordinal) && !string.Equals(type1.FullName, "System.Windows.Forms.Design.DataSourceConverter", StringComparison.Ordinal) && type1 != typeof(TypeConverter))
				{
					return typeConverter.CanConvertTo(typeof(string));
				}
			}
			return false;
		}

		public static JsonConverter CreateJsonConverterInstance(Type converterType, object[] args)
		{
			return (JsonConverter)JsonTypeReflector.CreatorCache.Get(converterType)(args);
		}

		public static NamingStrategy CreateNamingStrategyInstance(Type namingStrategyType, object[] args)
		{
			return (NamingStrategy)JsonTypeReflector.CreatorCache.Get(namingStrategyType)(args);
		}

		private static Type GetAssociatedMetadataType(Type type)
		{
			return JsonTypeReflector.AssociatedMetadataTypesCache.Get(type);
		}

		private static Type GetAssociateMetadataTypeFromAttribute(Type type)
		{
			Attribute[] attributes = ReflectionUtils.GetAttributes(type, null, true);
			for (int i = 0; i < (int)attributes.Length; i++)
			{
				Attribute attribute = attributes[i];
				Type type1 = attribute.GetType();
				if (string.Equals(type1.FullName, "System.ComponentModel.DataAnnotations.MetadataTypeAttribute", StringComparison.Ordinal))
				{
					if (JsonTypeReflector._metadataTypeAttributeReflectionObject == null)
					{
						JsonTypeReflector._metadataTypeAttributeReflectionObject = ReflectionObject.Create(type1, new string[] { "MetadataClassType" });
					}
					return (Type)JsonTypeReflector._metadataTypeAttributeReflectionObject.GetValue(attribute, "MetadataClassType");
				}
			}
			return null;
		}

		private static T GetAttribute<T>(Type type)
		where T : Attribute
		{
			T attribute;
			Type associatedMetadataType = JsonTypeReflector.GetAssociatedMetadataType(type);
			if (associatedMetadataType != null)
			{
				attribute = ReflectionUtils.GetAttribute<T>(associatedMetadataType, true);
				if (attribute != null)
				{
					return attribute;
				}
			}
			attribute = ReflectionUtils.GetAttribute<T>(type, true);
			if (attribute != null)
			{
				return attribute;
			}
			Type[] interfaces = type.GetInterfaces();
			for (int i = 0; i < (int)interfaces.Length; i++)
			{
				attribute = ReflectionUtils.GetAttribute<T>(interfaces[i], true);
				if (attribute != null)
				{
					return attribute;
				}
			}
			return default(T);
		}

		private static T GetAttribute<T>(MemberInfo memberInfo)
		where T : Attribute
		{
			T attribute;
			Type associatedMetadataType = JsonTypeReflector.GetAssociatedMetadataType(memberInfo.DeclaringType);
			if (associatedMetadataType != null)
			{
				MemberInfo memberInfoFromType = ReflectionUtils.GetMemberInfoFromType(associatedMetadataType, memberInfo);
				if (memberInfoFromType != null)
				{
					attribute = ReflectionUtils.GetAttribute<T>(memberInfoFromType, true);
					if (attribute != null)
					{
						return attribute;
					}
				}
			}
			attribute = ReflectionUtils.GetAttribute<T>(memberInfo, true);
			if (attribute != null)
			{
				return attribute;
			}
			if (memberInfo.DeclaringType != null)
			{
				Type[] interfaces = memberInfo.DeclaringType.GetInterfaces();
				for (int i = 0; i < (int)interfaces.Length; i++)
				{
					MemberInfo memberInfoFromType1 = ReflectionUtils.GetMemberInfoFromType(interfaces[i], memberInfo);
					if (memberInfoFromType1 != null)
					{
						attribute = ReflectionUtils.GetAttribute<T>(memberInfoFromType1, true);
						if (attribute != null)
						{
							return attribute;
						}
					}
				}
			}
			return default(T);
		}

		public static T GetAttribute<T>(ICustomAttributeProvider provider)
		where T : Attribute
		{
			Type type = provider as Type;
			Type type1 = type;
			if (type != null)
			{
				return JsonTypeReflector.GetAttribute<T>(type1);
			}
			MemberInfo memberInfo = provider as MemberInfo;
			MemberInfo memberInfo1 = memberInfo;
			if (memberInfo != null)
			{
				return JsonTypeReflector.GetAttribute<T>(memberInfo1);
			}
			return ReflectionUtils.GetAttribute<T>(provider, true);
		}

		public static T GetCachedAttribute<T>(object attributeProvider)
		where T : Attribute
		{
			return CachedAttributeGetter<T>.GetAttribute(attributeProvider);
		}

		public static NamingStrategy GetContainerNamingStrategy(JsonContainerAttribute containerAttribute)
		{
			if (containerAttribute.NamingStrategyInstance == null)
			{
				if (containerAttribute.NamingStrategyType == null)
				{
					return null;
				}
				containerAttribute.NamingStrategyInstance = JsonTypeReflector.CreateNamingStrategyInstance(containerAttribute.NamingStrategyType, containerAttribute.NamingStrategyParameters);
			}
			return containerAttribute.NamingStrategyInstance;
		}

		private static Func<object[], object> GetCreator(Type type)
		{
			Func<object> func;
			if (ReflectionUtils.HasDefaultConstructor(type, false))
			{
				func = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type);
			}
			else
			{
				func = null;
			}
			Func<object> func1 = func;
			return (object[] parameters) => {
				object obj;
				try
				{
					if (parameters == null)
					{
						if (func1 == null)
						{
							throw new JsonException("No parameterless constructor defined for '{0}'.".FormatWith(CultureInfo.InvariantCulture, type));
						}
						obj = func1();
					}
					else
					{
						object[] objArray = parameters;
						Func<object, Type> u003cu003e9_221 = JsonTypeReflector.<>c.<>9__22_1;
						if (u003cu003e9_221 == null)
						{
							u003cu003e9_221 = (object param) => {
								if (param == null)
								{
									throw new InvalidOperationException("Cannot pass a null parameter to the constructor.");
								}
								return param.GetType();
							};
							JsonTypeReflector.<>c.<>9__22_1 = u003cu003e9_221;
						}
						Type[] array = ((IEnumerable<object>)objArray).Select<object, Type>(u003cu003e9_221).ToArray<Type>();
						ConstructorInfo constructor = type.GetConstructor(array);
						if (constructor == null)
						{
							throw new JsonException("No matching parameterized constructor found for '{0}'.".FormatWith(CultureInfo.InvariantCulture, type));
						}
						obj = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor)(parameters);
					}
				}
				catch (Exception exception)
				{
					throw new JsonException("Error creating '{0}'.".FormatWith(CultureInfo.InvariantCulture, type), exception);
				}
				return obj;
			};
		}

		public static DataContractAttribute GetDataContractAttribute(Type type)
		{
			for (Type i = type; i != null; i = i.BaseType())
			{
				DataContractAttribute attribute = CachedAttributeGetter<DataContractAttribute>.GetAttribute(i);
				if (attribute != null)
				{
					return attribute;
				}
			}
			return null;
		}

		public static DataMemberAttribute GetDataMemberAttribute(MemberInfo memberInfo)
		{
			if (memberInfo.MemberType() == MemberTypes.Field)
			{
				return CachedAttributeGetter<DataMemberAttribute>.GetAttribute(memberInfo);
			}
			PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
			DataMemberAttribute attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(propertyInfo);
			if (attribute == null && propertyInfo.IsVirtual())
			{
				for (Type i = propertyInfo.DeclaringType; attribute == null && i != null; i = i.BaseType())
				{
					PropertyInfo memberInfoFromType = (PropertyInfo)ReflectionUtils.GetMemberInfoFromType(i, propertyInfo);
					if (memberInfoFromType != null && memberInfoFromType.IsVirtual())
					{
						attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(memberInfoFromType);
					}
				}
			}
			return attribute;
		}

		public static JsonConverter GetJsonConverter(object attributeProvider)
		{
			JsonConverterAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonConverterAttribute>(attributeProvider);
			if (cachedAttribute != null)
			{
				Func<object[], object> func = JsonTypeReflector.CreatorCache.Get(cachedAttribute.ConverterType);
				if (func != null)
				{
					return (JsonConverter)func(cachedAttribute.ConverterParameters);
				}
			}
			return null;
		}

		public static MemberSerialization GetObjectMemberSerialization(Type objectType, bool ignoreSerializableAttribute)
		{
			JsonObjectAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonObjectAttribute>(objectType);
			if (cachedAttribute != null)
			{
				return cachedAttribute.MemberSerialization;
			}
			if (JsonTypeReflector.GetDataContractAttribute(objectType) != null)
			{
				return MemberSerialization.OptIn;
			}
			if (!ignoreSerializableAttribute && JsonTypeReflector.IsSerializable(objectType))
			{
				return MemberSerialization.Fields;
			}
			return MemberSerialization.OptOut;
		}

		public static bool IsNonSerializable(ICustomAttributeProvider provider)
		{
			return ReflectionUtils.GetAttribute<NonSerializedAttribute>(provider, false) != null;
		}

		public static bool IsSerializable(ICustomAttributeProvider provider)
		{
			return ReflectionUtils.GetAttribute<SerializableAttribute>(provider, false) != null;
		}
	}
}