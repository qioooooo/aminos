using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001E0 RID: 480
	internal class StoreDeploymentMetadataPropertyEnumeration : IEnumerator
	{
		// Token: 0x060014D4 RID: 5332 RVA: 0x00036ABC File Offset: 0x00035ABC
		public StoreDeploymentMetadataPropertyEnumeration(IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY pI)
		{
			this._enum = pI;
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00036ACB File Offset: 0x00035ACB
		private StoreOperationMetadataProperty GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x060014D6 RID: 5334 RVA: 0x00036AE1 File Offset: 0x00035AE1
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x060014D7 RID: 5335 RVA: 0x00036AEE File Offset: 0x00035AEE
		public StoreOperationMetadataProperty Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x00036AF6 File Offset: 0x00035AF6
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x00036AFC File Offset: 0x00035AFC
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

		// Token: 0x060014DA RID: 5338 RVA: 0x00036B41 File Offset: 0x00035B41
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x04000812 RID: 2066
		private IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY _enum;

		// Token: 0x04000813 RID: 2067
		private bool _fValid;

		// Token: 0x04000814 RID: 2068
		private StoreOperationMetadataProperty _current;
	}
}
