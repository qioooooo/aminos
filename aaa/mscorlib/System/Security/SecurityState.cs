using System;
using System.Security.Permissions;

namespace System.Security
{
	// Token: 0x0200067F RID: 1663
	[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	public abstract class SecurityState
	{
		// Token: 0x06003CC4 RID: 15556 RVA: 0x000D0D70 File Offset: 0x000CFD70
		public bool IsStateAvailable()
		{
			AppDomainManager currentAppDomainManager = AppDomainManager.CurrentAppDomainManager;
			return currentAppDomainManager != null && currentAppDomainManager.CheckSecuritySettings(this);
		}

		// Token: 0x06003CC5 RID: 15557
		public abstract void EnsureState();
	}
}
