using System;
using System.Runtime.InteropServices;
using System.Web.Hosting;

namespace System.Web
{
	// Token: 0x020000A8 RID: 168
	internal sealed class ClientImpersonationContext : ImpersonationContext
	{
		// Token: 0x06000861 RID: 2145 RVA: 0x00025106 File Offset: 0x00024106
		internal ClientImpersonationContext(HttpContext context)
		{
			this.Start(context, true);
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00025116 File Offset: 0x00024116
		internal ClientImpersonationContext(HttpContext context, bool throwOnError)
		{
			this.Start(context, throwOnError);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00025128 File Offset: 0x00024128
		private void Start(HttpContext context, bool throwOnError)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				if (context != null)
				{
					intPtr = context.ImpersonationToken;
				}
				else
				{
					intPtr = HostingEnvironment.ApplicationIdentityToken;
				}
			}
			catch
			{
				if (throwOnError)
				{
					throw;
				}
			}
			if (intPtr != IntPtr.Zero)
			{
				base.ImpersonateToken(new HandleRef(this, intPtr));
			}
		}
	}
}
