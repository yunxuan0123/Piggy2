using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	public class ErrorContext
	{
		public Exception Error
		{
			get;
		}

		public bool Handled
		{
			get;
			set;
		}

		public object Member
		{
			get;
		}

		public object OriginalObject
		{
			get;
		}

		public string Path
		{
			get;
		}

		internal bool Traced
		{
			get;
			set;
		}

		internal ErrorContext(object originalObject, object member, string path, Exception error)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.OriginalObject = originalObject;
			this.Member = member;
			this.Error = error;
			this.Path = path;
		}
	}
}