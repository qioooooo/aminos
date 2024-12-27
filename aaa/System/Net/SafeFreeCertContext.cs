using System;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x0200051C RID: 1308
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeCertContext : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002846 RID: 10310 RVA: 0x000A5DA0 File Offset: 0x000A4DA0
		internal SafeFreeCertContext()
			: base(true)
		{
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x000A5DA9 File Offset: 0x000A4DA9
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Set(IntPtr value)
		{
			this.handle = value;
		}

		// Token: 0x06002848 RID: 10312 RVA: 0x000A5DB2 File Offset: 0x000A4DB2
		protected override bool ReleaseHandle()
		{
			UnsafeNclNativeMethods.SafeNetHandles.CertFreeCertificateContext(this.handle);
			return true;
		}

		// Token: 0x04002773 RID: 10099
		private const string CRYPT32 = "crypt32.dll";

		// Token: 0x04002774 RID: 10100
		private const string ADVAPI32 = "advapi32.dll";

		// Token: 0x04002775 RID: 10101
		private const uint CRYPT_ACQUIRE_SILENT_FLAG = 64U;
	}
}
