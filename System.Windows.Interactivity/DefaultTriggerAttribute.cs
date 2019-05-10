using System;
using System.Collections;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Interactivity
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple=true)]
	[CLSCompliant(false)]
	public sealed class DefaultTriggerAttribute : Attribute
	{
		private Type targetType;

		private Type triggerType;

		private object[] parameters;

		public IEnumerable Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		public Type TargetType
		{
			get
			{
				return this.targetType;
			}
		}

		public Type TriggerType
		{
			get
			{
				return this.triggerType;
			}
		}

		public DefaultTriggerAttribute(Type targetType, Type triggerType, object parameter)
		{
			Class6.yDnXvgqzyB5jw();
			this(targetType, triggerType, new object[] { parameter });
		}

		public DefaultTriggerAttribute(Type targetType, Type triggerType, params object[] parameters)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			if (!typeof(System.Windows.Interactivity.TriggerBase).IsAssignableFrom(triggerType))
			{
				CultureInfo currentCulture = CultureInfo.CurrentCulture;
				string defaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage = ExceptionStringTable.DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage;
				object[] name = new object[] { triggerType.Name };
				throw new ArgumentException(string.Format(currentCulture, defaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage, name));
			}
			this.targetType = targetType;
			this.triggerType = triggerType;
			this.parameters = parameters;
		}

		public System.Windows.Interactivity.TriggerBase Instantiate()
		{
			object obj = null;
			try
			{
				obj = Activator.CreateInstance(this.TriggerType, this.parameters);
			}
			catch
			{
			}
			return (System.Windows.Interactivity.TriggerBase)obj;
		}
	}
}