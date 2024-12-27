using System;
using System.Collections;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000335 RID: 821
	public sealed class X509ChainElementEnumerator : IEnumerator
	{
		// Token: 0x060019F2 RID: 6642 RVA: 0x0005A6A2 File Offset: 0x000596A2
		private X509ChainElementEnumerator()
		{
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x0005A6AA File Offset: 0x000596AA
		internal X509ChainElementEnumerator(X509ChainElementCollection chainElements)
		{
			this.m_chainElements = chainElements;
			this.m_current = -1;
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060019F4 RID: 6644 RVA: 0x0005A6C0 File Offset: 0x000596C0
		public X509ChainElement Current
		{
			get
			{
				return this.m_chainElements[this.m_current];
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060019F5 RID: 6645 RVA: 0x0005A6D3 File Offset: 0x000596D3
		object IEnumerator.Current
		{
			get
			{
				return this.m_chainElements[this.m_current];
			}
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x0005A6E6 File Offset: 0x000596E6
		public bool MoveNext()
		{
			if (this.m_current == this.m_chainElements.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x0005A70E File Offset: 0x0005970E
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x04001AE6 RID: 6886
		private X509ChainElementCollection m_chainElements;

		// Token: 0x04001AE7 RID: 6887
		private int m_current;
	}
}
