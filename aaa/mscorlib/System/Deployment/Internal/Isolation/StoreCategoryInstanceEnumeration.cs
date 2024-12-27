using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001EA RID: 490
	internal class StoreCategoryInstanceEnumeration : IEnumerator
	{
		// Token: 0x0600150B RID: 5387 RVA: 0x00036DB5 File Offset: 0x00035DB5
		public StoreCategoryInstanceEnumeration(IEnumSTORE_CATEGORY_INSTANCE pI)
		{
			this._enum = pI;
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x00036DC4 File Offset: 0x00035DC4
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x00036DC7 File Offset: 0x00035DC7
		private STORE_CATEGORY_INSTANCE GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x0600150E RID: 5390 RVA: 0x00036DDD File Offset: 0x00035DDD
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x0600150F RID: 5391 RVA: 0x00036DEA File Offset: 0x00035DEA
		public STORE_CATEGORY_INSTANCE Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x00036DF4 File Offset: 0x00035DF4
		public bool MoveNext()
		{
			STORE_CATEGORY_INSTANCE[] array = new STORE_CATEGORY_INSTANCE[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = num == 1U;
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x00036E39 File Offset: 0x00035E39
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000821 RID: 2081
		private IEnumSTORE_CATEGORY_INSTANCE _enum;

		// Token: 0x04000822 RID: 2082
		private bool _fValid;

		// Token: 0x04000823 RID: 2083
		private STORE_CATEGORY_INSTANCE _current;
	}
}
