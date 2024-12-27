using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x0200031A RID: 794
	internal sealed class SafeCertContextHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06001903 RID: 6403 RVA: 0x0005572A File Offset: 0x0005472A
		private SafeCertContextHandle()
			: base(true)
		{
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x00055733 File Offset: 0x00054733
		internal SafeCertContextHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001905 RID: 6405 RVA: 0x00055743 File Offset: 0x00054743
		internal static SafeCertContextHandle InvalidHandle
		{
			get
			{
				return new SafeCertContextHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06001906 RID: 6406
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("crypt32.dll", SetLastError = true)]
		private static extern bool CertFreeCertificateContext(IntPtr pCertContext);

		// Token: 0x06001907 RID: 6407 RVA: 0x0005574F File Offset: 0x0005474F
		protected override bool ReleaseHandle()
		{
			return SafeCertContextHandle.CertFreeCertificateContext(this.handle);
		}
	}
}
