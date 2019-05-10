using System;
using System.Runtime.CompilerServices;

namespace System.Windows.Interactivity
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
	public sealed class CustomPropertyValueEditorAttribute : Attribute
	{
		public System.Windows.Interactivity.CustomPropertyValueEditor CustomPropertyValueEditor
		{
			get;
			private set;
		}

		public CustomPropertyValueEditorAttribute(System.Windows.Interactivity.CustomPropertyValueEditor customPropertyValueEditor)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.CustomPropertyValueEditor = customPropertyValueEditor;
		}
	}
}