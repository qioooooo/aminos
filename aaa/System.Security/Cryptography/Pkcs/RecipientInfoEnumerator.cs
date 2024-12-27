using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x0200006F RID: 111
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class RecipientInfoEnumerator : IEnumerator
	{
		// Token: 0x0600015E RID: 350 RVA: 0x00007536 File Offset: 0x00006536
		private RecipientInfoEnumerator()
		{
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000753E File Offset: 0x0000653E
		internal RecipientInfoEnumerator(RecipientInfoCollection RecipientInfos)
		{
			this.m_recipientInfos = RecipientInfos;
			this.m_current = -1;
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00007554 File Offset: 0x00006554
		public RecipientInfo Current
		{
			get
			{
				return this.m_recipientInfos[this.m_current];
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00007567 File Offset: 0x00006567
		object IEnumerator.Current
		{
			get
			{
				return this.m_recipientInfos[this.m_current];
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000757A File Offset: 0x0000657A
		public bool MoveNext()
		{
			if (this.m_current == this.m_recipientInfos.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000075A2 File Offset: 0x000065A2
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x04000489 RID: 1161
		private RecipientInfoCollection m_recipientInfos;

		// Token: 0x0400048A RID: 1162
		private int m_current;
	}
}
