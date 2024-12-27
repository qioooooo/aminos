using System;
using System.Collections;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200032E RID: 814
	public sealed class X509Certificate2Enumerator : IEnumerator
	{
		// Token: 0x060019C6 RID: 6598 RVA: 0x000597C6 File Offset: 0x000587C6
		private X509Certificate2Enumerator()
		{
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x000597CE File Offset: 0x000587CE
		internal X509Certificate2Enumerator(X509Certificate2Collection mappings)
		{
			this.baseEnumerator = ((IEnumerable)mappings).GetEnumerator();
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060019C8 RID: 6600 RVA: 0x000597E2 File Offset: 0x000587E2
		public X509Certificate2 Current
		{
			get
			{
				return (X509Certificate2)this.baseEnumerator.Current;
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060019C9 RID: 6601 RVA: 0x000597F4 File Offset: 0x000587F4
		object IEnumerator.Current
		{
			get
			{
				return this.baseEnumerator.Current;
			}
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x00059801 File Offset: 0x00058801
		public bool MoveNext()
		{
			return this.baseEnumerator.MoveNext();
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x0005980E File Offset: 0x0005880E
		bool IEnumerator.MoveNext()
		{
			return this.baseEnumerator.MoveNext();
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x0005981B File Offset: 0x0005881B
		public void Reset()
		{
			this.baseEnumerator.Reset();
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x00059828 File Offset: 0x00058828
		void IEnumerator.Reset()
		{
			this.baseEnumerator.Reset();
		}

		// Token: 0x04001ABF RID: 6847
		private IEnumerator baseEnumerator;
	}
}
