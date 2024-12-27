using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000108 RID: 264
	internal class StoreSubcategoryEnumeration : IEnumerator
	{
		// Token: 0x0600063F RID: 1599 RVA: 0x0001F349 File Offset: 0x0001E349
		public StoreSubcategoryEnumeration(IEnumSTORE_CATEGORY_SUBCATEGORY pI)
		{
			this._enum = pI;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0001F358 File Offset: 0x0001E358
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0001F35B File Offset: 0x0001E35B
		private STORE_CATEGORY_SUBCATEGORY GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0001F371 File Offset: 0x0001E371
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x0001F37E File Offset: 0x0001E37E
		public STORE_CATEGORY_SUBCATEGORY Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0001F388 File Offset: 0x0001E388
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

		// Token: 0x06000645 RID: 1605 RVA: 0x0001F3CD File Offset: 0x0001E3CD
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x040004FC RID: 1276
		private IEnumSTORE_CATEGORY_SUBCATEGORY _enum;

		// Token: 0x040004FD RID: 1277
		private bool _fValid;

		// Token: 0x040004FE RID: 1278
		private STORE_CATEGORY_SUBCATEGORY _current;
	}
}
