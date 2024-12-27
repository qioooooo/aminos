using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008A6 RID: 2214
	internal sealed class SafeCertStoreHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600510B RID: 20747 RVA: 0x00123A32 File Offset: 0x00122A32
		private SafeCertStoreHandle()
			: base(true)
		{
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x00123A3B File Offset: 0x00122A3B
		internal SafeCertStoreHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x0600510D RID: 20749 RVA: 0x00123A4B File Offset: 0x00122A4B
		internal static SafeCertStoreHandle InvalidHandle
		{
			get
			{
				return new SafeCertStoreHandle(IntPtr.Zero);
			}
		}

		// Token: 0x0600510E RID: 20750
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _FreeCertStoreContext(IntPtr hCertStore);

		// Token: 0x0600510F RID: 20751 RVA: 0x00123A57 File Offset: 0x00122A57
		protected override bool ReleaseHandle()
		{
			SafeCertStoreHandle._FreeCertStoreContext(this.handle);
			return true;
		}
	}
}
