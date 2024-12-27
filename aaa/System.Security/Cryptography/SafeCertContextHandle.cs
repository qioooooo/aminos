using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x02000051 RID: 81
	internal sealed class SafeCertContextHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00003691 File Offset: 0x00002691
		private SafeCertContextHandle()
			: base(true)
		{
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000369A File Offset: 0x0000269A
		internal SafeCertContextHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000036AA File Offset: 0x000026AA
		internal static SafeCertContextHandle InvalidHandle
		{
			get
			{
				return new SafeCertContextHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06000090 RID: 144
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("crypt32.dll", SetLastError = true)]
		private static extern bool CertFreeCertificateContext(IntPtr pCertContext);

		// Token: 0x06000091 RID: 145 RVA: 0x000036B6 File Offset: 0x000026B6
		protected override bool ReleaseHandle()
		{
			return SafeCertContextHandle.CertFreeCertificateContext(this.handle);
		}
	}
}
