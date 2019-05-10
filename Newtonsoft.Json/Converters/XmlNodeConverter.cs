using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	public class XmlNodeConverter : JsonConverter
	{
		internal readonly static List<IXmlNode> EmptyChildNodes;

		public string DeserializeRootElementName
		{
			get;
			set;
		}

		public bool EncodeSpecialCharacters
		{
			get;
			set;
		}

		public bool OmitRootObject
		{
			get;
			set;
		}

		public bool WriteArrayAttribute
		{
			get;
			set;
		}

		static XmlNodeConverter()
		{
			Class6.yDnXvgqzyB5jw();
			XmlNodeConverter.EmptyChildNodes = new List<IXmlNode>();
		}

		public XmlNodeConverter()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		private static void AddAttribute(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName, string attributeName, XmlNamespaceManager manager, string attributePrefix)
		{
			IXmlNode xmlNode;
			if (currentNode.NodeType == XmlNodeType.Document)
			{
				throw JsonSerializationException.Create(reader, "JSON root object has property '{0}' that will be converted to an attribute. A root object cannot have any attribute properties. Consider specifying a DeserializeRootElementName.".FormatWith(CultureInfo.InvariantCulture, propertyName));
			}
			string str = XmlConvert.EncodeName(attributeName);
			string xmlValue = XmlNodeConverter.ConvertTokenToXmlValue(reader);
			xmlNode = (!string.IsNullOrEmpty(attributePrefix) ? document.CreateAttribute(str, manager.LookupNamespace(attributePrefix), xmlValue) : document.CreateAttribute(str, xmlValue));
			((IXmlElement)currentNode).SetAttributeNode(xmlNode);
		}

		private void AddJsonArrayAttribute(IXmlElement element, IXmlDocument document)
		{
			element.SetAttributeNode(document.CreateAttribute("json:Array", "http://james.newtonking.com/projects/json", "true"));
			if (element is XElementWrapper && element.GetPrefixOfNamespace("http://james.newtonking.com/projects/json") == null)
			{
				element.SetAttributeNode(document.CreateAttribute("xmlns:json", "http://www.w3.org/2000/xmlns/", "http://james.newtonking.com/projects/json"));
			}
		}

		private static bool AllSameName(IXmlNode node)
		{
			bool flag;
			List<IXmlNode>.Enumerator enumerator = node.ChildNodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.LocalName == node.LocalName)
					{
						continue;
					}
					flag = false;
					return flag;
				}
				return true;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return flag;
		}

		public override bool CanConvert(Type valueType)
		{
			if (valueType.AssignableToTypeName("System.Xml.Linq.XObject", false))
			{
				return this.method_0(valueType);
			}
			if (!valueType.AssignableToTypeName("System.Xml.XmlNode", false))
			{
				return false;
			}
			return this.IsXmlNode(valueType);
		}

		private static string ConvertTokenToXmlValue(JsonReader reader)
		{
			object obj;
			switch (reader.TokenType)
			{
				case JsonToken.Integer:
				{
					object value = reader.Value;
					obj = value;
					if (!(value is BigInteger))
					{
						return XmlConvert.ToString(Convert.ToInt64(reader.Value, CultureInfo.InvariantCulture));
					}
					return ((BigInteger)obj).ToString(CultureInfo.InvariantCulture);
				}
				case JsonToken.Float:
				{
					object value1 = reader.Value;
					obj = value1;
					if (value1 is decimal)
					{
						return XmlConvert.ToString((decimal)obj);
					}
					object obj1 = reader.Value;
					obj = obj1;
					if (obj1 is float)
					{
						return XmlConvert.ToString((float)obj);
					}
					return XmlConvert.ToString(Convert.ToDouble(reader.Value, CultureInfo.InvariantCulture));
				}
				case JsonToken.String:
				{
					object value2 = reader.Value;
					if (value2 != null)
					{
						return value2.ToString();
					}
					return null;
				}
				case JsonToken.Boolean:
				{
					return XmlConvert.ToString(Convert.ToBoolean(reader.Value, CultureInfo.InvariantCulture));
				}
				case JsonToken.Null:
				{
					return null;
				}
				case JsonToken.Undefined:
				case JsonToken.EndObject:
				case JsonToken.EndArray:
				case JsonToken.EndConstructor:
				{
					throw JsonSerializationException.Create(reader, "Cannot get an XML string value from token type '{0}'.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
				}
				case JsonToken.Date:
				{
					object obj2 = reader.Value;
					obj = obj2;
					if (obj2 is DateTimeOffset)
					{
						return XmlConvert.ToString((DateTimeOffset)obj);
					}
					DateTime dateTime = Convert.ToDateTime(reader.Value, CultureInfo.InvariantCulture);
					return XmlConvert.ToString(dateTime, DateTimeUtils.ToSerializationMode(dateTime.Kind));
				}
				case JsonToken.Bytes:
				{
					return Convert.ToBase64String((byte[])reader.Value);
				}
				default:
				{
					throw JsonSerializationException.Create(reader, "Cannot get an XML string value from token type '{0}'.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
				}
			}
		}

		private void CreateDocumentType(JsonReader reader, IXmlDocument document, IXmlNode currentNode)
		{
			string xmlValue = null;
			string str = null;
			string xmlValue1 = null;
			string str1 = null;
			while (reader.Read())
			{
				if (reader.TokenType != JsonToken.EndObject)
				{
					string str2 = reader.Value.ToString();
					if (str2 == "@name")
					{
						reader.ReadAndAssert();
						xmlValue = XmlNodeConverter.ConvertTokenToXmlValue(reader);
					}
					else if (str2 == "@public")
					{
						reader.ReadAndAssert();
						str = XmlNodeConverter.ConvertTokenToXmlValue(reader);
					}
					else if (str2 == "@system")
					{
						reader.ReadAndAssert();
						xmlValue1 = XmlNodeConverter.ConvertTokenToXmlValue(reader);
					}
					else
					{
						if (str2 != "@internalSubset")
						{
							throw JsonSerializationException.Create(reader, string.Concat("Unexpected property name encountered while deserializing XmlDeclaration: ", reader.Value));
						}
						reader.ReadAndAssert();
						str1 = XmlNodeConverter.ConvertTokenToXmlValue(reader);
					}
				}
				else
				{
					break;
				}
			}
			IXmlNode xmlNode = document.CreateXmlDocumentType(xmlValue, str, xmlValue1, str1);
			currentNode.AppendChild(xmlNode);
		}

		private void CreateElement(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string elementName, XmlNamespaceManager manager, string elementPrefix, Dictionary<string, string> attributeNameValues)
		{
			bool flag;
			IXmlElement xmlElement = this.CreateElement(elementName, document, elementPrefix, manager);
			currentNode.AppendChild(xmlElement);
			if (attributeNameValues != null)
			{
				foreach (KeyValuePair<string, string> attributeNameValue in attributeNameValues)
				{
					string str = XmlConvert.EncodeName(attributeNameValue.Key);
					string prefix = MiscellaneousUtils.GetPrefix(attributeNameValue.Key);
					xmlElement.SetAttributeNode((!string.IsNullOrEmpty(prefix) ? document.CreateAttribute(str, manager.LookupNamespace(prefix) ?? string.Empty, attributeNameValue.Value) : document.CreateAttribute(str, attributeNameValue.Value)));
				}
			}
			switch (reader.TokenType)
			{
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Date:
				case JsonToken.Bytes:
				{
					string xmlValue = XmlNodeConverter.ConvertTokenToXmlValue(reader);
					if (xmlValue == null)
					{
						return;
					}
					xmlElement.AppendChild(document.CreateTextNode(xmlValue));
					return;
				}
				case JsonToken.Null:
				{
					return;
				}
				case JsonToken.Undefined:
				case JsonToken.EndArray:
				case JsonToken.EndConstructor:
				{
					manager.PushScope();
					this.DeserializeNode(reader, document, manager, xmlElement);
					flag = manager.PopScope();
					manager.RemoveNamespace(string.Empty, manager.DefaultNamespace);
					return;
				}
				case JsonToken.EndObject:
				{
					manager.RemoveNamespace(string.Empty, manager.DefaultNamespace);
					return;
				}
				default:
				{
					manager.PushScope();
					this.DeserializeNode(reader, document, manager, xmlElement);
					flag = manager.PopScope();
					manager.RemoveNamespace(string.Empty, manager.DefaultNamespace);
					return;
				}
			}
		}

		private IXmlElement CreateElement(string elementName, IXmlDocument document, string elementPrefix, XmlNamespaceManager manager)
		{
			string str = (this.EncodeSpecialCharacters ? XmlConvert.EncodeLocalName(elementName) : XmlConvert.EncodeName(elementName));
			string str1 = (string.IsNullOrEmpty(elementPrefix) ? manager.DefaultNamespace : manager.LookupNamespace(elementPrefix));
			if (string.IsNullOrEmpty(str1))
			{
				return document.CreateElement(str);
			}
			return document.CreateElement(str, str1);
		}

		private void CreateInstruction(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName)
		{
			if (propertyName != "?xml")
			{
				IXmlNode xmlNode = document.CreateProcessingInstruction(propertyName.Substring(1), XmlNodeConverter.ConvertTokenToXmlValue(reader));
				currentNode.AppendChild(xmlNode);
				return;
			}
			string xmlValue = null;
			string str = null;
			string xmlValue1 = null;
			while (reader.Read())
			{
				if (reader.TokenType != JsonToken.EndObject)
				{
					string str1 = reader.Value.ToString();
					if (str1 == "@version")
					{
						reader.ReadAndAssert();
						xmlValue = XmlNodeConverter.ConvertTokenToXmlValue(reader);
					}
					else if (str1 == "@encoding")
					{
						reader.ReadAndAssert();
						str = XmlNodeConverter.ConvertTokenToXmlValue(reader);
					}
					else
					{
						if (str1 != "@standalone")
						{
							throw JsonSerializationException.Create(reader, string.Concat("Unexpected property name encountered while deserializing XmlDeclaration: ", reader.Value));
						}
						reader.ReadAndAssert();
						xmlValue1 = XmlNodeConverter.ConvertTokenToXmlValue(reader);
					}
				}
				else
				{
					break;
				}
			}
			IXmlNode xmlNode1 = document.CreateXmlDeclaration(xmlValue, str, xmlValue1);
			currentNode.AppendChild(xmlNode1);
		}

		private void DeserializeNode(JsonReader reader, IXmlDocument document, XmlNamespaceManager manager, IXmlNode currentNode)
		{
			string str;
			string str1;
			do
			{
				JsonToken tokenType = reader.TokenType;
				switch (tokenType)
				{
					case JsonToken.StartConstructor:
					{
						string str2 = reader.Value.ToString();
						while (reader.Read())
						{
							if (reader.TokenType != JsonToken.EndConstructor)
							{
								this.DeserializeValue(reader, document, manager, str2, currentNode);
							}
							else
							{
								goto Label0;
							}
						}
						goto Label0;
					}
					case JsonToken.PropertyName:
					{
						if (currentNode.NodeType == XmlNodeType.Document && document.DocumentElement != null)
						{
							throw JsonSerializationException.Create(reader, "JSON root object has multiple properties. The root object must have a single property in order to create a valid XML document. Consider specifying a DeserializeRootElementName.");
						}
						string str3 = reader.Value.ToString();
						reader.ReadAndAssert();
						if (reader.TokenType != JsonToken.StartArray)
						{
							this.DeserializeValue(reader, document, manager, str3, currentNode);
							continue;
						}
						else
						{
							int num = 0;
							while (reader.Read())
							{
								if (reader.TokenType != JsonToken.EndArray)
								{
									this.DeserializeValue(reader, document, manager, str3, currentNode);
									num++;
								}
								else
								{
									break;
								}
							}
							if (num != 1 || !this.WriteArrayAttribute)
							{
								continue;
							}
							MiscellaneousUtils.GetQualifiedNameParts(str3, out str, out str1);
							string str4 = (string.IsNullOrEmpty(str) ? manager.DefaultNamespace : manager.LookupNamespace(str));
							List<IXmlNode>.Enumerator enumerator = currentNode.ChildNodes.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									IXmlElement current = enumerator.Current as IXmlElement;
									IXmlElement xmlElement = current;
									if (current == null || !(xmlElement.LocalName == str1) || !(xmlElement.NamespaceUri == str4))
									{
										continue;
									}
									this.AddJsonArrayAttribute(xmlElement, document);
									goto Label0;
								}
								continue;
							}
							finally
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						break;
					}
					case JsonToken.Comment:
					{
						currentNode.AppendChild(document.CreateComment((string)reader.Value));
						continue;
					}
				}
				if ((int)tokenType - (int)JsonToken.EndObject > (int)JsonToken.StartObject)
				{
					throw JsonSerializationException.Create(reader, string.Concat("Unexpected JsonToken when deserializing node: ", reader.TokenType));
				}
				return;
			Label0:
			}
			while (reader.Read());
		}

		private void DeserializeValue(JsonReader reader, IXmlDocument document, XmlNamespaceManager manager, string propertyName, IXmlNode currentNode)
		{
			if (!this.EncodeSpecialCharacters)
			{
				if (propertyName == "#text")
				{
					currentNode.AppendChild(document.CreateTextNode(XmlNodeConverter.ConvertTokenToXmlValue(reader)));
					return;
				}
				if (propertyName == "#cdata-section")
				{
					currentNode.AppendChild(document.CreateCDataSection(XmlNodeConverter.ConvertTokenToXmlValue(reader)));
					return;
				}
				if (propertyName == "#whitespace")
				{
					currentNode.AppendChild(document.CreateWhitespace(XmlNodeConverter.ConvertTokenToXmlValue(reader)));
					return;
				}
				if (propertyName == "#significant-whitespace")
				{
					currentNode.AppendChild(document.CreateSignificantWhitespace(XmlNodeConverter.ConvertTokenToXmlValue(reader)));
					return;
				}
				if (!string.IsNullOrEmpty(propertyName) && propertyName[0] == '?')
				{
					this.CreateInstruction(reader, document, currentNode, propertyName);
					return;
				}
				if (string.Equals(propertyName, "!DOCTYPE", StringComparison.OrdinalIgnoreCase))
				{
					this.CreateDocumentType(reader, document, currentNode);
					return;
				}
			}
			if (reader.TokenType == JsonToken.StartArray)
			{
				this.ReadArrayElements(reader, document, propertyName, currentNode, manager);
				return;
			}
			this.ReadElement(reader, document, currentNode, propertyName, manager);
		}

		private string GetPropertyName(IXmlNode node, XmlNamespaceManager manager)
		{
			switch (node.NodeType)
			{
				case XmlNodeType.Element:
				{
					if (node.NamespaceUri == "http://james.newtonking.com/projects/json")
					{
						return string.Concat("$", node.LocalName);
					}
					return this.ResolveFullName(node, manager);
				}
				case XmlNodeType.Attribute:
				{
					if (node.NamespaceUri == "http://james.newtonking.com/projects/json")
					{
						return string.Concat("$", node.LocalName);
					}
					return string.Concat("@", this.ResolveFullName(node, manager));
				}
				case XmlNodeType.Text:
				{
					return "#text";
				}
				case XmlNodeType.CDATA:
				{
					return "#cdata-section";
				}
				case XmlNodeType.EntityReference:
				case XmlNodeType.Entity:
				case XmlNodeType.Document:
				case XmlNodeType.DocumentFragment:
				case XmlNodeType.Notation:
				case XmlNodeType.EndElement:
				case XmlNodeType.EndEntity:
				{
					throw new JsonSerializationException(string.Concat("Unexpected XmlNodeType when getting node name: ", node.NodeType));
				}
				case XmlNodeType.ProcessingInstruction:
				{
					return string.Concat("?", this.ResolveFullName(node, manager));
				}
				case XmlNodeType.Comment:
				{
					return "#comment";
				}
				case XmlNodeType.DocumentType:
				{
					return string.Concat("!", this.ResolveFullName(node, manager));
				}
				case XmlNodeType.Whitespace:
				{
					return "#whitespace";
				}
				case XmlNodeType.SignificantWhitespace:
				{
					return "#significant-whitespace";
				}
				case XmlNodeType.XmlDeclaration:
				{
					return "?xml";
				}
				default:
				{
					throw new JsonSerializationException(string.Concat("Unexpected XmlNodeType when getting node name: ", node.NodeType));
				}
			}
		}

		private bool IsArray(IXmlNode node)
		{
			bool flag;
			List<IXmlNode>.Enumerator enumerator = node.Attributes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IXmlNode current = enumerator.Current;
					if (!(current.LocalName == "Array") || !(current.NamespaceUri == "http://james.newtonking.com/projects/json"))
					{
						continue;
					}
					flag = XmlConvert.ToBoolean(current.Value);
					return flag;
				}
				return false;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return flag;
		}

		private bool IsNamespaceAttribute(string attributeName, out string prefix)
		{
			if (attributeName.StartsWith("xmlns", StringComparison.Ordinal))
			{
				if (attributeName.Length == 5)
				{
					prefix = string.Empty;
					return true;
				}
				if (attributeName[5] == ':')
				{
					prefix = attributeName.Substring(6, attributeName.Length - 6);
					return true;
				}
			}
			prefix = null;
			return false;
		}

		private bool IsXmlNode(Type valueType)
		{
			return typeof(XmlNode).IsAssignableFrom(valueType);
		}

		private bool method_0(Type valueType)
		{
			return typeof(XObject).IsAssignableFrom(valueType);
		}

		private void PushParentNamespaces(IXmlNode node, XmlNamespaceManager manager)
		{
			List<IXmlNode> xmlNodes = null;
			IXmlNode xmlNode = node;
			while (true)
			{
				IXmlNode parentNode = xmlNode.ParentNode;
				xmlNode = parentNode;
				if (parentNode == null)
				{
					break;
				}
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					if (xmlNodes == null)
					{
						xmlNodes = new List<IXmlNode>();
					}
					xmlNodes.Add(xmlNode);
				}
			}
			if (xmlNodes != null)
			{
				xmlNodes.Reverse();
				foreach (IXmlNode xmlNode1 in xmlNodes)
				{
					manager.PushScope();
					foreach (IXmlNode attribute in xmlNode1.Attributes)
					{
						if (!(attribute.NamespaceUri == "http://www.w3.org/2000/xmlns/") || !(attribute.LocalName != "xmlns"))
						{
							continue;
						}
						manager.AddNamespace(attribute.LocalName, attribute.Value);
					}
				}
			}
		}

		private void ReadArrayElements(JsonReader reader, IXmlDocument document, string propertyName, IXmlNode currentNode, XmlNamespaceManager manager)
		{
			string prefix = MiscellaneousUtils.GetPrefix(propertyName);
			IXmlElement xmlElement = this.CreateElement(propertyName, document, prefix, manager);
			currentNode.AppendChild(xmlElement);
			int num = 0;
			while (reader.Read())
			{
				if (reader.TokenType != JsonToken.EndArray)
				{
					this.DeserializeValue(reader, document, manager, propertyName, xmlElement);
					num++;
				}
				else
				{
					break;
				}
			}
			if (this.WriteArrayAttribute)
			{
				this.AddJsonArrayAttribute(xmlElement, document);
			}
			if (num == 1 && this.WriteArrayAttribute)
			{
				foreach (IXmlNode childNode in xmlElement.ChildNodes)
				{
					IXmlElement xmlElement1 = childNode as IXmlElement;
					IXmlElement xmlElement2 = xmlElement1;
					if (xmlElement1 == null || !(xmlElement2.LocalName == propertyName))
					{
						continue;
					}
					this.AddJsonArrayAttribute(xmlElement2, document);
					return;
				}
			}
		}

		private Dictionary<string, string> ReadAttributeElements(JsonReader reader, XmlNamespaceManager manager)
		{
			string xmlValue;
			string str;
			string str1;
			Dictionary<string, string> strs = null;
			bool flag = false;
			while (!flag)
			{
				if (!reader.Read())
				{
					break;
				}
				JsonToken tokenType = reader.TokenType;
				if (tokenType == JsonToken.PropertyName)
				{
					string str2 = reader.Value.ToString();
					if (string.IsNullOrEmpty(str2))
					{
						flag = true;
					}
					else
					{
						char chr = str2[0];
						if (chr != '$')
						{
							if (chr != '@')
							{
								flag = true;
							}
							else
							{
								if (strs == null)
								{
									strs = new Dictionary<string, string>();
								}
								str2 = str2.Substring(1);
								reader.ReadAndAssert();
								xmlValue = XmlNodeConverter.ConvertTokenToXmlValue(reader);
								strs.Add(str2, xmlValue);
								if (!this.IsNamespaceAttribute(str2, out str))
								{
									continue;
								}
								manager.AddNamespace(str, xmlValue);
							}
						}
						else if (str2 == "$values" || str2 == "$id" || str2 == "$ref" || str2 == "$type" || str2 == "$value")
						{
							string str3 = manager.LookupPrefix("http://james.newtonking.com/projects/json");
							if (str3 == null)
							{
								if (strs == null)
								{
									strs = new Dictionary<string, string>();
								}
								int? nullable = null;
								while (manager.LookupNamespace(string.Concat("json", nullable)) != null)
								{
									nullable = new int?(nullable.GetValueOrDefault() + 1);
								}
								str3 = string.Concat("json", nullable);
								strs.Add(string.Concat("xmlns:", str3), "http://james.newtonking.com/projects/json");
								manager.AddNamespace(str3, "http://james.newtonking.com/projects/json");
							}
							if (str2 != "$values")
							{
								str2 = str2.Substring(1);
								reader.ReadAndAssert();
								if (!JsonTokenUtils.IsPrimitiveToken(reader.TokenType))
								{
									throw JsonSerializationException.Create(reader, string.Concat("Unexpected JsonToken: ", reader.TokenType));
								}
								if (strs == null)
								{
									strs = new Dictionary<string, string>();
								}
								object value = reader.Value;
								if (value != null)
								{
									str1 = value.ToString();
								}
								else
								{
									str1 = null;
								}
								xmlValue = str1;
								strs.Add(string.Concat(str3, ":", str2), xmlValue);
							}
							else
							{
								flag = true;
							}
						}
						else
						{
							flag = true;
						}
					}
				}
				else
				{
					if (tokenType != JsonToken.Comment && tokenType != JsonToken.EndObject)
					{
						throw JsonSerializationException.Create(reader, string.Concat("Unexpected JsonToken: ", reader.TokenType));
					}
					flag = true;
				}
			}
			return strs;
		}

		private void ReadElement(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName, XmlNamespaceManager manager)
		{
			Dictionary<string, string> strs;
			if (string.IsNullOrEmpty(propertyName))
			{
				throw JsonSerializationException.Create(reader, "XmlNodeConverter cannot convert JSON with an empty property name to XML.");
			}
			Dictionary<string, string> strs1 = null;
			string prefix = null;
			if (!this.EncodeSpecialCharacters)
			{
				if (this.ShouldReadInto(reader))
				{
					strs = this.ReadAttributeElements(reader, manager);
				}
				else
				{
					strs = null;
				}
				strs1 = strs;
				prefix = MiscellaneousUtils.GetPrefix(propertyName);
				if (propertyName.StartsWith('@'))
				{
					string str = propertyName.Substring(1);
					string prefix1 = MiscellaneousUtils.GetPrefix(str);
					XmlNodeConverter.AddAttribute(reader, document, currentNode, propertyName, str, manager, prefix1);
					return;
				}
				if (propertyName.StartsWith('$'))
				{
					if (propertyName == "$values")
					{
						propertyName = propertyName.Substring(1);
						prefix = manager.LookupPrefix("http://james.newtonking.com/projects/json");
						this.CreateElement(reader, document, currentNode, propertyName, manager, prefix, strs1);
						return;
					}
					if (propertyName == "$id" || propertyName == "$ref" || propertyName == "$type" || propertyName == "$value")
					{
						string str1 = propertyName.Substring(1);
						string str2 = manager.LookupPrefix("http://james.newtonking.com/projects/json");
						XmlNodeConverter.AddAttribute(reader, document, currentNode, propertyName, str1, manager, str2);
						return;
					}
				}
			}
			else if (this.ShouldReadInto(reader))
			{
				reader.ReadAndAssert();
			}
			this.CreateElement(reader, document, currentNode, propertyName, manager, prefix, strs1);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JsonToken tokenType = reader.TokenType;
			if (tokenType != JsonToken.StartObject)
			{
				if (tokenType != JsonToken.Null)
				{
					throw JsonSerializationException.Create(reader, "XmlNodeConverter can only convert JSON that begins with an object.");
				}
				return null;
			}
			XmlNamespaceManager xmlNamespaceManagers = new XmlNamespaceManager(new NameTable());
			IXmlDocument xDocumentWrapper = null;
			IXmlNode xmlNode = null;
			if (typeof(XObject).IsAssignableFrom(objectType))
			{
				if (objectType != typeof(XContainer) && objectType != typeof(XDocument) && objectType != typeof(XElement) && objectType != typeof(XNode) && objectType != typeof(XObject))
				{
					throw JsonSerializationException.Create(reader, "XmlNodeConverter only supports deserializing XDocument, XElement, XContainer, XNode or XObject.");
				}
				xDocumentWrapper = new XDocumentWrapper(new XDocument());
				xmlNode = xDocumentWrapper;
			}
			if (typeof(XmlNode).IsAssignableFrom(objectType))
			{
				if (objectType != typeof(XmlDocument) && objectType != typeof(XmlElement) && objectType != typeof(XmlNode))
				{
					throw JsonSerializationException.Create(reader, "XmlNodeConverter only supports deserializing XmlDocument, XmlElement or XmlNode.");
				}
				xDocumentWrapper = new XmlDocumentWrapper(new XmlDocument()
				{
					XmlResolver = null
				});
				xmlNode = xDocumentWrapper;
			}
			if (xDocumentWrapper == null || xmlNode == null)
			{
				throw JsonSerializationException.Create(reader, string.Concat("Unexpected type when converting XML: ", objectType));
			}
			if (string.IsNullOrEmpty(this.DeserializeRootElementName))
			{
				reader.ReadAndAssert();
				this.DeserializeNode(reader, xDocumentWrapper, xmlNamespaceManagers, xmlNode);
			}
			else
			{
				this.ReadElement(reader, xDocumentWrapper, xmlNode, this.DeserializeRootElementName, xmlNamespaceManagers);
			}
			if (objectType == typeof(XElement))
			{
				XElement wrappedNode = (XElement)xDocumentWrapper.DocumentElement.WrappedNode;
				wrappedNode.Remove();
				return wrappedNode;
			}
			if (objectType != typeof(XmlElement))
			{
				return xDocumentWrapper.WrappedNode;
			}
			return xDocumentWrapper.DocumentElement.WrappedNode;
		}

		private string ResolveFullName(IXmlNode node, XmlNamespaceManager manager)
		{
			string str;
			if (node.NamespaceUri == null || node.LocalName == "xmlns" && node.NamespaceUri == "http://www.w3.org/2000/xmlns/")
			{
				str = null;
			}
			else
			{
				str = manager.LookupPrefix(node.NamespaceUri);
			}
			string str1 = str;
			if (string.IsNullOrEmpty(str1))
			{
				return XmlConvert.DecodeName(node.LocalName);
			}
			return string.Concat(str1, ":", XmlConvert.DecodeName(node.LocalName));
		}

		private void SerializeGroupedNodes(JsonWriter writer, IXmlNode node, XmlNamespaceManager manager, bool writePropertyName)
		{
			object obj;
			int count = node.ChildNodes.Count;
			if (count != 0)
			{
				if (count == 1)
				{
					string propertyName = this.GetPropertyName(node.ChildNodes[0], manager);
					this.WriteGroupedNodes(writer, manager, writePropertyName, node.ChildNodes, propertyName);
					return;
				}
				Dictionary<string, object> strs = null;
				string str = null;
				for (int i = 0; i < node.ChildNodes.Count; i++)
				{
					IXmlNode item = node.ChildNodes[i];
					string propertyName1 = this.GetPropertyName(item, manager);
					if (strs != null)
					{
						if (strs.TryGetValue(propertyName1, out obj))
						{
							List<IXmlNode> xmlNodes = obj as List<IXmlNode>;
							List<IXmlNode> xmlNodes1 = xmlNodes;
							if (xmlNodes == null)
							{
								xmlNodes1 = new List<IXmlNode>()
								{
									(IXmlNode)obj
								};
								strs[propertyName1] = xmlNodes1;
							}
							xmlNodes1.Add(item);
						}
						else
						{
							strs.Add(propertyName1, item);
						}
					}
					else if (str == null)
					{
						str = propertyName1;
					}
					else if (propertyName1 != str)
					{
						strs = new Dictionary<string, object>();
						if (i <= 1)
						{
							strs.Add(str, node.ChildNodes[0]);
						}
						else
						{
							List<IXmlNode> xmlNodes2 = new List<IXmlNode>(i);
							for (int j = 0; j < i; j++)
							{
								xmlNodes2.Add(node.ChildNodes[j]);
							}
							strs.Add(str, xmlNodes2);
						}
						strs.Add(propertyName1, item);
					}
				}
				if (strs == null)
				{
					this.WriteGroupedNodes(writer, manager, writePropertyName, node.ChildNodes, str);
					return;
				}
				foreach (KeyValuePair<string, object> keyValuePair in strs)
				{
					List<IXmlNode> value = keyValuePair.Value as List<IXmlNode>;
					List<IXmlNode> xmlNodes3 = value;
					if (value == null)
					{
						this.WriteGroupedNodes(writer, manager, writePropertyName, (IXmlNode)keyValuePair.Value, keyValuePair.Key);
					}
					else
					{
						this.WriteGroupedNodes(writer, manager, writePropertyName, xmlNodes3, keyValuePair.Key);
					}
				}
			}
		}

		private void SerializeNode(JsonWriter writer, IXmlNode node, XmlNamespaceManager manager, bool writePropertyName)
		{
			switch (node.NodeType)
			{
				case XmlNodeType.Element:
				{
					if (this.IsArray(node) && XmlNodeConverter.AllSameName(node) && node.ChildNodes.Count > 0)
					{
						this.SerializeGroupedNodes(writer, node, manager, false);
						return;
					}
					manager.PushScope();
					foreach (IXmlNode attribute in node.Attributes)
					{
						if (attribute.NamespaceUri != "http://www.w3.org/2000/xmlns/")
						{
							continue;
						}
						manager.AddNamespace((attribute.LocalName != "xmlns" ? XmlConvert.DecodeName(attribute.LocalName) : string.Empty), attribute.Value);
					}
					if (writePropertyName)
					{
						writer.WritePropertyName(this.GetPropertyName(node, manager));
					}
					if (!this.ValueAttributes(node.Attributes) && node.ChildNodes.Count == 1 && node.ChildNodes[0].NodeType == XmlNodeType.Text)
					{
						writer.WriteValue(node.ChildNodes[0].Value);
					}
					else if (node.ChildNodes.Count != 0 || node.Attributes.Count != 0)
					{
						writer.WriteStartObject();
						for (int i = 0; i < node.Attributes.Count; i++)
						{
							this.SerializeNode(writer, node.Attributes[i], manager, true);
						}
						this.SerializeGroupedNodes(writer, node, manager, true);
						writer.WriteEndObject();
					}
					else if (!((IXmlElement)node).IsEmpty)
					{
						writer.WriteValue(string.Empty);
					}
					else
					{
						writer.WriteNull();
					}
					manager.PopScope();
					return;
				}
				case XmlNodeType.Attribute:
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
				{
					if (node.NamespaceUri == "http://www.w3.org/2000/xmlns/" && node.Value == "http://james.newtonking.com/projects/json")
					{
						return;
					}
					if (node.NamespaceUri == "http://james.newtonking.com/projects/json" && node.LocalName == "Array")
					{
						return;
					}
					if (writePropertyName)
					{
						writer.WritePropertyName(this.GetPropertyName(node, manager));
					}
					writer.WriteValue(node.Value);
					return;
				}
				case XmlNodeType.EntityReference:
				case XmlNodeType.Entity:
				case XmlNodeType.Notation:
				case XmlNodeType.EndElement:
				case XmlNodeType.EndEntity:
				{
					throw new JsonSerializationException(string.Concat("Unexpected XmlNodeType when serializing nodes: ", node.NodeType));
				}
				case XmlNodeType.Comment:
				{
					if (!writePropertyName)
					{
						return;
					}
					writer.WriteComment(node.Value);
					return;
				}
				case XmlNodeType.Document:
				case XmlNodeType.DocumentFragment:
				{
					this.SerializeGroupedNodes(writer, node, manager, writePropertyName);
					return;
				}
				case XmlNodeType.DocumentType:
				{
					IXmlDocumentType xmlDocumentType = (IXmlDocumentType)node;
					writer.WritePropertyName(this.GetPropertyName(node, manager));
					writer.WriteStartObject();
					if (!string.IsNullOrEmpty(xmlDocumentType.Name))
					{
						writer.WritePropertyName("@name");
						writer.WriteValue(xmlDocumentType.Name);
					}
					if (!string.IsNullOrEmpty(xmlDocumentType.Public))
					{
						writer.WritePropertyName("@public");
						writer.WriteValue(xmlDocumentType.Public);
					}
					if (!string.IsNullOrEmpty(xmlDocumentType.System))
					{
						writer.WritePropertyName("@system");
						writer.WriteValue(xmlDocumentType.System);
					}
					if (!string.IsNullOrEmpty(xmlDocumentType.InternalSubset))
					{
						writer.WritePropertyName("@internalSubset");
						writer.WriteValue(xmlDocumentType.InternalSubset);
					}
					writer.WriteEndObject();
					return;
				}
				case XmlNodeType.XmlDeclaration:
				{
					IXmlDeclaration xmlDeclaration = (IXmlDeclaration)node;
					writer.WritePropertyName(this.GetPropertyName(node, manager));
					writer.WriteStartObject();
					if (!string.IsNullOrEmpty(xmlDeclaration.Version))
					{
						writer.WritePropertyName("@version");
						writer.WriteValue(xmlDeclaration.Version);
					}
					if (!string.IsNullOrEmpty(xmlDeclaration.Encoding))
					{
						writer.WritePropertyName("@encoding");
						writer.WriteValue(xmlDeclaration.Encoding);
					}
					if (!string.IsNullOrEmpty(xmlDeclaration.Standalone))
					{
						writer.WritePropertyName("@standalone");
						writer.WriteValue(xmlDeclaration.Standalone);
					}
					writer.WriteEndObject();
					return;
				}
				default:
				{
					throw new JsonSerializationException(string.Concat("Unexpected XmlNodeType when serializing nodes: ", node.NodeType));
				}
			}
		}

		private bool ShouldReadInto(JsonReader reader)
		{
			switch (reader.TokenType)
			{
				case JsonToken.StartConstructor:
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Null:
				case JsonToken.Date:
				case JsonToken.Bytes:
				{
					return false;
				}
				default:
				{
					return true;
				}
			}
		}

		private bool ValueAttributes(List<IXmlNode> c)
		{
			bool flag;
			List<IXmlNode>.Enumerator enumerator = c.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IXmlNode current = enumerator.Current;
					if (current.NamespaceUri == "http://james.newtonking.com/projects/json" || current.NamespaceUri == "http://www.w3.org/2000/xmlns/" && current.Value == "http://james.newtonking.com/projects/json")
					{
						continue;
					}
					flag = true;
					return flag;
				}
				return false;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return flag;
		}

		private IXmlNode WrapXml(object value)
		{
			XObject xObject = value as XObject;
			XObject xObject1 = xObject;
			if (xObject != null)
			{
				return XContainerWrapper.WrapNode(xObject1);
			}
			XmlNode xmlNodes = value as XmlNode;
			XmlNode xmlNodes1 = xmlNodes;
			if (xmlNodes == null)
			{
				throw new ArgumentException("Value must be an XML object.", "value");
			}
			return XmlNodeWrapper.WrapNode(xmlNodes1);
		}

		private void WriteGroupedNodes(JsonWriter writer, XmlNamespaceManager manager, bool writePropertyName, List<IXmlNode> groupedNodes, string elementNames)
		{
			if ((groupedNodes.Count != 1 ? false : !this.IsArray(groupedNodes[0])))
			{
				this.SerializeNode(writer, groupedNodes[0], manager, writePropertyName);
				return;
			}
			if (writePropertyName)
			{
				writer.WritePropertyName(elementNames);
			}
			writer.WriteStartArray();
			for (int i = 0; i < groupedNodes.Count; i++)
			{
				this.SerializeNode(writer, groupedNodes[i], manager, false);
			}
			writer.WriteEndArray();
		}

		private void WriteGroupedNodes(JsonWriter writer, XmlNamespaceManager manager, bool writePropertyName, IXmlNode node, string elementNames)
		{
			if (!this.IsArray(node))
			{
				this.SerializeNode(writer, node, manager, writePropertyName);
				return;
			}
			if (writePropertyName)
			{
				writer.WritePropertyName(elementNames);
			}
			writer.WriteStartArray();
			this.SerializeNode(writer, node, manager, false);
			writer.WriteEndArray();
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			IXmlNode xmlNode = this.WrapXml(value);
			XmlNamespaceManager xmlNamespaceManagers = new XmlNamespaceManager(new NameTable());
			this.PushParentNamespaces(xmlNode, xmlNamespaceManagers);
			if (!this.OmitRootObject)
			{
				writer.WriteStartObject();
			}
			this.SerializeNode(writer, xmlNode, xmlNamespaceManagers, !this.OmitRootObject);
			if (!this.OmitRootObject)
			{
				writer.WriteEndObject();
			}
		}
	}
}