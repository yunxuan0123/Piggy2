using System;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	internal class Class3 : XObjectWrapper, IXmlDeclaration, IXmlNode
	{
		internal XDeclaration Declaration
		{
			get;
		}

		public string Encoding
		{
			get
			{
				return this.Declaration.Encoding;
			}
			set
			{
				this.Declaration.Encoding = value;
			}
		}

		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.XmlDeclaration;
			}
		}

		public string Standalone
		{
			get
			{
				return this.Declaration.Standalone;
			}
			set
			{
				this.Declaration.Standalone = value;
			}
		}

		public string Version
		{
			get
			{
				return this.Declaration.Version;
			}
		}

		public Class3(XDeclaration declaration)
		{
			Class6.yDnXvgqzyB5jw();
			base(null);
			this.Declaration = declaration;
		}
	}
}