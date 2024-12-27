using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000330 RID: 816
	public struct X509ChainStatus
	{
		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060019CE RID: 6606 RVA: 0x00059835 File Offset: 0x00058835
		// (set) Token: 0x060019CF RID: 6607 RVA: 0x0005983D File Offset: 0x0005883D
		public X509ChainStatusFlags Status
		{
			get
			{
				return this.m_status;
			}
			set
			{
				this.m_status = value;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060019D0 RID: 6608 RVA: 0x00059846 File Offset: 0x00058846
		// (set) Token: 0x060019D1 RID: 6609 RVA: 0x0005985C File Offset: 0x0005885C
		public string StatusInformation
		{
			get
			{
				if (this.m_statusInformation == null)
				{
					return string.Empty;
				}
				return this.m_statusInformation;
			}
			set
			{
				this.m_statusInformation = value;
			}
		}

		// Token: 0x04001AD8 RID: 6872
		private X509ChainStatusFlags m_status;

		// Token: 0x04001AD9 RID: 6873
		private string m_statusInformation;
	}
}
