using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001E2 RID: 482
	internal class StoreAssemblyEnumeration : IEnumerator
	{
		// Token: 0x060014DF RID: 5343 RVA: 0x00036B55 File Offset: 0x00035B55
		public StoreAssemblyEnumeration(IEnumSTORE_ASSEMBLY pI)
		{
			this._enum = pI;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00036B64 File Offset: 0x00035B64
		private STORE_ASSEMBLY GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x060014E1 RID: 5345 RVA: 0x00036B7A File Offset: 0x00035B7A
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x060014E2 RID: 5346 RVA: 0x00036B87 File Offset: 0x00035B87
		public STORE_ASSEMBLY Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x00036B8F File Offset: 0x00035B8F
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x00036B94 File Offset: 0x00035B94
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

		// Token: 0x060014E5 RID: 5349 RVA: 0x00036BD9 File Offset: 0x00035BD9
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000815 RID: 2069
		private IEnumSTORE_ASSEMBLY _enum;

		// Token: 0x04000816 RID: 2070
		private bool _fValid;

		// Token: 0x04000817 RID: 2071
		private STORE_ASSEMBLY _current;
	}
}
