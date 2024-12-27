using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000FE RID: 254
	internal class StoreDeploymentMetadataEnumeration : IEnumerator
	{
		// Token: 0x06000608 RID: 1544 RVA: 0x0001F05E File Offset: 0x0001E05E
		public StoreDeploymentMetadataEnumeration(IEnumSTORE_DEPLOYMENT_METADATA pI)
		{
			this._enum = pI;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001F06D File Offset: 0x0001E06D
		private IDefinitionAppId GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x0001F083 File Offset: 0x0001E083
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x0001F08B File Offset: 0x0001E08B
		public IDefinitionAppId Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001F093 File Offset: 0x0001E093
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001F098 File Offset: 0x0001E098
		public bool MoveNext()
		{
			IDefinitionAppId[] array = new IDefinitionAppId[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = num == 1U;
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0001F0D4 File Offset: 0x0001E0D4
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x040004ED RID: 1261
		private IEnumSTORE_DEPLOYMENT_METADATA _enum;

		// Token: 0x040004EE RID: 1262
		private bool _fValid;

		// Token: 0x040004EF RID: 1263
		private IDefinitionAppId _current;
	}
}
