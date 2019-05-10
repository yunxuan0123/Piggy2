using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	internal class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, ICollection, IDictionary, Interface2
	{
		private readonly IDictionary _dictionary;

		private readonly IDictionary<TKey, TValue> _genericDictionary;

		private readonly IReadOnlyDictionary<TKey, TValue> _readOnlyDictionary;

		private object _syncRoot;

		public int Count
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Count;
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.Count;
				}
				return this._genericDictionary.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.IsReadOnly;
				}
				if (this._readOnlyDictionary != null)
				{
					return true;
				}
				return this._genericDictionary.IsReadOnly;
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				if (this._dictionary != null)
				{
					return (TValue)this._dictionary[key];
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary[key];
				}
				return this._genericDictionary[key];
			}
			set
			{
				if (this._dictionary != null)
				{
					this._dictionary[key] = value;
					return;
				}
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				this._genericDictionary[key] = value;
			}
		}

		public ICollection<TKey> Keys
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Keys.Cast<TKey>().ToList<TKey>();
				}
				if (this._readOnlyDictionary == null)
				{
					return this._genericDictionary.Keys;
				}
				return this._readOnlyDictionary.Keys.ToList<TKey>();
			}
		}

		bool System.Collections.ICollection.IsSynchronized
		{
			get
			{
				if (this._dictionary == null)
				{
					return false;
				}
				return this._dictionary.IsSynchronized;
			}
		}

		object System.Collections.ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		bool System.Collections.IDictionary.IsFixedSize
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return false;
				}
				if (this._readOnlyDictionary != null)
				{
					return true;
				}
				return this._dictionary.IsFixedSize;
			}
		}

		object System.Collections.IDictionary.this[object key]
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary[key];
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary[(TKey)key];
				}
				return this._genericDictionary[(TKey)key];
			}
			set
			{
				if (this._dictionary != null)
				{
					this._dictionary[key] = value;
					return;
				}
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				this._genericDictionary[(TKey)key] = (TValue)value;
			}
		}

		ICollection System.Collections.IDictionary.Keys
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Keys.ToList<TKey>();
				}
				if (this._readOnlyDictionary == null)
				{
					return this._dictionary.Keys;
				}
				return this._readOnlyDictionary.Keys.ToList<TKey>();
			}
		}

		ICollection System.Collections.IDictionary.Values
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Values.ToList<TValue>();
				}
				if (this._readOnlyDictionary == null)
				{
					return this._dictionary.Values;
				}
				return this._readOnlyDictionary.Values.ToList<TValue>();
			}
		}

		public object UnderlyingDictionary
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary;
				}
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary;
				}
				return this._genericDictionary;
			}
		}

		public ICollection<TValue> Values
		{
			get
			{
				if (this._dictionary != null)
				{
					return this._dictionary.Values.Cast<TValue>().ToList<TValue>();
				}
				if (this._readOnlyDictionary == null)
				{
					return this._genericDictionary.Values;
				}
				return this._readOnlyDictionary.Values.ToList<TValue>();
			}
		}

		public DictionaryWrapper(IDictionary dictionary)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._dictionary = dictionary;
		}

		public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._genericDictionary = dictionary;
		}

		public DictionaryWrapper(IReadOnlyDictionary<TKey, TValue> dictionary)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._readOnlyDictionary = dictionary;
		}

		public void Add(TKey key, TValue value)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Add(key, value);
				return;
			}
			if (this._genericDictionary == null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.Add(key, value);
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary != null)
			{
				((IList)this._dictionary).Add(item);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			IDictionary<TKey, TValue> tKeys = this._genericDictionary;
			if (tKeys == null)
			{
				return;
			}
			tKeys.Add(item);
		}

		public void Clear()
		{
			if (this._dictionary != null)
			{
				this._dictionary.Clear();
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.Clear();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary != null)
			{
				return ((IList)this._dictionary).Contains(item);
			}
			if (this._readOnlyDictionary != null)
			{
				return this._readOnlyDictionary.Contains<KeyValuePair<TKey, TValue>>(item);
			}
			return this._genericDictionary.Contains(item);
		}

		public bool ContainsKey(TKey key)
		{
			if (this._dictionary != null)
			{
				return this._dictionary.Contains(key);
			}
			if (this._readOnlyDictionary != null)
			{
				return this._readOnlyDictionary.ContainsKey(key);
			}
			return this._genericDictionary.ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (this._dictionary == null)
			{
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				this._genericDictionary.CopyTo(array, arrayIndex);
			}
			else
			{
				IDictionaryEnumerator enumerator = this._dictionary.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						DictionaryEntry entry = enumerator.Entry;
						int num = arrayIndex;
						arrayIndex = num + 1;
						array[num] = new KeyValuePair<TKey, TValue>((TKey)entry.Key, (TValue)entry.Value);
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
					else
					{
					}
				}
			}
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (this._dictionary == null)
			{
				if (this._readOnlyDictionary != null)
				{
					return this._readOnlyDictionary.GetEnumerator();
				}
				return this._genericDictionary.GetEnumerator();
			}
			return (
				from DictionaryEntry  in this._dictionary
				select new KeyValuePair<TKey, TValue>((TKey)de.Key, (TValue)de.Value)).GetEnumerator();
		}

		public bool Remove(TKey key)
		{
			if (this._dictionary == null)
			{
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				return this._genericDictionary.Remove(key);
			}
			if (!this._dictionary.Contains(key))
			{
				return false;
			}
			this._dictionary.Remove(key);
			return true;
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			if (this._dictionary == null)
			{
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				return this._genericDictionary.Remove(item);
			}
			if (!this._dictionary.Contains(item.Key))
			{
				return true;
			}
			if (!object.Equals(this._dictionary[item.Key], item.Value))
			{
				return false;
			}
			this._dictionary.Remove(item.Key);
			return true;
		}

		public void Remove(object key)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Remove(key);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.Remove((TKey)key);
		}

		void System.Collections.ICollection.CopyTo(Array array, int index)
		{
			if (this._dictionary != null)
			{
				this._dictionary.CopyTo(array, index);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.CopyTo((KeyValuePair<TKey, TValue>[])array, index);
		}

		void System.Collections.IDictionary.Add(object key, object value)
		{
			if (this._dictionary != null)
			{
				this._dictionary.Add(key, value);
				return;
			}
			if (this._readOnlyDictionary != null)
			{
				throw new NotSupportedException();
			}
			this._genericDictionary.Add((TKey)key, (TValue)value);
		}

		bool System.Collections.IDictionary.Contains(object key)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.ContainsKey((TKey)key);
			}
			if (this._readOnlyDictionary == null)
			{
				return this._dictionary.Contains(key);
			}
			return this._readOnlyDictionary.ContainsKey((TKey)key);
		}

		IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator()
		{
			if (this._dictionary != null)
			{
				return this._dictionary.GetEnumerator();
			}
			if (this._readOnlyDictionary != null)
			{
				return new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this._readOnlyDictionary.GetEnumerator());
			}
			return new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator<TKey, TValue>(this._genericDictionary.GetEnumerator());
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			if (this._dictionary == null)
			{
				if (this._readOnlyDictionary != null)
				{
					throw new NotSupportedException();
				}
				return this._genericDictionary.TryGetValue(key, out value);
			}
			if (!this._dictionary.Contains(key))
			{
				value = default(TValue);
				return false;
			}
			value = (TValue)this._dictionary[key];
			return true;
		}

		[IsReadOnly]
		private struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : IEnumerator, IDictionaryEnumerator
		{
			private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;

			public object Current
			{
				get
				{
					KeyValuePair<TEnumeratorKey, TEnumeratorValue> current = this._e.Current;
					object key = current.Key;
					current = this._e.Current;
					return new DictionaryEntry(key, (object)current.Value);
				}
			}

			public DictionaryEntry Entry
			{
				get
				{
					return (DictionaryEntry)this.Current;
				}
			}

			public object Key
			{
				get
				{
					return this.Entry.Key;
				}
			}

			public object Value
			{
				get
				{
					return this.Entry.Value;
				}
			}

			public DictionaryEnumerator(IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
			{
				Class6.yDnXvgqzyB5jw();
				ValidationUtils.ArgumentNotNull(e, "e");
				this._e = e;
			}

			public bool MoveNext()
			{
				return this._e.MoveNext();
			}

			public void Reset()
			{
				this._e.Reset();
			}
		}
	}
}