using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	[IsReadOnly]
	public struct JEnumerable<T> : IEnumerable<T>, IJEnumerable<T>, IEquatable<JEnumerable<T>>, IEnumerable
	where T : JToken
	{
		public readonly static JEnumerable<T> Empty;

		private readonly IEnumerable<T> _enumerable;

		public IJEnumerable<JToken> this[object key]
		{
			get
			{
				if (this._enumerable == null)
				{
					return (JEnumerable<JToken>)JEnumerable<JToken>.Empty;
				}
				return new JEnumerable<JToken>(Extensions.Values<T, JToken>(this._enumerable, key));
			}
		}

		static JEnumerable()
		{
			Class6.yDnXvgqzyB5jw();
			JEnumerable<T>.Empty = new JEnumerable<T>(Enumerable.Empty<T>());
		}

		public JEnumerable(IEnumerable<T> enumerable)
		{
			Class6.yDnXvgqzyB5jw();
			ValidationUtils.ArgumentNotNull(enumerable, "enumerable");
			this._enumerable = enumerable;
		}

		public bool Equals(JEnumerable<T> other)
		{
			return object.Equals(this._enumerable, other._enumerable);
		}

		public override bool Equals(object obj)
		{
			object obj1 = obj;
			object obj2 = obj1;
			if (!(obj1 is JEnumerable<T>))
			{
				return false;
			}
			return this.Equals((JEnumerable<T>)obj2);
		}

		public IEnumerator<T> GetEnumerator()
		{
			object empty = this._enumerable;
			if (empty == null)
			{
				empty = JEnumerable<T>.Empty;
			}
			return ((IEnumerable<T>)empty).GetEnumerator();
		}

		public override int GetHashCode()
		{
			if (this._enumerable == null)
			{
				return 0;
			}
			return this._enumerable.GetHashCode();
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}