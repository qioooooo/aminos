using System;
using System.Collections;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001DE RID: 478
	internal class StoreDeploymentMetadataEnumeration : IEnumerator
	{
		// Token: 0x060014C9 RID: 5321 RVA: 0x00036A32 File Offset: 0x00035A32
		public StoreDeploymentMetadataEnumeration(IEnumSTORE_DEPLOYMENT_METADATA pI)
		{
			this._enum = pI;
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x00036A41 File Offset: 0x00035A41
		private IDefinitionAppId GetCurrent()
		{
			if (!this._fValid)
			{
				throw new InvalidOperationException();
			}
			return this._current;
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x060014CB RID: 5323 RVA: 0x00036A57 File Offset: 0x00035A57
		object IEnumerator.Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060014CC RID: 5324 RVA: 0x00036A5F File Offset: 0x00035A5F
		public IDefinitionAppId Current
		{
			get
			{
				return this.GetCurrent();
			}
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x00036A67 File Offset: 0x00035A67
		public IEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x00036A6C File Offset: 0x00035A6C
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

		// Token: 0x060014CF RID: 5327 RVA: 0x00036AA8 File Offset: 0x00035AA8
		public void Reset()
		{
			this._fValid = false;
			this._enum.Reset();
		}

		// Token: 0x0400080F RID: 2063
		private IEnumSTORE_DEPLOYMENT_METADATA _enum;

		// Token: 0x04000810 RID: 2064
		private bool _fValid;

		// Token: 0x04000811 RID: 2065
		private IDefinitionAppId _current;
	}
}
