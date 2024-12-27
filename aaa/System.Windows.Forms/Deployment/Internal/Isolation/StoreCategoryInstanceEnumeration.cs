using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000DF RID: 223
	internal class StoreCategoryInstanceEnumeration : IEnumerator
	{
		// Token: 0x06000360 RID: 864 RVA: 0x00007D29 File Offset: 0x00006D29
		public StoreCategoryInstanceEnumeration(IEnumSTORE_CATEGORY_INSTANCE pI)
		{
			this._enum = pI;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00007D38 File Offset: 0x00006D38
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00007D3B File Offset: 0x00006D3B
		private STORE_CATEGORY_INSTANCE GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00007D51 File Offset: 0x00006D51
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00007D5E File Offset: 0x00006D5E
		public STORE_CATEGORY_INSTANCE Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00007D68 File Offset: 0x00006D68
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

		// Token: 0x06000366 RID: 870 RVA: 0x00007DAD File Offset: 0x00006DAD
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000D8B RID: 3467
		private IEnumSTORE_CATEGORY_INSTANCE _enum;

		// Token: 0x04000D8C RID: 3468
		private bool _fValid;

		// Token: 0x04000D8D RID: 3469
		private STORE_CATEGORY_INSTANCE _current;
	}
}
