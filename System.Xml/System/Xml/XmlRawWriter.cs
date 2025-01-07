using System;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
	internal abstract class XmlRawWriter : XmlWriter
	{
		public override void WriteStartDocument()
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		public override void WriteStartDocument(bool standalone)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		public override void WriteEndDocument()
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
		}

		public override void WriteEndElement()
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		public override void WriteFullEndElement()
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			if (this.base64Encoder == null)
			{
				this.base64Encoder = new XmlRawWriterBase64Encoder(this);
			}
			this.base64Encoder.Encode(buffer, index, count);
		}

		public override string LookupPrefix(string ns)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		public override WriteState WriteState
		{
			get
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
			}
		}

		public override XmlSpace XmlSpace
		{
			get
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
			}
		}

		public override string XmlLang
		{
			get
			{
				throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
			}
		}

		public override void WriteNmToken(string name)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		public override void WriteName(string name)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		public override void WriteQualifiedName(string localName, string ns)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		public override void WriteCData(string text)
		{
			this.WriteString(text);
		}

		public override void WriteCharEntity(char ch)
		{
			this.WriteString(new string(new char[] { ch }));
		}

		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			this.WriteString(new string(new char[] { lowChar, highChar }));
		}

		public override void WriteWhitespace(string ws)
		{
			this.WriteString(ws);
		}

		public override void WriteChars(char[] buffer, int index, int count)
		{
			this.WriteString(new string(buffer, index, count));
		}

		public override void WriteRaw(char[] buffer, int index, int count)
		{
			this.WriteString(new string(buffer, index, count));
		}

		public override void WriteRaw(string data)
		{
			this.WriteString(data);
		}

		public override void WriteValue(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.WriteString(XmlUntypedConverter.Untyped.ToString(value, this.resolver));
		}

		public override void WriteValue(string value)
		{
			this.WriteString(value);
		}

		public override void WriteAttributes(XmlReader reader, bool defattr)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		public override void WriteNode(XmlReader reader, bool defattr)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		public override void WriteNode(XPathNavigator navigator, bool defattr)
		{
			throw new InvalidOperationException(Res.GetString("Xml_InvalidOperation"));
		}

		internal virtual IXmlNamespaceResolver NamespaceResolver
		{
			get
			{
				return this.resolver;
			}
			set
			{
				this.resolver = value;
			}
		}

		internal virtual void WriteXmlDeclaration(XmlStandalone standalone)
		{
		}

		internal virtual void WriteXmlDeclaration(string xmldecl)
		{
		}

		internal abstract void StartElementContent();

		internal virtual void OnRootElement(ConformanceLevel conformanceLevel)
		{
		}

		internal abstract void WriteEndElement(string prefix, string localName, string ns);

		internal virtual void WriteFullEndElement(string prefix, string localName, string ns)
		{
			this.WriteEndElement(prefix, localName, ns);
		}

		internal virtual void WriteQualifiedName(string prefix, string localName, string ns)
		{
			if (prefix.Length != 0)
			{
				this.WriteString(prefix);
				this.WriteString(":");
			}
			this.WriteString(localName);
		}

		internal abstract void WriteNamespaceDeclaration(string prefix, string ns);

		internal virtual void WriteEndBase64()
		{
			this.base64Encoder.Flush();
		}

		internal virtual void Close(WriteState currentState)
		{
			this.Close();
		}

		internal const int SurHighStart = 55296;

		internal const int SurHighEnd = 56319;

		internal const int SurLowStart = 56320;

		internal const int SurLowEnd = 57343;

		internal const int SurMask = 64512;

		protected XmlRawWriterBase64Encoder base64Encoder;

		protected IXmlNamespaceResolver resolver;
	}
}
