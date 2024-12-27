using System;
using System.Collections;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000342 RID: 834
	public sealed class X509ExtensionEnumerator : IEnumerator
	{
		// Token: 0x06001A40 RID: 6720 RVA: 0x0005B76E File Offset: 0x0005A76E
		private X509ExtensionEnumerator()
		{
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x0005B776 File Offset: 0x0005A776
		internal X509ExtensionEnumerator(X509ExtensionCollection extensions)
		{
			this.m_extensions = extensions;
			this.m_current = -1;
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001A42 RID: 6722 RVA: 0x0005B78C File Offset: 0x0005A78C
		public X509Extension Current
		{
			get
			{
				return this.m_extensions[this.m_current];
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001A43 RID: 6723 RVA: 0x0005B79F File Offset: 0x0005A79F
		object IEnumerator.Current
		{
			get
			{
				return this.m_extensions[this.m_current];
			}
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x0005B7B2 File Offset: 0x0005A7B2
		public bool MoveNext()
		{
			if (this.m_current == this.m_extensions.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x0005B7DA File Offset: 0x0005A7DA
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x04001B22 RID: 6946
		private X509ExtensionCollection m_extensions;

		// Token: 0x04001B23 RID: 6947
		private int m_current;
	}
}
