using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.IO;
using System.Numerics;

namespace Newtonsoft.Json.Bson
{
	[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
	public class BsonWriter : JsonWriter
	{
		private readonly BsonBinaryWriter _writer;

		private BsonToken _root;

		private BsonToken _parent;

		private string _propertyName;

		public DateTimeKind DateTimeKindHandling
		{
			get
			{
				return this._writer.DateTimeKindHandling;
			}
			set
			{
				this._writer.DateTimeKindHandling = value;
			}
		}

		public BsonWriter(Stream stream)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			ValidationUtils.ArgumentNotNull(stream, "stream");
			this._writer = new BsonBinaryWriter(new BinaryWriter(stream));
		}

		public BsonWriter(BinaryWriter writer)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			ValidationUtils.ArgumentNotNull(writer, "writer");
			this._writer = new BsonBinaryWriter(writer);
		}

		private void AddParent(BsonToken container)
		{
			this.AddToken(container);
			this._parent = container;
		}

		internal void AddToken(BsonToken token)
		{
			if (this._parent == null)
			{
				if (token.Type != BsonType.Object && token.Type != BsonType.Array)
				{
					throw JsonWriterException.Create(this, "Error writing {0} value. BSON must start with an Object or Array.".FormatWith(CultureInfo.InvariantCulture, token.Type), null);
				}
				this._parent = token;
				this._root = token;
				return;
			}
			BsonObject bsonObjects = this._parent as BsonObject;
			BsonObject bsonObjects1 = bsonObjects;
			if (bsonObjects == null)
			{
				((BsonArray)this._parent).Add(token);
				return;
			}
			bsonObjects1.Add(this._propertyName, token);
			this._propertyName = null;
		}

		private void AddValue(object value, BsonType type)
		{
			this.AddToken(new BsonValue(value, type));
		}

		public override void Close()
		{
			base.Close();
			if (base.CloseOutput)
			{
				BsonBinaryWriter bsonBinaryWriter = this._writer;
				if (bsonBinaryWriter == null)
				{
					return;
				}
				bsonBinaryWriter.Close();
			}
		}

		public override void Flush()
		{
			this._writer.Flush();
		}

		private void RemoveParent()
		{
			this._parent = this._parent.Parent;
		}

		public override void WriteComment(string text)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON comment as BSON.", null);
		}

		protected override void WriteEnd(JsonToken token)
		{
			base.WriteEnd(token);
			this.RemoveParent();
			if (base.Top == 0)
			{
				this._writer.WriteToken(this._root);
			}
		}

		public override void WriteNull()
		{
			base.WriteNull();
			this.AddToken(BsonEmpty.Null);
		}

		public void WriteObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if ((int)value.Length != 12)
			{
				throw JsonWriterException.Create(this, "An object id must be 12 bytes", null);
			}
			base.SetWriteState(JsonToken.Undefined, null);
			this.AddValue(value, BsonType.Oid);
		}

		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			this._propertyName = name;
		}

		public override void WriteRaw(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		public override void WriteRawValue(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		public void WriteRegex(string pattern, string options)
		{
			ValidationUtils.ArgumentNotNull(pattern, "pattern");
			base.SetWriteState(JsonToken.Undefined, null);
			this.AddToken(new BsonRegex(pattern, options));
		}

		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new BsonArray());
		}

		public override void WriteStartConstructor(string name)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON constructor as BSON.", null);
		}

		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new BsonObject());
		}

		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddToken(BsonEmpty.Undefined);
		}

		public override void WriteValue(object value)
		{
			object obj = value;
			object obj1 = obj;
			if (!(obj is BigInteger))
			{
				base.WriteValue(value);
				return;
			}
			BigInteger bigInteger = (BigInteger)obj1;
			base.SetWriteState(JsonToken.Integer, null);
			this.AddToken(new BsonBinary(bigInteger.ToByteArray(), BsonBinaryType.Binary));
		}

		public override void WriteValue(string value)
		{
			BsonToken @null;
			base.WriteValue(value);
			if (value == null)
			{
				@null = BsonEmpty.Null;
			}
			else
			{
				@null = new BsonString(value, true);
			}
			this.AddToken(@null);
		}

		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			if (value > 2147483647)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 32 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			if (value > 9223372036854775807L)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 64 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddToken((value ? BsonBoolean.True : BsonBoolean.False));
		}

		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			string str = null;
			str = value.ToString(CultureInfo.InvariantCulture);
			this.AddToken(new BsonString(str, true));
		}

		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			this.AddValue(value, BsonType.Date);
		}

		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Date);
		}

		public override void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.WriteValue(value);
			this.AddToken(new BsonBinary(value, BsonBinaryType.Binary));
		}

		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonBinary(value.ToByteArray(), BsonBinaryType.Uuid));
		}

		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		public override void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}
	}
}