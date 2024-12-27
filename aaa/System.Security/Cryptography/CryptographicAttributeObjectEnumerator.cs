using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	// Token: 0x02000057 RID: 87
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class CryptographicAttributeObjectEnumerator : IEnumerator
	{
		// Token: 0x060000BA RID: 186 RVA: 0x00003B7A File Offset: 0x00002B7A
		private CryptographicAttributeObjectEnumerator()
		{
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003B82 File Offset: 0x00002B82
		internal CryptographicAttributeObjectEnumerator(CryptographicAttributeObjectCollection attributes)
		{
			this.m_attributes = attributes;
			this.m_current = -1;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00003B98 File Offset: 0x00002B98
		public CryptographicAttributeObject Current
		{
			get
			{
				return this.m_attributes[this.m_current];
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00003BAB File Offset: 0x00002BAB
		object IEnumerator.Current
		{
			get
			{
				return this.m_attributes[this.m_current];
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003BBE File Offset: 0x00002BBE
		public bool MoveNext()
		{
			if (this.m_current == this.m_attributes.Count - 1)
			{
				return false;
			}
			this.m_current++;
			return true;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003BE6 File Offset: 0x00002BE6
		public void Reset()
		{
			this.m_current = -1;
		}

		// Token: 0x04000430 RID: 1072
		private CryptographicAttributeObjectCollection m_attributes;

		// Token: 0x04000431 RID: 1073
		private int m_current;
	}
}
