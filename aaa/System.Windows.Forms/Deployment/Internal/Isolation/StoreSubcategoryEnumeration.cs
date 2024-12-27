using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000DD RID: 221
	internal class StoreSubcategoryEnumeration : IEnumerator
	{
		// Token: 0x06000355 RID: 853 RVA: 0x00007C91 File Offset: 0x00006C91
		public StoreSubcategoryEnumeration(IEnumSTORE_CATEGORY_SUBCATEGORY pI)
		{
			this._enum = pI;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00007CA0 File Offset: 0x00006CA0
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00007CA3 File Offset: 0x00006CA3
		private STORE_CATEGORY_SUBCATEGORY GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00007CB9 File Offset: 0x00006CB9
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000359 RID: 857 RVA: 0x00007CC6 File Offset: 0x00006CC6
		public STORE_CATEGORY_SUBCATEGORY Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00007CD0 File Offset: 0x00006CD0
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

		// Token: 0x0600035B RID: 859 RVA: 0x00007D15 File Offset: 0x00006D15
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000D88 RID: 3464
		private IEnumSTORE_CATEGORY_SUBCATEGORY _enum;

		// Token: 0x04000D89 RID: 3465
		private bool _fValid;

		// Token: 0x04000D8A RID: 3466
		private STORE_CATEGORY_SUBCATEGORY _current;
	}
}
