using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace System.Windows.Interactivity
{
	public sealed class InvokeCommandAction : TriggerAction<DependencyObject>
	{
		private string commandName;

		public readonly static DependencyProperty CommandProperty;

		public readonly static DependencyProperty CommandParameterProperty;

		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(InvokeCommandAction.CommandProperty);
			}
			set
			{
				base.SetValue(InvokeCommandAction.CommandProperty, value);
			}
		}

		public string CommandName
		{
			get
			{
				base.ReadPreamble();
				return this.commandName;
			}
			set
			{
				if (this.CommandName != value)
				{
					base.WritePreamble();
					this.commandName = value;
					base.WritePostscript();
				}
			}
		}

		public object CommandParameter
		{
			get
			{
				return base.GetValue(InvokeCommandAction.CommandParameterProperty);
			}
			set
			{
				base.SetValue(InvokeCommandAction.CommandParameterProperty, value);
			}
		}

		static InvokeCommandAction()
		{
			Class6.yDnXvgqzyB5jw();
			InvokeCommandAction.CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeCommandAction), null);
			InvokeCommandAction.CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(InvokeCommandAction), null);
		}

		public InvokeCommandAction()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		protected override void Invoke(object parameter)
		{
			if (base.AssociatedObject != null)
			{
				ICommand command = this.ResolveCommand();
				if (command != null && command.CanExecute(this.CommandParameter))
				{
					command.Execute(this.CommandParameter);
				}
			}
		}

		private ICommand ResolveCommand()
		{
			ICommand command = null;
			if (this.Command != null)
			{
				command = this.Command;
			}
			else if (base.AssociatedObject != null)
			{
				PropertyInfo[] properties = base.AssociatedObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
				for (int i = 0; i < (int)properties.Length; i++)
				{
					PropertyInfo propertyInfo = properties[i];
					if (typeof(ICommand).IsAssignableFrom(propertyInfo.PropertyType) && string.Equals(propertyInfo.Name, this.CommandName, StringComparison.Ordinal))
					{
						command = (ICommand)propertyInfo.GetValue(base.AssociatedObject, null);
					}
				}
			}
			return command;
		}
	}
}