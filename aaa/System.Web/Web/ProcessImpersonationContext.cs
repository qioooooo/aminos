using System;
using System.Runtime.InteropServices;

namespace System.Web
{
	// Token: 0x020000A6 RID: 166
	internal sealed class ProcessImpersonationContext : ImpersonationContext
	{
		// Token: 0x0600085F RID: 2143 RVA: 0x000250D4 File Offset: 0x000240D4
		internal ProcessImpersonationContext()
		{
			base.ImpersonateToken(new HandleRef(this, IntPtr.Zero));
		}
	}
}
