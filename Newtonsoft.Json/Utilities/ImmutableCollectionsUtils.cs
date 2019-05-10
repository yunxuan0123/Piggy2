using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	internal static class ImmutableCollectionsUtils
	{
		private readonly static IList<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo> ArrayContractImmutableCollectionDefinitions;

		private readonly static IList<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo> DictionaryContractImmutableCollectionDefinitions;

		static ImmutableCollectionsUtils()
		{
			Class6.yDnXvgqzyB5jw();
			ImmutableCollectionsUtils.ArrayContractImmutableCollectionDefinitions = new List<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>()
			{
				new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableList`1", "System.Collections.Immutable.ImmutableList`1", "System.Collections.Immutable.ImmutableList"),
				new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableList`1", "System.Collections.Immutable.ImmutableList`1", "System.Collections.Immutable.ImmutableList"),
				new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue"),
				new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue`1", "System.Collections.Immutable.ImmutableQueue"),
				new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableStack`1", "System.Collections.Immutable.ImmutableStack`1", "System.Collections.Immutable.ImmutableStack"),
				new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableStack`1", "System.Collections.Immutable.ImmutableStack`1", "System.Collections.Immutable.ImmutableStack"),
				new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableSet`1", "System.Collections.Immutable.ImmutableSortedSet`1", "System.Collections.Immutable.ImmutableSortedSet"),
				new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableSortedSet`1", "System.Collections.Immutable.ImmutableSortedSet`1", "System.Collections.Immutable.ImmutableSortedSet"),
				new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableHashSet`1", "System.Collections.Immutable.ImmutableHashSet`1", "System.Collections.Immutable.ImmutableHashSet"),
				new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableArray`1", "System.Collections.Immutable.ImmutableArray`1", "System.Collections.Immutable.ImmutableArray")
			};
			ImmutableCollectionsUtils.DictionaryContractImmutableCollectionDefinitions = new List<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>()
			{
				new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.IImmutableDictionary`2", "System.Collections.Immutable.ImmutableSortedDictionary`2", "System.Collections.Immutable.ImmutableSortedDictionary"),
				new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableSortedDictionary`2", "System.Collections.Immutable.ImmutableSortedDictionary`2", "System.Collections.Immutable.ImmutableSortedDictionary"),
				new ImmutableCollectionsUtils.ImmutableCollectionTypeInfo("System.Collections.Immutable.ImmutableDictionary`2", "System.Collections.Immutable.ImmutableDictionary`2", "System.Collections.Immutable.ImmutableDictionary")
			};
		}

		internal static bool TryBuildImmutableForArrayContract(Type underlyingType, Type collectionItemType, out Type createdType, out ObjectConstructor<object> parameterizedCreator)
		{
			if (underlyingType.IsGenericType())
			{
				Type genericTypeDefinition = underlyingType.GetGenericTypeDefinition();
				string fullName = genericTypeDefinition.FullName;
				ImmutableCollectionsUtils.ImmutableCollectionTypeInfo immutableCollectionTypeInfo = ImmutableCollectionsUtils.ArrayContractImmutableCollectionDefinitions.FirstOrDefault<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>((ImmutableCollectionsUtils.ImmutableCollectionTypeInfo d) => d.ContractTypeName == fullName);
				if (immutableCollectionTypeInfo != null)
				{
					Type type = genericTypeDefinition.Assembly().GetType(immutableCollectionTypeInfo.CreatedTypeName);
					Type type1 = genericTypeDefinition.Assembly().GetType(immutableCollectionTypeInfo.BuilderTypeName);
					if (type != null && type1 != null)
					{
						MethodInfo methodInfo = ((IEnumerable<MethodInfo>)type1.GetMethods()).FirstOrDefault<MethodInfo>((MethodInfo m) => {
							if (m.Name != "CreateRange")
							{
								return false;
							}
							return (int)m.GetParameters().Length == 1;
						});
						if (methodInfo != null)
						{
							createdType = type.MakeGenericType(new Type[] { collectionItemType });
							MethodInfo methodInfo1 = methodInfo.MakeGenericMethod(new Type[] { collectionItemType });
							parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(methodInfo1);
							return true;
						}
					}
				}
			}
			createdType = null;
			parameterizedCreator = null;
			return false;
		}

		internal static bool TryBuildImmutableForDictionaryContract(Type underlyingType, Type keyItemType, Type valueItemType, out Type createdType, out ObjectConstructor<object> parameterizedCreator)
		{
			if (underlyingType.IsGenericType())
			{
				Type genericTypeDefinition = underlyingType.GetGenericTypeDefinition();
				string fullName = genericTypeDefinition.FullName;
				ImmutableCollectionsUtils.ImmutableCollectionTypeInfo immutableCollectionTypeInfo = ImmutableCollectionsUtils.DictionaryContractImmutableCollectionDefinitions.FirstOrDefault<ImmutableCollectionsUtils.ImmutableCollectionTypeInfo>((ImmutableCollectionsUtils.ImmutableCollectionTypeInfo d) => d.ContractTypeName == fullName);
				if (immutableCollectionTypeInfo != null)
				{
					Type type = genericTypeDefinition.Assembly().GetType(immutableCollectionTypeInfo.CreatedTypeName);
					Type type1 = genericTypeDefinition.Assembly().GetType(immutableCollectionTypeInfo.BuilderTypeName);
					if (type != null && type1 != null)
					{
						MethodInfo methodInfo = ((IEnumerable<MethodInfo>)type1.GetMethods()).FirstOrDefault<MethodInfo>((MethodInfo m) => {
							ParameterInfo[] parameters = m.GetParameters();
							if (!(m.Name == "CreateRange") || (int)parameters.Length != 1 || !parameters[0].ParameterType.IsGenericType())
							{
								return false;
							}
							return parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>);
						});
						if (methodInfo != null)
						{
							createdType = type.MakeGenericType(new Type[] { keyItemType, valueItemType });
							MethodInfo methodInfo1 = methodInfo.MakeGenericMethod(new Type[] { keyItemType, valueItemType });
							parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(methodInfo1);
							return true;
						}
					}
				}
			}
			createdType = null;
			parameterizedCreator = null;
			return false;
		}

		internal class ImmutableCollectionTypeInfo
		{
			public string BuilderTypeName
			{
				get;
				set;
			}

			public string ContractTypeName
			{
				get;
				set;
			}

			public string CreatedTypeName
			{
				get;
				set;
			}

			public ImmutableCollectionTypeInfo(string contractTypeName, string createdTypeName, string builderTypeName)
			{
				Class6.yDnXvgqzyB5jw();
				base();
				this.ContractTypeName = contractTypeName;
				this.CreatedTypeName = createdTypeName;
				this.BuilderTypeName = builderTypeName;
			}
		}
	}
}