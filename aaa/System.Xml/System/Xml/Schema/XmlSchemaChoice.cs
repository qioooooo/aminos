using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200023B RID: 571
	public class XmlSchemaChoice : XmlSchemaGroupBase
	{
		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06001B41 RID: 6977 RVA: 0x0008137D File Offset: 0x0008037D
		[XmlElement("sequence", typeof(XmlSchemaSequence))]
		[XmlElement("any", typeof(XmlSchemaAny))]
		[XmlElement("element", typeof(XmlSchemaElement))]
		[XmlElement("choice", typeof(XmlSchemaChoice))]
		[XmlElement("group", typeof(XmlSchemaGroupRef))]
		public override XmlSchemaObjectCollection Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06001B42 RID: 6978 RVA: 0x00081385 File Offset: 0x00080385
		internal override bool IsEmpty
		{
			get
			{
				return base.IsEmpty;
			}
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x0008138D File Offset: 0x0008038D
		internal override void SetItems(XmlSchemaObjectCollection newItems)
		{
			this.items = newItems;
		}

		// Token: 0x04001100 RID: 4352
		private XmlSchemaObjectCollection items = new XmlSchemaObjectCollection();
	}
}
