using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000D7 RID: 215
	internal class StoreAssemblyEnumeration : IEnumerator
	{
		// Token: 0x06000334 RID: 820 RVA: 0x00007AC9 File Offset: 0x00006AC9
		public StoreAssemblyEnumeration(IEnumSTORE_ASSEMBLY pI)
		{
			this._enum = pI;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00007AD8 File Offset: 0x00006AD8
		private STORE_ASSEMBLY GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00007AEE File Offset: 0x00006AEE
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000337 RID: 823 RVA: 0x00007AFB File Offset: 0x00006AFB
		public STORE_ASSEMBLY Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00007B03 File Offset: 0x00006B03
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00007B08 File Offset: 0x00006B08
		public bool MoveNext()
		{
			STORE_ASSEMBLY[] array = new STORE_ASSEMBLY[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = num == 1U;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00007B4D File Offset: 0x00006B4D
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000D7F RID: 3455
		private IEnumSTORE_ASSEMBLY _enum;

		// Token: 0x04000D80 RID: 3456
		private bool _fValid;

		// Token: 0x04000D81 RID: 3457
		private STORE_ASSEMBLY _current;
	}
}
