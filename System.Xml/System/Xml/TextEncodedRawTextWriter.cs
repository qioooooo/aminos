﻿using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	internal class TextEncodedRawTextWriter : XmlEncodedRawTextWriter
	{
		public TextEncodedRawTextWriter(TextWriter writer, XmlWriterSettings settings)
			: base(writer, settings)
		{
		}

		public TextEncodedRawTextWriter(Stream stream, Encoding encoding, XmlWriterSettings settings, bool closeOutput)
			: base(stream, encoding, settings, closeOutput)
		{
		}

		internal override void WriteXmlDeclaration(XmlStandalone standalone)
		{
		}

		internal override void WriteXmlDeclaration(string xmldecl)
		{
		}

		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
		}

		public override void WriteStartElement(string prefix, string localName, string ns)
		{
		}

		internal override void WriteEndElement(string prefix, string localName, string ns)
		{
		}

		internal override void WriteFullEndElement(string prefix, string localName, string ns)
		{
		}

		internal override void StartElementContent()
		{
		}

		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			this.inAttributeValue = true;
		}

		public override void WriteEndAttribute()
		{
			this.inAttributeValue = false;
		}

		internal override void WriteNamespaceDeclaration(string prefix, string ns)
		{
		}

		public override void WriteCData(string text)
		{
			base.WriteRaw(text);
		}

		public override void WriteComment(string text)
		{
		}

		public override void WriteProcessingInstruction(string name, string text)
		{
		}

		public override void WriteEntityRef(string name)
		{
		}

		public override void WriteCharEntity(char ch)
		{
		}

		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
		}

		public override void WriteWhitespace(string ws)
		{
			if (!this.inAttributeValue)
			{
				base.WriteRaw(ws);
			}
		}

		public override void WriteString(string textBlock)
		{
			if (!this.inAttributeValue)
			{
				base.WriteRaw(textBlock);
			}
		}

		public override void WriteChars(char[] buffer, int index, int count)
		{
			if (!this.inAttributeValue)
			{
				base.WriteRaw(buffer, index, count);
			}
		}

		public override void WriteRaw(char[] buffer, int index, int count)
		{
			if (!this.inAttributeValue)
			{
				base.WriteRaw(buffer, index, count);
			}
		}

		public override void WriteRaw(string data)
		{
			if (!this.inAttributeValue)
			{
				base.WriteRaw(data);
			}
		}
	}
}
