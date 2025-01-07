using System;

namespace System.Xml
{
	internal class XmlTextWriterBase64Encoder : Base64Encoder
	{
		internal XmlTextWriterBase64Encoder(XmlTextEncoder xmlTextEncoder)
		{
			this.xmlTextEncoder = xmlTextEncoder;
		}

		internal override void WriteChars(char[] chars, int index, int count)
		{
			this.xmlTextEncoder.WriteRaw(chars, index, count);
		}

		private XmlTextEncoder xmlTextEncoder;
	}
}
