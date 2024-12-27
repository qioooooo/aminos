using System;
using System.Collections;

namespace System.Security.Cryptography
{
	// Token: 0x02000321 RID: 801
	public sealed class OidEnumerator : IEnumerator
	{
		// Token: 0x0600192B RID: 6443 RVA: 0x00055AA2 File Offset: 0x00054AA2
		private OidEnumerator()
		{
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x00055AAA File Offset: 0x00054AAA
		internal OidEnumerator(OidCollection oids)
		{
			this.m_oids = oids;
			this.m_current = -1;
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x0600192D RID: 6445 RVA: 0x00055AC0 File Offset: 0x00054AC0
		public Oid Current
		{
			get
			{
				return this.m_oids[this.m_current];
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x0600192E RID: 6446 RVA: 0x00055AD3 File Offset: 0x00054AD3
		object IEnumerator.Current
		{
			get
			{
				return this.m_oids[this.m_current];
			}
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x00055AE6 File Offset: 0x00054AE6
		public bool MoveNext()
		{
			if (this.m_current == this.m_oids.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x00055B0E File Offset: 0x00054B0E
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x04001A7D RID: 6781
		private OidCollection m_oids;

		// Token: 0x04001A7E RID: 6782
		private int m_current;
	}
}
