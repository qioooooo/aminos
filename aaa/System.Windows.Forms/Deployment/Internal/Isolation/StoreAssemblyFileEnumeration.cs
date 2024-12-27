using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000D9 RID: 217
	internal class StoreAssemblyFileEnumeration : IEnumerator
	{
		// Token: 0x0600033F RID: 831 RVA: 0x00007B61 File Offset: 0x00006B61
		public StoreAssemblyFileEnumeration(IEnumSTORE_ASSEMBLY_FILE pI)
		{
			this._enum = pI;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00007B70 File Offset: 0x00006B70
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00007B73 File Offset: 0x00006B73
		private STORE_ASSEMBLY_FILE GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000342 RID: 834 RVA: 0x00007B89 File Offset: 0x00006B89
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00007B96 File Offset: 0x00006B96
		public STORE_ASSEMBLY_FILE Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00007BA0 File Offset: 0x00006BA0
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

		// Token: 0x06000345 RID: 837 RVA: 0x00007BE5 File Offset: 0x00006BE5
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000D82 RID: 3458
		private IEnumSTORE_ASSEMBLY_FILE _enum;

		// Token: 0x04000D83 RID: 3459
		private bool _fValid;

		// Token: 0x04000D84 RID: 3460
		private STORE_ASSEMBLY_FILE _current;
	}
}
