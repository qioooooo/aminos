using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000237 RID: 567
	public class XmlSchemaAppInfo : XmlSchemaObject
	{
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06001B07 RID: 6919 RVA: 0x00081037 File Offset: 0x00080037
		// (set) Token: 0x06001B08 RID: 6920 RVA: 0x0008103F File Offset: 0x0008003F
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

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06001B09 RID: 6921 RVA: 0x00081048 File Offset: 0x00080048
		// (set) Token: 0x06001B0A RID: 6922 RVA: 0x00081050 File Offset: 0x00080050
		[XmlAnyElement]
		[XmlText]
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

		// Token: 0x040010E9 RID: 4329
		private string source;

		// Token: 0x040010EA RID: 4330
		private XmlNode[] markup;
	}
}
