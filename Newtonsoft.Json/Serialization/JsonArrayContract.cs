using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	public class JsonArrayContract : JsonContainerContract
	{
		private readonly Type _genericCollectionDefinitionType;

		private Type _genericWrapperType;

		private ObjectConstructor<object> _genericWrapperCreator;

		private Func<object> _genericTemporaryCollectionCreator;

		private readonly ConstructorInfo _parameterizedConstructor;

		private ObjectConstructor<object> _parameterizedCreator;

		private ObjectConstructor<object> _overrideCreator;

		internal bool CanDeserialize
		{
			get;
			private set;
		}

		public Type CollectionItemType
		{
			get;
		}

		public bool HasParameterizedCreator
		{
			get;
			set;
		}

		internal bool HasParameterizedCreatorInternal
		{
			get
			{
				if (this.HasParameterizedCreator || this._parameterizedCreator != null)
				{
					return true;
				}
				return this._parameterizedConstructor != null;
			}
		}

		internal bool IsArray
		{
			get;
		}

		public bool IsMultidimensionalArray
		{
			get;
		}

		public ObjectConstructor<object> OverrideCreator
		{
			get
			{
				return this._overrideCreator;
			}
			set
			{
				this._overrideCreator = value;
				this.CanDeserialize = true;
			}
		}

		internal ObjectConstructor<object> ParameterizedCreator
		{
			get
			{
				if (this._parameterizedCreator == null)
				{
					this._parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(this._parameterizedConstructor);
				}
				return this._parameterizedCreator;
			}
		}

		internal bool ShouldCreateWrapper
		{
			get;
		}

		public JsonArrayContract(Type underlyingType)
		{
			Class6.yDnXvgqzyB5jw();
			base(underlyingType);
			bool hasParameterizedCreatorInternal;
			Type type;
			Type type1;
			ObjectConstructor<object> objectConstructor;
			this.ContractType = JsonContractType.Array;
			this.IsArray = base.CreatedType.IsArray;
			if (this.IsArray)
			{
				this.CollectionItemType = ReflectionUtils.GetCollectionItemType(base.UnderlyingType);
				this.IsReadOnlyOrFixedSize = true;
				this._genericCollectionDefinitionType = typeof(List<>).MakeGenericType(new Type[] { this.CollectionItemType });
				hasParameterizedCreatorInternal = true;
				this.IsMultidimensionalArray = (!this.IsArray ? false : base.UnderlyingType.GetArrayRank() > 1);
			}
			else if (typeof(IList).IsAssignableFrom(underlyingType))
			{
				if (!ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(ICollection<>), out this._genericCollectionDefinitionType))
				{
					this.CollectionItemType = ReflectionUtils.GetCollectionItemType(underlyingType);
				}
				else
				{
					this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				}
				if (underlyingType == typeof(IList))
				{
					base.CreatedType = typeof(List<object>);
				}
				if (this.CollectionItemType != null)
				{
					this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(underlyingType, this.CollectionItemType);
				}
				this.IsReadOnlyOrFixedSize = ReflectionUtils.InheritsGenericDefinition(underlyingType, typeof(ReadOnlyCollection<>));
				hasParameterizedCreatorInternal = true;
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(ICollection<>), out this._genericCollectionDefinitionType))
			{
				this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(underlyingType, typeof(ICollection<>)) || ReflectionUtils.IsGenericDefinition(underlyingType, typeof(IList<>)))
				{
					base.CreatedType = typeof(List<>).MakeGenericType(new Type[] { this.CollectionItemType });
				}
				if (ReflectionUtils.IsGenericDefinition(underlyingType, typeof(ISet<>)))
				{
					base.CreatedType = typeof(HashSet<>).MakeGenericType(new Type[] { this.CollectionItemType });
				}
				this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(underlyingType, this.CollectionItemType);
				hasParameterizedCreatorInternal = true;
				this.ShouldCreateWrapper = true;
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(IReadOnlyCollection<>), out type))
			{
				this.CollectionItemType = type.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(underlyingType, typeof(IReadOnlyCollection<>)) || ReflectionUtils.IsGenericDefinition(underlyingType, typeof(IReadOnlyList<>)))
				{
					base.CreatedType = typeof(ReadOnlyCollection<>).MakeGenericType(new Type[] { this.CollectionItemType });
				}
				this._genericCollectionDefinitionType = typeof(List<>).MakeGenericType(new Type[] { this.CollectionItemType });
				this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(base.CreatedType, this.CollectionItemType);
				this.StoreFSharpListCreatorIfNecessary(underlyingType);
				this.IsReadOnlyOrFixedSize = true;
				hasParameterizedCreatorInternal = this.HasParameterizedCreatorInternal;
			}
			else if (!ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(IEnumerable<>), out type))
			{
				hasParameterizedCreatorInternal = false;
				this.ShouldCreateWrapper = true;
			}
			else
			{
				this.CollectionItemType = type.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(base.UnderlyingType, typeof(IEnumerable<>)))
				{
					base.CreatedType = typeof(List<>).MakeGenericType(new Type[] { this.CollectionItemType });
				}
				this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(underlyingType, this.CollectionItemType);
				this.StoreFSharpListCreatorIfNecessary(underlyingType);
				if (!underlyingType.IsGenericType() || !(underlyingType.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
				{
					this._genericCollectionDefinitionType = typeof(List<>).MakeGenericType(new Type[] { this.CollectionItemType });
					this.IsReadOnlyOrFixedSize = true;
					this.ShouldCreateWrapper = true;
					hasParameterizedCreatorInternal = this.HasParameterizedCreatorInternal;
				}
				else
				{
					this._genericCollectionDefinitionType = type;
					this.IsReadOnlyOrFixedSize = false;
					this.ShouldCreateWrapper = false;
					hasParameterizedCreatorInternal = true;
				}
			}
			this.CanDeserialize = hasParameterizedCreatorInternal;
			if (ImmutableCollectionsUtils.TryBuildImmutableForArrayContract(underlyingType, this.CollectionItemType, out type1, out objectConstructor))
			{
				base.CreatedType = type1;
				this._parameterizedCreator = objectConstructor;
				this.IsReadOnlyOrFixedSize = true;
				this.CanDeserialize = true;
			}
		}

		internal IList CreateTemporaryCollection()
		{
			Type type;
			if (this._genericTemporaryCollectionCreator == null)
			{
				type = (this.IsMultidimensionalArray || this.CollectionItemType == null ? typeof(object) : this.CollectionItemType);
				Type type1 = typeof(List<>).MakeGenericType(new Type[] { type });
				this._genericTemporaryCollectionCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type1);
			}
			return (IList)this._genericTemporaryCollectionCreator();
		}

		internal Interface1 CreateWrapper(object list)
		{
			Type type;
			if (this._genericWrapperCreator == null)
			{
				this._genericWrapperType = typeof(CollectionWrapper<>).MakeGenericType(new Type[] { this.CollectionItemType });
				type = (ReflectionUtils.InheritsGenericDefinition(this._genericCollectionDefinitionType, typeof(List<>)) || this._genericCollectionDefinitionType.GetGenericTypeDefinition() == typeof(IEnumerable<>) ? typeof(ICollection<>).MakeGenericType(new Type[] { this.CollectionItemType }) : this._genericCollectionDefinitionType);
				ConstructorInfo constructor = this._genericWrapperType.GetConstructor(new Type[] { type });
				this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor);
			}
			return (Interface1)this._genericWrapperCreator(new object[] { list });
		}

		private void StoreFSharpListCreatorIfNecessary(Type underlyingType)
		{
			if (!this.HasParameterizedCreatorInternal && underlyingType.Name == "FSharpList`1")
			{
				FSharpUtils.EnsureInitialized(underlyingType.Assembly());
				this._parameterizedCreator = FSharpUtils.CreateSeq(this.CollectionItemType);
			}
		}
	}
}