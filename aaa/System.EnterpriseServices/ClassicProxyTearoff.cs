using System;

namespace System.EnterpriseServices
{
	// Token: 0x02000027 RID: 39
	internal class ClassicProxyTearoff : ProxyTearoff, IObjectControl, IObjectConstruct
	{
		// Token: 0x06000085 RID: 133 RVA: 0x00002639 File Offset: 0x00001639
		internal override void Init(ServicedComponentProxy scp)
		{
			this._scp = scp;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002642 File Offset: 0x00001642
		internal override void SetCanBePooled(bool fCanBePooled)
		{
			this._fCanBePooled = fCanBePooled;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000264B File Offset: 0x0000164B
		void IObjectControl.Activate()
		{
			this._scp.ActivateObject();
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002658 File Offset: 0x00001658
		void IObjectControl.Deactivate()
		{
			ComponentServices.DeactivateObject(this._scp.GetTransparentProxy(), true);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000266B File Offset: 0x0000166B
		bool IObjectControl.CanBePooled()
		{
			return this._fCanBePooled;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00002673 File Offset: 0x00001673
		void IObjectConstruct.Construct(object obj)
		{
			this._scp.DispatchConstruct(((IObjectConstructString)obj).ConstructString);
		}

		// Token: 0x0400002B RID: 43
		private ServicedComponentProxy _scp;

		// Token: 0x0400002C RID: 44
		private bool _fCanBePooled;
	}
}
