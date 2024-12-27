using System;

namespace System.EnterpriseServices
{
	// Token: 0x02000028 RID: 40
	internal class WeakProxyTearoff : ProxyTearoff, IObjectControl, IObjectConstruct
	{
		// Token: 0x0600008C RID: 140 RVA: 0x00002693 File Offset: 0x00001693
		internal override void Init(ServicedComponentProxy scp)
		{
			this._scp = new WeakReference(scp, true);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000026A2 File Offset: 0x000016A2
		internal override void SetCanBePooled(bool fCanBePooled)
		{
			this._fCanBePooled = fCanBePooled;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000026AC File Offset: 0x000016AC
		void IObjectControl.Activate()
		{
			ServicedComponentProxy servicedComponentProxy = (ServicedComponentProxy)this._scp.Target;
			servicedComponentProxy.ActivateObject();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000026D0 File Offset: 0x000016D0
		void IObjectControl.Deactivate()
		{
			ServicedComponentProxy servicedComponentProxy = (ServicedComponentProxy)this._scp.Target;
			if (servicedComponentProxy != null)
			{
				ComponentServices.DeactivateObject(servicedComponentProxy.GetTransparentProxy(), true);
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000026FD File Offset: 0x000016FD
		bool IObjectControl.CanBePooled()
		{
			return this._fCanBePooled;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00002708 File Offset: 0x00001708
		void IObjectConstruct.Construct(object obj)
		{
			ServicedComponentProxy servicedComponentProxy = (ServicedComponentProxy)this._scp.Target;
			servicedComponentProxy.DispatchConstruct(((IObjectConstructString)obj).ConstructString);
		}

		// Token: 0x0400002D RID: 45
		private WeakReference _scp;

		// Token: 0x0400002E RID: 46
		private bool _fCanBePooled;
	}
}
