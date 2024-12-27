using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000275 RID: 629
	public class XmlSchemaSimpleContent : XmlSchemaContentModel
	{
		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06001D2D RID: 7469 RVA: 0x00085C05 File Offset: 0x00084C05
		// (set) Token: 0x06001D2E RID: 7470 RVA: 0x00085C0D File Offset: 0x00084C0D
		[XmlElement("extension", typeof(XmlSchemaSimpleContentExtension))]
		[XmlElement("restriction", typeof(XmlSchemaSimpleContentRestriction))]
		public override XmlSchemaContent Content
		{
			get
			{
				return this.content;
			}
			set
			{
				this.content = value;
			}
		}

		// Token: 0x040011CF RID: 4559
		private XmlSchemaContent content;
	}
}
