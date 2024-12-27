using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000D3 RID: 211
	internal class StoreDeploymentMetadataEnumeration : IEnumerator
	{
		// Token: 0x0600031E RID: 798 RVA: 0x000079A6 File Offset: 0x000069A6
		public StoreDeploymentMetadataEnumeration(IEnumSTORE_DEPLOYMENT_METADATA pI)
		{
			this._enum = pI;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000079B5 File Offset: 0x000069B5
		private IDefinitionAppId GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000320 RID: 800 RVA: 0x000079CB File Offset: 0x000069CB
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000321 RID: 801 RVA: 0x000079D3 File Offset: 0x000069D3
		public IDefinitionAppId Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x000079DB File Offset: 0x000069DB
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x000079E0 File Offset: 0x000069E0
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

		// Token: 0x06000324 RID: 804 RVA: 0x00007A1C File Offset: 0x00006A1C
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000D79 RID: 3449
		private IEnumSTORE_DEPLOYMENT_METADATA _enum;

		// Token: 0x04000D7A RID: 3450
		private bool _fValid;

		// Token: 0x04000D7B RID: 3451
		private IDefinitionAppId _current;
	}
}
