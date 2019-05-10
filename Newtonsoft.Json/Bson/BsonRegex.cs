using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Bson
{
	internal class BsonRegex : BsonToken
	{
		public BsonString Options
		{
			get;
			set;
		}

		public BsonString Pattern
		{
			get;
			set;
		}

		public override BsonType Type
		{
			get
			{
				return BsonType.Regex;
			}
		}

		public BsonRegex(string pattern, string options)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.Pattern = new BsonString(pattern, false);
			this.Options = new BsonString(options, false);
		}
	}
}