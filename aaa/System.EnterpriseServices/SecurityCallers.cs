using System;
using System.Collections;

namespace System.EnterpriseServices
{
	// Token: 0x0200004E RID: 78
	public sealed class SecurityCallers : IEnumerable
	{
		// Token: 0x06000192 RID: 402 RVA: 0x0000626A File Offset: 0x0000526A
		private SecurityCallers()
		{
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006272 File Offset: 0x00005272
		internal SecurityCallers(ISecurityCallersColl ifc)
		{
			this._ex = ifc;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00006281 File Offset: 0x00005281
		public int Count
		{
			get
			{
				return this._ex.Count;
			}
		}

		// Token: 0x1700002C RID: 44
		public SecurityIdentity this[int idx]
		{
			get
			{
				return new SecurityIdentity(this._ex.GetItem(idx));
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000062A4 File Offset: 0x000052A4
		public IEnumerator GetEnumerator()
		{
			IEnumerator enumerator = null;
			this._ex.GetEnumerator(out enumerator);
			return new SecurityIdentityEnumerator(enumerator, this);
		}

		// Token: 0x0400009B RID: 155
		private ISecurityCallersColl _ex;
	}
}
