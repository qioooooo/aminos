using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000D5 RID: 213
	internal class StoreDeploymentMetadataPropertyEnumeration : IEnumerator
	{
		// Token: 0x06000329 RID: 809 RVA: 0x00007A30 File Offset: 0x00006A30
		public StoreDeploymentMetadataPropertyEnumeration(IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY pI)
		{
			this._enum = pI;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00007A3F File Offset: 0x00006A3F
		private StoreOperationMetadataProperty GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600032B RID: 811 RVA: 0x00007A55 File Offset: 0x00006A55
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600032C RID: 812 RVA: 0x00007A62 File Offset: 0x00006A62
		public StoreOperationMetadataProperty Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00007A6A File Offset: 0x00006A6A
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00007A70 File Offset: 0x00006A70
		public bool MoveNext()
		{
			StoreOperationMetadataProperty[] array = new StoreOperationMetadataProperty[1];
			uint num = this._enum.Next(1U, array);
			if (num == 1U)
			{
				this._current = array[0];
			}
			return this._fValid = num == 1U;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00007AB5 File Offset: 0x00006AB5
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000D7C RID: 3452
		private IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY _enum;

		// Token: 0x04000D7D RID: 3453
		private bool _fValid;

		// Token: 0x04000D7E RID: 3454
		private StoreOperationMetadataProperty _current;
	}
}
