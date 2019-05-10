using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Schema
{
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public class ValidationEventArgs : EventArgs
	{
		private readonly JsonSchemaException _ex;

		public JsonSchemaException Exception
		{
			get
			{
				return this._ex;
			}
		}

		public string Message
		{
			get
			{
				return this._ex.Message;
			}
		}

		public string Path
		{
			get
			{
				return this._ex.Path;
			}
		}

		internal ValidationEventArgs(JsonSchemaException ex)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			ValidationUtils.ArgumentNotNull(ex, "ex");
			this._ex = ex;
		}
	}
}