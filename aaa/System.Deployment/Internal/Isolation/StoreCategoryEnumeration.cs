using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000106 RID: 262
	internal class StoreCategoryEnumeration : IEnumerator
	{
		// Token: 0x06000634 RID: 1588 RVA: 0x0001F2B1 File Offset: 0x0001E2B1
		public StoreCategoryEnumeration(IEnumSTORE_CATEGORY pI)
		{
			this._enum = pI;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001F2C0 File Offset: 0x0001E2C0
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001F2C3 File Offset: 0x0001E2C3
		private STORE_CATEGORY GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x0001F2D9 File Offset: 0x0001E2D9
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x0001F2E6 File Offset: 0x0001E2E6
		public STORE_CATEGORY Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0001F2F0 File Offset: 0x0001E2F0
		public bool MoveNext()
		{
			STORE_CATEGORY[] array = new STORE_CATEGORY[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = num == 1U;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001F335 File Offset: 0x0001E335
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x040004F9 RID: 1273
		private IEnumSTORE_CATEGORY _enum;

		// Token: 0x040004FA RID: 1274
		private bool _fValid;

		// Token: 0x040004FB RID: 1275
		private STORE_CATEGORY _current;
	}
}
