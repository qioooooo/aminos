using System;
using System.Runtime.InteropServices;
using System.Web.Hosting;

namespace System.Web
{
	// Token: 0x020000A7 RID: 167
	internal sealed class ApplicationImpersonationContext : ImpersonationContext
	{
		// Token: 0x06000860 RID: 2144 RVA: 0x000250ED File Offset: 0x000240ED
		internal ApplicationImpersonationContext()
		{
			base.ImpersonateToken(new HandleRef(this, HostingEnvironment.ApplicationIdentityToken));
		}
	}
}
