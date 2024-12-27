using System;

namespace System.Xml.Schema
{
	// Token: 0x02000217 RID: 535
	internal sealed class SchemaNotation
	{
		// Token: 0x060019B1 RID: 6577 RVA: 0x0007BBA0 File Offset: 0x0007ABA0
		internal SchemaNotation(XmlQualifiedName name)
		{
			this.name = name;
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x060019B2 RID: 6578 RVA: 0x0007BBAF File Offset: 0x0007ABAF
		internal XmlQualifiedName Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x060019B3 RID: 6579 RVA: 0x0007BBB7 File Offset: 0x0007ABB7
		// (set) Token: 0x060019B4 RID: 6580 RVA: 0x0007BBBF File Offset: 0x0007ABBF
		internal string SystemLiteral
		{
			get
			{
				return this.systemLiteral;
			}
			set
			{
				this.systemLiteral = value;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x060019B5 RID: 6581 RVA: 0x0007BBC8 File Offset: 0x0007ABC8
		// (set) Token: 0x060019B6 RID: 6582 RVA: 0x0007BBD0 File Offset: 0x0007ABD0
		internal string Pubid
		{
			get
			{
				return this.pubid;
			}
			set
			{
				this.pubid = value;
			}
		}

		// Token: 0x04001004 RID: 4100
		internal const int SYSTEM = 0;

		// Token: 0x04001005 RID: 4101
		internal const int PUBLIC = 1;

		// Token: 0x04001006 RID: 4102
		private XmlQualifiedName name;

		// Token: 0x04001007 RID: 4103
		private string systemLiteral;

		// Token: 0x04001008 RID: 4104
		private string pubid;
	}
}
