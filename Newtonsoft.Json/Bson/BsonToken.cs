using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Bson
{
	internal abstract class BsonToken
	{
		public int CalculatedSize
		{
			get;
			set;
		}

		public BsonToken Parent
		{
			get;
			set;
		}

		public abstract BsonType Type
		{
			get;
		}

		protected BsonToken()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}
	}
}