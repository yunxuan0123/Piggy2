using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	internal class XDocumentWrapper : XContainerWrapper, IXmlDocument, IXmlNode
	{
		public override List<IXmlNode> ChildNodes
		{
			get
			{
				List<IXmlNode> childNodes = base.ChildNodes;
				if (this.Document.Declaration != null && (childNodes.Count == 0 || childNodes[0].NodeType != XmlNodeType.XmlDeclaration))
				{
					childNodes.Insert(0, new Class3(this.Document.Declaration));
				}
				return childNodes;
			}
		}

		private XDocument Document
		{
			get
			{
				return (XDocument)base.WrappedNode;
			}
		}

		public IXmlElement DocumentElement
		{
			get
			{
				if (this.Document.Root == null)
				{
					return null;
				}
				return new XElementWrapper(this.Document.Root);
			}
		}

		protected override bool HasChildNodes
		{
			get
			{
				if (base.HasChildNodes)
				{
					return true;
				}
				return this.Document.Declaration != null;
			}
		}

		public XDocumentWrapper(XDocument document)
		{
			Class6.yDnXvgqzyB5jw();
			base(document);
		}

		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			Class3 class3 = newChild as Class3;
			Class3 class31 = class3;
			if (class3 == null)
			{
				return base.AppendChild(newChild);
			}
			this.Document.Declaration = class31.Declaration;
			return class31;
		}

		public IXmlNode CreateAttribute(string name, string value)
		{
			return new XAttributeWrapper(new XAttribute(name, value));
		}

		public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value)
		{
			return new XAttributeWrapper(new XAttribute(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceUri), value));
		}

		public IXmlNode CreateCDataSection(string data)
		{
			return new XObjectWrapper(new XCData(data));
		}

		public IXmlNode CreateComment(string text)
		{
			return new XObjectWrapper(new XComment(text));
		}

		public IXmlElement CreateElement(string elementName)
		{
			return new XElementWrapper(new XElement(elementName));
		}

		public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
		{
			return new XElementWrapper(new XElement(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceUri)));
		}

		public IXmlNode CreateProcessingInstruction(string target, string data)
		{
			return new XProcessingInstructionWrapper(new XProcessingInstruction(target, data));
		}

		public IXmlNode CreateSignificantWhitespace(string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		public IXmlNode CreateTextNode(string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		public IXmlNode CreateWhitespace(string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new Class3(new XDeclaration(version, encoding, standalone));
		}

		public IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new Class4(new XDocumentType(name, publicId, systemId, internalSubset));
		}
	}
}