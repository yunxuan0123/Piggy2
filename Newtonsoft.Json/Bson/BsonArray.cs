using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Bson
{
	internal class BsonArray : BsonToken, IEnumerable<BsonToken>, IEnumerable
	{
		private readonly List<BsonToken> _children;

		public override BsonType Type
		{
			get
			{
				return BsonType.Array;
			}
		}

		public BsonArray()
		{
			Class6.yDnXvgqzyB5jw();
			this._children = new List<BsonToken>();
			base();
		}

		public void Add(BsonToken token)
		{
			this._children.Add(token);
			token.Parent = this;
		}

		public IEnumerator<BsonToken> GetEnumerator()
		{
			return this._children.GetEnumerator();
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}