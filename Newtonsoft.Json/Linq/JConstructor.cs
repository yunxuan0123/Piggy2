using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json.Linq
{
	public class JConstructor : JContainer
	{
		private string _name;

		private readonly List<JToken> _values;

		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				object obj = key;
				object obj1 = obj;
				if (!(obj is int))
				{
					throw new ArgumentException("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				return this.GetItem((int)obj1);
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				object obj = key;
				object obj1 = obj;
				if (!(obj is int))
				{
					throw new ArgumentException("Set JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				this.SetItem((int)obj1, value);
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		public override JTokenType Type
		{
			get
			{
				return JTokenType.Constructor;
			}
		}

		public JConstructor()
		{
			Class6.yDnXvgqzyB5jw();
			this._values = new List<JToken>();
			base();
		}

		public JConstructor(JConstructor other)
		{
			Class6.yDnXvgqzyB5jw();
			this._values = new List<JToken>();
			base(other);
			this._name = other.Name;
		}

		public JConstructor(string name, params object[] content)
		{
			Class6.yDnXvgqzyB5jw();
			this(name, (object)content);
		}

		public JConstructor(string name, object content)
		{
			Class6.yDnXvgqzyB5jw();
			this(name);
			this.Add(content);
		}

		public JConstructor(string name)
		{
			Class6.yDnXvgqzyB5jw();
			this._values = new List<JToken>();
			base();
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("Constructor name cannot be empty.", "name");
			}
			this._name = name;
		}

		internal override JToken CloneToken()
		{
			return new JConstructor(this);
		}

		internal override bool DeepEquals(JToken node)
		{
			JConstructor jConstructor = node as JConstructor;
			JConstructor jConstructor1 = jConstructor;
			if (jConstructor == null || !(this._name == jConstructor1.Name))
			{
				return false;
			}
			return base.ContentsEqual(jConstructor1);
		}

		internal override int GetDeepHashCode()
		{
			return this._name.GetHashCode() ^ base.ContentsHashCode();
		}

		internal override int IndexOfItem(JToken item)
		{
			return this._values.IndexOfReference<JToken>(item);
		}

		public static new JConstructor Load(JsonReader reader)
		{
			return JConstructor.Load(reader, null);
		}

		public static new JConstructor Load(JsonReader reader, JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.StartConstructor)
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JConstructor jConstructor = new JConstructor((string)reader.Value);
			jConstructor.SetLineInfo(reader as IJsonLineInfo, settings);
			jConstructor.ReadTokenFrom(reader, settings);
			return jConstructor;
		}

		public static new Task<JConstructor> LoadAsync(JsonReader reader, CancellationToken cancellationToken = null)
		{
			return JConstructor.LoadAsync(reader, null, cancellationToken);
		}

		public static new async Task<JConstructor> LoadAsync(JsonReader reader, JsonLoadSettings settings, CancellationToken cancellationToken = null)
		{
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable;
			if (reader.TokenType == JsonToken.None)
			{
				configuredTaskAwaitable = reader.ReadAsync(cancellationToken).ConfigureAwait(false);
				if (!await configuredTaskAwaitable)
				{
					throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
				}
			}
			configuredTaskAwaitable = reader.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			if (reader.TokenType != JsonToken.StartConstructor)
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JConstructor jConstructor = new JConstructor((string)reader.Value);
			jConstructor.SetLineInfo(reader as IJsonLineInfo, settings);
			ConfiguredTaskAwaitable configuredTaskAwaitable1 = jConstructor.ReadTokenFromAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable1;
			return jConstructor;
		}

		internal override void MergeItem(object content, JsonMergeSettings settings)
		{
			JConstructor jConstructor = content as JConstructor;
			JConstructor jConstructor1 = jConstructor;
			if (jConstructor == null)
			{
				return;
			}
			if (jConstructor1.Name != null)
			{
				this.Name = jConstructor1.Name;
			}
			JContainer.MergeEnumerableContent(this, jConstructor1, settings);
		}

		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartConstructor(this._name);
			int count = this._values.Count;
			for (int i = 0; i < count; i++)
			{
				this._values[i].WriteTo(writer, converters);
			}
			writer.WriteEndConstructor();
		}

		public override async Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = writer.WriteStartConstructorAsync(this._name, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			for (int i = 0; i < this._values.Count; i++)
			{
				configuredTaskAwaitable = this._values[i].WriteToAsync(writer, cancellationToken, converters).ConfigureAwait(false);
				await configuredTaskAwaitable;
			}
			configuredTaskAwaitable = writer.WriteEndConstructorAsync(cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}
	}
}