using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x0200031D RID: 797
	internal sealed class SafeCertChainHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06001912 RID: 6418 RVA: 0x000557C1 File Offset: 0x000547C1
		private SafeCertChainHandle()
			: base(true)
		{
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x000557CA File Offset: 0x000547CA
		internal SafeCertChainHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001914 RID: 6420 RVA: 0x000557DA File Offset: 0x000547DA
		internal static SafeCertChainHandle InvalidHandle
		{
			get
			{
				return new SafeCertChainHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06001915 RID: 6421
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("crypt32.dll", SetLastError = true)]
		private static extern void CertFreeCertificateChain(IntPtr handle);

		// Token: 0x06001916 RID: 6422 RVA: 0x000557E6 File Offset: 0x000547E6
		protected override bool ReleaseHandle()
		{
			SafeCertChainHandle.CertFreeCertificateChain(this.handle);
			return true;
		}
	}
}
