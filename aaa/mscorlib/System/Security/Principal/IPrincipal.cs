using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004B0 RID: 1200
	[ComVisible(true)]
	public interface IPrincipal
	{
		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x0600309A RID: 12442
		IIdentity Identity { get; }

		// Token: 0x0600309B RID: 12443
		bool IsInRole(string role);
	}
}
