using System;
using System.Xml.Schema;

namespace System.Xml.XPath
{
	// Token: 0x0200011B RID: 283
	internal class XPathNavigatorReaderWithSI : XPathNavigatorReader, IXmlSchemaInfo
	{
		// Token: 0x060010FB RID: 4347 RVA: 0x0004D28E File Offset: 0x0004C28E
		internal XPathNavigatorReaderWithSI(XPathNavigator navToRead, IXmlLineInfo xli, IXmlSchemaInfo xsi)
			: base(navToRead, xli, xsi)
		{
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x060010FC RID: 4348 RVA: 0x0004D299 File Offset: 0x0004C299
		public virtual XmlSchemaValidity Validity
		{
			get
			{
				if (!base.IsReading)
				{
					return XmlSchemaValidity.NotKnown;
				}
				return this.schemaInfo.Validity;
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x060010FD RID: 4349 RVA: 0x0004D2B0 File Offset: 0x0004C2B0
		public override bool IsDefault
		{
			get
			{
				return base.IsReading && this.schemaInfo.IsDefault;
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x060010FE RID: 4350 RVA: 0x0004D2C7 File Offset: 0x0004C2C7
		public virtual bool IsNil
		{
			get
			{
				return base.IsReading && this.schemaInfo.IsNil;
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060010FF RID: 4351 RVA: 0x0004D2DE File Offset: 0x0004C2DE
		public virtual XmlSchemaSimpleType MemberType
		{
			get
			{
				if (!base.IsReading)
				{
					return null;
				}
				return this.schemaInfo.MemberType;
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001100 RID: 4352 RVA: 0x0004D2F5 File Offset: 0x0004C2F5
		public virtual XmlSchemaType SchemaType
		{
			get
			{
				if (!base.IsReading)
				{
					return null;
				}
				return this.schemaInfo.SchemaType;
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001101 RID: 4353 RVA: 0x0004D30C File Offset: 0x0004C30C
		public virtual XmlSchemaElement SchemaElement
		{
			get
			{
				if (!base.IsReading)
				{
					return null;
				}
				return this.schemaInfo.SchemaElement;
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001102 RID: 4354 RVA: 0x0004D323 File Offset: 0x0004C323
		public virtual XmlSchemaAttribute SchemaAttribute
		{
			get
			{
				if (!base.IsReading)
				{
					return null;
				}
				return this.schemaInfo.SchemaAttribute;
			}
		}
	}
}
