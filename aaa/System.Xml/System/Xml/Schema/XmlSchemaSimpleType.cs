using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000278 RID: 632
	public class XmlSchemaSimpleType : XmlSchemaType
	{
		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06001D42 RID: 7490 RVA: 0x00085D0C File Offset: 0x00084D0C
		// (set) Token: 0x06001D43 RID: 7491 RVA: 0x00085D14 File Offset: 0x00084D14
		[XmlElement("union", typeof(XmlSchemaSimpleTypeUnion))]
		[XmlElement("list", typeof(XmlSchemaSimpleTypeList))]
		[XmlElement("restriction", typeof(XmlSchemaSimpleTypeRestriction))]
		public XmlSchemaSimpleTypeContent Content
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

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06001D44 RID: 7492 RVA: 0x00085D1D File Offset: 0x00084D1D
		internal override XmlQualifiedName DerivedFrom
		{
			get
			{
				if (this.content == null)
				{
					return XmlQualifiedName.Empty;
				}
				if (this.content is XmlSchemaSimpleTypeRestriction)
				{
					return ((XmlSchemaSimpleTypeRestriction)this.content).BaseTypeName;
				}
				return XmlQualifiedName.Empty;
			}
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x00085D50 File Offset: 0x00084D50
		internal override XmlSchemaObject Clone()
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)base.MemberwiseClone();
			if (this.content != null)
			{
				xmlSchemaSimpleType.Content = (XmlSchemaSimpleTypeContent)this.content.Clone();
			}
			return xmlSchemaSimpleType;
		}

		// Token: 0x040011D8 RID: 4568
		private XmlSchemaSimpleTypeContent content;
	}
}
