using System;

namespace Newtonsoft.Json.Utilities
{
	internal class FSharpFunction
	{
		private readonly object _instance;

		private readonly MethodCall<object, object> _invoker;

		public FSharpFunction(object instance, MethodCall<object, object> invoker)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this._instance = instance;
			this._invoker = invoker;
		}

		public object Invoke(params object[] args)
		{
			return this._invoker(this._instance, args);
		}
	}
}