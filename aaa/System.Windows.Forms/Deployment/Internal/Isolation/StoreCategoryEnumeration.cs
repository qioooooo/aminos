using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000DB RID: 219
	internal class StoreCategoryEnumeration : IEnumerator
	{
		// Token: 0x0600034A RID: 842 RVA: 0x00007BF9 File Offset: 0x00006BF9
		public StoreCategoryEnumeration(IEnumSTORE_CATEGORY pI)
		{
			this._enum = pI;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00007C08 File Offset: 0x00006C08
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00007C0B File Offset: 0x00006C0B
		private STORE_CATEGORY GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600034D RID: 845 RVA: 0x00007C21 File Offset: 0x00006C21
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00007C2E File Offset: 0x00006C2E
		public STORE_CATEGORY Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00007C38 File Offset: 0x00006C38
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

		// Token: 0x06000350 RID: 848 RVA: 0x00007C7D File Offset: 0x00006C7D
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000D85 RID: 3461
		private IEnumSTORE_CATEGORY _enum;

		// Token: 0x04000D86 RID: 3462
		private bool _fValid;

		// Token: 0x04000D87 RID: 3463
		private STORE_CATEGORY _current;
	}
}
