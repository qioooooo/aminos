using System;

namespace System.Xml.Serialization
{
	// Token: 0x02000319 RID: 793
	public class XmlReflectionMember
	{
		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x0600258B RID: 9611 RVA: 0x000B354A File Offset: 0x000B254A
		// (set) Token: 0x0600258C RID: 9612 RVA: 0x000B3552 File Offset: 0x000B2552
		public Type MemberType
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x0600258D RID: 9613 RVA: 0x000B355B File Offset: 0x000B255B
		// (set) Token: 0x0600258E RID: 9614 RVA: 0x000B3563 File Offset: 0x000B2563
		public XmlAttributes XmlAttributes
		{
			get
			{
				return this.xmlAttributes;
			}
			set
			{
				this.xmlAttributes = value;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x0600258F RID: 9615 RVA: 0x000B356C File Offset: 0x000B256C
		// (set) Token: 0x06002590 RID: 9616 RVA: 0x000B3574 File Offset: 0x000B2574
		public SoapAttributes SoapAttributes
		{
			get
			{
				return this.soapAttributes;
			}
			set
			{
				this.soapAttributes = value;
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06002591 RID: 9617 RVA: 0x000B357D File Offset: 0x000B257D
		// (set) Token: 0x06002592 RID: 9618 RVA: 0x000B3593 File Offset: 0x000B2593
		public string MemberName
		{
			get
			{
				if (this.memberName != null)
				{
					return this.memberName;
				}
				return string.Empty;
			}
			set
			{
				this.memberName = value;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06002593 RID: 9619 RVA: 0x000B359C File Offset: 0x000B259C
		// (set) Token: 0x06002594 RID: 9620 RVA: 0x000B35A4 File Offset: 0x000B25A4
		public bool IsReturnValue
		{
			get
			{
				return this.isReturnValue;
			}
			set
			{
				this.isReturnValue = value;
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06002595 RID: 9621 RVA: 0x000B35AD File Offset: 0x000B25AD
		// (set) Token: 0x06002596 RID: 9622 RVA: 0x000B35B5 File Offset: 0x000B25B5
		public bool OverrideIsNullable
		{
			get
			{
				return this.overrideIsNullable;
			}
			set
			{
				this.overrideIsNullable = value;
			}
		}

		// Token: 0x040015A8 RID: 5544
		private string memberName;

		// Token: 0x040015A9 RID: 5545
		private Type type;

		// Token: 0x040015AA RID: 5546
		private XmlAttributes xmlAttributes = new XmlAttributes();

		// Token: 0x040015AB RID: 5547
		private SoapAttributes soapAttributes = new SoapAttributes();

		// Token: 0x040015AC RID: 5548
		private bool isReturnValue;

		// Token: 0x040015AD RID: 5549
		private bool overrideIsNullable;
	}
}
