using System;
using System.IO;

namespace System.Xml.Xsl
{
	// Token: 0x02000009 RID: 9
	internal class QueryReaderSettings
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002424 File Offset: 0x00001424
		public QueryReaderSettings(XmlNameTable xmlNameTable)
		{
			this.xmlReaderSettings = new XmlReaderSettings();
			this.xmlReaderSettings.NameTable = xmlNameTable;
			this.xmlReaderSettings.ConformanceLevel = ConformanceLevel.Document;
			this.xmlReaderSettings.XmlResolver = null;
			this.xmlReaderSettings.ProhibitDtd = true;
			this.xmlReaderSettings.CloseInput = true;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002480 File Offset: 0x00001480
		public QueryReaderSettings(XmlReader reader)
		{
			XmlValidatingReader xmlValidatingReader = reader as XmlValidatingReader;
			if (xmlValidatingReader != null)
			{
				this.validatingReader = true;
				reader = xmlValidatingReader.Impl.Reader;
			}
			this.xmlReaderSettings = reader.Settings;
			if (this.xmlReaderSettings != null)
			{
				this.xmlReaderSettings = this.xmlReaderSettings.Clone();
				this.xmlReaderSettings.NameTable = reader.NameTable;
				this.xmlReaderSettings.CloseInput = true;
				this.xmlReaderSettings.LineNumberOffset = 0;
				this.xmlReaderSettings.LinePositionOffset = 0;
				XmlTextReaderImpl xmlTextReaderImpl = reader as XmlTextReaderImpl;
				if (xmlTextReaderImpl != null)
				{
					this.xmlReaderSettings.XmlResolver = xmlTextReaderImpl.GetResolver();
					return;
				}
			}
			else
			{
				this.xmlNameTable = reader.NameTable;
				XmlTextReader xmlTextReader = reader as XmlTextReader;
				if (xmlTextReader != null)
				{
					XmlTextReaderImpl impl = xmlTextReader.Impl;
					this.entityHandling = impl.EntityHandling;
					this.namespaces = impl.Namespaces;
					this.normalization = impl.Normalization;
					this.prohibitDtd = impl.ProhibitDtd;
					this.whitespaceHandling = impl.WhitespaceHandling;
					this.xmlResolver = impl.GetResolver();
					return;
				}
				this.entityHandling = EntityHandling.ExpandEntities;
				this.namespaces = true;
				this.normalization = true;
				this.prohibitDtd = true;
				this.whitespaceHandling = WhitespaceHandling.All;
				this.xmlResolver = null;
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000025BC File Offset: 0x000015BC
		public XmlReader CreateReader(Stream stream, string baseUri)
		{
			XmlReader xmlReader;
			if (this.xmlReaderSettings != null)
			{
				xmlReader = XmlReader.Create(stream, this.xmlReaderSettings, baseUri);
			}
			else
			{
				xmlReader = new XmlTextReaderImpl(baseUri, stream, this.xmlNameTable)
				{
					EntityHandling = this.entityHandling,
					Namespaces = this.namespaces,
					Normalization = this.normalization,
					ProhibitDtd = this.prohibitDtd,
					WhitespaceHandling = this.whitespaceHandling,
					XmlResolver = this.xmlResolver
				};
			}
			if (this.validatingReader)
			{
				xmlReader = new XmlValidatingReader(xmlReader);
			}
			return xmlReader;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002649 File Offset: 0x00001649
		public XmlNameTable NameTable
		{
			get
			{
				if (this.xmlReaderSettings == null)
				{
					return this.xmlNameTable;
				}
				return this.xmlReaderSettings.NameTable;
			}
		}

		// Token: 0x040000B5 RID: 181
		private bool validatingReader;

		// Token: 0x040000B6 RID: 182
		private XmlReaderSettings xmlReaderSettings;

		// Token: 0x040000B7 RID: 183
		private XmlNameTable xmlNameTable;

		// Token: 0x040000B8 RID: 184
		private EntityHandling entityHandling;

		// Token: 0x040000B9 RID: 185
		private bool namespaces;

		// Token: 0x040000BA RID: 186
		private bool normalization;

		// Token: 0x040000BB RID: 187
		private bool prohibitDtd;

		// Token: 0x040000BC RID: 188
		private WhitespaceHandling whitespaceHandling;

		// Token: 0x040000BD RID: 189
		private XmlResolver xmlResolver;
	}
}
