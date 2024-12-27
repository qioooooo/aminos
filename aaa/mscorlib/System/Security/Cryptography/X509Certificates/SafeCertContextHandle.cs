using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008A5 RID: 2213
	internal sealed class SafeCertContextHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06005105 RID: 20741 RVA: 0x001239DA File Offset: 0x001229DA
		private SafeCertContextHandle()
			: base(true)
		{
		}

		// Token: 0x06005106 RID: 20742 RVA: 0x001239E3 File Offset: 0x001229E3
		internal SafeCertContextHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06005107 RID: 20743 RVA: 0x001239F3 File Offset: 0x001229F3
		internal static SafeCertContextHandle InvalidHandle
		{
			get
			{
				return new SafeCertContextHandle(IntPtr.Zero);
			}
		}

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06005108 RID: 20744 RVA: 0x001239FF File Offset: 0x001229FF
		internal IntPtr pCertContext
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					return IntPtr.Zero;
				}
				return Marshal.ReadIntPtr(this.handle);
			}
		}

		// Token: 0x06005109 RID: 20745
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _FreePCertContext(IntPtr pCert);

		// Token: 0x0600510A RID: 20746 RVA: 0x00123A24 File Offset: 0x00122A24
		protected override bool ReleaseHandle()
		{
			SafeCertContextHandle._FreePCertContext(this.handle);
			return true;
		}
	}
}
