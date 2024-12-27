using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x0200007D RID: 125
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class SignerInfoEnumerator : IEnumerator
	{
		// Token: 0x060001EF RID: 495 RVA: 0x0000B56E File Offset: 0x0000A56E
		private SignerInfoEnumerator()
		{
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000B576 File Offset: 0x0000A576
		internal SignerInfoEnumerator(SignerInfoCollection signerInfos)
		{
			this.m_signerInfos = signerInfos;
			this.m_current = -1;
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000B58C File Offset: 0x0000A58C
		public SignerInfo Current
		{
			get
			{
				return this.m_signerInfos[this.m_current];
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000B59F File Offset: 0x0000A59F
		object IEnumerator.Current
		{
			get
			{
				return this.m_signerInfos[this.m_current];
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000B5B2 File Offset: 0x0000A5B2
		public bool MoveNext()
		{
			if (this.m_current == this.m_signerInfos.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000B5DA File Offset: 0x0000A5DA
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x040004B9 RID: 1209
		private SignerInfoCollection m_signerInfos;

		// Token: 0x040004BA RID: 1210
		private int m_current;
	}
}
