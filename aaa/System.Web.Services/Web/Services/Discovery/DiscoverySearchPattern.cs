using System;
using System.Security.Permissions;

namespace System.Web.Services.Discovery
{
	// Token: 0x02000097 RID: 151
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class DiscoverySearchPattern
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060003F7 RID: 1015
		public abstract string Pattern { get; }

		// Token: 0x060003F8 RID: 1016
		public abstract DiscoveryReference GetDiscoveryReference(string filename);
	}
}
