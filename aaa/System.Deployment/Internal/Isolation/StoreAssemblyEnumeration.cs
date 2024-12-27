using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000102 RID: 258
	internal class StoreAssemblyEnumeration : IEnumerator
	{
		// Token: 0x0600061E RID: 1566 RVA: 0x0001F181 File Offset: 0x0001E181
		public StoreAssemblyEnumeration(IEnumSTORE_ASSEMBLY pI)
		{
			this._enum = pI;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0001F190 File Offset: 0x0001E190
		private STORE_ASSEMBLY GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0001F1A6 File Offset: 0x0001E1A6
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x0001F1B3 File Offset: 0x0001E1B3
		public STORE_ASSEMBLY Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0001F1BB File Offset: 0x0001E1BB
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001F1C0 File Offset: 0x0001E1C0
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

		// Token: 0x06000624 RID: 1572 RVA: 0x0001F205 File Offset: 0x0001E205
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x040004F3 RID: 1267
		private IEnumSTORE_ASSEMBLY _enum;

		// Token: 0x040004F4 RID: 1268
		private bool _fValid;

		// Token: 0x040004F5 RID: 1269
		private STORE_ASSEMBLY _current;
	}
}
