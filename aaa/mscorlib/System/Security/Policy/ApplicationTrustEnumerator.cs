using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x02000486 RID: 1158
	[ComVisible(true)]
	public sealed class ApplicationTrustEnumerator : IEnumerator
	{
		// Token: 0x06002E7A RID: 11898 RVA: 0x0009DD91 File Offset: 0x0009CD91
		private ApplicationTrustEnumerator()
		{
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x0009DD99 File Offset: 0x0009CD99
		internal ApplicationTrustEnumerator(ApplicationTrustCollection trusts)
		{
			this.m_trusts = trusts;
			this.m_current = -1;
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06002E7C RID: 11900 RVA: 0x0009DDAF File Offset: 0x0009CDAF
		public ApplicationTrust Current
		{
			get
			{
				return this.m_trusts[this.m_current];
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06002E7D RID: 11901 RVA: 0x0009DDC2 File Offset: 0x0009CDC2
		object IEnumerator.Current
		{
			get
			{
				return this.m_trusts[this.m_current];
			}
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x0009DDD5 File Offset: 0x0009CDD5
		public bool MoveNext()
		{
			if (this.m_current == this.m_trusts.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x0009DDFD File Offset: 0x0009CDFD
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x04001798 RID: 6040
		private ApplicationTrustCollection m_trusts;

		// Token: 0x04001799 RID: 6041
		private int m_current;
	}
}
