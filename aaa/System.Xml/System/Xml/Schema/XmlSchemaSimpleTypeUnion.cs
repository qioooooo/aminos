using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200027C RID: 636
	public class XmlSchemaSimpleTypeUnion : XmlSchemaSimpleTypeContent
	{
		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06001D56 RID: 7510 RVA: 0x00085E95 File Offset: 0x00084E95
		[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
		public XmlSchemaObjectCollection BaseTypes
		{
			get
			{
				return this.baseTypes;
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06001D57 RID: 7511 RVA: 0x00085E9D File Offset: 0x00084E9D
		// (set) Token: 0x06001D58 RID: 7512 RVA: 0x00085EA5 File Offset: 0x00084EA5
		[XmlAttribute("memberTypes")]
		public XmlQualifiedName[] MemberTypes
		{
			get
			{
				return this.memberTypes;
			}
			set
			{
				this.memberTypes = value;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06001D59 RID: 7513 RVA: 0x00085EAE File Offset: 0x00084EAE
		[XmlIgnore]
		public XmlSchemaSimpleType[] BaseMemberTypes
		{
			get
			{
				return this.baseMemberTypes;
			}
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x00085EB6 File Offset: 0x00084EB6
		internal void SetBaseMemberTypes(XmlSchemaSimpleType[] baseMemberTypes)
		{
			this.baseMemberTypes = baseMemberTypes;
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x00085EC0 File Offset: 0x00084EC0
		internal override XmlSchemaObject Clone()
		{
			if (this.memberTypes != null && this.memberTypes.Length > 0)
			{
				XmlSchemaSimpleTypeUnion xmlSchemaSimpleTypeUnion = (XmlSchemaSimpleTypeUnion)base.MemberwiseClone();
				XmlQualifiedName[] array = new XmlQualifiedName[this.memberTypes.Length];
				for (int i = 0; i < this.memberTypes.Length; i++)
				{
					array[i] = this.memberTypes[i].Clone();
				}
				xmlSchemaSimpleTypeUnion.MemberTypes = array;
				return xmlSchemaSimpleTypeUnion;
			}
			return this;
		}

		// Token: 0x040011DF RID: 4575
		private XmlSchemaObjectCollection baseTypes = new XmlSchemaObjectCollection();

		// Token: 0x040011E0 RID: 4576
		private XmlQualifiedName[] memberTypes;

		// Token: 0x040011E1 RID: 4577
		private XmlSchemaSimpleType[] baseMemberTypes;
	}
}
