using System;

namespace Newtonsoft.Json
{
	public class DefaultJsonNameTable : JsonNameTable
	{
		private readonly static int HashCodeRandomizer;

		private int _count;

		private DefaultJsonNameTable.Entry[] _entries;

		private int _mask;

		static DefaultJsonNameTable()
		{
			Class6.yDnXvgqzyB5jw();
			DefaultJsonNameTable.HashCodeRandomizer = Environment.TickCount;
		}

		public DefaultJsonNameTable()
		{
			Class6.yDnXvgqzyB5jw();
			this._mask = 31;
			base();
			this._entries = new DefaultJsonNameTable.Entry[this._mask + 1];
		}

		public string Add(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int length = key.Length;
			if (length == 0)
			{
				return string.Empty;
			}
			int hashCodeRandomizer = length + DefaultJsonNameTable.HashCodeRandomizer;
			for (int i = 0; i < key.Length; i++)
			{
				hashCodeRandomizer = hashCodeRandomizer + (hashCodeRandomizer << 7 ^ key[i]);
			}
			hashCodeRandomizer = hashCodeRandomizer - (hashCodeRandomizer >> 17);
			hashCodeRandomizer = hashCodeRandomizer - (hashCodeRandomizer >> 11);
			hashCodeRandomizer = hashCodeRandomizer - (hashCodeRandomizer >> 5);
			for (DefaultJsonNameTable.Entry j = this._entries[hashCodeRandomizer & this._mask]; j != null; j = j.Next)
			{
				if (j.HashCode == hashCodeRandomizer && j.Value.Equals(key))
				{
					return j.Value;
				}
			}
			return this.AddEntry(key, hashCodeRandomizer);
		}

		private string AddEntry(string str, int hashCode)
		{
			int num = hashCode & this._mask;
			DefaultJsonNameTable.Entry entry = new DefaultJsonNameTable.Entry(str, hashCode, this._entries[num]);
			this._entries[num] = entry;
			int num1 = this._count;
			this._count = num1 + 1;
			if (num1 == this._mask)
			{
				this.Grow();
			}
			return entry.Value;
		}

		public override string Get(char[] key, int start, int length)
		{
			if (length == 0)
			{
				return string.Empty;
			}
			int num = length + DefaultJsonNameTable.HashCodeRandomizer;
			num = num + (num << 7 ^ key[start]);
			int num1 = start + length;
			for (int i = start + 1; i < num1; i++)
			{
				num = num + (num << 7 ^ key[i]);
			}
			num = num - (num >> 17);
			num = num - (num >> 11);
			num = num - (num >> 5);
			int num2 = num & this._mask;
			for (DefaultJsonNameTable.Entry j = this._entries[num2]; j != null; j = j.Next)
			{
				if (j.HashCode == num && DefaultJsonNameTable.TextEquals(j.Value, key, start, length))
				{
					return j.Value;
				}
			}
			return null;
		}

		private void Grow()
		{
			DefaultJsonNameTable.Entry next = null;
			DefaultJsonNameTable.Entry[] entryArray = this._entries;
			int num = this._mask * 2 + 1;
			DefaultJsonNameTable.Entry[] entryArray1 = new DefaultJsonNameTable.Entry[num + 1];
			for (int i = 0; i < (int)entryArray.Length; i++)
			{
				for (DefaultJsonNameTable.Entry j = entryArray[i]; j != null; j = next)
				{
					int hashCode = j.HashCode & num;
					next = j.Next;
					j.Next = entryArray1[hashCode];
					entryArray1[hashCode] = j;
				}
			}
			this._entries = entryArray1;
			this._mask = num;
		}

		private static bool TextEquals(string str1, char[] str2, int int_0, int int_1)
		{
			if (str1.Length != int_1)
			{
				return false;
			}
			for (int i = 0; i < str1.Length; i++)
			{
				if (str1[i] != str2[int_0 + i])
				{
					return false;
				}
			}
			return true;
		}

		private class Entry
		{
			internal readonly string Value;

			internal readonly int HashCode;

			internal DefaultJsonNameTable.Entry Next;

			internal Entry(string value, int hashCode, DefaultJsonNameTable.Entry next)
			{
				Class6.yDnXvgqzyB5jw();
				base();
				this.Value = value;
				this.HashCode = hashCode;
				this.Next = next;
			}
		}
	}
}