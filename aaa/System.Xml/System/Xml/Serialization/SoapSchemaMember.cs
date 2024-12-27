using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002F2 RID: 754
	public class SoapSchemaMember
	{
		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x0600235C RID: 9052 RVA: 0x000A8139 File Offset: 0x000A7139
		// (set) Token: 0x0600235D RID: 9053 RVA: 0x000A8141 File Offset: 0x000A7141
		public XmlQualifiedName MemberType
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

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x0600235E RID: 9054 RVA: 0x000A814A File Offset: 0x000A714A
		// (set) Token: 0x0600235F RID: 9055 RVA: 0x000A8160 File Offset: 0x000A7160
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

		// Token: 0x040014F3 RID: 5363
		private string memberName;

		// Token: 0x040014F4 RID: 5364
		private XmlQualifiedName type = XmlQualifiedName.Empty;
	}
}
