using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001E4 RID: 484
	internal class StoreAssemblyFileEnumeration : IEnumerator
	{
		// Token: 0x060014EA RID: 5354 RVA: 0x00036BED File Offset: 0x00035BED
		public StoreAssemblyFileEnumeration(IEnumSTORE_ASSEMBLY_FILE pI)
		{
			this._enum = pI;
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x00036BFC File Offset: 0x00035BFC
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x00036BFF File Offset: 0x00035BFF
		private STORE_ASSEMBLY_FILE GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x060014ED RID: 5357 RVA: 0x00036C15 File Offset: 0x00035C15
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x060014EE RID: 5358 RVA: 0x00036C22 File Offset: 0x00035C22
		public STORE_ASSEMBLY_FILE Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x00036C2C File Offset: 0x00035C2C
		public bool MoveNext()
		{
			STORE_ASSEMBLY_FILE[] array = new STORE_ASSEMBLY_FILE[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = num == 1U;
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x00036C71 File Offset: 0x00035C71
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000818 RID: 2072
		private IEnumSTORE_ASSEMBLY_FILE _enum;

		// Token: 0x04000819 RID: 2073
		private bool _fValid;

		// Token: 0x0400081A RID: 2074
		private STORE_ASSEMBLY_FILE _current;
	}
}
