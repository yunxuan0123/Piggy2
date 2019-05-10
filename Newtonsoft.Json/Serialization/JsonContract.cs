using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
	public abstract class JsonContract
	{
		internal bool IsNullable;

		internal bool IsConvertable;

		internal bool IsEnum;

		internal Type NonNullableUnderlyingType;

		internal ReadType InternalReadType;

		internal JsonContractType ContractType;

		internal bool IsReadOnlyOrFixedSize;

		internal bool IsSealed;

		internal bool IsInstantiable;

		private List<SerializationCallback> _onDeserializedCallbacks;

		private IList<SerializationCallback> _onDeserializingCallbacks;

		private IList<SerializationCallback> _onSerializedCallbacks;

		private IList<SerializationCallback> _onSerializingCallbacks;

		private IList<SerializationErrorCallback> _onErrorCallbacks;

		private Type _createdType;

		public JsonConverter Converter
		{
			get;
			set;
		}

		public Type CreatedType
		{
			get
			{
				return this._createdType;
			}
			set
			{
				this._createdType = value;
				this.IsSealed = this._createdType.IsSealed();
				this.IsInstantiable = (this._createdType.IsInterface() ? false : !this._createdType.IsAbstract());
			}
		}

		public Func<object> DefaultCreator
		{
			get;
			set;
		}

		public bool DefaultCreatorNonPublic
		{
			get;
			set;
		}

		internal JsonConverter InternalConverter
		{
			get;
			set;
		}

		public bool? IsReference
		{
			get;
			set;
		}

		public IList<SerializationCallback> OnDeserializedCallbacks
		{
			get
			{
				if (this._onDeserializedCallbacks == null)
				{
					this._onDeserializedCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializedCallbacks;
			}
		}

		public IList<SerializationCallback> OnDeserializingCallbacks
		{
			get
			{
				if (this._onDeserializingCallbacks == null)
				{
					this._onDeserializingCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializingCallbacks;
			}
		}

		public IList<SerializationErrorCallback> OnErrorCallbacks
		{
			get
			{
				if (this._onErrorCallbacks == null)
				{
					this._onErrorCallbacks = new List<SerializationErrorCallback>();
				}
				return this._onErrorCallbacks;
			}
		}

		public IList<SerializationCallback> OnSerializedCallbacks
		{
			get
			{
				if (this._onSerializedCallbacks == null)
				{
					this._onSerializedCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializedCallbacks;
			}
		}

		public IList<SerializationCallback> OnSerializingCallbacks
		{
			get
			{
				if (this._onSerializingCallbacks == null)
				{
					this._onSerializingCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializingCallbacks;
			}
		}

		public Type UnderlyingType
		{
			get;
		}

		internal JsonContract(Type underlyingType)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			ValidationUtils.ArgumentNotNull(underlyingType, "underlyingType");
			this.UnderlyingType = underlyingType;
			underlyingType = ReflectionUtils.EnsureNotByRefType(underlyingType);
			this.IsNullable = ReflectionUtils.IsNullable(underlyingType);
			this.NonNullableUnderlyingType = (!this.IsNullable || !ReflectionUtils.IsNullableType(underlyingType) ? underlyingType : Nullable.GetUnderlyingType(underlyingType));
			this.CreatedType = this.NonNullableUnderlyingType;
			this.IsConvertable = ConvertUtils.IsConvertible(this.NonNullableUnderlyingType);
			this.IsEnum = this.NonNullableUnderlyingType.IsEnum();
			this.InternalReadType = ReadType.Read;
		}

		internal static SerializationCallback CreateSerializationCallback(MethodInfo callbackMethodInfo)
		{
			return (object o, StreamingContext context) => callbackMethodInfo.Invoke(o, new object[] { context });
		}

		internal static SerializationErrorCallback CreateSerializationErrorCallback(MethodInfo callbackMethodInfo)
		{
			return (object o, StreamingContext context, ErrorContext econtext) => callbackMethodInfo.Invoke(o, new object[] { context, econtext });
		}

		internal void InvokeOnDeserialized(object o, StreamingContext context)
		{
			if (this._onDeserializedCallbacks != null)
			{
				foreach (SerializationCallback _onDeserializedCallback in this._onDeserializedCallbacks)
				{
					_onDeserializedCallback(o, context);
				}
			}
		}

		internal void InvokeOnDeserializing(object o, StreamingContext context)
		{
			if (this._onDeserializingCallbacks != null)
			{
				foreach (SerializationCallback _onDeserializingCallback in this._onDeserializingCallbacks)
				{
					_onDeserializingCallback(o, context);
				}
			}
		}

		internal void InvokeOnError(object o, StreamingContext context, ErrorContext errorContext)
		{
			if (this._onErrorCallbacks != null)
			{
				foreach (SerializationErrorCallback _onErrorCallback in this._onErrorCallbacks)
				{
					_onErrorCallback(o, context, errorContext);
				}
			}
		}

		internal void InvokeOnSerialized(object o, StreamingContext context)
		{
			if (this._onSerializedCallbacks != null)
			{
				foreach (SerializationCallback _onSerializedCallback in this._onSerializedCallbacks)
				{
					_onSerializedCallback(o, context);
				}
			}
		}

		internal void InvokeOnSerializing(object o, StreamingContext context)
		{
			if (this._onSerializingCallbacks != null)
			{
				foreach (SerializationCallback _onSerializingCallback in this._onSerializingCallbacks)
				{
					_onSerializingCallback(o, context);
				}
			}
		}
	}
}