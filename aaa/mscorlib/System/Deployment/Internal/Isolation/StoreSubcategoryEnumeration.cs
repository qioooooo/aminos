using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001E8 RID: 488
	internal class StoreSubcategoryEnumeration : IEnumerator
	{
		// Token: 0x06001500 RID: 5376 RVA: 0x00036D1D File Offset: 0x00035D1D
		public StoreSubcategoryEnumeration(IEnumSTORE_CATEGORY_SUBCATEGORY pI)
		{
			this._enum = pI;
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x00036D2C File Offset: 0x00035D2C
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x00036D2F File Offset: 0x00035D2F
		private STORE_CATEGORY_SUBCATEGORY GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001503 RID: 5379 RVA: 0x00036D45 File Offset: 0x00035D45
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06001504 RID: 5380 RVA: 0x00036D52 File Offset: 0x00035D52
		public STORE_CATEGORY_SUBCATEGORY Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x00036D5C File Offset: 0x00035D5C
		public bool MoveNext()
		{
			STORE_CATEGORY_SUBCATEGORY[] array = new STORE_CATEGORY_SUBCATEGORY[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = num == 1U;
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x00036DA1 File Offset: 0x00035DA1
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x0400081E RID: 2078
		private IEnumSTORE_CATEGORY_SUBCATEGORY _enum;

		// Token: 0x0400081F RID: 2079
		private bool _fValid;

		// Token: 0x04000820 RID: 2080
		private STORE_CATEGORY_SUBCATEGORY _current;
	}
}
