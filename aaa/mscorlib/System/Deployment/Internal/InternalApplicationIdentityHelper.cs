using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal
{
	// Token: 0x0200006A RID: 106
	[ComVisible(false)]
	public static class InternalApplicationIdentityHelper
	{
		// Token: 0x06000633 RID: 1587 RVA: 0x0001560E File Offset: 0x0001460E
		public static object GetInternalAppId(ApplicationIdentity id)
		{
			return id.Identity;
		}
	}
}
