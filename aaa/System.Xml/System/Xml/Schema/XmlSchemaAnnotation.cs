using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000234 RID: 564
	public class XmlSchemaAnnotation : XmlSchemaObject
	{
		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06001AE5 RID: 6885 RVA: 0x00080CEA File Offset: 0x0007FCEA
		// (set) Token: 0x06001AE6 RID: 6886 RVA: 0x00080CF2 File Offset: 0x0007FCF2
		[XmlAttribute("id", DataType = "ID")]
		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x00080CFB File Offset: 0x0007FCFB
		[XmlElement("documentation", typeof(XmlSchemaDocumentation))]
		[XmlElement("appinfo", typeof(XmlSchemaAppInfo))]
		public XmlSchemaObjectCollection Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06001AE8 RID: 6888 RVA: 0x00080D03 File Offset: 0x0007FD03
		// (set) Token: 0x06001AE9 RID: 6889 RVA: 0x00080D0B File Offset: 0x0007FD0B
		[XmlAnyAttribute]
		public XmlAttribute[] UnhandledAttributes
		{
			get
			{
				return this.moreAttributes;
			}
			set
			{
				this.moreAttributes = value;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06001AEA RID: 6890 RVA: 0x00080D14 File Offset: 0x0007FD14
		// (set) Token: 0x06001AEB RID: 6891 RVA: 0x00080D1C File Offset: 0x0007FD1C
		[XmlIgnore]
		internal override string IdAttribute
		{
			get
			{
				return this.Id;
			}
			set
			{
				this.Id = value;
			}
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x00080D25 File Offset: 0x0007FD25
		internal override void SetUnhandledAttributes(XmlAttribute[] moreAttributes)
		{
			this.moreAttributes = moreAttributes;
		}

		// Token: 0x040010E0 RID: 4320
		private string id;

		// Token: 0x040010E1 RID: 4321
		private XmlSchemaObjectCollection items = new XmlSchemaObjectCollection();

		// Token: 0x040010E2 RID: 4322
		private XmlAttribute[] moreAttributes;
	}
}
