using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	public class ErrorEventArgs : EventArgs
	{
		public object CurrentObject
		{
			get;
		}

		public Newtonsoft.Json.Serialization.ErrorContext ErrorContext
		{
			get;
		}

		public ErrorEventArgs(object currentObject, Newtonsoft.Json.Serialization.ErrorContext errorContext)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.CurrentObject = currentObject;
			this.ErrorContext = errorContext;
		}
	}
}