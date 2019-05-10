using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	internal class XElementWrapper : XContainerWrapper, IXmlElement, IXmlNode
	{
		private List<IXmlNode> _attributes;

		public override List<IXmlNode> Attributes
		{
			get
			{
				if (this._attributes == null)
				{
					if (this.Element.HasAttributes || this.HasImplicitNamespaceAttribute(this.NamespaceUri))
					{
						this._attributes = new List<IXmlNode>();
						foreach (XAttribute xAttribute in this.Element.Attributes())
						{
							this._attributes.Add(new XAttributeWrapper(xAttribute));
						}
						string namespaceUri = this.NamespaceUri;
						if (this.HasImplicitNamespaceAttribute(namespaceUri))
						{
							this._attributes.Insert(0, new XAttributeWrapper(new XAttribute("xmlns", namespaceUri)));
						}
					}
					else
					{
						this._attributes = XmlNodeConverter.EmptyChildNodes;
					}
				}
				return this._attributes;
			}
		}

		private XElement Element
		{
			get
			{
				return (XElement)base.WrappedNode;
			}
		}

		public bool IsEmpty
		{
			get
			{
				return this.Element.IsEmpty;
			}
		}

		public override string LocalName
		{
			get
			{
				return this.Element.Name.LocalName;
			}
		}

		public override string NamespaceUri
		{
			get
			{
				return this.Element.Name.NamespaceName;
			}
		}

		public override string Value
		{
			get
			{
				return this.Element.Value;
			}
			set
			{
				this.Element.Value = value;
			}
		}

		public XElementWrapper(XElement element)
		{
			Class6.yDnXvgqzyB5jw();
			base(element);
		}

		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			this._attributes = null;
			return base.AppendChild(newChild);
		}

		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return this.Element.GetPrefixOfNamespace(namespaceUri);
		}

		private bool HasImplicitNamespaceAttribute(string namespaceUri)
		{
			string str;
			if (!string.IsNullOrEmpty(namespaceUri))
			{
				string str1 = namespaceUri;
				IXmlNode parentNode = this.ParentNode;
				if (parentNode != null)
				{
					str = parentNode.NamespaceUri;
				}
				else
				{
					str = null;
				}
				if (str1 != str && string.IsNullOrEmpty(this.GetPrefixOfNamespace(namespaceUri)))
				{
					bool flag = false;
					if (this.Element.HasAttributes)
					{
						foreach (XAttribute xAttribute in this.Element.Attributes())
						{
							if (!(xAttribute.Name.LocalName == "xmlns") || !string.IsNullOrEmpty(xAttribute.Name.NamespaceName) || !(xAttribute.Value == namespaceUri))
							{
								continue;
							}
							flag = true;
						}
					}
					if (!flag)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void SetAttributeNode(IXmlNode attribute)
		{
			XObjectWrapper xObjectWrapper = (XObjectWrapper)attribute;
			this.Element.Add(xObjectWrapper.WrappedNode);
			this._attributes = null;
		}
	}
}