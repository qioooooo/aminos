using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x02000052 RID: 82
	internal sealed class SafeCertStoreHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000092 RID: 146 RVA: 0x000036C3 File Offset: 0x000026C3
		private SafeCertStoreHandle()
			: base(true)
		{
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000036CC File Offset: 0x000026CC
		internal SafeCertStoreHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000094 RID: 148 RVA: 0x000036DC File Offset: 0x000026DC
		internal static SafeCertStoreHandle InvalidHandle
		{
			get
			{
				return new SafeCertStoreHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06000095 RID: 149
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("crypt32.dll", SetLastError = true)]
		private static extern bool CertCloseStore(IntPtr hCertStore, uint dwFlags);

		// Token: 0x06000096 RID: 150 RVA: 0x000036E8 File Offset: 0x000026E8
		protected override bool ReleaseHandle()
		{
			return SafeCertStoreHandle.CertCloseStore(this.handle, 0U);
		}
	}
}
