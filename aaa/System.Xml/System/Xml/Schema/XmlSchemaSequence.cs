using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000272 RID: 626
	public class XmlSchemaSequence : XmlSchemaGroupBase
	{
		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06001CEC RID: 7404 RVA: 0x00083D38 File Offset: 0x00082D38
		[XmlElement("group", typeof(XmlSchemaGroupRef))]
		[XmlElement("choice", typeof(XmlSchemaChoice))]
		[XmlElement("any", typeof(XmlSchemaAny))]
		[XmlElement("element", typeof(XmlSchemaElement))]
		[XmlElement("sequence", typeof(XmlSchemaSequence))]
		public override XmlSchemaObjectCollection Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06001CED RID: 7405 RVA: 0x00083D40 File Offset: 0x00082D40
		internal override bool IsEmpty
		{
			get
			{
				return base.IsEmpty || this.items.Count == 0;
			}
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x00083D5A File Offset: 0x00082D5A
		internal override void SetItems(XmlSchemaObjectCollection newItems)
		{
			this.items = newItems;
		}

		// Token: 0x040011B9 RID: 4537
		private XmlSchemaObjectCollection items = new XmlSchemaObjectCollection();
	}
}
