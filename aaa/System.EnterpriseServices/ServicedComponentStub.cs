using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200002A RID: 42
	internal class ServicedComponentStub : IManagedObjectInfo
	{
		// Token: 0x06000097 RID: 151 RVA: 0x0000273F File Offset: 0x0000173F
		internal ServicedComponentStub(ServicedComponentProxy scp)
		{
			this.Refresh(scp);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000274E File Offset: 0x0000174E
		internal void Refresh(ServicedComponentProxy scp)
		{
			this._scp = new WeakReference(scp, true);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00002760 File Offset: 0x00001760
		void IManagedObjectInfo.GetIObjectControl(out IObjectControl pCtrl)
		{
			ServicedComponentProxy servicedComponentProxy = (ServicedComponentProxy)this._scp.Target;
			pCtrl = servicedComponentProxy.GetProxyTearoff() as IObjectControl;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000278C File Offset: 0x0000178C
		void IManagedObjectInfo.GetIUnknown(out IntPtr pUnk)
		{
			IntPtr zero = IntPtr.Zero;
			ServicedComponentProxy servicedComponentProxy = (ServicedComponentProxy)this._scp.Target;
			pUnk = servicedComponentProxy.GetOuterIUnknown();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000027BC File Offset: 0x000017BC
		void IManagedObjectInfo.SetInPool(bool fInPool, IntPtr pPooledObject)
		{
			ServicedComponentProxy servicedComponentProxy = (ServicedComponentProxy)this._scp.Target;
			servicedComponentProxy.SetInPool(fInPool, pPooledObject);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000027E4 File Offset: 0x000017E4
		void IManagedObjectInfo.SetWrapperStrength(bool bStrong)
		{
			ServicedComponentProxy servicedComponentProxy = (ServicedComponentProxy)this._scp.Target;
			Marshal.ChangeWrapperHandleStrength(servicedComponentProxy.GetTransparentProxy(), !bStrong);
		}

		// Token: 0x0400002F RID: 47
		private WeakReference _scp;
	}
}
