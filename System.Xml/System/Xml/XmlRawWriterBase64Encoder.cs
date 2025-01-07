using System;

namespace System.Xml
{
	internal class XmlRawWriterBase64Encoder : Base64Encoder
	{
		internal XmlRawWriterBase64Encoder(XmlRawWriter rawWriter)
		{
			this.rawWriter = rawWriter;
		}

		internal override void WriteChars(char[] chars, int index, int count)
		{
			this.rawWriter.WriteRaw(chars, index, count);
		}

		private XmlRawWriter rawWriter;
	}
}
