using System;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	internal class XmlElementWrapper : XmlNodeWrapper, IXmlElement, IXmlNode
	{
		private readonly XmlElement _element;

		public bool IsEmpty
		{
			get
			{
				return this._element.IsEmpty;
			}
		}

		public XmlElementWrapper(XmlElement element)
		{
			Class6.yDnXvgqzyB5jw();
			base(element);
			this._element = element;
		}

		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return this._element.GetPrefixOfNamespace(namespaceUri);
		}

		public void SetAttributeNode(IXmlNode attribute)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)attribute;
			this._element.SetAttributeNode((XmlAttribute)xmlNodeWrapper.WrappedNode);
		}
	}
}