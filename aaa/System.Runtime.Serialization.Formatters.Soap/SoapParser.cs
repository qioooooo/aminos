using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000004 RID: 4
	internal sealed class SoapParser : ISerParser
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002308 File Offset: 0x00001308
		internal SoapParser(Stream stream)
		{
			if (this.bDebug)
			{
				this.xmlReader = new XmlTextReader(this.textReader);
			}
			else
			{
				this.xmlReader = new XmlTextReader(stream);
			}
			this.xmlReader.XmlResolver = null;
			this.xmlReader.ProhibitDtd = true;
			this.soapHandler = new SoapHandler(this);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002368 File Offset: 0x00001368
		[Conditional("_LOGGING")]
		private void TraceStream(Stream stream)
		{
			this.bDebug = true;
			TextReader textReader = new StreamReader(stream);
			string text = textReader.ReadToEnd();
			this.textReader = new StringReader(text);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002396 File Offset: 0x00001396
		internal void Init(ObjectReader objectReader)
		{
			this.objectReader = objectReader;
			this.soapHandler.Init(objectReader);
			this.bStop = false;
			this.depth = 0;
			this.xmlReader.ResetState();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000023C4 File Offset: 0x000013C4
		public void Run()
		{
			try
			{
				this.soapHandler.Start(this.xmlReader);
				this.ParseXml();
			}
			catch (EndOfStreamException)
			{
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002400 File Offset: 0x00001400
		internal void Stop()
		{
			this.bStop = true;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000240C File Offset: 0x0000140C
		private void ParseXml()
		{
			while (!this.bStop && this.xmlReader.Read())
			{
				if (this.depth < this.xmlReader.Depth)
				{
					this.soapHandler.StartChildren();
					this.depth = this.xmlReader.Depth;
				}
				else if (this.depth > this.xmlReader.Depth)
				{
					this.soapHandler.FinishChildren(this.xmlReader.Prefix, this.xmlReader.LocalName, this.xmlReader.NamespaceURI);
					this.depth = this.xmlReader.Depth;
				}
				switch (this.xmlReader.NodeType)
				{
				case XmlNodeType.Element:
				{
					this.soapHandler.StartElement(this.xmlReader.Prefix, this.xmlReader.LocalName, this.xmlReader.NamespaceURI);
					int attributeCount = this.xmlReader.AttributeCount;
					while (this.xmlReader.MoveToNextAttribute())
					{
						this.soapHandler.Attribute(this.xmlReader.Prefix, this.xmlReader.LocalName, this.xmlReader.NamespaceURI, this.xmlReader.Value);
					}
					this.xmlReader.MoveToElement();
					if (this.xmlReader.IsEmptyElement)
					{
						this.soapHandler.EndElement(this.xmlReader.Prefix, this.xmlReader.LocalName, this.xmlReader.NamespaceURI);
					}
					break;
				}
				case XmlNodeType.Text:
					this.soapHandler.Text(this.xmlReader.Value);
					break;
				case XmlNodeType.CDATA:
					this.soapHandler.Text(this.xmlReader.Value);
					break;
				case XmlNodeType.Comment:
					this.soapHandler.Comment(this.xmlReader.Value);
					break;
				case XmlNodeType.Whitespace:
					this.soapHandler.Text(this.xmlReader.Value);
					break;
				case XmlNodeType.SignificantWhitespace:
					this.soapHandler.Text(this.xmlReader.Value);
					break;
				case XmlNodeType.EndElement:
					this.soapHandler.EndElement(this.xmlReader.Prefix, this.xmlReader.LocalName, this.xmlReader.NamespaceURI);
					break;
				}
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002681 File Offset: 0x00001681
		[Conditional("SER_LOGGING")]
		private static void Dump(string name, XmlReader xmlReader)
		{
		}

		// Token: 0x0400000A RID: 10
		internal XmlTextReader xmlReader;

		// Token: 0x0400000B RID: 11
		internal SoapHandler soapHandler;

		// Token: 0x0400000C RID: 12
		internal ObjectReader objectReader;

		// Token: 0x0400000D RID: 13
		internal bool bStop;

		// Token: 0x0400000E RID: 14
		private int depth;

		// Token: 0x0400000F RID: 15
		private bool bDebug;

		// Token: 0x04000010 RID: 16
		private TextReader textReader;
	}
}
