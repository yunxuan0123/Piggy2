using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	internal class XContainerWrapper : XObjectWrapper
	{
		private List<IXmlNode> _childNodes;

		public override List<IXmlNode> ChildNodes
		{
			get
			{
				if (this._childNodes == null)
				{
					if (this.HasChildNodes)
					{
						this._childNodes = new List<IXmlNode>();
						foreach (XNode xNode in this.Container.Nodes())
						{
							this._childNodes.Add(XContainerWrapper.WrapNode(xNode));
						}
					}
					else
					{
						this._childNodes = XmlNodeConverter.EmptyChildNodes;
					}
				}
				return this._childNodes;
			}
		}

		private XContainer Container
		{
			get
			{
				return (XContainer)base.WrappedNode;
			}
		}

		protected virtual bool HasChildNodes
		{
			get
			{
				return this.Container.LastNode != null;
			}
		}

		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Container.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Container.Parent);
			}
		}

		public XContainerWrapper(XContainer container)
		{
			Class6.yDnXvgqzyB5jw();
			base(container);
		}

		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			this.Container.Add(newChild.WrappedNode);
			this._childNodes = null;
			return newChild;
		}

		internal static IXmlNode WrapNode(XObject node)
		{
			XDocument xDocument = node as XDocument;
			XDocument xDocument1 = xDocument;
			if (xDocument != null)
			{
				return new XDocumentWrapper(xDocument1);
			}
			XElement xElement = node as XElement;
			XElement xElement1 = xElement;
			if (xElement != null)
			{
				return new XElementWrapper(xElement1);
			}
			XContainer xContainer = node as XContainer;
			XContainer xContainer1 = xContainer;
			if (xContainer != null)
			{
				return new XContainerWrapper(xContainer1);
			}
			XProcessingInstruction xProcessingInstruction = node as XProcessingInstruction;
			XProcessingInstruction xProcessingInstruction1 = xProcessingInstruction;
			if (xProcessingInstruction != null)
			{
				return new XProcessingInstructionWrapper(xProcessingInstruction1);
			}
			XText xText = node as XText;
			XText xText1 = xText;
			if (xText != null)
			{
				return new XTextWrapper(xText1);
			}
			XComment xComment = node as XComment;
			XComment xComment1 = xComment;
			if (xComment != null)
			{
				return new XCommentWrapper(xComment1);
			}
			XAttribute xAttribute = node as XAttribute;
			XAttribute xAttribute1 = xAttribute;
			if (xAttribute != null)
			{
				return new XAttributeWrapper(xAttribute1);
			}
			XDocumentType xDocumentType = node as XDocumentType;
			XDocumentType xDocumentType1 = xDocumentType;
			if (xDocumentType != null)
			{
				return new Class4(xDocumentType1);
			}
			return new XObjectWrapper(node);
		}
	}
}