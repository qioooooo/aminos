using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x0200051B RID: 1307
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeCertChain : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002843 RID: 10307 RVA: 0x000A5D56 File Offset: 0x000A4D56
		internal SafeFreeCertChain(IntPtr handle)
			: base(false)
		{
			base.SetHandle(handle);
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x000A5D68 File Offset: 0x000A4D68
		public override string ToString()
		{
			return "0x" + base.DangerousGetHandle().ToString("x");
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x000A5D92 File Offset: 0x000A4D92
		protected override bool ReleaseHandle()
		{
			UnsafeNclNativeMethods.SafeNetHandles.CertFreeCertificateChain(this.handle);
			return true;
		}

		// Token: 0x04002772 RID: 10098
		private const string CRYPT32 = "crypt32.dll";
	}
}
