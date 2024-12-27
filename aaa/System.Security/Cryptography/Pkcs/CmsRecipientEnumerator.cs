using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x02000061 RID: 97
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class CmsRecipientEnumerator : IEnumerator
	{
		// Token: 0x060000F7 RID: 247 RVA: 0x0000646E File Offset: 0x0000546E
		private CmsRecipientEnumerator()
		{
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00006476 File Offset: 0x00005476
		internal CmsRecipientEnumerator(CmsRecipientCollection recipients)
		{
			this.m_recipients = recipients;
			this.m_current = -1;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x0000648C File Offset: 0x0000548C
		public CmsRecipient Current
		{
			get
			{
				return this.m_recipients[this.m_current];
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000649F File Offset: 0x0000549F
		object IEnumerator.Current
		{
			get
			{
				return this.m_recipients[this.m_current];
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000064B2 File Offset: 0x000054B2
		public bool MoveNext()
		{
			if (this.m_current == this.m_recipients.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000064DA File Offset: 0x000054DA
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x04000455 RID: 1109
		private CmsRecipientCollection m_recipients;

		// Token: 0x04000456 RID: 1110
		private int m_current;
	}
}
