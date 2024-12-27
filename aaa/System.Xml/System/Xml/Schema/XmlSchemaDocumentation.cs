using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000249 RID: 585
	public class XmlSchemaDocumentation : XmlSchemaObject
	{
		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x000827D4 File Offset: 0x000817D4
		// (set) Token: 0x06001BDE RID: 7134 RVA: 0x000827DC File Offset: 0x000817DC
		[XmlAttribute("source", DataType = "anyURI")]
		public string Source
		{
			get
			{
				return this.source;
			}
			set
			{
				this.source = value;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06001BDF RID: 7135 RVA: 0x000827E5 File Offset: 0x000817E5
		// (set) Token: 0x06001BE0 RID: 7136 RVA: 0x000827ED File Offset: 0x000817ED
		[XmlAttribute("xml:lang")]
		public string Language
		{
			get
			{
				return this.language;
			}
			set
			{
				this.language = (string)XmlSchemaDocumentation.languageType.Datatype.ParseValue(value, null, null);
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06001BE1 RID: 7137 RVA: 0x0008280C File Offset: 0x0008180C
		// (set) Token: 0x06001BE2 RID: 7138 RVA: 0x00082814 File Offset: 0x00081814
		[XmlText]
		[XmlAnyElement]
		public XmlNode[] Markup
		{
			get
			{
				return this.markup;
			}
			set
			{
				this.markup = value;
			}
		}

		// Token: 0x04001147 RID: 4423
		private string source;

		// Token: 0x04001148 RID: 4424
		private string language;

		// Token: 0x04001149 RID: 4425
		private XmlNode[] markup;

		// Token: 0x0400114A RID: 4426
		private static XmlSchemaSimpleType languageType = DatatypeImplementation.GetSimpleTypeFromXsdType(new XmlQualifiedName("language", "http://www.w3.org/2001/XMLSchema"));
	}
}
