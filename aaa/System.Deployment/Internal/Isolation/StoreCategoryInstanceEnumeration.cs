using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200010A RID: 266
	internal class StoreCategoryInstanceEnumeration : IEnumerator
	{
		// Token: 0x0600064A RID: 1610 RVA: 0x0001F3E1 File Offset: 0x0001E3E1
		public StoreCategoryInstanceEnumeration(IEnumSTORE_CATEGORY_INSTANCE pI)
		{
			this._enum = pI;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001F3F0 File Offset: 0x0001E3F0
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0001F3F3 File Offset: 0x0001E3F3
		private STORE_CATEGORY_INSTANCE GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x0001F409 File Offset: 0x0001E409
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x0001F416 File Offset: 0x0001E416
		public STORE_CATEGORY_INSTANCE Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001F420 File Offset: 0x0001E420
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

		// Token: 0x06000650 RID: 1616 RVA: 0x0001F465 File Offset: 0x0001E465
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x040004FF RID: 1279
		private IEnumSTORE_CATEGORY_INSTANCE _enum;

		// Token: 0x04000500 RID: 1280
		private bool _fValid;

		// Token: 0x04000501 RID: 1281
		private STORE_CATEGORY_INSTANCE _current;
	}
}
