using System;
using System.Xml.Schema;

namespace System.Xml
{
	// Token: 0x020000E4 RID: 228
	internal sealed class XmlNameEx : XmlName
	{
		// Token: 0x06000DED RID: 3565 RVA: 0x0003DFF0 File Offset: 0x0003CFF0
		internal XmlNameEx(string prefix, string localName, string ns, int hashCode, XmlDocument ownerDoc, XmlName next, IXmlSchemaInfo schemaInfo)
			: base(prefix, localName, ns, hashCode, ownerDoc, next)
		{
			this.SetValidity(schemaInfo.Validity);
			this.SetIsDefault(schemaInfo.IsDefault);
			this.SetIsNil(schemaInfo.IsNil);
			this.memberType = schemaInfo.MemberType;
			this.schemaType = schemaInfo.SchemaType;
			this.decl = ((schemaInfo.SchemaElement != null) ? schemaInfo.SchemaElement : schemaInfo.SchemaAttribute);
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000DEE RID: 3566 RVA: 0x0003E06C File Offset: 0x0003D06C
		public override XmlSchemaValidity Validity
		{
			get
			{
				if (!this.ownerDoc.CanReportValidity)
				{
					return XmlSchemaValidity.NotKnown;
				}
				return (XmlSchemaValidity)(this.flags & 3);
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000DEF RID: 3567 RVA: 0x0003E085 File Offset: 0x0003D085
		public override bool IsDefault
		{
			get
			{
				return (this.flags & 4) != 0;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000DF0 RID: 3568 RVA: 0x0003E095 File Offset: 0x0003D095
		public override bool IsNil
		{
			get
			{
				return (this.flags & 8) != 0;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x0003E0A5 File Offset: 0x0003D0A5
		public override XmlSchemaSimpleType MemberType
		{
			get
			{
				return this.memberType;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x0003E0AD File Offset: 0x0003D0AD
		public override XmlSchemaType SchemaType
		{
			get
			{
				return this.schemaType;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x0003E0B5 File Offset: 0x0003D0B5
		public override XmlSchemaElement SchemaElement
		{
			get
			{
				return this.decl as XmlSchemaElement;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000DF4 RID: 3572 RVA: 0x0003E0C2 File Offset: 0x0003D0C2
		public override XmlSchemaAttribute SchemaAttribute
		{
			get
			{
				return this.decl as XmlSchemaAttribute;
			}
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0003E0CF File Offset: 0x0003D0CF
		public void SetValidity(XmlSchemaValidity value)
		{
			this.flags = (byte)(((int)this.flags & -4) | (int)((byte)value));
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0003E0E4 File Offset: 0x0003D0E4
		public void SetIsDefault(bool value)
		{
			if (value)
			{
				this.flags |= 4;
				return;
			}
			this.flags = (byte)((int)this.flags & -5);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0003E109 File Offset: 0x0003D109
		public void SetIsNil(bool value)
		{
			if (value)
			{
				this.flags |= 8;
				return;
			}
			this.flags = (byte)((int)this.flags & -9);
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0003E130 File Offset: 0x0003D130
		public override bool Equals(IXmlSchemaInfo schemaInfo)
		{
			return schemaInfo != null && schemaInfo.Validity == (XmlSchemaValidity)(this.flags & 3) && schemaInfo.IsDefault == ((this.flags & 4) != 0) && schemaInfo.IsNil == ((this.flags & 8) != 0) && schemaInfo.MemberType == this.memberType && schemaInfo.SchemaType == this.schemaType && schemaInfo.SchemaElement == this.decl as XmlSchemaElement && schemaInfo.SchemaAttribute == this.decl as XmlSchemaAttribute;
		}

		// Token: 0x04000973 RID: 2419
		private const byte ValidityMask = 3;

		// Token: 0x04000974 RID: 2420
		private const byte IsDefaultBit = 4;

		// Token: 0x04000975 RID: 2421
		private const byte IsNilBit = 8;

		// Token: 0x04000976 RID: 2422
		private byte flags;

		// Token: 0x04000977 RID: 2423
		private XmlSchemaSimpleType memberType;

		// Token: 0x04000978 RID: 2424
		private XmlSchemaType schemaType;

		// Token: 0x04000979 RID: 2425
		private object decl;
	}
}
