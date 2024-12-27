using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000104 RID: 260
	internal class StoreAssemblyFileEnumeration : IEnumerator
	{
		// Token: 0x06000629 RID: 1577 RVA: 0x0001F219 File Offset: 0x0001E219
		public StoreAssemblyFileEnumeration(IEnumSTORE_ASSEMBLY_FILE pI)
		{
			this._enum = pI;
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0001F228 File Offset: 0x0001E228
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001F22B File Offset: 0x0001E22B
		private STORE_ASSEMBLY_FILE GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x0001F241 File Offset: 0x0001E241
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x0001F24E File Offset: 0x0001E24E
		public STORE_ASSEMBLY_FILE Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001F258 File Offset: 0x0001E258
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

		// Token: 0x0600062F RID: 1583 RVA: 0x0001F29D File Offset: 0x0001E29D
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x040004F6 RID: 1270
		private IEnumSTORE_ASSEMBLY_FILE _enum;

		// Token: 0x040004F7 RID: 1271
		private bool _fValid;

		// Token: 0x040004F8 RID: 1272
		private STORE_ASSEMBLY_FILE _current;
	}
}
