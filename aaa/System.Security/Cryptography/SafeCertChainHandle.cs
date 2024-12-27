using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x02000054 RID: 84
	internal sealed class SafeCertChainHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600009C RID: 156 RVA: 0x00003728 File Offset: 0x00002728
		private SafeCertChainHandle()
			: base(true)
		{
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003731 File Offset: 0x00002731
		internal SafeCertChainHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00003741 File Offset: 0x00002741
		internal static SafeCertChainHandle InvalidHandle
		{
			get
			{
				return new SafeCertChainHandle(IntPtr.Zero);
			}
		}

		// Token: 0x0600009F RID: 159
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("crypt32.dll", SetLastError = true)]
		private static extern void CertFreeCertificateChain(IntPtr handle);

		// Token: 0x060000A0 RID: 160 RVA: 0x0000374D File Offset: 0x0000274D
		protected override bool ReleaseHandle()
		{
			SafeCertChainHandle.CertFreeCertificateChain(this.handle);
			return true;
		}
	}
}
