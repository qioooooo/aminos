using System;
using System.Collections;

namespace System.EnterpriseServices
{
	// Token: 0x0200004D RID: 77
	internal class SecurityIdentityEnumerator : IEnumerator
	{
		// Token: 0x0600018E RID: 398 RVA: 0x0000620E File Offset: 0x0000520E
		internal SecurityIdentityEnumerator(IEnumerator E, SecurityCallers c)
		{
			this._E = E;
			this._callers = c;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00006224 File Offset: 0x00005224
		public bool MoveNext()
		{
			return this._E.MoveNext();
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00006231 File Offset: 0x00005231
		public void Reset()
		{
			this._E.Reset();
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00006240 File Offset: 0x00005240
		public object Current
		{
			get
			{
				object obj = this._E.Current;
				return this._callers[(int)obj];
			}
		}

		// Token: 0x04000099 RID: 153
		private IEnumerator _E;

		// Token: 0x0400009A RID: 154
		private SecurityCallers _callers;
	}
}
