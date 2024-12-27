using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200027A RID: 634
	public class XmlSchemaSimpleTypeList : XmlSchemaSimpleTypeContent
	{
		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06001D47 RID: 7495 RVA: 0x00085D90 File Offset: 0x00084D90
		// (set) Token: 0x06001D48 RID: 7496 RVA: 0x00085D98 File Offset: 0x00084D98
		[XmlAttribute("itemType")]
		public XmlQualifiedName ItemTypeName
		{
			get
			{
				return this.itemTypeName;
			}
			set
			{
				this.itemTypeName = ((value == null) ? XmlQualifiedName.Empty : value);
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06001D49 RID: 7497 RVA: 0x00085DB1 File Offset: 0x00084DB1
		// (set) Token: 0x06001D4A RID: 7498 RVA: 0x00085DB9 File Offset: 0x00084DB9
		[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
		public XmlSchemaSimpleType ItemType
		{
			get
			{
				return this.itemType;
			}
			set
			{
				this.itemType = value;
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06001D4B RID: 7499 RVA: 0x00085DC2 File Offset: 0x00084DC2
		// (set) Token: 0x06001D4C RID: 7500 RVA: 0x00085DCA File Offset: 0x00084DCA
		[XmlIgnore]
		public XmlSchemaSimpleType BaseItemType
		{
			get
			{
				return this.baseItemType;
			}
			set
			{
				this.baseItemType = value;
			}
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x00085DD4 File Offset: 0x00084DD4
		internal override XmlSchemaObject Clone()
		{
			XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = (XmlSchemaSimpleTypeList)base.MemberwiseClone();
			xmlSchemaSimpleTypeList.ItemTypeName = this.itemTypeName.Clone();
			return xmlSchemaSimpleTypeList;
		}

		// Token: 0x040011D9 RID: 4569
		private XmlQualifiedName itemTypeName = XmlQualifiedName.Empty;

		// Token: 0x040011DA RID: 4570
		private XmlSchemaSimpleType itemType;

		// Token: 0x040011DB RID: 4571
		private XmlSchemaSimpleType baseItemType;
	}
}
