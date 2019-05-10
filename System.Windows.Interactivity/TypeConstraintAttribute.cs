using System;
using System.Runtime.CompilerServices;

namespace System.Windows.Interactivity
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
	public sealed class TypeConstraintAttribute : Attribute
	{
		public Type Constraint
		{
			get;
			private set;
		}

		public TypeConstraintAttribute(Type constraint)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.Constraint = constraint;
		}
	}
}