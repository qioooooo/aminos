using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001E6 RID: 486
	internal class StoreCategoryEnumeration : IEnumerator
	{
		// Token: 0x060014F5 RID: 5365 RVA: 0x00036C85 File Offset: 0x00035C85
		public StoreCategoryEnumeration(IEnumSTORE_CATEGORY pI)
		{
			this._enum = pI;
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x00036C94 File Offset: 0x00035C94
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00036C97 File Offset: 0x00035C97
		private STORE_CATEGORY GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x060014F8 RID: 5368 RVA: 0x00036CAD File Offset: 0x00035CAD
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x060014F9 RID: 5369 RVA: 0x00036CBA File Offset: 0x00035CBA
		public STORE_CATEGORY Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00036CC4 File Offset: 0x00035CC4
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

		// Token: 0x060014FB RID: 5371 RVA: 0x00036D09 File Offset: 0x00035D09
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x0400081B RID: 2075
		private IEnumSTORE_CATEGORY _enum;

		// Token: 0x0400081C RID: 2076
		private bool _fValid;

		// Token: 0x0400081D RID: 2077
		private STORE_CATEGORY _current;
	}
}
