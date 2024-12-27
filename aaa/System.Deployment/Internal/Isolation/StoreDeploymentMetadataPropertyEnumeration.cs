using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000100 RID: 256
	internal class StoreDeploymentMetadataPropertyEnumeration : IEnumerator
	{
		// Token: 0x06000613 RID: 1555 RVA: 0x0001F0E8 File Offset: 0x0001E0E8
		public StoreDeploymentMetadataPropertyEnumeration(IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY pI)
		{
			this._enum = pI;
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0001F0F7 File Offset: 0x0001E0F7
		private StoreOperationMetadataProperty GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x0001F10D File Offset: 0x0001E10D
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x0001F11A File Offset: 0x0001E11A
		public StoreOperationMetadataProperty Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0001F122 File Offset: 0x0001E122
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0001F128 File Offset: 0x0001E128
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

		// Token: 0x06000619 RID: 1561 RVA: 0x0001F16D File Offset: 0x0001E16D
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x040004F0 RID: 1264
		private IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY _enum;

		// Token: 0x040004F1 RID: 1265
		private bool _fValid;

		// Token: 0x040004F2 RID: 1266
		private StoreOperationMetadataProperty _current;
	}
}
