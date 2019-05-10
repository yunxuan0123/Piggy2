using System;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	internal class Class4 : XObjectWrapper, IXmlDocumentType, IXmlNode
	{
		private readonly XDocumentType _documentType;

		public string InternalSubset
		{
			get
			{
				return this._documentType.InternalSubset;
			}
		}

		public override string LocalName
		{
			get
			{
				return "DOCTYPE";
			}
		}

		public string Name
		{
			get
			{
				return this._documentType.Name;
			}
		}

		public string Public
		{
			get
			{
				return this._documentType.PublicId;
			}
		}

		public string System
		{
			get
			{
				return this._documentType.SystemId;
			}
		}

		public Class4(XDocumentType documentType)
		{
			Class6.yDnXvgqzyB5jw();
			base(documentType);
			this._documentType = documentType;
		}
	}
}