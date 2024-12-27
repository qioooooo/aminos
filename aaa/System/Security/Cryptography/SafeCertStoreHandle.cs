using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x0200031B RID: 795
	internal sealed class SafeCertStoreHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06001908 RID: 6408 RVA: 0x0005575C File Offset: 0x0005475C
		private SafeCertStoreHandle()
			: base(true)
		{
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x00055765 File Offset: 0x00054765
		internal SafeCertStoreHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x0600190A RID: 6410 RVA: 0x00055775 File Offset: 0x00054775
		internal static SafeCertStoreHandle InvalidHandle
		{
			get
			{
				return new SafeCertStoreHandle(IntPtr.Zero);
			}
		}

		// Token: 0x0600190B RID: 6411
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("crypt32.dll", SetLastError = true)]
		private static extern bool CertCloseStore(IntPtr hCertStore, uint dwFlags);

		// Token: 0x0600190C RID: 6412 RVA: 0x00055781 File Offset: 0x00054781
		protected override bool ReleaseHandle()
		{
			return SafeCertStoreHandle.CertCloseStore(this.handle, 0U);
		}
	}
}
