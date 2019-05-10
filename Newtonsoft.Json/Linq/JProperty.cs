using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json.Linq
{
	public class JProperty : JContainer
	{
		private readonly JProperty.JPropertyList _content;

		private readonly string _name;

		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._content;
			}
		}

		public string Name
		{
			[DebuggerStepThrough]
			get
			{
				return this._name;
			}
		}

		public override JTokenType Type
		{
			[DebuggerStepThrough]
			get
			{
				return JTokenType.Property;
			}
		}

		public JToken Value
		{
			[DebuggerStepThrough]
			get
			{
				return this._content._token;
			}
			set
			{
				base.CheckReentrancy();
				object obj = value;
				if (obj == null)
				{
					obj = JValue.CreateNull();
				}
				JToken jTokens = (JToken)obj;
				if (this._content._token != null)
				{
					this.SetItem(0, jTokens);
					return;
				}
				this.InsertItem(0, jTokens, false);
			}
		}

		public JProperty(JProperty other)
		{
			Class6.yDnXvgqzyB5jw();
			this._content = new JProperty.JPropertyList();
			base(other);
			this._name = other.Name;
		}

		internal JProperty(string name)
		{
			Class6.yDnXvgqzyB5jw();
			this._content = new JProperty.JPropertyList();
			base();
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
		}

		public JProperty(string name, params object[] content)
		{
			Class6.yDnXvgqzyB5jw();
			this(name, (object)content);
		}

		public JProperty(string name, object content)
		{
			Class6.yDnXvgqzyB5jw();
			this._content = new JProperty.JPropertyList();
			base();
			JToken jArrays;
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
			if (base.IsMultiContent(content))
			{
				jArrays = new JArray(content);
			}
			else
			{
				jArrays = JContainer.CreateFromContent(content);
			}
			this.Value = jArrays;
		}

		internal override void ClearItems()
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		internal override JToken CloneToken()
		{
			return new JProperty(this);
		}

		internal override bool ContainsItem(JToken item)
		{
			return this.Value == item;
		}

		internal override bool DeepEquals(JToken node)
		{
			JProperty jProperty = node as JProperty;
			JProperty jProperty1 = jProperty;
			if (jProperty == null || !(this._name == jProperty1.Name))
			{
				return false;
			}
			return base.ContentsEqual(jProperty1);
		}

		internal override int GetDeepHashCode()
		{
			int deepHashCode;
			int hashCode = this._name.GetHashCode();
			JToken value = this.Value;
			if (value != null)
			{
				deepHashCode = value.GetDeepHashCode();
			}
			else
			{
				deepHashCode = 0;
			}
			return hashCode ^ deepHashCode;
		}

		internal override JToken GetItem(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.Value;
		}

		internal override int IndexOfItem(JToken item)
		{
			return this._content.IndexOf(item);
		}

		internal override void InsertItem(int index, JToken item, bool skipParentCheck)
		{
			if (item != null && item.Type == JTokenType.Comment)
			{
				return;
			}
			if (this.Value != null)
			{
				throw new JsonException("{0} cannot have multiple values.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
			}
			base.InsertItem(0, item, false);
		}

		public static new JProperty Load(JsonReader reader)
		{
			return JProperty.Load(reader, null);
		}

		public static new JProperty Load(JsonReader reader, JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JProperty jProperty = new JProperty((string)reader.Value);
			jProperty.SetLineInfo(reader as IJsonLineInfo, settings);
			jProperty.ReadTokenFrom(reader, settings);
			return jProperty;
		}

		public static new Task<JProperty> LoadAsync(JsonReader reader, CancellationToken cancellationToken = null)
		{
			return JProperty.LoadAsync(reader, null, cancellationToken);
		}

		public static new async Task<JProperty> LoadAsync(JsonReader reader, JsonLoadSettings settings, CancellationToken cancellationToken = null)
		{
			ConfiguredTaskAwaitable<bool> configuredTaskAwaitable;
			if (reader.TokenType == JsonToken.None)
			{
				configuredTaskAwaitable = reader.ReadAsync(cancellationToken).ConfigureAwait(false);
				if (!await configuredTaskAwaitable)
				{
					throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
				}
			}
			configuredTaskAwaitable = reader.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable;
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JProperty jProperty = new JProperty((string)reader.Value);
			jProperty.SetLineInfo(reader as IJsonLineInfo, settings);
			ConfiguredTaskAwaitable configuredTaskAwaitable1 = jProperty.ReadTokenFromAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			await configuredTaskAwaitable1;
			return jProperty;
		}

		internal override void MergeItem(object content, JsonMergeSettings settings)
		{
			JToken value;
			JProperty jProperty = content as JProperty;
			if (jProperty != null)
			{
				value = jProperty.Value;
			}
			else
			{
				value = null;
			}
			JToken jTokens = value;
			if (jTokens != null && jTokens.Type != JTokenType.Null)
			{
				this.Value = jTokens;
			}
		}

		internal override bool RemoveItem(JToken item)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		internal override void RemoveItemAt(int index)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		internal override void SetItem(int index, JToken item)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (JContainer.IsTokenUnchanged(this.Value, item))
			{
				return;
			}
			JObject parent = (JObject)base.Parent;
			if (parent != null)
			{
				parent.InternalPropertyChanging(this);
			}
			else
			{
			}
			base.SetItem(0, item);
			JObject jObjects = (JObject)base.Parent;
			if (jObjects == null)
			{
				return;
			}
			jObjects.InternalPropertyChanged(this);
		}

		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WritePropertyName(this._name);
			JToken value = this.Value;
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			value.WriteTo(writer, converters);
		}

		public override Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			Task task = writer.WritePropertyNameAsync(this._name, cancellationToken);
			if (task.IsCompletedSucessfully())
			{
				return this.WriteValueAsync(writer, cancellationToken, converters);
			}
			return this.WriteToAsync(task, writer, cancellationToken, converters);
		}

		private async Task WriteToAsync(Task task, JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			ConfiguredTaskAwaitable configuredTaskAwaitable = task.ConfigureAwait(false);
			await configuredTaskAwaitable;
			configuredTaskAwaitable = this.WriteValueAsync(writer, cancellationToken, converters).ConfigureAwait(false);
			await configuredTaskAwaitable;
		}

		private Task WriteValueAsync(JsonWriter writer, CancellationToken cancellationToken, JsonConverter[] converters)
		{
			JToken value = this.Value;
			if (value == null)
			{
				return writer.WriteNullAsync(cancellationToken);
			}
			return value.WriteToAsync(writer, cancellationToken, converters);
		}

		private class JPropertyList : IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
		{
			internal JToken _token;

			public int Count
			{
				get
				{
					if (this._token == null)
					{
						return 0;
					}
					return 1;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			public JToken this[int index]
			{
				get
				{
					if (index != 0)
					{
						return null;
					}
					return this._token;
				}
				set
				{
					if (index == 0)
					{
						this._token = value;
					}
				}
			}

			public JPropertyList()
			{
				Class6.yDnXvgqzyB5jw();
				base();
			}

			public void Add(JToken item)
			{
				this._token = item;
			}

			public void Clear()
			{
				this._token = null;
			}

			public bool Contains(JToken item)
			{
				return this._token == item;
			}

			public void CopyTo(JToken[] array, int arrayIndex)
			{
				if (this._token != null)
				{
					array[arrayIndex] = this._token;
				}
			}

			public IEnumerator<JToken> GetEnumerator()
			{
				JProperty.JPropertyList jPropertyLists = null;
				if (jPropertyLists._token != null)
				{
					yield return jPropertyLists._token;
				}
			}

			public int IndexOf(JToken item)
			{
				if (this._token != item)
				{
					return -1;
				}
				return 0;
			}

			public void Insert(int index, JToken item)
			{
				if (index == 0)
				{
					this._token = item;
				}
			}

			public bool Remove(JToken item)
			{
				if (this._token != item)
				{
					return false;
				}
				this._token = null;
				return true;
			}

			public void RemoveAt(int index)
			{
				if (index == 0)
				{
					this._token = null;
				}
			}

			IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}
	}
}