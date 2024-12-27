using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000233 RID: 563
	public class XmlSchemaAll : XmlSchemaGroupBase
	{
		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06001AE1 RID: 6881 RVA: 0x00080CAC File Offset: 0x0007FCAC
		[XmlElement("element", typeof(XmlSchemaElement))]
		public override XmlSchemaObjectCollection Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06001AE2 RID: 6882 RVA: 0x00080CB4 File Offset: 0x0007FCB4
		internal override bool IsEmpty
		{
			get
			{
				return base.IsEmpty || this.items.Count == 0;
			}
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x00080CCE File Offset: 0x0007FCCE
		internal override void SetItems(XmlSchemaObjectCollection newItems)
		{
			this.items = newItems;
		}

		// Token: 0x040010DF RID: 4319
		private XmlSchemaObjectCollection items = new XmlSchemaObjectCollection();
	}
}
