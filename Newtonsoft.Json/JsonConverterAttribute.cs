using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple=false)]
	public sealed class JsonConverterAttribute : Attribute
	{
		private readonly Type _converterType;

		public object[] ConverterParameters
		{
			get;
		}

		public Type ConverterType
		{
			get
			{
				return this._converterType;
			}
		}

		public JsonConverterAttribute(Type converterType)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			if (converterType == null)
			{
				throw new ArgumentNullException("converterType");
			}
			this._converterType = converterType;
		}

		public JsonConverterAttribute(Type converterType, params object[] converterParameters)
		{
			Class6.yDnXvgqzyB5jw();
			this(converterType);
			this.ConverterParameters = converterParameters;
		}
	}
}