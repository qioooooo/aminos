using System;
using System.Collections;

namespace System.Security.Policy
{
	// Token: 0x0200048B RID: 1163
	internal sealed class EvidenceEnumerator : IEnumerator
	{
		// Token: 0x06002EC3 RID: 11971 RVA: 0x0009F45A File Offset: 0x0009E45A
		public EvidenceEnumerator(Evidence evidence)
		{
			this.m_evidence = evidence;
			this.Reset();
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x0009F470 File Offset: 0x0009E470
		public bool MoveNext()
		{
			if (this.m_enumerator == null)
			{
				return false;
			}
			if (this.m_enumerator.MoveNext())
			{
				return true;
			}
			if (this.m_first)
			{
				this.m_enumerator = this.m_evidence.GetAssemblyEnumerator();
				this.m_first = false;
				return this.m_enumerator != null && this.m_enumerator.MoveNext();
			}
			return false;
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06002EC5 RID: 11973 RVA: 0x0009F4CD File Offset: 0x0009E4CD
		public object Current
		{
			get
			{
				if (this.m_enumerator == null)
				{
					return null;
				}
				return this.m_enumerator.Current;
			}
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x0009F4E4 File Offset: 0x0009E4E4
		public void Reset()
		{
			this.m_first = true;
			if (this.m_evidence != null)
			{
				this.m_enumerator = this.m_evidence.GetHostEnumerator();
				return;
			}
			this.m_enumerator = null;
		}

		// Token: 0x040017A7 RID: 6055
		private bool m_first;

		// Token: 0x040017A8 RID: 6056
		private Evidence m_evidence;

		// Token: 0x040017A9 RID: 6057
		private IEnumerator m_enumerator;
	}
}
